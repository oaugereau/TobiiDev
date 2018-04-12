using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using AForge.Video.FFMPEG;
using System.Windows.Forms;

namespace AnalazingEyeGaze
{
    public class Program
    {

        static List<Color> colors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Yellow, Color.Orange, Color.Cyan, Color.DarkBlue, Color.HotPink };

        static void Main(string[] args)
        {

            List<string> fileToProcess = new List<string>();

            //Process all file from a folder or open a single file
            Console.WriteLine("Process all the recordings from a folder (A) or a single file (S) ?");
            string process = Console.ReadLine().ToUpper();
            //Folder
            if (process == "A")
            {
                Console.WriteLine("Write the path of the folder");
                string path = Console.ReadLine();
                //Take all csv files
                string[] filenames = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories);
                //Take only files which first line is "Timestamp;GazeX;GazeY;LeftEye;RightEye"
                foreach (var item in filenames)
                {
                    if (File.ReadAllLines(item)[0].Contains("GazeX;GazeY"))
                        //if (File.ReadAllLines(item)[0] == "Timestamp;GazeX;GazeY;LeftEye;RightEye")
                        fileToProcess.Add(item.Replace(".csv", ""));
                }
            }
            //File
            else
            {
                Console.WriteLine("Write the name of the file to open (without extention)");
                fileToProcess.Add(Console.ReadLine());
            }

