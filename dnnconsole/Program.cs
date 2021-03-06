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
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /* 전역 선언 필요 */
                Pytorch dnn = new Pytorch();


                if (dnn.LoadModel(@"D:\visual_code\dnn_test_c\dnn_test_c\dnn_test_c\dnn_setting_anomaly_patchcore_bmw.json"))
                {

                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"D:\deeplearning\data\mvtec_anomaly_detection\bmw\test\ng");
                    foreach (System.IO.FileInfo File in di.GetFiles())
                    {
                       
                        
                                //using (Bitmap bmp = new Bitmap(File.FullName))
                                {
                                 // dnn.AddImage(bmp);
                                    dnn.AddImage(File.FullName);
                                    int category = 0;
                                    var drst = dnn.PredictAnomalyDection(category);
                                    Console.WriteLine("rst :{0},{1}  " , drst.container_list[0].candidates[0].prob, drst.container_list[0].candidates[0].obj_id == 1 ? "ok":"ng");
                                    dnn.ReleaseSegmContainer(ref drst);

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
