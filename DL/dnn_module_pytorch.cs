using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrapper;
using ExampleCommon;
using System.Drawing.Imaging;
using C_STRUCTURE;
using System.Runtime.InteropServices;

namespace DL
{
    class dnn_module_pytorch
    {
        private int gpu_index_ { get; set; }
        public bool isinit_ { get; set; }

        public dnn_module_pytorch(int gpu =0)
        {
            gpu_index_ = gpu;
        }
        public bool load_model(string configpath = "dnn_setting.json")
        {

            if(!PytorchWrapper.torch_init(configpath, gpu_index_))
            {
                isinit_ = false;
                return false;
            }
            isinit_ = true;
            return true;
        }
        public bool add_image(string filepath)
        {
            return PytorchWrapper.torch_add_image_file(filepath, gpu_index_);
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
                success = PytorchWrapper.torch_add_encoded_image(pnt, imageData.Length, gpu_index_);
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
        public BboxContainer_Rst_List predict_efficient_detect()
        {
            BboxContainer_Rst_List rst = new BboxContainer_Rst_List();
            PytorchWrapper.torch_effdet(ref rst, gpu_index_);
            return rst;
        } 
        public Binary_Rst_List predict_binary_classification()
        {
            Binary_Rst_List rst = new Binary_Rst_List();
            PytorchWrapper.torch_binary_classification(ref rst, gpu_index_);
            return rst;
        }
        public SegmContainer_Rst_List predict_anomaly_detection(int category)
        {
            SegmContainer_Rst_List rst = new SegmContainer_Rst_List();
            PytorchWrapper.torch_anomaly_detection(ref rst, category, gpu_index_);
            return rst;
        }
        public void ReleaseSegmContainer(ref SegmContainer_Rst_List container)
        {
            CommonWrapper.release_segm_container(ref container);
        }

        public void dispose()
        {
            PytorchWrapper.torch_dispose(gpu_index_);
        }
    }
}
