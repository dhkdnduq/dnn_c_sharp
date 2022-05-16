using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using YoloWrapper;
using OpenCvSharp;
using System.IO;
using System.Diagnostics;
using Wrapper;
using C_STRUCTURE;

namespace Wrapper
{
    public static class TensorRTWrapper
    {
        public const string DnnLibraryName = @"dl_cpp.dll";
       
        [DllImport(DnnLibraryName, EntryPoint = "trt_init")]
        public static extern bool trt_init(string configurationFilename, int gpu = 0);
        [DllImport(DnnLibraryName, EntryPoint = "trt_get_batch_size")]
        public static extern int trt_get_batch_size(int gpu = 0);


        [DllImport(DnnLibraryName, EntryPoint = "trt_add_image_file")]
        public static extern bool trt_add_image_file(string filename, int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "trt_add_encoded_image")]
        public static extern bool trt_add_encoded_image(IntPtr pArray, int nSize, int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "trt_clear_buffer")]
        public static extern void trt_clear_buffer(int gpu = 0);

         
        [DllImport(DnnLibraryName, EntryPoint = "trt_category_classification")]
        public static extern int trt_category_classification(ref Category_Rst_List container, int gpu = 0, bool is_clear_buffer = true);

        [DllImport(DnnLibraryName, EntryPoint = "trt_yolact")]
        public static extern int trt_yolact(ref SegmContainer_Rst_List container, int gpu = 0, bool is_clear_buffer = true);

        [DllImport(DnnLibraryName, EntryPoint = "trt_yolov5")]
        public static extern int trt_yolov5(ref BboxContainer_Rst_List container, int gpu = 0, bool is_clear_buffer = true);

        [DllImport(DnnLibraryName, EntryPoint = "trt_dispose")]
        public static extern int trt_dispose(int gpu = 0);
    }
}
