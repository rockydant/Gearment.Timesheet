using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oqtane.Modules
{
    public class ImageUtils
    {
        public static Size ScaleSize(Size from, int? maxWidth, int? maxHeight)
        {
            return ScaleSize(from.Width, from.Height, maxWidth, maxHeight);
        }

        public static Size ScaleSize(int Width, int Height, int? maxWidth, int? maxHeight)
        {
            if (!maxWidth.HasValue && !maxHeight.HasValue) throw new ArgumentException("At least one scale factor (toWidth or toHeight) must not be null.");
            if (Height == 0 || Width == 0) throw new ArgumentException("Cannot scale size from zero.");

            double? widthScale = null;
            double? heightScale = null;

            if (maxWidth.HasValue)
            {
                widthScale = maxWidth.Value / (double)Width;
            }
            if (maxHeight.HasValue)
            {
                heightScale = maxHeight.Value / (double)Height;
            }

            double scale = Math.Min((double)(widthScale ?? heightScale),
                                     (double)(heightScale ?? widthScale));

            return new Size((int)Math.Floor(Width * scale), (int)Math.Ceiling(Height * scale));
        }

        public static Image ResizeImage(Image image, int? maxWidth, int? maxHeight)
        {
            var szNew = ScaleSize(image.Width, image.Height, maxWidth, maxHeight);

            //start the resize with a new image
            var newImage = new Bitmap(szNew.Width, szNew.Height);

            //set the new resolution
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //start the resizing
            using (var graphics = Graphics.FromImage(newImage))
            {
                //set some encoding specs
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, 0, 0, szNew.Width, szNew.Height);
            }

            return newImage;

        }

    }
}
