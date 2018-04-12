using RecordingEyeGaze;
using AnalazingEyeGaze;
using Retrieval;
using TagCloud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EyeXFramework;
using Tobii.EyeX.Framework;
using System.Net;
using System.Web.Script.Serialization;
using JinsTest;


namespace SpeedMeter
{
    public partial class Form1 : Form
    {
        //Reading score for reading/not reading estimation
        public int readingScore = 0;
        //System is recoding or not recording
        private static bool recording = false;
        //The layout loaded by the user
        private static PcGts page;
        //Speed defined in the textbox
        private static double speed;
        //pictureBoxText
        private static Graphics graphicsText;
        private static Pen pen1 = new System.Drawing.Pen(Color.Gray, 2F);
        private static Timer timer = new Timer();
        //The index of the curent underlying line 
        private int lineIdx = 0; //line index
        //The index of the current underlying word
        private int wordIdx = 0;
        //The index of the current underlying pixel (in abscissa)
        private float pixelIdx = 0;
        //For underlying continuously, pixels are print SIZE by SIZE (1 by one is not possible, it takes too many time) 
        private const float SIZE = 6;
        //Elapsed time of the recording
        private Stopwatch sw = new Stopwatch();
        private DateTime timeStart;
        private DateTime timeStop;
        //private Record rec;
        private int[] scenario;
        private EyeXHost eyeXHost = new EyeXHost();
        private double posX;
        private double posY;
        private WordsEstimationOnline wordEstimation;
        private GazePointDataStream lightlyFilteredGazeDataStream;
        private Bitmap bmpGaze;


