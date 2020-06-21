// Copyright © 2020 Randall Maas. All rights reserved.
// See LICENSE file in the project root for full license information.  
using Anki.AudioKinetic;
using Anki.Resources.SDK;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Example_1___Playing_Sounds
{
    public partial class Sounds : Form
    {
        /// <summary>
        /// The audio player
        /// </summary>
        readonly WaveOut waveOut = new WaveOut();

        /// <summary>
        /// The wrapper around the resources manager
        /// </summary>
        Assets assets;
        /// <summary>
        /// A wrapper around the audio resources
        /// </summary>
        AudioAssets audioAssets;

        public Sounds()
        {
            InitializeComponent();
            // Dial the volume down
            waveOut.Volume = 0.5f;
            FormClosing += (s, e) => waveOut.Stop();
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
            audioAssets = assets.AudioAssets;
            // List the files
            // First list each of the sound banks
            foreach (var soundBankName in audioAssets.SoundBankNames)
            {
                // Get the sound bank
                var soundBank = audioAssets.SoundBank(soundBankName);
                // Get the sounds in the soundbank
                foreach (var fileInfo in soundBank.Sounds)
                {
                    // See if the name for the sound id is known
                    // (Spoiler: it usually isn't)
                    object id = AudioAssets.StringForID(fileInfo.ID);
                    if (null == id)
                        id = fileInfo.ID;
                    // Because the libary is still new, it reports things it
                    // thinks are WEM resources.. but are not.  Still working
                    // out how better to resolve that.  This next step screens
                    // out the bogus ones for now.
                    if (0 == fileInfo.Size && 0 == fileInfo.PrefetchSize) 
                        continue;
                    #if false
                    // Open the WEM stream for the id.. to check that it's real
                    var WEM = audioAssets.WEM(fileInfo.ID);
                    if (null == WEM) continue;
                    WEM.Dispose();
                    #endif
                    // And put it in the combo list
                    comboBox1.Items.Add(id);
                }
            }
            comboBox1.SelectedIndex=0;
        }

        /// <summary>
        /// When the play button is clicked, start playing teh sound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void play_Click(object sender, EventArgs e)
        {
            // Play sound
            var item = comboBox1.SelectedItem;
            // Look up the id for the selected sound.  For obvious reasons, we
            // try to display the text name, if we have one -- we usually don't --
            // so that needs to be converted back into an ID
            uint id;
            if (item is string name)
                id = AudioAssets.IDForString(name);
            else
                id = (uint)item;

            // Open the WEM stream for the id
            var WEM = audioAssets.WEM(id);
            if (null == WEM) return;
            // Stop whatever was playing
            waveOut.Stop();
            // Start the new stuff
            waveOut.Init(WEM.WaveProvider());
            waveOut.Play();
        }
    }
}