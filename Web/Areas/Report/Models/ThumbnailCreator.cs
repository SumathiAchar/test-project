using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    /// <summary>
    /// Creates thumnails
    /// </summary>
    public class ThumbnailCreator
    {
        /// <summary>
        /// Types of image format supported
        /// </summary>
        private static readonly IDictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>{
            {"image/png", ImageFormat.Png},
            {"image/gif", ImageFormat.Gif},
            {"image/jpeg", ImageFormat.Jpeg}
        };

        private readonly ImageResizer _resizer;

        public ThumbnailCreator()
        {
            _resizer = new ImageResizer();
        }

        /// <summary>
        /// Creates Thumbnail
        /// </summary>
        /// <param name="source"></param>
        /// <param name="desiredSize"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public byte[] Create(Stream source, ImageSize desiredSize, string contentType)
        {
            using (var image = Image.FromStream(source))
            {
                var originalSize = new ImageSize
                {
                    Height = image.Height,
                    Width = image.Width
                };

                var size = _resizer.Resize(originalSize, desiredSize);
                using (var thumbnail = new Bitmap(size.Width, size.Height))
                {
                    ScaleImage(image, thumbnail);

                    using (var memoryStream = new MemoryStream())
                    {
                        thumbnail.Save(memoryStream, ImageFormats[contentType]);

                        return memoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Scale the image as per destination height and width
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void ScaleImage(Image source, Image destination)
        {
            using (var graphics = Graphics.FromImage(destination))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(source, 0, 0, destination.Width, destination.Height);
            }
        }
    }
}