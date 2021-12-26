using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ExampleCommon;
using System.Drawing.Imaging;
using System.Diagnostics;
using YoloWrapper;
using System.IO;
using ConsoleTables;
using C_STRUCTURE;
namespace DL
{
    class dnn_module_darknet
    {
        model_config config_ = new model_config();
        private static Dictionary<int, string> namesDic_ = new Dictionary<int, string>();
        private static YoloWrapper.YoloWrapper wrapper_;
        public bool isinit_ { get; set; }
        public bool load_model(string configpath = "dnn_setting.json")
        {
            if (!config_.load_config(configpath))
                return false;


            if(!File.Exists(config_.yolo_weights) || !config_.yolo_enable)
            {
                return false;
            }
        
            try
            {
             
                wrapper_ = new YoloWrapper.YoloWrapper(config_.yolo_cfg, config_.yolo_weights, 0);
                var lines = File.ReadAllLines(config_.yolo_class);
                for (var i = 0; i < lines.Length; i++)
                    if (!namesDic_.ContainsKey(i))
                        namesDic_.Add(i, lines[i]);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                isinit_ = false;
                return false;
            }
            isinit_ = true;
          
            
            return true;

             
        }
        private static void log(Yolov3_BoundingBox[] bbox)
        {
            Console.WriteLine("Result：");
            var table = new ConsoleTable("Type", "Confidence", "X", "Y", "Width", "Height");
            foreach (var item in bbox.Where(o => o.h > 0 || o.w > 0))
            {
                //var type = namesDic_[(int)item.obj_id];
                table.AddRow(item.obj_id, item.prob, item.x, item.y, item.w, item.h);
            }
            table.Write(Format.MarkDown);
            Console.WriteLine(table.Rows.Count);

        }
       
        public List<Yolov3_BoundingBox> predict_object_detect(Bitmap bitmap)
        {
            List<Yolov3_BoundingBox> lbox = new List<Yolov3_BoundingBox>();
            if (!isinit_)
                return lbox;
            
            //var bbox = wrapper_.Detect("yolo2.bmp");
            var bbox = wrapper_.Detect(bitmap.ToByteArray(ImageFormat.Bmp));

            foreach (var item in bbox.Where(o => o.prob > config_.yolo_threshold))
            {
                 lbox.Add(item);
            }
            //convert(bbox);
            return lbox;
        }
     
    }
}
