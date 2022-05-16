using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.IO;
using C_STRUCTURE;

namespace Wrapper
{
   
    public static class PytorchWrapper
    {
        public const string DnnLibraryName = @"dl_cpp.dll";
     
        [DllImport(DnnLibraryName, EntryPoint = "torch_init")]
        public static extern bool torch_init(string configurationFilename,int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "torch_add_image_file")]
        public static extern bool torch_add_image_file(string filename, int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "torch_add_encoded_image")]
        public static extern bool torch_add_encoded_image(IntPtr pArray, int nSize, int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "torch_clear_buffer")]
        public static extern void torch_clear_buffer(int gpu = 0);

        [DllImport(DnnLibraryName, EntryPoint = "torch_effdet")]
        public static extern int torch_effdet(ref BboxContainer_Rst_List container ,int gpu = 0, bool is_clear_buffer = true);

        [DllImport(DnnLibraryName, EntryPoint = "torch_binary_classification")]
        public static extern int torch_binary_classification(ref Binary_Rst_List container, int gpu = 0, bool is_clear_buffer = true);


        [DllImport(DnnLibraryName, EntryPoint = "torch_anomaly_detection")]
        public static extern int torch_anomaly_detection(ref SegmContainer_Rst_List container,int category , int gpu = 0, bool is_clear_buffer = true);

        [DllImport(DnnLibraryName, EntryPoint = "torch_dispose")]
        public static extern int torch_dispose(int gpu = 0);


    }
}
