using Anki.Resources.SDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

/// <summary>
/// An example of how to play the composite images -- the image layers and
/// image maps to sprite sequences.  Not all do anything.  Most of the ones
/// are in the "Weather" groupings.
/// 
/// Note: these often expect to layer sprite sequences on top of the eyes,
/// which are are rendered by the procedure face module (which isn't emulated
/// here...)
/// </summary>
namespace Example_3___Composite_Image
{
    public partial class Sprites : Form
    {
        /// <summary>
        /// A scaling factor to make it easier to see the image detail
        /// </summary>
        const float zoom = 2.0f;

        /// <summary>
        /// The wrapper around the resources manager
        /// </summary>
        Assets assets;

        /// <summary>
        /// The drawable sprite boxes to update on the display
        /// </summary>
        IList<SpriteControl> spriteBoxes;

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
            // List the composite images (the layouts are the starting point)
            foreach (var imageLayoutName in assets.ImageLayoutTriggerNames)
            {
                // And put it in the combo list
                comboBox1.Items.Add(imageLayoutName);
            }
            comboBox1.SelectedIndex=0;
        }

        /// <summary>
        /// When the selection is clicked, start playing the sequence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void play_Click(object sender, EventArgs e)
        {
            // Play sprite sequence
            var imageLayoutName = comboBox1.SelectedItem;

            // stop timer
            timer1.Stop();
            // Get the composite image
            var compositeImage = assets.CompositeImageForTrigger((string)imageLayoutName);
            var list = new List<SpriteControl>();
            spriteBoxes = list;
            // replace the contents
            panel1.Controls.Clear();
            panel1.BackColor= Color.Transparent;
            // Create the layers
            foreach (var layerName in compositeImage.LayerNames)
            {
                // Get the image name mapping
                var imageMap = (Dictionary<string,string>)compositeImage.ImageMap(layerName);
                var panel = new Panel();
                panel.Size = panel1.Size;
                panel1.Controls.Add(panel);
                panel.BackColor= Color.Transparent;

                // Create the layout for this layer
                foreach (var spriteBox in compositeImage.Layout(layerName))
                {
                    var pictureBox      = new PictureBox();
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Width    = (int)(spriteBox.width * zoom);
                    pictureBox.Height   = (int)(spriteBox.height* zoom);
                    pictureBox.Location = new Point((int)(spriteBox.x* zoom), (int)(spriteBox.y* zoom));
                    pictureBox.BackColor= Color.Transparent;
                    // Add the sprite box to... the layers panel
                    panel.Controls.Add(pictureBox);
                    
                    // Look up the sprite sequence used to drive this sprite box
                    if (null != imageMap && imageMap.TryGetValue(spriteBox.spriteBoxName, out var spriteSequenceName))
                    {
                        // note name and method
                        var a = new SpriteControl(pictureBox, assets.SpriteSequence(spriteSequenceName), spriteBox.spriteRenderMethod);
                        list.Add(a);
                    }
                }
            }
            // start the timer
            timer1.Start();
        }

        /// <summary>
        /// This is called to update each frame of each of the layers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Iterate over each of the sprite boxes,
            // updating their sprite sequences
            foreach (var item in spriteBoxes)
                item.Advance(colorMatrix);
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


    }
}