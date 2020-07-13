using Anki.Resources.SDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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
namespace Example_5__Text_Substitutions
{
    public partial class TextSubstitutionForm : Form
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
        /// The do dad used to 
        /// </summary>
        TextSubstitution localizer;

        public TextSubstitutionForm()
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
            comboBox2.Items.Clear();
            // List the locales to the combo list
            comboBox1.Items.AddRange(assets.Locales.ToArray());
            // List the localization resources to the combo list
            comboBox2.Items.AddRange(assets.LocalizationFiles.ToArray());
            if (0 != comboBox1.Items.Count)
                comboBox1.SelectedIndex = 0;
            if (0 != comboBox2.Items.Count)
                comboBox2.SelectedIndex = 0;
        }


        /// <summary>
        /// This is used to select the language or the localization file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var locale       = (string) comboBox1.SelectedItem;
            var resourceName = (string) comboBox2.SelectedItem;
            comboBox3.Items.Clear();
            if (null == locale || null == resourceName)
            {
                return;
            }
            // Look up the localizer for this language/resource name
            localizer = assets.LocalizedTextSubstitution(resourceName, locale);
            // Fill in the list for later fun
            comboBox3.Items.AddRange(localizer.Keys.ToArray());
            comboBox3.SelectedIndex = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change the label
            var key = (string)comboBox3.SelectedItem;
            if (null == key)
                return;
            // Build a table of the different substitutions that can be employed
            // note: I'm not a fan of the keys having the braces.. it feels like
            // the braces should be stripped off
            var substitutions = new Dictionary<string, string>();
            substitutions["{0}"] = textBox1.Text;
            substitutions["{1}"] = textBox2.Text;
            substitutions["{2}"] = textBox3.Text;
            label1.Text = localizer.LocalizedText(key, substitutions);
        }
    }
}