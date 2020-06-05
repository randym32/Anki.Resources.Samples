using Anki.AudioKinetic;
using Anki.Resources.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Example_1___Playing_Sounds
{
    public partial class Sprites : Form
    {
        /// <summary>
        /// The wrapper around the resources manager
        /// </summary>
        Assets assets;

        public Sprites()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the human presses the open button, show a file dialog to open
        /// the right thing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                return;

            // Open the assets
            // (You should put a lot more error checking here!)
            assets = new Assets(fbd.SelectedPath);
            comboBox1.Items.Clear();
            // List the sprite sequences
            foreach (var spriteSequenceName in assets.SpriteSequenceNames)
            {
                // And put it in the combo list
                comboBox1.Items.Add(spriteSequenceName);
            }
            comboBox1.SelectedIndex=0;
        }

        /// <summary>
        /// The sprite sequence to display
        /// </summary>
        SpriteSequence spriteSequence;
        /// <summary>
        /// The object used to enumerator over the sprites
        /// </summary>
        IEnumerator<Bitmap> itr;

        /// <summary>
        /// When the selection is clicked, start playing the sequence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void play_Click(object sender, EventArgs e)
        {
            // Play sprite sequence
            var spriteSequenceName = comboBox1.SelectedItem;
            // stop timer
            timer1.Stop();
            // Get the sprite sequence
            spriteSequence = assets.SpriteSequence((string)spriteSequenceName);
            itr = spriteSequence.Bitmaps.GetEnumerator();
            // start the timer
            timer1.Start();
        }

        /// <summary>
        /// This is called to update each frame.
        /// It gets the next image, and then colorizes it by applying a color matrix
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Get the image and advance to the next one
            if (!itr.MoveNext())
            {
                itr = spriteSequence.Bitmaps.GetEnumerator();
                return;
            }
            var img = itr.Current;
            // Display it, but first, colorize it
            if (null != img)
                pictureBox1.Image = ApplyColorMatrix(img, colorMatrix);
        }

        /// <summary>
        /// This is used to select a new color (to colorize the sprites)
        /// and update the color matrix which does the coloring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, System.EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Allow the user to select a custom color.
            MyDialog.AllowFullOpen = true;
            MyDialog.AnyColor = true;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = color;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() != DialogResult.OK)
                return;
            // Update the color matrix
            color = MyDialog.Color;
            colorMatrix = new ColorMatrix(new float[][]
                        {
                            new float[] { color.R / 255.0f, 0, 0, 0, 0 },
                            new float[] { 0, color.G / 255.0f, 0, 0, 0 },
                            new float[] { 0, 0, color.B / 255.0f, 0, 0 },
                            new float[] { 0, 0, 0, 1, 0 },
                            new float[] { 0, 0, 0, 0, 1 }
                        }
                        );
        }

        /// <summary>
        /// Tracks the prvious color selected
        /// </summary>
        Color color = Color.Green;

        /// <summary>
        /// A color matrix that will be used to give color to the image
        /// </summary>
        ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                        {
                            new float[]{0, 0, 0, 0, 0},
                            new float[]{0, 1, 0f, 0, 0},
                            new float[]{0f, 0f, 0f, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0, 0, 0, 1}
                        });

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


            using (Graphics graphics = Graphics.FromImage(bmp32BppDest))
            {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bmp32BppSource, new Rectangle(0, 0, bmp32BppSource.Width, bmp32BppSource.Height),
                                    0, 0, bmp32BppSource.Width, bmp32BppSource.Height, GraphicsUnit.Pixel, bmpAttributes);


            }


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

            using (Graphics graphics = Graphics.FromImage(bmpNew))
            {
                graphics.DrawImage(sourceImage, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }

            return bmpNew;
        }
    }
}