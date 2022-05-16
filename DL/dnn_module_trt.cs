using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrapper;
using YoloWrapper;
using ExampleCommon;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using C_STRUCTURE;

namespace DL
{
    class dnn_module_trt
    {
        private int gpu_index_ { get; set; }
        public bool isinit_ { get; set; }
        public dnn_module_trt(int gpu = 0)
        {
            gpu_index_ = gpu;
        }
        public bool load_model(string configpath = "dnn_setting.json")
        {
            if (!TensorRTWrapper.trt_init(configpath,gpu_index_))
            {
                isinit_ = false;
                return false;
            }
            isinit_ = true;
            return true;
        }
        public int get_batch_size()
        {
            return TensorRTWrapper.trt_get_batch_size(gpu_index_);
        }
        public bool add_image(string filepath)
        {
            return TensorRTWrapper.trt_add_image_file(filepath, gpu_index_);
        }
        public bool add_image(Bitmap bitmap)
        {
            var imageData = bitmap.ToByteArray(ImageFormat.Bmp);

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);
            bool success = false;
            try
            {
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                success = TensorRTWrapper.trt_add_encoded_image(pnt, imageData.Length, gpu_index_);
            }
            catch (Exception e)
            {
                success = false;
                throw new ArgumentException(e.ToString());

            }
            finally
            {
                Marshal.FreeHGlobal(pnt);
            }
            return success;
        }
        public Category_Rst_List predict_category_classification()
        {
            Category_Rst_List rst = new Category_Rst_List();
            TensorRTWrapper.trt_category_classification(ref rst, gpu_index_);
            return rst;
        }
        public SegmContainer_Rst_List predict_yolact()
        {
            SegmContainer_Rst_List rst = new SegmContainer_Rst_List();
            TensorRTWrapper.trt_yolact(ref rst, gpu_index_);
            return rst;
        }
        public BboxContainer_Rst_List predict_yolov5()
        {
            BboxContainer_Rst_List rst = new BboxContainer_Rst_List();
            TensorRTWrapper.trt_yolov5(ref rst, gpu_index_);
            return rst;
        }

        public void clear_buffer()
        {
            TensorRTWrapper.trt_clear_buffer(gpu_index_);
        }

        public void release_segm_container(ref SegmContainer_Rst_List container)
        {
            CommonWrapper.release_segm_container(ref container);
        }
        public void dispose()
        {
            isinit_ = false;
            TensorRTWrapper.trt_dispose(gpu_index_);
        }
    }
}
