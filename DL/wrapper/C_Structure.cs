using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

static class Constants
{
    public const int MAX_OBJECTS = 10; 
    public const int MAX_BATCH_SIZE = 10; //need to change stack memory
}
namespace C_STRUCTURE
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Yolov3_BoundingBox
    {
        public UInt32 x, y, w, h;
        public float prob;
        public UInt32 obj_id;
        public UInt32 track_id;
        public UInt32 frames_counter;
        public float x_3d, y_3d, z_3d;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Yolov3_BboxContainer
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_OBJECTS)]
        public Yolov3_BoundingBox[] candidates;

    }

    public struct BoundingBox
    {
        public UInt32 x, y, w, h;
        public float prob;
        public UInt32 obj_id;
        public float x_3d, y_3d, z_3d;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct BboxContainer
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_OBJECTS)]
        public BoundingBox[] candidates;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BboxContainer_Rst_List
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_BATCH_SIZE)]
        public BboxContainer[] container_list;

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageInfo
    {
        public IntPtr data;
        public int size;
        public  Bitmap ToBitmap()
        {
            if (data == IntPtr.Zero)
            {
                return new Bitmap(0,0);
            }
            byte[] imagePixels = new byte[size];
            Marshal.Copy(data, imagePixels, 0, size);
            
            MemoryStream mmstream = new MemoryStream(imagePixels);
            Bitmap processed = new Bitmap(mmstream);
            return processed;
        }

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SegmContainer
    {
        public UInt32 cnt;
        public ImageInfo mask_img;
        public ImageInfo display_img;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_OBJECTS)]
        public BoundingBox[] candidates;

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SegmContainer_Rst_List
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_BATCH_SIZE)]
        public SegmContainer[] container_list;
        public SegmContainer this[int index]
        {
            get { return container_list[index]; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Binary_Rst_List
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.MAX_BATCH_SIZE)]
        public bool[] container_list;

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Category_Rst_List
    {
        public UInt32 cnt;
        [MarshalAs(UnmanagedType.U4, SizeConst = Constants.MAX_BATCH_SIZE)]
        public UInt32[] container_list;

    }


}
