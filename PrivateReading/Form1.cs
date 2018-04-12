using EyeXFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.EyeX.Framework;

namespace PrivateReading
{
    public partial class Form1 : Form
    {

        private EyeXHost eyeXHost = new EyeXHost();
        private FixationDataStream fixation;
        private Point eyeGazeAbsolute;

        public Form1()
        {
            eyeXHost.Start();
            fixation = eyeXHost.CreateFixationDataStream(FixationDataMode.Slow);
            InitializeComponent();
            pictureBox1.ImageLocation = @"..\a.png";
            pictureBox3.ImageLocation = @"..\c.png";
            pictureBox2.ImageLocation = @"..\b.png";
            fixation.Next += fixationEventHandler;
        }

        private void fixationEventHandler(object sender, FixationEventArgs e)
        {
            if (!double.IsNaN(e.X) && !double.IsNaN(e.Y))
            {
                eyeGazeAbsolute.X = (int)e.X;
                eyeGazeAbsolute.Y = (int)e.Y;
            }

            //this.pictureBox1.Invalidate();
            //this.pictureBox2.Invalidate();
            //this.pictureBox3.Invalidate();
            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //If the eye gaze is in the paragraph display the original text
            Rectangle rec = RectangleToScreen(pictureBox1.Bounds);
            rec.Inflate(50, 50);
            if (rec.Contains(eyeGazeAbsolute))
            {
                pictureBox1.ImageLocation = @"..\a.png";
            }
            //Else display a fake one
            else
            {
                pictureBox1.ImageLocation = @"..\d.png";
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            //If the eye gaze is in the paragraph display the original text
            Rectangle rec = RectangleToScreen(pictureBox2.Bounds);
            rec.Inflate(50, 50);
            if (rec.Contains(eyeGazeAbsolute))
            {
                pictureBox2.ImageLocation = @"..\b.png";
            }
            //Else display a fake one
            else
            {
                pictureBox2.ImageLocation = @"..\d.png";
            }
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            //If the eye gaze is in the paragraph display the original text
            Rectangle rec = RectangleToScreen(pictureBox3.Bounds);
            rec.Inflate(50, 50);
            if (rec.Contains(eyeGazeAbsolute))
            {
                pictureBox3.ImageLocation = @"..\c.png";
                
            }
            //Else display a fake one
            else
            {
                pictureBox3.ImageLocation = @"..\d.png";
            }
            this.pictureBox1.Refresh();
            this.pictureBox2.Refresh();
        }
    }
}