        public Form1()
        {
            //rec = new Record(true);
            InitializeComponent();
            comboBoxStimuli.SelectedItem = comboBoxStimuli.Items[0];
            speed = 60000f / int.Parse(this.textBoxSpeed.Text);
            timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            eyeXHost.Start();
            bmpGaze = new Bitmap(pictureBoxGaze.Width, pictureBoxGaze.Height);
            comboBoxStimuli.SelectedItem = comboBoxStimuli.Items[2];
            this.pictureBoxTexte.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public void defineScenario()
        {
            if (textBoxScenario.Text == "")
            {
                int nbLines = page.Lines.Count;
                scenario = Enumerable.Range(0, nbLines).ToArray();
            }
            else
            {
                string[] textScenario = textBoxScenario.Text.Split(',');
                //The line 1 is index 0 => int-1
                scenario = Array.ConvertAll(textScenario, s => (int.Parse(s) - 1));
            }
        }

        //This function is automatically called every interval of time after timer.Start()  and until timer.Stop()
        private void timer_Tick(object sender, EventArgs e)
        {
            //Word per word underlying
            if (comboBoxStimuli.SelectedItem == comboBoxStimuli.Items[0])
            {
                wordUnderlying();
            }
            else if (comboBoxStimuli.SelectedItem == comboBoxStimuli.Items[1])
            {
                lineUnderlying();
            }
            else
            {
                //do nothing
            }
            //display the elapsed time
            textBoxElapsedTime.Text = sw.Elapsed.TotalMilliseconds.ToString();
        }

        private void lineUnderlying()
        {
            TextLine tl = page.Lines[scenario[lineIdx]];
            float y = tl.Coords.Rectangle.Y + tl.Coords.Rectangle.Height + 1;
            PointF p1 = new PointF(pixelIdx, y);
            PointF p2 = new PointF(pixelIdx + SIZE + 1, y);
            graphicsText.DrawLine(pen1, p1, p2);
            //After printing the last pixel of the line
            if (pixelIdx >= tl.Coords.Rectangle.Right - SIZE)
            {
                Console.WriteLine(timer.Interval);
                //If it is the last line of the scenario: end of the document
                if (lineIdx == scenario.Length - 1)
                {
                    timer.Stop();
                }
                //Else go to next line
                else
                {
                    lineIdx++;
                    tl = page.Lines[scenario[lineIdx]];
                    setTimerPixelLine(tl);
                    pixelIdx = tl.Coords.Rectangle.Left;
                }
            }
            else
            {
                pixelIdx += SIZE + 1;
            }
        }

        //Set the timer for printing two pixels for a given line 
        private void setTimerPixelLine(TextLine tl)
        {
            int lengthLine = tl.LineLength;
            int nbWords = tl.Words.Count;
            double timeOneLine = speed * nbWords;
            //the pixel are printed SIZE by SIZE
            double timePixels = SIZE * timeOneLine / lengthLine;
            timer.Interval = (int)Math.Floor(timePixels);
        }

        private void wordUnderlying()
        {
            TextLine tl = page.Lines[scenario[lineIdx]];
            int y = tl.Coords.Rectangle.Y + tl.Coords.Rectangle.Height + 1;
            Word word = tl.Words[wordIdx];
            //g.DrawLine(pen1, new Point(item.Coords.Rectangle.X, y), new Point(item.Coords.Rectangle.X + item.Coords.Rectangle.Width, y));
            Point p1 = new Point(word.Coords.Rectangle.X, y);
            Point p2 = new Point(word.Coords.Rectangle.X + word.Coords.Rectangle.Width, y);
            graphicsText.DrawLine(pen1, p1, p2);

            if (wordIdx == tl.Words.Count - 1)
            {
                //If last line of the scenario: end of the document
                if (lineIdx == scenario.Length - 1)
                {
                    timer.Stop();
                }
                //Go to next line
                else
                {
                    wordIdx = 0;
                    lineIdx++;
                }
            }
            else
            {
                wordIdx++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select a PAGE file (xml)";
            ofd.DefaultExt = "xml";
            ofd.Filter = "xml FIles (*.xml)|*.xml|All (*.*)|*.*";
            ofd.ShowDialog();
            string file = ofd.FileName;
            if (file == null || !File.Exists(file)) return;
            //deserialize the xml
            page = Retrieval.Program.loadLayout(file);
            labelPageWords.Text = "Page: " + page.Words.Count + " words";
            string imagePath = Path.Combine(Path.GetDirectoryName(file), page.Page.ImageFilename);
            if (File.Exists(imagePath))
            {
                this.pictureBoxTexte.ImageLocation = imagePath;
                this.pictureBoxTexte.Image = new Bitmap(this.pictureBoxTexte.ImageLocation);
                graphicsText = this.pictureBoxTexte.CreateGraphics();
            }

            else
            {
                Console.WriteLine("The picture is not in the same directory as the xml");
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int x = this.PointToScreen(new Point(0, 0)).X;
            int screenWidth = SystemInformation.PrimaryMonitorSize.Width;
            if (page == null) return;
            defineScenario();
            //Start recording and displaying
            if (!recording)
            {
                richTextBox1.Clear();
                this.pictureBoxTexte.Image = new Bitmap(this.pictureBoxTexte.ImageLocation);
                bmpGaze = new Bitmap(pictureBoxGaze.Width, pictureBoxGaze.Height);
                //Read line by line
                if (comboBoxStimuli.SelectedItem == comboBoxStimuli.Items[1])
                {
                    pixelIdx = page.Lines[scenario[0]].Coords.Rectangle.Left;
                    setTimerPixelLine(page.Lines[scenario[0]]);
                }
                //Read word by word
                else if (comboBoxStimuli.SelectedItem == comboBoxStimuli.Items[0])
                {
                    timer.Interval = (int)Math.Round(speed); ;
                }
                lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
                lightlyFilteredGazeDataStream.Next += gazeEvent;
                wordEstimation = new WordsEstimationOnline(page.averageWordWidth, page.averageSpaceLength);
                //Start to measure the elapsed time
                sw.Start();
                labelRecording.Text = "Recording...";
                recording = true;
                timer.Start();
                timeStart = DateTime.Now;
            }

            //Stop recording
            else
            {
                stopRecording();
                lightlyFilteredGazeDataStream.Next -= gazeEvent;

                //estimate nb of words and speed of the recording
                //save temporary the data for the analysis
                //rec.saveFiles("temp\\save");
                //GazeData gd = new GazeData("temp\\save");
                //gd.fixationBusher2008();
                //gd.lineBreakDetectionSimple();
                //float estimatedWordsBasedOnLineLength = gd.TotalLinesLength / (page.averageWordWidth + page.averageSpaceLength);
                ////I had one word per line in the estimation because the fixation are found at the beggining of the words
                //estimatedWordsBasedOnLineLength += gd.lines.Count;
                //textBoxNbWords.Text = estimatedWordsBasedOnLineLength.ToString();
                ////The estimated speed is just the nb of estimated words / elapsed time
                //int speed = (int)(60000 * estimatedWordsBasedOnLineLength / double.Parse(textBoxElapsedTime.Text));
                //textBoxEstimatedSpeed.Text = speed.ToString();

                //Display the gaze and fixations
                displayGazeOnPicture(wordEstimation.gazes, Brushes.Red);
                displayGazeOnPicture(wordEstimation.fixations, Brushes.Blue);

                //display the tag cloud
                List<string> readWords = TagCloud.Program.findClosestWords(translateGaze(wordEstimation.fixations), page);
                Dictionary<string, int> wordCloud = new Dictionary<string, int>();
                foreach (var item in readWords)
                {
                    if (wordCloud.ContainsKey(item))
                        wordCloud[item]++;
                    else
                        wordCloud.Add(item, 1);
                }

                foreach (var item in wordCloud)
                {
                    richTextBox1.SelectionFont = new Font("Tahoma", item.Value * 5 + 10);
                    richTextBox1.AppendText(item.Key + " ");
                }

                //Not reading
                readingScore = 0;
                labelReading.Invalidate(); // = "Not Reading";

                timeStop = DateTime.Now;
            }
        }

        private void sendRecordToServer()
        {
            if (wordEstimation == null)
            {
                //TODO: Error
                return;
            }
            List<int> wordsPerMinute = computeWordsPerMinute(wordEstimation.fixations); //Not implemented yet
            string wordCount = string.Join(",", wordsPerMinute); //Not implemented yet
            var w = wordEstimation.nbWords;
            var startTime = timeStart.ToString("yyyy-MM-dd HH:mm:ss");
            var endTime = timeStop.ToString("yyyy-MM-dd HH:mm:ss");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://rll-test.herokuapp.com");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    user = "shoya",
                    start_time = startTime,
                    end_time = endTime,
                    //word_counts = "[" + wordCount + "]" //Not implemented yet
                    word_counts  = "["+ w.ToString() +"]" //I send just one value because my recordings take less than one minute
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        //TODO Not implemented yet
        private List<int> computeWordsPerMinute(List<Gaze> gaze)
        {
            List<int> wordsPerMinute = new List<int>();
            return wordsPerMinute;
        }

        private List<Gaze> translateGaze(List<Gaze> gazes)
        {
            List<Gaze> translated = new List<Gaze>();
            int pictPosX = PointToScreen(pictureBoxTexte.Location).X;
            int pictPosY = PointToScreen(pictureBoxTexte.Location).Y;
            foreach (Gaze item in gazes)
            {
                Gaze g = new Gaze();
                g = item;
                g.gazeX = item.gazeX - pictPosX;
                g.gazeY = item.gazeY - pictPosY;
                translated.Add(g);
            }
            return translated;
        }

        private void displayGazeOnPicture(List<Gaze> gazes, Brush brushColor)
        {
            int pictPosX = PointToScreen(pictureBoxTexte.Location).X;
            int pictPosY = PointToScreen(pictureBoxTexte.Location).Y;
            foreach (Gaze item in gazes)
            {
                graphicsText.FillEllipse(brushColor, (int)item.gazeX - pictPosX, (int)item.gazeY - pictPosY, 7, 7);
            }
            graphicsText.Flush();
        }

        private void gazeEvent(object sender, GazePointEventArgs e)
        {
            posX = e.X;
            posY = e.Y;
            //Display the point in a the pictureboxGaze
            pictureBoxGaze.Invalidate();
            //Update the estimation of number of read words.
            wordEstimation.update((float)posX, (float)posY, (float)e.Timestamp);
            //Reading / Not Reading estimation
            readingEstimation();
            labelEstWords.Invalidate();
            labelEstSpeed.Invalidate();
            labelReading.Invalidate();
            //Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) @{2:0} - {3}", e.X, e.Y, e.Timestamp);
        }

        private void readingEstimation()
        {
            int count = wordEstimation.fixations.Count;
            if (count < 3) return;
            //If the 3 last fixations are aligned 
            var A = wordEstimation.fixations[count - 1];
            var B = wordEstimation.fixations[count - 2];
            var C = wordEstimation.fixations[count - 3];
            if (A.gazeX > B.gazeX && B.gazeX > C.gazeX && Math.Abs(A.gazeY - B.gazeY) < 50 && Math.Abs(B.gazeY - C.gazeY) < 50)
                readingScore = 20;
            else
                readingScore--;
            Console.WriteLine("Reading score =  " + readingScore);
        }

        private void stopRecording()
        {
            //rec.pauseRecord();
            labelRecording.Text = "Stop recording.";
            recording = false;
            sw.Stop();
            textBoxElapsedTime.Text = sw.Elapsed.TotalMilliseconds.ToString();
            sw.Reset();
            timer.Stop();
            wordIdx = 0;
            lineIdx = 0;
        }

        private void textBoxSpeed_TextChanged(object sender, EventArgs e)
        {
            speed = 60000.0 / float.Parse(this.textBoxSpeed.Text);
            timer.Interval = (int)Math.Round(speed);
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            graphicsText = this.pictureBoxTexte.CreateGraphics();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //Open a window for saving the file
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save the Tobii file (csv)";
            sfd.DefaultExt = "csv";
            sfd.Filter = "csv FIles (*.csv)|*.csv|All (*.*)|*.*";
            sfd.ShowDialog();
            string file = sfd.FileName;
            if (file == null) return;
            //Wait few ms. The saveFiles function take a screenshot and it sometimes take a screenshot with the SaveFileDialog still opened
            System.Threading.Thread.Sleep(200);
            //rec.saveFiles(Path.ChangeExtension(file, null), int.Parse(textBoxSpeed.Text));
        }

        private void buttonEstimateSpeed_Click(object sender, EventArgs e)
        {
            if (page == null)
            {
                var result = MessageBox.Show("You must load a layout first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select a Tobii CSV file (csv)";
            ofd.DefaultExt = "csv";
            ofd.Filter = "csv FIles (*.csv)|*.csv|All (*.*)|*.*";
            ofd.ShowDialog();
            string file = ofd.FileName;
            if (file == null || !File.Exists(file)) return;
            SpeedEstimator se = new SpeedEstimator(file, page);

            List<double> speedLine = se.speedPerLine();
            speedLine.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("average (from line): " + speedLine.Average());

            //List<float> speedFix = se.speedPerFixation();
            //Console.WriteLine("average (from fixations): " + speedFix.Average());

            //display the gaze
            displayGazeOnPicture(se.gd.gazes, Brushes.Red);
            labelEstWords.Text = se.estimatedNumberOfReadWords().ToString();
            labelEstSpeed.Text = (int)(speedLine.Average()) + "";
        }

        private void pictureBoxGaze_Paint(object sender, PaintEventArgs e)
        {
            if (posX > 0 && posY > 0)
            {
                //scale the position to the window
                double height = eyeXHost.ScreenBounds.Value.Height;
                double width = eyeXHost.ScreenBounds.Value.Width;
                Graphics.FromImage(bmpGaze).FillEllipse(Brushes.Red, (int)(posX * pictureBoxGaze.Width / width), (int)(posY * pictureBoxGaze.Height / height), 7, 7);
                e.Graphics.DrawImage(bmpGaze, 0, 0);
            }
        }

        private void labelEstWords_Paint(object sender, PaintEventArgs e)
        {
            if (wordEstimation != null)
                labelEstWords.Text = wordEstimation.nbWords.ToString();
        }

        private void labelEstSpeed_Paint(object sender, PaintEventArgs e)
        {
            if (wordEstimation != null)
            {
                int speed = (int)(60000 * wordEstimation.nbWords / double.Parse(textBoxElapsedTime.Text));
                //int speed = (int)(60000 * wordEstimation.nbWords / sw.Elapsed.TotalMilliseconds);
                labelEstSpeed.Text = speed.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void labelReading_Paint(object sender, PaintEventArgs e)
        {
            if (readingScore <= 0)
            {
                labelReading.Text = "Not Reading";
            }
            else
            {
                labelReading.Text = "Reading";
            }
            //Console.WriteLine(labelReading.Text);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            sendRecordToServer();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxTexte_Click(object sender, EventArgs e)
        {

        }

        private void buttonJins_Click(object sender, EventArgs e)
        {
            Console.SetOut(new ControlWriter(textBoxJINS));
            JinsTest.Program.detect();
            JinsTest.Program.connect();
        }
    }
}
