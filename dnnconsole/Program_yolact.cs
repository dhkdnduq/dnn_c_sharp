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

namespace dnnconsole
{
    class Program_Yolact
    {
        static void Main_Yolact(string[] args)
        {
            try
            {
                /* 전역 선언 필요 */
                TensorRT dnn = new TensorRT();


                if (dnn.LoadModel(@"D:\visual_code\dnn_test_c\dnn_test_c\dnn_test_c\dnn_setting_yolact.json"))
                {

                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"yolact\\");
                  
                    foreach (System.IO.FileInfo File in di.GetFiles())
                    {
                        while(true)
                        {
                                using (Bitmap bmp = new Bitmap(File.FullName))
                                {
                                        for (int i = 0; i < 10; i++)
                                            dnn.AddImage(bmp);

                                    sw.Restart();
                                
                                     var drst = dnn.PredictYolact();
                                    for(int j=0;j<10;j++)
                                    {
                                            var test = drst[j];
                                            var dispimg = test.display_img.ToBitmap();
                                            var maskimg = test.mask_img.ToBitmap();
                                    }
                                
                                    dnn.ReleaseSegmContainer(ref drst);

                                    sw.Stop();
                                    Console.WriteLine("rst :{0}  tack :{1}" , 0,sw.ElapsedMilliseconds.ToString());
                                    ;
                                    /*
                                    using(Mat mat = new Mat(File.FullName))
                                    {
                                        //Mat mat2 = mat.Flip(FlipMode.X);
                                        foreach (var box in drst)
                                        {
                                            Rect rect = new Rect();

                                            rect.X = (int)box.x;
                                            rect.Y = (int)box.y;
                                            rect.Width = (int)box.w;
                                            rect.Height = (int)box.h;

                                            Cv2.Rectangle(mat, rect, new Scalar(255, 0, 0), 5);
                                            Console.WriteLine(rect.ToString());

                                        }
                                        if(drst.Count >= 1)
                                        {

                                            Cv2.ImShow("view", mat);
                                            Cv2.WaitKey();
                                            Cv2.DestroyWindow("view");
                                            Console.WriteLine("TIme " + sw.ElapsedMilliseconds.ToString() + " msec");
                                        }
                                    }
                                    */

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
