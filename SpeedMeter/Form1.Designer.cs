namespace SpeedMeter
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxTexte = new System.Windows.Forms.PictureBox();
            this.textBoxSpeed = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelRecording = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxElapsedTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxStimuli = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonEstimateSpeed = new System.Windows.Forms.Button();
            this.buttonEstimateSpeedFolder = new System.Windows.Forms.Button();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxGaze = new System.Windows.Forms.PictureBox();
            this.labelEstWords = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelEstSpeed = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelPageWords = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelReading = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonJins = new System.Windows.Forms.Button();
            this.textBoxJINS = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGaze)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = " Open document";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxTexte
            // 
            this.pictureBoxTexte.Location = new System.Drawing.Point(185, 3);
            this.pictureBoxTexte.Name = "pictureBoxTexte";
            this.pictureBoxTexte.Size = new System.Drawing.Size(250, 367);
            this.pictureBoxTexte.TabIndex = 1;
            this.pictureBoxTexte.TabStop = false;
            this.pictureBoxTexte.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            this.pictureBoxTexte.Click += new System.EventHandler(this.pictureBoxTexte_Click);
            // 
            // textBoxSpeed
            // 
            this.textBoxSpeed.Location = new System.Drawing.Point(12, 462);
            this.textBoxSpeed.Name = "textBoxSpeed";
            this.textBoxSpeed.Size = new System.Drawing.Size(48, 20);
            this.textBoxSpeed.TabIndex = 2;
            this.textBoxSpeed.Text = "218";
            this.textBoxSpeed.Visible = false;
            this.textBoxSpeed.TextChanged += new System.EventHandler(this.textBoxSpeed_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 446);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Speed";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 463);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "words/min";
            this.label2.Visible = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(13, 80);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(76, 39);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Start/Stop reading";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelRecording
            // 
            this.labelRecording.AutoSize = true;
            this.labelRecording.Location = new System.Drawing.Point(14, 129);
            this.labelRecording.Name = "labelRecording";
            this.labelRecording.Size = new System.Drawing.Size(71, 13);
            this.labelRecording.TabIndex = 6;
            this.labelRecording.Text = "Not recording";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(11, 567);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(111, 50);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save the recording";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxElapsedTime
            // 
            this.textBoxElapsedTime.Enabled = false;
            this.textBoxElapsedTime.Location = new System.Drawing.Point(9, 336);
            this.textBoxElapsedTime.Name = "textBoxElapsedTime";
            this.textBoxElapsedTime.Size = new System.Drawing.Size(109, 20);
            this.textBoxElapsedTime.TabIndex = 8;
            this.textBoxElapsedTime.Text = "0";
            this.textBoxElapsedTime.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Time recorded (ms):";
            this.label3.Visible = false;
            // 
            // comboBoxStimuli
            // 
            this.comboBoxStimuli.FormattingEnabled = true;
            this.comboBoxStimuli.Items.AddRange(new object[] {
            "Underlying word",
            "Underlying line",
            "No Stimuli"});
            this.comboBoxStimuli.Location = new System.Drawing.Point(11, 501);
            this.comboBoxStimuli.Name = "comboBoxStimuli";
            this.comboBoxStimuli.Size = new System.Drawing.Size(110, 21);
            this.comboBoxStimuli.TabIndex = 10;
            this.comboBoxStimuli.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 485);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Stimuli";
            this.label4.Visible = false;
            // 
            // buttonEstimateSpeed
            // 
            this.buttonEstimateSpeed.Location = new System.Drawing.Point(11, 623);
            this.buttonEstimateSpeed.Name = "buttonEstimateSpeed";
            this.buttonEstimateSpeed.Size = new System.Drawing.Size(59, 55);
            this.buttonEstimateSpeed.TabIndex = 12;
            this.buttonEstimateSpeed.Text = "Estimate Speed from file";
            this.buttonEstimateSpeed.UseVisualStyleBackColor = true;
            this.buttonEstimateSpeed.Visible = false;
            this.buttonEstimateSpeed.Click += new System.EventHandler(this.buttonEstimateSpeed_Click);
            // 
            // buttonEstimateSpeedFolder
            // 
            this.buttonEstimateSpeedFolder.Location = new System.Drawing.Point(76, 623);
            this.buttonEstimateSpeedFolder.Name = "buttonEstimateSpeedFolder";
            this.buttonEstimateSpeedFolder.Size = new System.Drawing.Size(58, 55);
            this.buttonEstimateSpeedFolder.TabIndex = 13;
            this.buttonEstimateSpeedFolder.Text = "Estimate Speed - folder";
            this.buttonEstimateSpeedFolder.UseVisualStyleBackColor = true;
            this.buttonEstimateSpeedFolder.Visible = false;
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.Location = new System.Drawing.Point(11, 541);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.Size = new System.Drawing.Size(110, 20);
            this.textBoxScenario.TabIndex = 14;
            this.textBoxScenario.Text = "2,3,5";
            this.textBoxScenario.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 525);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Scenario";
            this.label5.Visible = false;
            // 
            // pictureBoxGaze
            // 
            this.pictureBoxGaze.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBoxGaze.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxGaze.Location = new System.Drawing.Point(540, 30);
            this.pictureBoxGaze.Name = "pictureBoxGaze";
            this.pictureBoxGaze.Size = new System.Drawing.Size(266, 325);
            this.pictureBoxGaze.TabIndex = 20;
            this.pictureBoxGaze.TabStop = false;
            this.pictureBoxGaze.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxGaze_Paint);
            // 
            // labelEstWords
            // 
            this.labelEstWords.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelEstWords.AutoSize = true;
            this.labelEstWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEstWords.Location = new System.Drawing.Point(574, 358);
            this.labelEstWords.Name = "labelEstWords";
            this.labelEstWords.Size = new System.Drawing.Size(42, 46);
            this.labelEstWords.TabIndex = 21;
            this.labelEstWords.Text = "0";
            this.labelEstWords.Paint += new System.Windows.Forms.PaintEventHandler(this.labelEstWords_Paint);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(706, 369);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 31);
            this.label8.TabIndex = 22;
            this.label8.Text = "words";
            // 
            // labelEstSpeed
            // 
            this.labelEstSpeed.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelEstSpeed.AutoSize = true;
            this.labelEstSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEstSpeed.Location = new System.Drawing.Point(574, 404);
            this.labelEstSpeed.Name = "labelEstSpeed";
            this.labelEstSpeed.Size = new System.Drawing.Size(42, 46);
            this.labelEstSpeed.TabIndex = 23;
            this.labelEstSpeed.Text = "0";
            this.labelEstSpeed.Paint += new System.Windows.Forms.PaintEventHandler(this.labelEstSpeed_Paint);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(706, 415);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 31);
            this.label9.TabIndex = 24;
            this.label9.Text = "wpm";
            // 
            // labelPageWords
            // 
            this.labelPageWords.AutoSize = true;
            this.labelPageWords.Location = new System.Drawing.Point(10, 59);
            this.labelPageWords.Name = "labelPageWords";
            this.labelPageWords.Size = new System.Drawing.Size(75, 13);
            this.labelPageWords.TabIndex = 25;
            this.labelPageWords.Text = "Page: 0 words";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.MediumBlue;
            this.label10.Location = new System.Drawing.Point(579, 482);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 17);
            this.label10.TabIndex = 26;
            this.label10.Text = "200 wmp: average reader";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Green;
            this.label11.Location = new System.Drawing.Point(579, 511);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 17);
            this.label11.TabIndex = 27;
            this.label11.Text = "300 wmp: good reader";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkRed;
            this.label12.Location = new System.Drawing.Point(579, 450);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(145, 17);
            this.label12.TabIndex = 28;
            this.label12.Text = "100 wmp: slow reader";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(138, 742);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(249, 186);
            this.richTextBox1.TabIndex = 29;
            this.richTextBox1.Text = "";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(138, 726);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Tag Cloud";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(644, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Eye gaze";
            // 
            // labelReading
            // 
            this.labelReading.AutoSize = true;
            this.labelReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReading.Location = new System.Drawing.Point(13, 151);
            this.labelReading.Name = "labelReading";
            this.labelReading.Size = new System.Drawing.Size(76, 15);
            this.labelReading.TabIndex = 32;
            this.labelReading.Text = "Not Reading";
            this.labelReading.Paint += new System.Windows.Forms.PaintEventHandler(this.labelReading_Paint);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(22, 391);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 43);
            this.buttonSend.TabIndex = 34;
            this.buttonSend.Text = "Send to server";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Visible = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonJins
            // 
            this.buttonJins.Location = new System.Drawing.Point(12, 184);
            this.buttonJins.Name = "buttonJins";
            this.buttonJins.Size = new System.Drawing.Size(75, 45);
            this.buttonJins.TabIndex = 35;
            this.buttonJins.Text = "Start/stop JINS";
            this.buttonJins.UseVisualStyleBackColor = true;
            this.buttonJins.Click += new System.EventHandler(this.buttonJins_Click);
            // 
            // textBoxJINS
            // 
            this.textBoxJINS.Location = new System.Drawing.Point(9, 235);
            this.textBoxJINS.Multiline = true;
            this.textBoxJINS.Name = "textBoxJINS";
            this.textBoxJINS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxJINS.Size = new System.Drawing.Size(147, 53);
            this.textBoxJINS.TabIndex = 36;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(876, 583);
            this.Controls.Add(this.textBoxJINS);
            this.Controls.Add(this.buttonJins);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.labelReading);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelPageWords);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelEstSpeed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelEstWords);
            this.Controls.Add(this.pictureBoxGaze);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxScenario);
            this.Controls.Add(this.buttonEstimateSpeedFolder);
            this.Controls.Add(this.buttonEstimateSpeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxStimuli);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxElapsedTime);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelRecording);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSpeed);
            this.Controls.Add(this.pictureBoxTexte);
            this.Controls.Add(this.button1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(50, 50);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(75, 0, 0, 0);
            this.Text = "Eyetracking Wordometer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGaze)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxTexte;
        private System.Windows.Forms.TextBox textBoxSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelRecording;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxElapsedTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxStimuli;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonEstimateSpeed;
        private System.Windows.Forms.Button buttonEstimateSpeedFolder;
        private System.Windows.Forms.TextBox textBoxScenario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxGaze;
        private System.Windows.Forms.Label labelEstWords;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelEstSpeed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelPageWords;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelReading;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonJins;
        private System.Windows.Forms.TextBox textBoxJINS;
    }
}

