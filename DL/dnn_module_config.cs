using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    internal class model_config
    { 
        //darknet
        public bool yolo_enable = false;
        public string yolo_weights;
        public string yolo_cfg;
        public string yolo_class;
        public float yolo_threshold =0.5f;
        public bool load_config(string configpath)
        {
            if (!File.Exists(configpath))
            {
                return false;
            }
            using (StreamReader r = new StreamReader(configpath))
            {

                string json_s = r.ReadToEnd();
                JObject json = JObject.Parse(json_s);
                
                JToken jobject_yolo;
                if(json.TryGetValue("darknet", out jobject_yolo))
                {
                    yolo_weights = jobject_yolo.Value<string>("weights");
                    yolo_cfg = jobject_yolo.Value<string>("cfg");
                    yolo_class= jobject_yolo.Value<string>("class");
                    yolo_enable = jobject_yolo.Value<string>("enable") == "true" ? true : false;
                    yolo_threshold = jobject_yolo.Value<float>("threshod");
               
                }

            }

            return true;
        }
    }

}
