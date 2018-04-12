using EyeXFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.EyeX.Framework;
using Tesseract;
using System.IO;
using System.Speech.Synthesis;

namespace Displaying
{
    enum Mode
    {
        TagCLoud, SpeechSynthesize
    }
    public partial class Form1 : Form
    {
        private EyeXHost eyeXHost = new EyeXHost();
        private FixationDataStream fixation;
        private Bitmap bmp;
        private Brush brush = Brushes.Red;
        private int relativeX;
        private int relativeY;
        private int absoluteX;
        private int absoluteY;
        private int previousX;
        private int previousY;
        private List<Word> words = new List<Word>();
        private Word readWord = new Word("____", Rectangle.Empty);
        private Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
        private SpeechSynthesizer s = new SpeechSynthesizer();
        private Mode mode = Mode.TagCLoud;
        private string previousWord = "";


        public Form1()
        {
            eyeXHost.Start();
            fixation = eyeXHost.CreateFixationDataStream(FixationDataMode.Slow);
            InitializeComponent();
            comboBox1.SelectedItem = comboBox1.Items[0];
            s.SelectVoice("Microsoft Zira Desktop");
            //bmp = new Bitmap(500, 500);
            //using (Graphics g = Graphics.FromImage(bmp))
            //{
            //    g.Clear(Color.White);
            //}
        }

        private void fixationEventHandler(object sender, FixationEventArgs e)
        {
            if (!double.IsNaN(e.X) && !double.IsNaN(e.Y))
            {
                absoluteX = (int)e.X;
                absoluteY = (int)e.Y;

                //Find the closest word
                int squaredDist = 0;
                readWord = closestWord(relativeX, relativeY, out squaredDist);
                int thresholdMinDistSquare = 80 * 80;
                if (squaredDist < thresholdMinDistSquare)
                {
                    //If mode == tag cloud
                    if (mode == Mode.TagCLoud)
                    {
                        if (wordFrequency.ContainsKey(readWord.Text))
                        {
                            wordFrequency[readWord.Text]++;
                        }
                        else
                        {
                            wordFrequency.Add(readWord.Text, 1);
                        }
                        //sort the dictionary
                        //wordFrequency = wordFrequency.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                        //Invalidate the picturebox for calling pictureBox1_Paint again
                        this.pictureBoxWordle.Invalidate();
                    }
                    //Else if mode == Speech Synthesize
                    else
                    {
                        //Read the word only if the previous fixation is near the current one
                        if (Math.Abs(previousX - absoluteX) < 5 && Math.Abs(previousY - absoluteY) < 5 && readWord.Text != previousWord)
                        {
                            s.Speak(readWord.Text);
                            previousWord = readWord.Text;
                        }
                        previousX = absoluteX;
                        previousY = absoluteY;
                    }
                }
                else readWord = null;
                this.labelRead.Invalidate();
                this.pictureBoxText.Invalidate();
            }
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (bmp != null)
            {
                using (Bitmap tempBmp = new Bitmap(bmp))
                {
                    using (Graphics g = Graphics.FromImage(tempBmp))
                    {
                        relativeX = absoluteX - PointToScreen(pictureBoxText.Location).X;
                        relativeY = absoluteY - PointToScreen(pictureBoxText.Location).Y;
                        //enlever la position absolue de l'image pour afficher la vrai position
                        g.FillEllipse(brush, new Rectangle(relativeX - 3, relativeY - 3, 7, 7));
                    }
                    e.Graphics.DrawImage(tempBmp, 0, 0);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select a picture";
            ofd.DefaultExt = "png";
            ofd.Filter = "PNG FIles (*.png)|*.png|All (*.*)|*.*";
            ofd.ShowDialog();
            if (!File.Exists(ofd.FileName))
                return;
            bmp = new Bitmap(ofd.FileName);
            this.pictureBoxText.Invalidate();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            fixation.Next += fixationEventHandler;
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {
            fixation.Next -= fixationEventHandler;
        }

        private void buttonOCR_Click(object sender, EventArgs e)
        {
            TesseractEngine te = new TesseractEngine("./", "eng", EngineMode.TesseractAndCube);
            Page page = te.Process(bmp);
            string texte = page.GetHOCRText(0);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (var iter = page.GetIterator())
                {
                    do
                    {
                        Rect symbolBounds;
                        if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out symbolBounds))
                        {
                            string wordText = iter.GetText(PageIteratorLevel.Word);
                            Rectangle rectangle = symbolBounds.getRectangle();
                            g.DrawRectangle(new Pen(Color.Blue), rectangle);
                            words.Add(new Word(wordText, rectangle));
                        }
                    } while (iter.Next(PageIteratorLevel.Word));
                }
            }

        }

        private Word closestWord(int X, int Y, out int squaredDist)
        {
            Word closest = words[0];
            squaredDist = int.MaxValue;
            foreach (Word w in words)
            {
                int dist = (w.CenterX - X) * (w.CenterX - X) + (w.CenterY - Y) * (w.CenterY - Y);
                if (dist < squaredDist)
                {
                    closest = w;
                    squaredDist = dist;
                }
            }
            return closest;
        }

        private void labelRead_Paint(object sender, PaintEventArgs e)
        {
            if (readWord != null)
                this.labelRead.Text = readWord.Text;
            else
                this.labelRead.Text = "";
        }

        private void pictureBoxWordle_Paint(object sender, PaintEventArgs e)
        {
            //Refresh the Wordle
            //Take the 10 most frequent words.
            //int sumOccurancy = 0;
            //for (int i = 0; i < 10; i++)
            //{
            //    sumOccurancy += wordFrequency.ElementAt(i).Value;
            //}
            //Print them proportionaly to they occurancy.
            if (wordFrequency.Count < 1) return;
            Console.WriteLine(wordFrequency.Count);
            flowLayoutPanelTag.Controls.Clear();
            Bitmap wordle = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            SolidBrush myBrush = new SolidBrush(Color.Red);
            //Take the range of the frequency
            int max = wordFrequency.Values.Max();
            using (Graphics g = Graphics.FromImage(wordle))
            {
                for (int i = 0; i < wordFrequency.Count; i++)
                {
                    int val = wordFrequency.ElementAt(i).Value;
                    int size = 26;
                    //Create 5 different size
                    if (val < max / 4) size = 8;
                    else if (val < max / 2) size = 12;
                    else if (val < 3 * max / 4) size = 20;
                    Font f = new System.Drawing.Font(FontFamily.GenericSansSerif, size);
                    //g.DrawString(wordFrequency.ElementAt(i).Key, f, myBrush, 10, i * 100 + 20);
                    Label l = new Label();
                    l.Text = wordFrequency.ElementAt(i).Key;
                    l.Font = f;
                    l.AutoSize = true;
                    flowLayoutPanelTag.Controls.Add(l);
                }
            }
            e.Graphics.DrawImage(wordle, 0, 0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem == comboBox1.Items[0])
            {
                mode = Mode.TagCLoud;
            }
            else
            {
                mode = Mode.SpeechSynthesize;
            }
        }
    }

    public static class MyExt
    {
        public static Rectangle getRectangle(this Rect rect)
        {
            return new Rectangle(rect.X1, rect.Y1, rect.Width, rect.Height);
        }
    }
}
