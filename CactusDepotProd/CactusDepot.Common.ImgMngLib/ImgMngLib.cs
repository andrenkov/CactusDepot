using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace CactusDepot.Shared.ImageMngLib
{
    public class ImgMngLib
    {
        public static Image? ImageResize(Image image, int width, int height, bool keepproportion = true, bool portrait = false)
        {
            //https://www.prowaretech.com/articles/current/asp-net-core/image-utility-for-sixlabors-imagesharp
            //https://andrewlock.net/using-imagesharp-to-resize-images-in-asp-net-core-part-4-saving-to-disk/
            //https://blog.elmah.io/upload-and-resize-an-image-with-asp-net-core-and-imagesharp/ !!!!!!

            //using (FileStream stream = File.OpenRead("foo.jpg"))
            //using (FileStream output = File.OpenWrite("bar.jpg"))
            //{
            //    Image img = new(stream);
            //    img.Resize(image.Width / 2, image.Height / 2)
            //         .Greyscale()
            //         .Save(output);
            //}


            return null;
        }
    }
}