            foreach (var item in fileToProcess)
            {
                processOneFile(item);
            }
        }

        public static string processOneFile(string filenameWithoutExtention)
        {
            Console.WriteLine("Processing the file: " + filenameWithoutExtention);
            GazeData gd = new GazeData(filenameWithoutExtention);
            //writeImage(gd, gd.pngOriginal); //Display the raw gazes in a picture
            //writeVideo(gd, gd.aviOriginal); //Create a video of the gazes

            //Compute the fixations 
            gd.gazes = GazeData.fixationBusher2008(gd.gazes);
            writeText(gd, gd.csvFixations); //save the fixations in a file
            writeImage(gd, gd.pngFixations, false); //Display the fixations in a picture
                                                    //writeVideo(gd, gd.aviFixations);

            ////Line break detection
            ////gd.rowDataLineBreak(-40, 3, 2); //not usually used
            //gd.lines = GazeData.lineBreakDetectionSimple(ref gd.gazes, -500);
            ////gd.lineBreakDetectionOkoso(); //not usually used
            //writeText(gd, gd.csvFixations); //save the fixations in a file
            //writeImage(gd, gd.pngLineBreak, false);
            ////writeVideo(gd, gd.aviLineBreak);

            //Align gaze with lines
            //align(gd, "doc.layout");
            //writeImage(gd, gd.pngFixations);
            //writeVideo(gd, gd.aviFixations);

            return gd.csvFixations;
        }



        //public static void align(GazeData gd, string layout)
        //{
        //    //Extract the line position
        //    string[] lines = File.ReadAllLines(layout);
        //    float[] linesY = Array.ConvertAll(lines, float.Parse);
        //    //Don't work if two eye gaze are exactly at the same line
        //    float[] gazeY = gd.gazeY.Distinct().ToArray();
        //    //Match (DTW) ?
        //    List<KeyValuePair<int, int>> matchs = DTW(gazeY, linesY);
        //    for (int i = 0; i < gd.gazeY.Count; i++)
        //    {
        //        foreach (var item in matchs)
        //        {
        //            if (gd.gazeY[i] == gazeY[item.Key])
        //            {
        //                gd.gazeY[i] = linesY[item.Value];
        //                continue;
        //            }
        //        }
        //    }
        //}

        public static float[,] DTW(float[] s, float[] t)
        {
            float[,] match = new float[s.Length, t.Length];
            for (int i = 0; i < s.Length; i++)
            {
                match[i, 0] = float.MaxValue;
            }
            for (int i = 0; i < t.Length; i++)
            {
                match[0, i] = float.MaxValue;
            }
            match[0, 0] = 0;
            for (int i = 1; i < s.Length; i++)
            {
                for (int j = 1; j < t.Length; j++)
                {
                    float cost = Math.Abs(s[i] - t[j]);
                    //match[i, j] = cost + Math.Min(Math.Min(match[i - 1, j] * 2, match[i, j - 1] * 2), match[i - 1, j - 1]);
                    match[i, j] = cost + Math.Min(Math.Min(match[i - 1, j], match[i, j - 1]), match[i - 1, j - 1]);
                }
            }
            return match;
        }

        public static float[,] LocalDTW(float[] s, float[] t)
        {
            float[,] match = new float[s.Length, t.Length];
            for (int i = 0; i < s.Length; i++)
            {
                match[i, 0] = Math.Abs(s[i] - t[0]);
            }
            for (int i = 0; i < t.Length; i++)
            {
                match[0, i] = Math.Abs(s[0] - t[i]);
            }

            for (int i = 1; i < s.Length; i++)
            {
                for (int j = 1; j < t.Length; j++)
                {
                    float cost = Math.Abs(s[i] - t[j]);
                    //match[i, j] = cost + Math.Min(Math.Min(match[i - 1, j] * 2, match[i, j - 1] * 2), match[i - 1, j - 1]);
                    match[i, j] = cost + Math.Min(Math.Min(match[i - 1, j], match[i, j - 1]), match[i - 1, j - 1]);
                }
            }
            return match;
        }

        public static List<KeyValuePair<int, int>> Backtrack(float[,] match)
        {
            KeyValuePair<int, int> position = new KeyValuePair<int, int>(match.GetLength(0) - 1, match.GetLength(1) - 1);
            List<KeyValuePair<int, int>> matchs = new List<KeyValuePair<int, int>>();
            matchs.Add(position);
            while (position.Key != 0 && position.Value != 0)
            {
                KeyValuePair<int, int> position1 = position;
                KeyValuePair<int, int> position2 = position;
                KeyValuePair<int, int> position3 = position;
                if (position.Key - 1 >= 0)
                    position1 = new KeyValuePair<int, int>(position.Key - 1, position.Value);
                if (position.Value - 1 >= 0)
                    position2 = new KeyValuePair<int, int>(position.Key, position.Value - 1);
                if (position.Value - 1 >= 0 && position.Key - 1 >= 0)
                    position3 = new KeyValuePair<int, int>(position.Key - 1, position.Value - 1);

                if (match[position1.Key, position1.Value] < match[position2.Key, position2.Value])
                {
                    if (match[position1.Key, position1.Value] < match[position3.Key, position3.Value])
                        matchs.Add(position1);
                    else
                        matchs.Add(position3);
                }
                else
                {
                    if (match[position2.Key, position2.Value] < match[position3.Key, position3.Value])
                        matchs.Add(position2);
                    else
                        matchs.Add(position3);
                }
                position = matchs[matchs.Count - 1];
            }
            return matchs;
        }

        public static List<KeyValuePair<int, int>> DTWandBacktrack(float[] s, float[] t)
        {
            float[,] match = DTW(s, t);
            return Backtrack(match);
        }

        public static void writeImage(GazeData gd, string outputFile, bool useBackGround)
        {
            if (gd.width > 0 && gd.height > 0)
            {
                // quick hack because some people though it was a good idea to put tobii on a second screen but we have no idea what is the size of the first screen!!!
                if (gd.gazes[gd.gazes.Count - 1].gazeX > gd.width) 
                {
                    gd.width += Screen.PrimaryScreen.Bounds.Width;
                    if( Screen.PrimaryScreen.Bounds.Height > gd.height)
                            gd.height = Screen.PrimaryScreen.Bounds.Height;
                }
                Bitmap image = new Bitmap(gd.width, gd.height);
                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.White);
                SolidBrush brush = new SolidBrush(Color.Red);
                //Put the background
                if (gd.backgroundPath != "" && useBackGround)
                {
                    Image back = Image.FromFile(gd.backgroundPath);
                    g.DrawImage(back, (gd.width - back.Width) / 2, (gd.height - back.Height) / 2);
                }
                for (int i = 0; i < gd.gazes.Count; i++)
                {
                    //Use different color for different lines
                    if (gd.gazes[i].idLine == 0)
                    {
                        brush.Color = Color.Black;
                    }
                    else brush.Color = colors[gd.gazes[i].idLine % colors.Count];
                    g.FillEllipse(brush, new Rectangle((int)gd.gazes[i].gazeX - 5, (int)gd.gazes[i].gazeY - 5, 10, 10));
                }
                g.Save();
                image.Save(outputFile);
                Console.WriteLine("The image " + outputFile + " file has been written");
            }
            else
            {
                Console.WriteLine("The image cannot be written");
            }
        }
        public static void writeVideo(GazeData gd, string path)
        {
            Bitmap image = new Bitmap(gd.width, gd.height);
            Graphics g = Graphics.FromImage(image);
            SolidBrush brush = new SolidBrush(Color.Red);
            //Display the gaze in one video
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(path, gd.width, gd.height);
            //Bitmap image = new Bitmap(width, height);
            //Graphics g = Graphics.FromImage(image);
            //Brush brush = Brushes.Red;
            g.Clear(Color.Black);
            //Put the background
            if (gd.backgroundPath != "")
            {
                Image back = Image.FromFile(gd.backgroundPath);
                g.DrawImage(back, (gd.width - back.Width) / 2, (gd.height - back.Height) / 2);
            }
            Console.WriteLine("Writing the video, it can take some times...");
            // write the frames
            for (int i = 0; i < gd.gazes.Count; i++)
            {
                //Use different color for different lines
                if (gd.gazes[i].idLine == 0) brush.Color = Color.Black;
                else brush.Color = colors[gd.gazes[i].idLine % colors.Count];
                g.FillEllipse(brush, new Rectangle((int)gd.gazes[i].gazeX - 5, (int)gd.gazes[i].gazeY - 5, 10, 10));
                g.Save();
                writer.WriteVideoFrame(image, new TimeSpan(0, 0, 0, 0, (int)gd.gazes[i].timestamp));
            }
            writer.Close();
        }
        public static void writeText(GazeData gd, string outputFile)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            List<string> textLines = new List<string>();
            //header
            textLines.Add("timestamp;gazeX;gazeY;duration;idLine");
            //content
            foreach (var gaze in gd.gazes)
            {
                textLines.Add(gaze.timestamp + ";" + gaze.gazeX + ";" + gaze.gazeY + ";" + gaze.duration + ";" + gaze.idLine);
            }

            File.WriteAllLines(outputFile, textLines.ToArray());
        }
    }
}
