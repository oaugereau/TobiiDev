namespace Displaying
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxText = new System.Windows.Forms.PictureBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonOCR = new System.Windows.Forms.Button();
            this.labelRead = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxWordle = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanelTag = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWordle)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxText
            // 
            this.pictureBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxText.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxText.Location = new System.Drawing.Point(370, 8);
            this.pictureBoxText.Name = "pictureBoxText";
            this.pictureBoxText.Size = new System.Drawing.Size(964, 505);
            this.pictureBoxText.TabIndex = 2;
            this.pictureBoxText.TabStop = false;
            this.pictureBoxText.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(6, 33);
            this.buttonOpenFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(81, 26);
            this.buttonOpenFile.TabIndex = 4;
            this.buttonOpenFile.Text = "Open image";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(6, 598);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(81, 26);
            this.buttonExit.TabIndex = 5;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(6, 62);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(158, 48);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "Start EyeTracking";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(6, 113);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(158, 57);
            this.buttonStop.TabIndex = 8;
            this.buttonStop.Text = "Stop EyeTracking";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonOCR
            // 
            this.buttonOCR.Location = new System.Drawing.Point(97, 33);
            this.buttonOCR.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOCR.Name = "buttonOCR";
            this.buttonOCR.Size = new System.Drawing.Size(66, 26);
            this.buttonOCR.TabIndex = 9;
            this.buttonOCR.Text = "OCR";
            this.buttonOCR.UseVisualStyleBackColor = true;
            this.buttonOCR.Click += new System.EventHandler(this.buttonOCR_Click);
            // 
            // labelRead
            // 
            this.labelRead.AutoSize = true;
            this.labelRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRead.Location = new System.Drawing.Point(75, 192);
            this.labelRead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRead.Name = "labelRead";
            this.labelRead.Size = new System.Drawing.Size(64, 25);
            this.labelRead.TabIndex = 10;
            this.labelRead.Text = "____";
            this.labelRead.Paint += new System.Windows.Forms.PaintEventHandler(this.labelRead_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 197);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Read Word :";
            // 
            // pictureBoxWordle
            // 
            this.pictureBoxWordle.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxWordle.Location = new System.Drawing.Point(92, 597);
            this.pictureBoxWordle.Name = "pictureBoxWordle";
            this.pictureBoxWordle.Size = new System.Drawing.Size(284, 27);
            this.pictureBoxWordle.TabIndex = 12;
            this.pictureBoxWordle.TabStop = false;
            this.pictureBoxWordle.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxWordle_Paint);
            // 
            // flowLayoutPanelTag
            // 
            this.flowLayoutPanelTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelTag.Location = new System.Drawing.Point(6, 316);
            this.flowLayoutPanelTag.Name = "flowLayoutPanelTag";
            this.flowLayoutPanelTag.Size = new System.Drawing.Size(358, 263);
            this.flowLayoutPanelTag.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 300);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Tag cloud:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Tag cloud",
            "Speech Synthesizer "});
            this.comboBox1.Location = new System.Drawing.Point(12, 241);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 21);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 632);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanelTag);
            this.Controls.Add(this.pictureBoxWordle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelRead);
            this.Controls.Add(this.buttonOCR);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.pictureBoxText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWordle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxText;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonOCR;
        private System.Windows.Forms.Label labelRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxWordle;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

