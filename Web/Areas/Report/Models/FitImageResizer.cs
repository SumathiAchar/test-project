using System;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class ImageResizer
    {
        /// <summary>
        /// Resize the thumnails as per target size
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public ImageSize Resize(ImageSize originalSize, ImageSize targetSize)
        {
            var aspectRatio = originalSize.Width / (float)originalSize.Height;
            var width = targetSize.Width;
            var height = targetSize.Height;

            if (originalSize.Width > targetSize.Width || originalSize.Height > targetSize.Height)
            {
                if (aspectRatio > 1)
                    height = (int) (targetSize.Height/aspectRatio);
                else
                    width = (int) (targetSize.Width*aspectRatio);
            }
            else
            {
                width = originalSize.Width;
                height = originalSize.Height;
            }

            return new ImageSize
            {
                Width = Math.Max(width, 1),
                Height = Math.Max(height, 1)
            };
        }
    }
}