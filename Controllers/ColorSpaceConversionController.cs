using Color_Space_Conversion.Models;
using Emgu.CV;
using Microsoft.AspNetCore.Mvc;

namespace Color_Space_Conversion.Controllers
{
    public class ColorSpaceConversionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ColorSpaceConversionModel model)
        {
            Console.WriteLine(model.ColorType
                );
            string imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploaded");
            if(model.Image == null)
            {
                ViewBag.ErrorMessage = "Image Not Selected ";
                return View("Index");
            }
            if (model.ColorType == null)
            {
                ViewBag.ErrorMessage = "Color Type Not Selected";
                return View("Index");
            }
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
             string imgPath = Path.Combine(imageDirectory,model.Image.FileName);

            if (model.Image != null && model.Image.Length > 0)
            {
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
            }
            Mat originalImage = CvInvoke.Imread(imgPath);
            Mat processedImage = new Mat();
            switch (model.ColorType.ToString())
            {
                case "GrayScale":
                    CvInvoke.CvtColor(originalImage, processedImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    break;
                case "HSV":
                    CvInvoke.CvtColor(originalImage, processedImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
                    break;
                case "LAB":
                    CvInvoke.CvtColor(originalImage, processedImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Lab);
                    break;
                default:
                    processedImage = originalImage;
                    break;
            }
            if (processedImage.IsEmpty) {
                ViewBag.result = "Color Space Conversion failled";
            }
            string processedImageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProcessedImage");
            if (!Directory.Exists(processedImageDirectory))
            {
                Directory.CreateDirectory(processedImageDirectory); 
            }
            string filePath = Path.Combine(processedImageDirectory, Path.GetFileNameWithoutExtension(model.Image.FileName)+".png");
            CvInvoke.Imwrite(filePath, processedImage);
            ViewBag.OriginalImage = $"/Uploaded/{model.Image.FileName}";
            ViewBag.ProcessedImage = $"/ProcessedImage/{Path.GetFileNameWithoutExtension(model.Image.FileName)+".png"}";
            return View("Index");
        }
    }
}
