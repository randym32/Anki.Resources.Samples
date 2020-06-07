using System;
using System.Collections.Generic;
using Anki.Resources.SDK;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Example_3___Composite_Image
{
    /// <summary>
    /// A class to draw each sprite sequence in the picture box
    /// </summary>
    class SpriteControl
    {
        /// <summary>
        /// The sprite sequence that is being rendered
        /// </summary>
        SpriteSequence spriteSequence;

        /// <summary>
        /// "CustomHue" if the PNG images should be converted from gray scale
        /// to the colour using the current eye colour setting.
        /// "RGBA" if the image should be drawn as is.
        /// </summary>
        public string spriteRenderMethod;

        /// <summary>
        /// The object used to enumerator over the sprites
        /// </summary>
        public IEnumerator<Bitmap> itr;

        /// <summary>
        /// The control to draw within the screen.
        /// </summary>
        readonly PictureBox pictureBox;

        /// <summary>
        /// Constructs object to managing updating a sprite box element of the
        /// composite image.
        /// </summary>
        /// <param name="pictureBox">The picture box to draw in</param>
        /// <param name="spriteSequence">The sprite sequence</param>
        /// <param name="spriteRenderMethod">The method to draw in there</param>
        public SpriteControl(PictureBox pictureBox, SpriteSequence spriteSequence, string spriteRenderMethod)
        {
            this.pictureBox = pictureBox;
            this.spriteRenderMethod = spriteRenderMethod;
            this.spriteSequence = spriteSequence;
            // Make a note of the sprite sequences
            itr = spriteSequence.Bitmaps.GetEnumerator();
        }

        /// <summary>
        /// Advance a frame in the sprite sequence
        /// </summary>
        /// <param name="colorMatrix">The color transform to apply if the render
        /// method is "CustomHue"</param>
        internal void Advance(ColorMatrix colorMatrix)
        {
            // Get the image and advance to the next one
            if (!itr.MoveNext())
            {
                itr = spriteSequence.Bitmaps.GetEnumerator();
                return;
            }
            var img = itr.Current;
            if (null == img)
                return;
            // Display it, but first, colorize it if need be
            if ("RGBA" == spriteRenderMethod)
                pictureBox.Image = img;
            else
                pictureBox.Image = ApplyColorMatrix(img, colorMatrix);
        }

        /// <summary>
        /// Applies the color matrix to the image to color ize it
        /// </summary>
        /// <param name="sourceImage">The image to colorize</param>
        /// <param name="colorMatrix">The transform used to colorize it</param>
        /// <returns>The new image</returns>
        /// <remarks>From https://softwarebydefault.com/2013/03/03/colomatrix-image-filters/</remarks>
        static Bitmap ApplyColorMatrix(Image sourceImage, ColorMatrix colorMatrix)
        {
            Bitmap bmp32BppSource = GetArgbCopy(sourceImage);
            Bitmap bmp32BppDest = new Bitmap(bmp32BppSource.Width, bmp32BppSource.Height, PixelFormat.Format32bppArgb);

            using Graphics graphics = Graphics.FromImage(bmp32BppDest);
            ImageAttributes bmpAttributes = new ImageAttributes();
            bmpAttributes.SetColorMatrix(colorMatrix);

            graphics.DrawImage(bmp32BppSource, new Rectangle(0, 0, bmp32BppSource.Width, bmp32BppSource.Height),
                                0, 0, bmp32BppSource.Width, bmp32BppSource.Height, GraphicsUnit.Pixel, bmpAttributes);

            bmp32BppSource.Dispose();
            return bmp32BppDest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <returns></returns>
        /// <remarks>From https://softwarebydefault.com/2013/03/03/colomatrix-image-filters/</remarks>
        static Bitmap GetArgbCopy(Image sourceImage)
        {
            Bitmap bmpNew = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

            using Graphics graphics = Graphics.FromImage(bmpNew);
            graphics.DrawImage(sourceImage, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
            graphics.Flush();

            return bmpNew;
        }
    }
}
