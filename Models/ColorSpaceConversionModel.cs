using System.ComponentModel.DataAnnotations;

namespace Color_Space_Conversion.Models
{
    public class ColorSpaceConversionModel
    {
        [Required]
        public IFormFile? Image { get; set; }
        public ColorType? ColorType { get; set; }
    }
   public  enum ColorType
    {
        GrayScale,HSV,LAB
    }
}
