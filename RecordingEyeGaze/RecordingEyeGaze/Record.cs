using EyeXFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.EyeX.Framework;

namespace RecordingEyeGaze
{


    public class Record
    {
        private static int screenShotFrequency = 30; //take a screenshot every 30 gazes
        private string filenameWithoutExtention;
        //This lists contain the lines that will be written to the output files.
        private List<string> linesGaze = new List<string>();
        private List<string> linesEyePosition = new List<string>();
        private GazePointDataStream lightlyFilteredGazeDataStream;
        private EyePositionDataStream eyePosStream;
        private bool recording = false;
        private GazePointDataMode gpdm = GazePointDataMode.LightlyFiltered;
        private EyeXHost eyeXHost = new EyeXHost();
        private bool filtered = true;
        private bool takeScreenshot = false;

        public void clearTheRecord()
        {
            linesGaze.Clear();
            linesEyePosition.Clear();
        }

        public void saveFiles(int recordingSpeed = 0)
        {
            string eyePosFilename = filenameWithoutExtention + "_eyePos.csv";
            string paramFilename = filenameWithoutExtention + "_param.txt";
            string screenshotFolder = filenameWithoutExtention + "_screenshots";
            string gazeFilename = filenameWithoutExtention + ".csv";

            //Write the parameters
            File.WriteAllText(paramFilename, "Screen Bounds in pixels: " + eyeXHost.ScreenBounds + "\nDisplay Size in millimeters " + eyeXHost.DisplaySize + "\n");
            File.AppendAllText(paramFilename, "Eye tracking device status: " + eyeXHost.EyeTrackingDeviceStatus + "\n");
            File.AppendAllText(paramFilename, "User presence: " + eyeXHost.UserPresence + "\n");
            File.AppendAllText(paramFilename, "User profile name: " + eyeXHost.UserProfileName + "\n");
            File.AppendAllText(paramFilename, "Gaze Point Data Mode: " + gpdm.ToString() + "\n");
            if (recordingSpeed != 0)
            {
                File.AppendAllText(paramFilename, "Recording speed: " + recordingSpeed);
            }
            else
            {
                File.AppendAllText(paramFilename, "Recording speed: UNDEFINED");
            }

            //Write the headers
            File.WriteAllText(gazeFilename, "WindowsTime;Timestamp;GazeX;GazeY\n");
            File.WriteAllText(eyePosFilename, "WindowsTime;Timestamp;LeftEye;X;Y;Z;RightEye;X;Y;Z\n");
            //Write the files
            File.AppendAllLines(gazeFilename, linesGaze);
            File.AppendAllLines(eyePosFilename, linesEyePosition);

            //make a screenshot and save it
            //screenshot(screenshotFolder + "/01.png");
        }

        public void screenshot(string savePath)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(savePath, ImageFormat.Png);
        }

        public Record(bool filteered, string filenameWithoutExtention, bool takeScreenshot)
        {
            this.takeScreenshot = takeScreenshot;
            this.filenameWithoutExtention = filenameWithoutExtention;
            //Directory.CreateDirectory(filenameWithoutExtention + "_screenshots");
            eyeXHost.Start();
            if (filtered)
            {
                lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);
                gpdm = GazePointDataMode.LightlyFiltered;
            }
            else
            {
                gpdm = GazePointDataMode.Unfiltered;
                lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.Unfiltered);
            }
            eyePosStream = eyeXHost.CreateEyePositionDataStream();

        }

        public void recordLoop()
        {
            string input = Console.ReadLine();
            if (input == "q")
                return;
            else
            {
                if (recording == false)
                {
                    startRecord();
                    recording = true;
                }
                else
                {
                    pauseRecord();
                    recording = false;
                }
                recordLoop();
            }
        }

        public void startRecord()
        {
            lightlyFilteredGazeDataStream.Next += gazeEvent;
            eyePosStream.Next += eyePosEvent;
        }

        public void pauseRecord()
        {
            lightlyFilteredGazeDataStream.Next -= gazeEvent;
            eyePosStream.Next -= eyePosEvent;
        }

        private void gazeEvent(object sender, GazePointEventArgs e)
        {
            // Write the data to the console.
            Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) @{2:0}", e.X, e.Y, e.Timestamp);
            linesGaze.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.FFF") + ";" + e.Timestamp + ";" + e.X + ";" + e.Y);

            //IF screenshot mode is ON 
            if (takeScreenshot)
            {
                screenShotFrequency--;
                if (screenShotFrequency == 0)
                {
                    screenshot(filenameWithoutExtention + "_screenshots/" + e.Timestamp + ".png");
                    screenShotFrequency = 30;
                }
            }

        }

        private void eyePosEvent(object sender, EyePositionEventArgs e)
        {
            //Console.WriteLine(e.Timestamp + ";" + e.LeftEye.IsValid + ";" + e.RightEye.IsValid);
            linesEyePosition.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.FFF") + ";" + e.Timestamp + ";" + e.LeftEye.IsValid + ";" + e.LeftEye.X + ";" + e.LeftEye.Y + ";" + e.LeftEye.Z + ";" +
                e.RightEye.IsValid + ";" + e.RightEye.X + ";" + e.RightEye.Y + ";" + e.RightEye.Z);
        }

        internal void Dispose()
        {
            eyePosStream.Dispose();
            lightlyFilteredGazeDataStream.Dispose();
            eyeXHost.Dispose();
        }
    }
}
