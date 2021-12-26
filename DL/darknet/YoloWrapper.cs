using System;
using System.Runtime.InteropServices;
using C_STRUCTURE;
namespace YoloWrapper
{

    public class YoloWrapper : IDisposable
    {
        private const string YoloLibraryName = @"yolo_cpp_dll.dll";

        [DllImport(YoloLibraryName, EntryPoint = "init")]
        private static extern int InitializeYolo(string configurationFilename, string weightsFilename, int gpu);

        [DllImport(YoloLibraryName, EntryPoint = "detect_image")]
        private static extern int DetectImage(string filename, ref Yolov3_BboxContainer container);

        [DllImport(YoloLibraryName, EntryPoint = "detect_mat")]
        private static extern int DetectImage(IntPtr pArray, int nSize, ref Yolov3_BboxContainer container);

        [DllImport(YoloLibraryName, EntryPoint = "dispose")]
        private static extern int DisposeYolo();

        public YoloWrapper(string configurationFilename, string weightsFilename, int gpu)
        {
            try
            {
                   InitializeYolo(configurationFilename, weightsFilename, gpu);
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
            }
            
        }

        public void Dispose()
        {
            DisposeYolo();
        }

        public Yolov3_BoundingBox[] Detect(string filename)
        {
            var container = new Yolov3_BboxContainer();
            var count = DetectImage(filename, ref container);

            return container.candidates;
        }

        public Yolov3_BoundingBox[] Detect(byte[] imageData)
        {
            var container = new Yolov3_BboxContainer();

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                var count = DetectImage(pnt, imageData.Length, ref container);
                if (count == -1)
                {
                    throw new NotSupportedException($"{YoloLibraryName} has no OpenCV support");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(pnt);
            }

            return container.candidates;
        }

    }

}
