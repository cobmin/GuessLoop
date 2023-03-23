using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Helpers
{
    public class ModifyImage
    {
        public static void ResizeImage(string sourceFilePath, string destFilePath, float widthCm, float heightCm)
        {
            using (var sourceImage = new Bitmap(sourceFilePath))
            {
                // Calculate the target size in pixels based on the desired centimeter dimensions and the resolution of the source image
                float dpiX = sourceImage.HorizontalResolution;
                float dpiY = sourceImage.VerticalResolution;
                int widthPx = (int)(widthCm * dpiX / 2.54f);
                int heightPx = (int)(heightCm * dpiY / 2.54f);

                // Create a new bitmap with the target size
                using (var targetImage = new Bitmap(widthPx, heightPx))
                {
                    using (var graphics = Graphics.FromImage(targetImage))
                    {
                        // Set the interpolation mode to high quality to improve the image quality
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        // Draw the source image onto the target image, scaling it to fit within the target size
                        graphics.DrawImage(sourceImage, new Rectangle(0, 0, widthPx, heightPx), new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
                    }

                    // Save the target image to the specified file path
                    targetImage.Save(destFilePath);
                }
            }
        }
        public static void CropImage(string sourceFilePath, string destFilePath, Rectangle cropArea)
        {
            using (var sourceImage = new Bitmap(sourceFilePath))
            {
                // Create a new bitmap with the size of the crop area
                using (var croppedImage = new Bitmap(cropArea.Width, cropArea.Height))
                {
                    using (var graphics = Graphics.FromImage(croppedImage))
                    {
                        // Draw the cropped region of the source image onto the cropped image
                        graphics.DrawImage(sourceImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropArea, GraphicsUnit.Pixel);
                    }

                    // Save the cropped image to the specified file path
                    croppedImage.Save(destFilePath);
                }
            }
        }
        public static List<string> GetImages(string framePath)
        {
            Image frame;
            List<string> framePaths = new List<string>();
            try
            {
                foreach (string imagePath in Directory.GetFiles(framePath))
                {
                    string extension = Path.GetExtension(imagePath);

                    if (extension == ".jpg" || extension == ".png" || extension == ".gif")
                    {
                        framePaths.Add(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return framePaths;
        }
        public static void ToPrinterPaper(List<string> sourceFilePath)
        {
            var widthCm = 21;
            var heightCm = 27;
            var width = 950;
            var height = 1251;
            var counter = 0;

            using (var bitmap = new Bitmap(widthCm, heightCm))
            {
                using (var canvas = Graphics.FromImage(bitmap))
                {
                    foreach (var item in sourceFilePath)
                    {
                        var image = Image.FromFile(item);
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(image,
                                            new Rectangle(0,
                                                        0,
                                                        width,
                                                        height),
                                            new Rectangle(0,
                                                        0,
                                                        image.Width,
                                                        image.Height),
                                            GraphicsUnit.Pixel);
                        canvas.Save();
                    }
                }
                try
                {
                    bitmap.Save($".\\Img\\Output\\Pages\\Page{counter++}.png",
                                System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (Exception ex) { }
            }
        }
    }
}
