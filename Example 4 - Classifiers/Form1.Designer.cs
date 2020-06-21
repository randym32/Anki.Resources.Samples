using Emgu.CV.UI;

namespace Example_4___Classifier
{
    partial class ClassifierForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.imageBox1 = new ImageBox();
            this.imageBox2 = new ImageBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(159, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(629, 33);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.changeClassifier);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(13, 70);
            this.imageBox1.Name = "pictureBox1";
            this.imageBox1.Size = new System.Drawing.Size(360, 180);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.imageBox2.Location = new System.Drawing.Point(386, 70);
            this.imageBox2.Name = "pictureBox2";
            this.imageBox2.Size = new System.Drawing.Size(128, 128);
            this.imageBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox2.TabIndex = 2;
            this.imageBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(530, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "???";
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(530, 84);
            this.label2.Name = "label1";
            this.label2.Size = new System.Drawing.Size(120, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "";
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(530, 104);
            this.label3.Name = "label1";
            this.label3.Size = new System.Drawing.Size(120, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "";
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(530, 124);
            this.label4.Name = "label1";
            this.label4.Size = new System.Drawing.Size(120, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "";
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(530, 144);
            this.label5.Name = "label1";
            this.label5.Size = new System.Drawing.Size(120, 25);
            this.label5.TabIndex = 2;
            this.label5.Text = "";
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(530, 164);
            this.label6.Name = "label1";
            this.label6.Size = new System.Drawing.Size(120, 25);
            this.label6.TabIndex = 2;
            this.label6.Text = "";
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(530, 184);
            this.label7.Name = "label1";
            this.label7.Size = new System.Drawing.Size(120, 25);
            this.label7.TabIndex = 2;
            this.label7.Text = "";
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(530, 200);
            this.label8.Name = "label1";
            this.label8.Size = new System.Drawing.Size(120, 25);
            this.label8.TabIndex = 2;
            this.label8.Text = "";
            // 
            // Sprites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 266);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.imageBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.Name = "Sprites";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private ImageBox imageBox1;
        private ImageBox imageBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

