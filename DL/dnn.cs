using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrapper;
using C_STRUCTURE;
using ExampleCommon;
namespace DL
{
    public class Common
    {
        public void ReleaseSegmContainer(ref SegmContainer_Rst_List container)
        {
            CommonWrapper.release_segm_container(ref container);
        }
    }
    public class Pytorch:Common
    {
        internal dnn_module_pytorch torch_;

        public Pytorch(int gpu = 0)
        {
            torch_ = new dnn_module_pytorch(gpu);
        }
        public bool LoadModel(string configpath = "dnn_setting.json")
        {
            bool binit = torch_.load_model(configpath);
            if (!binit)
                return false;
            return true;
        }
        public bool AddImage(Bitmap bitmap)
        {
            return torch_.add_image(bitmap);
        }
        public bool AddImage(string filepath)
        {
            return torch_.add_image(filepath);
        }
        public BboxContainer_Rst_List PredictEfficientdet()
        {
            return torch_.predict_efficient_detect();
        }
        public SegmContainer_Rst_List PredictAnomalyDection(int category)
        {
            return torch_.predict_anomaly_detection(category);
        }

        public Binary_Rst_List PredictBinaryClassification()
        {
            return torch_.predict_binary_classification();
        }
      
        public void Dispose()
        {
            torch_.dispose();
        }
    }
    
    public class TensorRT : Common
    {
        internal dnn_module_trt trt_;
        public TensorRT(int gpu = 0)
        {
            trt_ = new dnn_module_trt(gpu);
        }
        public bool LoadModel(string configpath = "dnn_setting.json")
        {
            bool binit = trt_.load_model(configpath);
            if (!binit)
                return false;
            return true;
        }
        public bool AddImage(Bitmap bitmap)
        {
            return trt_.add_image(bitmap);
        }
        public int GetBatchSize()
        {
            return trt_.get_batch_size();
        }
        public bool AddImage(string filepath)
        {
            return trt_.add_image(filepath);
        }
        public Category_Rst_List PredictCategoryClassification()
        {
            return trt_.predict_category_classification();
        }
        public SegmContainer_Rst_List PredictYolact()
        {
            return trt_.predict_yolact();
        }
        public BboxContainer_Rst_List PredictYolov5()
        {
            return trt_.predict_yolov5();
        }

        public void ClearBuffer()
        {
            trt_.clear_buffer();
        }
        public void Dispose()
        {
            trt_.dispose(); ;
        }
    }

    public class Darknet
    {
        internal dnn_module_darknet yolo_;

        public Darknet()
        {
            yolo_ = new dnn_module_darknet();
        }
        public bool LoadModel(string configpath = "dnn_setting.json")
        {
            bool binit = yolo_.load_model(configpath);

            if (!binit)
                return false;

            return true;
        }

        public List<Yolov3_BoundingBox> PredictYolo(Bitmap bmp)
        {
            return yolo_.predict_object_detect(bmp);
        }
       
    }

}
