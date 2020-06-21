// Copyright © 2020 Randall Maas. All rights reserved.
// See LICENSE file in the project root for full license information.  
using Anki.Resources.SDK;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.CvEnum;

/// <summary>
/// An example of how to classify items in the images.
/// 
/// This demo window shows:
///   * a live camera feed (from the computers default camera)
///   * The cropped feed, so that the result fits within the aspect ratio;
///   * The cropped is overlayed with indicators where it things "people" are
///     (when the dfp is used).  The grid orientation may be wrong.
///   
///   * The labels of what it sees are listed to the right.  These depend
///     on which classifier/detector is used.
/// 
/// In general, it does not recognize most things, and is very sensitive to
/// coloration and contrast.  For instance, it recognizes my black spatula,
/// but not my chrome-handled spatula, nor bamboo spatula.  (Mobilenet has a
/// lot of irrelevant things it recognizes, so I've no clue if it is used in
/// the pet detector/tracker.)
/// 
/// Some tips:
///  1. The hand recognizer needs to see all of the fingers and thumb, so you
///      will have to spread your fingers a bit, but keep them in the visual
///      frame.
///
///  2. The hand recognizer needs the hand to be against a dark surface --
///     or perhaps just a contrasting surface.
/// 
///  3. Objects often should be close as possible to the camera, but still in the frame
/// </summary>
namespace Example_4___Classifier
{
    public partial class ClassifierForm : Form
    {
        /// <summary>
        /// The labels on the form that are available to display classification information.
        /// </summary>
        Label[] textLabels;

        /// <summary>
        /// The wrapper around the resources manager
        /// </summary>
        Assets assets;

        /// <summary>
        /// The video camera capture tool
        /// </summary>
        VideoCapture capture;

        /// <summary>
        /// Each of the steps in image processing to go thru.
        /// </summary>
        List<IProcessImage> imageProcessingSteps;

        /// <summary>
        /// The categorization labels that are already displayed
        /// </summary>
        Dictionary<string, int> usedLabels = new Dictionary<string, int>();

        public ClassifierForm()
        {
            // 
            imageProcessingSteps = new List<IProcessImage>();
            InitializeComponent();
            capture = new VideoCapture(); //create a camera capture
            capture.SetCaptureProperty(CapProp.FrameWidth, 128);
            capture.SetCaptureProperty(CapProp.FrameHeight, 128);

            // The places on the display that we can store stuff
            textLabels = new Label[8] { label1, label2, label3, label4, label5, label6, label7, label8 };

            // Process video frames whenever we have time
            Application.Idle += GetAndProcessFrame;
        }


