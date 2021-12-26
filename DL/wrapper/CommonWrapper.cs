using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using C_STRUCTURE;
namespace Wrapper
{
    static public class CommonWrapper
    {
        public const string DnnLibraryName = @"dl_cpp.dll";
        [DllImport(DnnLibraryName, EntryPoint = "release_image_info")]
        public static extern void release_image_info(ref ImageInfo info);

        [DllImport(DnnLibraryName, EntryPoint = "release_segm_container")]
        public static extern int release_segm_container(ref SegmContainer_Rst_List container);
    }
}
