using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DL;
using OpenCvSharp;
using OpenCvSharp.Extensions;

 
namespace dnnconsole
{
    class Program_trt
    {
        static void Main(string[] args)
        {
            try
            {
                #region object_detection 
                TensorRT dnn_det = new TensorRT(0);  
                if (dnn_det.LoadModel(@"dnn_setting_yolov5.json"))
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"yolo");

                    int batchSize = dnn_det.GetBatchSize(); 
                    var files =  di.GetFiles();

                    List<Bitmap> images = new List<Bitmap>();
                    for (int i = 0; i < batchSize; i++)
                    {
                        images.Add(new Bitmap(files[i].FullName));
                        dnn_det.AddImage(images[i]);
                    }

                    var batch_lists = dnn_det.PredictYolov5(); 
                    for(int image_idx=0;image_idx< batch_lists.cnt/*batchSize와 같음*/; image_idx++)
                    {
                        var finds = batch_lists[image_idx].cnt;  
                        var boxes = batch_lists[image_idx].candidates;  
                        Console.WriteLine($"{files[image_idx].FullName}: {finds}");
                        continue;
                        using (Bitmap bmp = new Bitmap(images[image_idx])) 
                        {
                            Mat dispmat = BitmapConverter.ToMat(bmp);  

                           
                            for (int object_idx = 0; object_idx < finds; object_idx++)
                            {
                                var box = boxes[object_idx];
                                Rect rect = new Rect();
                                rect.X = (int)box.x;
                                rect.Y = (int)box.y;
                                rect.Width = (int)box.w;
                                rect.Height = (int)box.h;
                                Cv2.Rectangle(dispmat, rect, new Scalar(255, 0, 0), 5);
                            }

                             
                            dispmat = dispmat.Resize(new OpenCvSharp.Size(400, 400));
                            if (finds >= 1)
                            {
                                Cv2.ImShow("view", dispmat);
                                Cv2.WaitKey();
                                Cv2.DestroyWindow("view");
                            }
                        }

                    }
                   
                }
                #endregion

                #region cnn 
                TensorRT dnn_cnn = new TensorRT(1); 
                if (dnn_cnn.LoadModel(@"dnn_setting_trt_classification_1.json")) 
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"cnn");

                    int batchSize = dnn_cnn.GetBatchSize(); 

                    var files = di.GetFiles();

                    List<Bitmap> images = new List<Bitmap>();
                    for (int i = 0; i < batchSize; i++)
                    {
                        images.Add(new Bitmap(files[i].FullName));
                        dnn_cnn.AddImage(images[i]); 
                    }
                    var batch_list = dnn_cnn.PredictCategoryClassification();
                    for(int image_idx = 0; image_idx < batch_list.cnt; image_idx++)
                    {
                        uint rst = batch_list.container_list[image_idx]; 
                        Console.WriteLine($"{files[image_idx].FullName}: {rst}");
                    }
                }
                #endregion
                dnn_cnn.Dispose();
                dnn_det.Dispose(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();

        }

    }
}
