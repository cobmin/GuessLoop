using ConsoleApp1.Helpers;
using System.Drawing;
using System.Drawing.Drawing2D;

List<string> framePaths = ModifyImage.GetImages(".\\Img\\Frames");
List<string> facePaths = ModifyImage.GetImages(".\\Img\\Faces");

foreach (var item in facePaths)
{
    string[] substrings = item.Split(new string[] { "\\" }, StringSplitOptions.None);
    var fileName = substrings[substrings.Length - 1].Split('.')[0];

    Rectangle cropArea = new Rectangle(191, 0, 950, 1251); 
    ModifyImage.CropImage(item, $".\\Img\\Output\\Cropped\\{fileName}.{substrings[substrings.Length - 1].Split('.')[1]}", cropArea);

    var face = Image.FromFile($".\\Img\\Output\\Cropped\\{fileName}.{substrings[substrings.Length - 1].Split('.')[1]}");

    using (face)
    {
        var counter = 0;
        var width = 950;
        var height = 1251;
        foreach (var framePathx in framePaths)
        {
            var frame = Image.FromFile(framePathx);
            using (var bitmap = new Bitmap(width, height))
            {
                using (var canvas = Graphics.FromImage(bitmap))
                {
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    canvas.DrawImage(face,
                                     new Rectangle(0,
                                                   0,
                                                   width,
                                                   height),
                                     new Rectangle(0,
                                                   0,
                                                   face.Width,
                                                   face.Height),
                                     GraphicsUnit.Pixel);
                    canvas.DrawImage(frame,
                                     new Rectangle(0,
                                                   0,
                                                   width,
                                                   height),
                                     new Rectangle(0,
                                                   0,
                                                   frame.Width,
                                                   frame.Height),
                                     GraphicsUnit.Pixel);
                    string text = fileName; // replace with your text
                    Font font = new Font("Calibri", 70); // replace with your font
                    Brush brush = new SolidBrush(Color.Black); // replace with your brush color
                    canvas.DrawString(text, font, brush, new PointF(140, 10));
                    canvas.Save();
                }
                try
                {
                    string frameColor;
                    if (counter == 1)
                        frameColor = "Blue";
                    else if (counter == 2)
                        frameColor = "White";
                    else
                        frameColor = "Big";
                    bitmap.Save($".\\Img\\Output\\FullSize\\{fileName}{frameColor}.{substrings[substrings.Length - 1].Split('.')[1]}",
                                System.Drawing.Imaging.ImageFormat.Png);
                    if(counter == 1|| counter == 2)
                        ModifyImage.ResizeImage($".\\Img\\Output\\FullSize\\{fileName}{frameColor}.{substrings[substrings.Length - 1].Split('.')[1]}", $".\\Img\\Output\\Final\\Small\\{fileName}{frameColor}.{substrings[substrings.Length - 1].Split('.')[1]}", 2.85f, 4.25f);
                    else
                        ModifyImage.ResizeImage($".\\Img\\Output\\FullSize\\{fileName}{frameColor}.{substrings[substrings.Length - 1].Split('.')[1]}", $".\\Img\\Output\\Final\\Big\\{fileName}{frameColor}.{substrings[substrings.Length - 1].Split('.')[1]}", 5f, 7.2f);
                    counter++;
                }
                catch (Exception ex) { }
            }
        }
    }
}

//List<string> cardPaths = ModifyImage.GetImages(".\\Img\\Output\\Resized");

//ModifyImage.ToPrinterPaper(cardPaths);