using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DL.util
{
    
    /// <summary>
    /// Allows to add graphic elements to the existing image.
    /// </summary>
    public class ImageEditor : IDisposable
    {
        private Graphics _graphics;
        private Bitmap _image;
        private string _fontFamily;
        private float _fontSize;
      
        public ImageEditor(Bitmap inputFile,  string fontFamily = "Ariel", float fontSize = 12)
        {
            _fontFamily = fontFamily;
            _fontSize = fontSize;
            _image = inputFile;
            _graphics = Graphics.FromImage(inputFile);
        }
        public Bitmap GetBitmap()
        {
            return new Bitmap(_image);
        }

        /// <summary>
        /// Adds rectangle with a label in particular position of the image
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        /// <param name="text"></param>
        /// <param name="colorName"></param>
        public void AddBox(float xmin, float xmax, float ymin, float ymax, string text = "", string colorName = "red")
        {
            var left = xmin * _image.Width;
            var right = xmax * _image.Width;
            var top = ymin * _image.Height;
            var bottom = ymax * _image.Height;


            var imageRectangle = new Rectangle(new Point(0, 0), new Size(_image.Width, _image.Height));
            _graphics.DrawImage(_image, imageRectangle);

            Color color = Color.FromName(colorName);
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(brush);

            _graphics.DrawRectangle(pen, left, top, right - left, bottom - top);
            var font = new Font(_fontFamily, _fontSize);
            SizeF size = _graphics.MeasureString(text, font);
            _graphics.DrawString(text, font, brush, new PointF(left, top - size.Height));
        }

        public void AddMask(List<Tuple<float, float>> list,string colorName = "red")
        {
            Color color = Color.FromName(colorName);
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(brush);
            PointF[] pf = new PointF[list.Count];
            for(int i=0;i<list.Count;i++ )
            {
                var pos = list[i];
                float pos_x = pos.Item1 * _image.Width;
                float pos_y = pos.Item2 * _image.Height;
                pf[i].X = pos_x;
                pf[i].Y = pos_y;
                _graphics.DrawRectangle(pen, pos_x, pos_y, 1, 1)
;            }
          
        }


        public void Dispose()
        {
            if (_image != null)
            {

                if (_graphics != null)
                {
                    _graphics.Dispose();
                }

                _image.Dispose();
            }
        }
    }
    

}
