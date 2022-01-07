using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace dnnconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /* 전역 선언 필요 */
                TensorRT dnn = new TensorRT();


                if (dnn.LoadModel(@"D:\visual_code\dnn_test_c\dnn_test_c\dnn_test_c\dnn_setting_yolov5.json"))
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"D:\deeplearning\yolov5-master\yolov5-master\data\images");
                  
                    foreach (System.IO.FileInfo File in di.GetFiles())
                    {
                        //while(true)
                        {
                                using (Bitmap bmp = new Bitmap(File.FullName))
                                {
                                    dnn.AddImage(bmp);
                                    sw.Restart();

                                    var drst = dnn.PredictYolov5();
                                    int batchSize = 0;
                                    var brst = drst[batchSize];
                                    Mat dispmat = BitmapConverter.ToMat(bmp);
                                    foreach (var box in brst.candidates)
                                    {
                                        Rect rect = new Rect();

                                        rect.X = (int)box.x;
                                        rect.Y = (int)box.y;
                                        rect.Width = (int)box.w;
                                        rect.Height = (int)box.h;

                                        Cv2.Rectangle(dispmat, rect, new Scalar(255, 0, 0), 5);
                                    }
                                    dispmat = dispmat.Resize(new OpenCvSharp.Size(400,400));
                                    if (drst.cnt >= 1)
                                    {
                                        Cv2.ImShow("view", dispmat);
                                        Cv2.WaitKey();
                                        Cv2.DestroyWindow("view");
                                    }

                                    sw.Stop();
                                    Console.WriteLine("rst :{0}  tack :{1}" , 0,sw.ElapsedMilliseconds.ToString());
                                  
                                }
                        }
                    }
                    dnn.Dispose();
                 }
             }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
          
        }
    }
}