        /// <summary>
        /// Gets a video frame and processes it.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void GetAndProcessFrame(object Sender, EventArgs e)
        {
            // Get the next image from the camera
            var mat = capture.QueryFrame();
            if (null == mat)
                return;

            // It is not clear if the whole image frame should be applied to
            // the image processing stage, or if the image should be broken up
            // into fixed sized regions.  Or if the regions should overlap.
            // Or if it is intended to be applied to just the center of the
            // image.
            //
            // Experiments will need to be done.

            // First, get the subframe that matches what I think the mobilenet aspect ratio is (1:1)
            var nWidth = mat.Width - mat.Height;
            var tmpi = new Mat(mat, new Rectangle((int)(nWidth / 2), 0, mat.Height, mat.Height));
            var subImage = new Mat();
            CvInvoke.Resize(
                    src: tmpi,
                    dst: subImage,
                    dsize: tmpi.Size);

            // This holds the image in the various sizes and formats.
            // It seems likely that stages will reuse the sizing/format so this
            // reduces the work to set it up each time.
            using var ic = new ImageContainer(subImage);

            // This will hold the localization grid
            Classification[,] localizationGrid = null;

            // This will hold the set of labels that we wish to display
            var newLabels = new Dictionary<string, int>();

            // Process each of the select stages and make a note of teh labels
            foreach (var stage in imageProcessingSteps)
            {
                // Process the image to get the classifications or localizations
                var ret = stage.ProcessImage(ic);

                // What kind of processing stage is?
                if (stage.IsCategorization())
                {
                    // This is a label, add the items to the list of labels to
                    // update on the display
                    if (ret.Length > 1)
                        foreach (var cat in ret)
                            if (cat.Probability >= 0.05)
                                newLabels[cat.Label] = -1;
                            else if (ret[0, 0].Probability >= 0.5)
                                newLabels[ret[0, 0].Label] = -1;
                }
                else
                    localizationGrid = ret;
            }

            // Update the labels displayed
            // First, retain the index of the previous labels
            var Used = new bool[textLabels.Length];
            foreach (var l in usedLabels)
            {
                if (!newLabels.TryGetValue(l.Key, out var oldIdx)) continue;
                newLabels[l.Key] = l.Value;
                Used[l.Value] = true;
            }
            // Next, assign indices to the remaining labels.
            var tmp = new string[newLabels.Count];
            newLabels.Keys.CopyTo(tmp, 0);
            foreach (var l in tmp)
            {
                var lidx = newLabels[l];
                // Is it already assigned?
                if (lidx >= 0) continue;
                // find a free label
                for (var idx2 = 0; idx2 < textLabels.Length; idx2++)
                    if (!Used[idx2])
                    {
                        // Assign it this one
                        Used[idx2] = true;
                        newLabels[l] = idx2;
                        break;
                    }
            }

            // Now go thru and draw all of the labels on the screen
            foreach (var kv in newLabels)
            {
                if (kv.Value < 0) continue;
                textLabels[kv.Value].Text = kv.Key;
            }
            // Blank out the text for the unused labels
            for (int lidx = 0; lidx < Used.Length; lidx++)
                if (!Used[lidx]) textLabels[lidx].Text = "";
            usedLabels = newLabels;

            // Draw the localization rectangles
            if (null != localizationGrid)
                for (int x = 0; x < 6; x++)
                    for (int y = 0; y < 6; y++)
                    {
                        if (localizationGrid[y, x].Probability >= 0.25)
                            CvInvoke.Rectangle(subImage,
                                new Rectangle((x * subImage.Width) / 6, (y * subImage.Height) / 6, subImage.Width / 6, subImage.Height / 6),
                                new Bgr(0, 255 * localizationGrid[y, x].Probability, 0).MCvScalar, 10);
                    }

            // Draw the images obtained from the camera
            imageBox1.Image = mat;
            imageBox2.Image = ic.Image(new Size(128, 128), Emgu.CV.CvEnum.DepthType.Cv8U, false); 
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
            comboBox1.Items.Add("All of the image classifiers");
            // List the image classifiers
            foreach (var kv in assets.Classifiers)
            {
                // See if it is something that we can demonstrate
                // We screen out the extra OpenCV classifiers because (in the
                // case the reflective maze), the YAML file is missing a line
                // that would let it be read by openCV.  We screen out the
                // TFLite extras because the default parameters won't always
                // work well.. an exception is made for mobile net.. for fun
                if (("TFLite" == kv.Value.Type || "OpenCV" == kv.Value.Type) && 
                    (null != kv.Value.Parameters || "mobilenet_v1_0" == kv.Value.Name))
                    // And put it in the combo list
                    comboBox1.Items.Add(kv.Key);
            }
            comboBox1.SelectedIndex=0;
            // Add item for the all of them
        }

        /// <summary>
        /// When the selection is clicked, change the image processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeClassifier(object sender, EventArgs e)
        {
            // Check to see if referred to all of them
            if (0== comboBox1.SelectedIndex)
            {
                // Add all of the items to the processing list
                var tmp = new List<IProcessImage>() ;
                var classifiers = comboBox1.Items;
                for (var idx = 1; idx < classifiers.Count;idx++)
                {
                    assets.Classifiers.TryGetValue((string)classifiers[idx], out var classy);
                    tmp.Add(classy.CreateClassifier());
                }
                // Make it the new processing list in town
                imageProcessingSteps = tmp;
            }
            else
            {
                // Create a classifier for just that name
                var classifierName = comboBox1.SelectedItem;
                assets.Classifiers.TryGetValue((string) classifierName, out var classy);
                imageProcessingSteps = new List<IProcessImage>() { classy.CreateClassifier() };
            }
        }
    }
}