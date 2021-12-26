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
        private static Dictionary<int, string> namesDic_ = new Dictionary<int, string>();
        public bool isinit_ { get; set; }
        public bool load_model(string configpath = "dnn_setting.json")
        {
            if (!TensorRTWrapper.trt_init(configpath))
            {
                isinit_ = false;
                return false;
            }
            isinit_ = true;
            return true;
        }
        public bool add_image(string filepath)
        {
            return TensorRTWrapper.trt_add_image_file(filepath);
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
                success = TensorRTWrapper.trt_add_encoded_image(pnt, imageData.Length);
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
            TensorRTWrapper.trt_category_classification(ref rst);
            return rst;
        }
        public SegmContainer_Rst_List predict_yolact()
        {
            SegmContainer_Rst_List rst = new SegmContainer_Rst_List();
            TensorRTWrapper.trt_yolact(ref rst);
            return rst;
        }
        public void clear_buffer()
        {
            TensorRTWrapper.trt_clear_buffer();
        }

        public void release_segm_container(ref SegmContainer_Rst_List container)
        {
            CommonWrapper.release_segm_container(ref container);
        }
        public void dispose()
        {
            TensorRTWrapper.trt_dispose();
        }
    }
}
