using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AnalazingEyeGaze
{
    public class GazeData : ICloneable
    {
        private double totalLinesLength = -1;
        public string paramFilename;
        public string filename;
        public string aviOriginal;
        public string pngOriginal;
        public string pngLineBreak;
        public string aviLineBreak;
        public string pngMedian;
        public string backgroundPath;
        public string pngFixations;
        public string aviFixations;
        public string csvFixations;
        public int width;
        public int height;
        public List<Gaze> gazes = new List<Gaze>();
        public List<GazeLine> lines = new List<GazeLine>();
        public int groundTruthID = -1;

        public double TotalLinesLength
        {
            get
            {
                if (totalLinesLength == -1 && lines != null)
                {
                    totalLinesLength = 0;
                    foreach (var item in lines)
                    {
                        totalLinesLength += item.Length;
                    }
                }
                return totalLinesLength;
            }
        }

        public GazeData()
        {

        }

        public GazeData(string nameWithoutExtention, int groundTruthID)
        {
            this.groundTruthID = groundTruthID;
            initialize(nameWithoutExtention);
        }

        private void initialize(string nameWithoutExtention)
        {
            paramFilename = nameWithoutExtention + "_param.txt";
            filename = nameWithoutExtention + ".csv";
            aviOriginal = nameWithoutExtention + ".avi";
            pngOriginal = nameWithoutExtention + "_gaze.png";
            pngFixations = nameWithoutExtention + "_fixations.png";
            aviFixations = nameWithoutExtention + "_fixations.avi";
            pngLineBreak = nameWithoutExtention + "_line.png";
            aviLineBreak = nameWithoutExtention + "_line.avi";
            pngMedian = nameWithoutExtention + "_median.png";
            backgroundPath = nameWithoutExtention + "_back.png";
            csvFixations = nameWithoutExtention + "_fix.csv";

            //Read the data
            if (!File.Exists(filename))
            {
                //Second possibility because of SMi files
                filename = filename.Replace(".csv", ".txt");
                if (!File.Exists(filename))
                    throw new Exception("Error, the file " + filename + " cannot be found. Press a key to exit the program...");
            }

            List<string> linesTxt = File.ReadAllLines(filename).ToList();

            //Inspect the first line, if "Timestamp;GazeX;GazeY;LeftEye;RightEye" it is a Tobii raw data
            if (linesTxt[0] == "Timestamp;GazeX;GazeY;LeftEye;RightEye" || linesTxt[0] == "TimeStamp;GazeX;GazeY" || linesTxt[0] == "TimeStamp;GazeX;GazeY;TimeStampWindows")
            {
                readTobii(linesTxt);
            }

            else if (linesTxt[0] == "WindowsTime;Timestamp;GazeX;GazeY")
            {
                readTobiiNew(linesTxt);
            }

            //Else if it is "## [BeGaze]" it is SMi
            else if (linesTxt[0] == "## [BeGaze]")
            {
                readSMiRaw(linesTxt);
            }

            //Else if the last line is "finish", it is a Fixation file made by the student from SMi
            else if (linesTxt[linesTxt.Count - 1] == "finish")
            {
                int i = 0;
                double timeZero = double.Parse(linesTxt[0].Split('\t')[0].Replace(',', '.'), new CultureInfo("en-US"));
                while (linesTxt[i] != "blink")
                {
                    string[] cols = linesTxt[i].Split('\t');
                    Gaze g = new Gaze();
                    g.timestamp = (float)(double.Parse(cols[0].Replace(',', '.'), new CultureInfo("en-US")) - timeZero);
                    g.gazeX = float.Parse(cols[2].Replace(',', '.'), new CultureInfo("en-US"));
                    g.gazeY = float.Parse(cols[3].Replace(',', '.'), new CultureInfo("en-US"));
                    gazes.Add(g);
                    i++;
                }
            }

            //a Tobii raw data made by Shoya
            else if (linesTxt[0] == "# timestamp,x,y")
            {
                gazes = readTobiiCsv(linesTxt, ',');
            }

            //Else exeption
            else
            {
                throw new Exception("Unknown type of input");
            }
        }

        private void readTobiiNew(List<string> linesTxt)
        {
            gazes.AddRange(readTobiiCsvNew(linesTxt));

            //Reading the size of the screen (parameter file)
            if (File.Exists(paramFilename))
            {
                string[] parameters = File.ReadAllLines(paramFilename);
                string screenSize = parameters[0].Split(',')[2];
                width = int.Parse(screenSize.Split('x')[0]);
                height = int.Parse(screenSize.Split('x')[1]);
            }
            //else Console.WriteLine("Warning, the file " + paramFilename + " does not exist.");
        }

        private IEnumerable<Gaze> readTobiiCsvNew(List<string> linesTxt, char split = ';')
        {
            List<Gaze> gazes = new List<Gaze>();
            if (linesTxt.Count < 2) return gazes;
            //Remove the header
            linesTxt.RemoveAt(0);
            //Convert decimal marker in numbers if the format it is a dot or a comma 00.000 (USA / Japan) or 00,000 (Europe)
            double timeZero = double.Parse(linesTxt[0].Split(split)[1].Replace(',', '.'), new CultureInfo("en-US"));

            foreach (string line in linesTxt)
            {
                string[] cols = line.Split(split);
                Gaze g = new Gaze();
                g.windowTime = DateTime.ParseExact(cols[0], "MM/dd/yyyy HH:mm:ss.FFF", null);
                g.timestamp = (float)(double.Parse(cols[1].Replace(',', '.'), new CultureInfo("en-US")) - timeZero);
                g.gazeX = float.Parse(cols[2].Replace(',', '.'), new CultureInfo("en-US"));
                g.gazeY = float.Parse(cols[3].Replace(',', '.'), new CultureInfo("en-US"));
                gazes.Add(g);
            }
            return gazes;
        }

        private void readSMiRaw(List<string> linesTxt)
        {
            //Remove the non-data lines
            string curentLine = linesTxt[0];
            int indexGazeX = 0;
            int indexGazeY = 0;
            while (curentLine.StartsWith("##") || curentLine.StartsWith("Time"))
            {
                //Save the saze of the screen
                if (curentLine.StartsWith("## Calibration Area:	"))
                {
                    width = int.Parse(curentLine.Split('\t')[1].Replace(',', '.'), new CultureInfo("en-US"));
                    height = int.Parse(curentLine.Split('\t')[2].Replace(',', '.'), new CultureInfo("en-US"));
                }
                //Save the index of the collomn that contain the position of the gaze, because it change depending on the files!
                else if (curentLine.StartsWith("Time"))
                {
                    string[] cols = curentLine.Split('\t');
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] == "R POR X [px]")
                        {
                            indexGazeX = i;
                        }
                        else if (cols[i] == "R POR Y [px]")
                        {
                            indexGazeY = i;
                        }
                    }
                }
                linesTxt.RemoveAt(0);
                curentLine = linesTxt[0];
            }

            double timeZero = double.Parse(linesTxt[0].Split('\t')[0].Replace(',', '.'), new CultureInfo("en-US"));

            foreach (var line in linesTxt)
            {
                string[] cols = line.Split('\t');
                if (indexGazeX < cols.Length && indexGazeY < cols.Length)
                {
                    Gaze g = new Gaze();
                    g.timestamp = (float)(double.Parse(cols[0].Replace(',', '.'), new CultureInfo("en-US")) - timeZero);
                    g.gazeX = float.Parse(cols[indexGazeX].Replace(',', '.'), new CultureInfo("en-US"));
                    g.gazeY = float.Parse(cols[indexGazeY].Replace(',', '.'), new CultureInfo("en-US"));
                    gazes.Add(g);
                }
            }
        }

        public static List<Gaze> readTobiiCsv(string path)
        {
            List<string> linesTxt = File.ReadAllLines(path).ToList();
            return readTobiiCsv(linesTxt);
        }

        public static List<Gaze> readTobiiCsv(List<string> linesTxt, char split = ';')
        {
            List<Gaze> gazes = new List<Gaze>();
            if (linesTxt.Count < 2) return gazes;
            //Remove the header
            linesTxt.RemoveAt(0);
            //Convert decimal marker in numbers if the format it is a dot or a comma 00.000 (USA / Japan) or 00,000 (Europe)
            double timeZero = double.Parse(linesTxt[0].Split(split)[0].Replace(',', '.'), new CultureInfo("en-US"));

            foreach (string line in linesTxt)
            {
                string[] cols = line.Split(split);
                Gaze g = new Gaze();
                g.timestamp = (float)(double.Parse(cols[0].Replace(',', '.'), new CultureInfo("en-US")) - timeZero);
                g.gazeX = float.Parse(cols[1].Replace(',', '.'), new CultureInfo("en-US"));
                g.gazeY = float.Parse(cols[2].Replace(',', '.'), new CultureInfo("en-US"));
                gazes.Add(g);
            }
            return gazes;
        }

        private void readTobii(List<string> linesTxt)
        {
            gazes.AddRange(readTobiiCsv(linesTxt));

            //Reading the size of the screen (parameter file)
            if (File.Exists(paramFilename))
            {
                string[] parameters = File.ReadAllLines(paramFilename);
                string screenSize = parameters[0].Split(',')[2];
                width = int.Parse(screenSize.Split('x')[0]);
                height = int.Parse(screenSize.Split('x')[1]);
            }
            if(File.Exists(backgroundPath))
            {
                Bitmap b = new Bitmap(backgroundPath);
                width = b.Width;
                height = b.Height;
                b.Dispose();
            }
            //else Console.WriteLine("Warning, the file " + paramFilename + " does not exist.");
        }

        public GazeData(string name)
        {
            string extension = Path.GetExtension(name);
            if (extension != "") name = name.Replace(extension, "");
            initialize(name);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Line break detection for row output of Tobii (with lightly filtered option)
        /// </summary>
        public void rowTobiiLineBreak(int distThreshold = -40, int windowSize = 13, int smallCluster = 15)
        {
            //Line break is done only if enough eye gaze points are available
            if (gazes.Count < windowSize) return;

            int half = (windowSize - 1) / 2;
            //Affect ID=0 for line break dots
            int idLine = 1;
            for (int i = 0; i < gazes.Count; i++)
            {
                double distX = 0;
                double distY = 0;
                for (int j = i - half; j <= i + half; j++)
                {
                    if (j > 0 && j + 1 < gazes.Count)
                    {
                        distX += gazes[j + 1].gazeX - gazes[j].gazeX;
                        distY += gazes[j + 1].gazeY - gazes[j].gazeY;
                    }
                }
                //Sum up the consecutive vectors
                //A standard reading should be approximatively horizontal => > Xthreshold and < Y threshold
                if (distX < distThreshold)
                {
                    idLine = 0;
                }
                else idLine = 1;
                gazes[i].idLine = idLine;
            }


            //assign different clusters (1 cluster -> 1 line)
            idLine = 1;
            List<Gaze> g = new List<Gaze>();
            g.Add(gazes.First());
            lines.Add(new GazeLine(g));

            for (int i = 1; i < gazes.Count; i++)
            {
                //If line break
                if (gazes[i].idLine == 0) continue;
                if (gazes[i - 1].idLine == 0)
                {
                    idLine++;
                    List<Gaze> newLine = new List<Gaze>();
                    lines.Add(new GazeLine(newLine));
                }
                gazes[i].idLine = idLine;
                lines[idLine - 1].gazes.Add(gazes[i]);
            }

            //filter small clusters
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].gazes.Count < smallCluster)
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }

            //remove all the gaze from the vector
            gazes.Clear();
            for (int i = 0; i < lines.Count; i++)
            {
                //merge back the list into gd
                gazes.AddRange(lines[i].gazes);
            }
        }

        /// <summary>
        /// Simple line break detection. The fixation detection should be used first.
        /// </summary>
        /// <param name="threshold">If the regression is bigger than this value, a new line is detected</param>
        public static List<GazeLine> lineBreakDetectionSimple(ref List<Gaze> gazes, int threshold = -120)
        {
            //List<GazeLine> lines = new List<GazeLine>();
            List<GazeLine> lines = new List<GazeLine>();
            if (gazes.Count < 1) return lines;
            int idLine = 1;
            gazes.First().idLine = idLine;
            List<Gaze> line = new List<Gaze>();
            line.Add(gazes.First());

            //New
            //Deleting a fixation between two regressions
            for (int i = 1; i < gazes.Count-1; i++)
            {
                if (gazes[i].gazeX - gazes[i - 1].gazeX < 0 && gazes[i + 1].gazeX - gazes[i].gazeX < 0)
                {
                    gazes.RemoveAt(i);
                    i--;
                }
            }

                //If the next gaze is horizontaly far we create a new line
                for (int i = 1; i < gazes.Count; i++)
            {
                if (gazes[i].gazeX - gazes[i - 1].gazeX < threshold)
                {
                    lines.Add(new GazeLine(line));
                    line = new List<Gaze>();
                    idLine++;
                }
                gazes[i].idLine = idLine;
                line.Add(gazes[i]);
            }

            //Add the last line when the process is over
            lines.Add(new GazeLine(line));

            //Filter the "line" with one dot. It is backward or noise
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].gazes.Count <= 1)
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }

            //Update the gazes
            gazes = new List<Gaze>();
            for (int i = 0; i < lines.Count; i++)
            {
                foreach (var item in lines[i].gazes)
                {
                    item.idLine = i + 1;
                    gazes.Add(item);
                }
            }

            return lines;
        }

        /// <summary>
        /// WebGazeAnalyze: A system for capturing and analyzing web reading behavior using eye gaze
        /// </summary>
        public void lineBreakBeymer2005()
        {
            //line regression
        }

        //public void lineBreakDetectionOkoso()
        //{
        //    int idLine = 1;
        //    gazes[0].idLine = idLine;
        //    gazes[1].idLine = idLine;
        //    lines.Clear();
        //    List<Gaze> line = new List<Gaze>();
        //    line.Add(gazes[0]);
        //    line.Add(gazes[1]);

        //    double a, b, c, d, radian, deg = 0;
        //    string[] status = new string[gazes.Count];
        //    for (int i = 2; i < gazes.Count; i++)
        //    {
        //        a = Math.Sqrt((gazes[i].gazeX - gazes[i - 2].gazeX) * (gazes[i].gazeX - gazes[i - 2].gazeX) + (gazes[i].gazeY - gazes[i - 2].gazeY) * (gazes[i].gazeY - gazes[i - 2].gazeY));
        //        b = Math.Sqrt((gazes[i - 1].gazeX - gazes[i - 2].gazeX) * (gazes[i - 1].gazeX - gazes[i - 2].gazeX) + (gazes[i - 1].gazeY - gazes[i - 2].gazeY) * (gazes[i - 1].gazeY - gazes[i - 2].gazeY));
        //        c = Math.Sqrt((gazes[i].gazeX - gazes[i - 1].gazeX) * (gazes[i].gazeX - gazes[i - 1].gazeX) + (gazes[i].gazeY - gazes[i - 1].gazeY) * (gazes[i].gazeY - gazes[i - 1].gazeY));
        //        //d = cos(A), the opposite angle of segment a
        //        d = (b * b + c * c - a * a) / (2 * b * c);
        //        if ((a + b <= c) || (a + c <= b) || (b + c <= a))
        //        {
        //            //do nothing 
        //            deg = 0;
        //        }
        //        else
        //        {
        //            radian = Math.Acos(d);
        //            deg = radian * 57.29577;
        //        }

        //        if (Math.Abs(gazes[i].gazeY - gazes[i - 1].gazeY) > 50 && Math.Abs(gazes[i].gazeY - gazes[i - 1].gazeY) * 2 > Math.Abs(gazes[i].gazeX - gazes[i - 1].gazeX))
        //        {
        //            status[i - 1] = "jump";
        //            lines.Add(new GazeLine(line));
        //            line = new List<Gaze>();
        //            idLine++;
        //        }
        //        else if (deg < 90 && Math.Abs(gazes[i].gazeY - gazes[i - 1].gazeY) > 70)
        //        {
        //            status[i - 1] = "jump";
        //            lines.Add(new GazeLine(line));
        //            line = new List<Gaze>();
        //            idLine++;
        //        }
        //        else if ((deg < 45) && (gazes[i].gazeY - gazes[i - 1].gazeY > 5) && (gazes[i - 1].gazeX - gazes[i].gazeX > 200))
        //        {
        //            status[i - 1] = "linebreak";
        //            lines.Add(new GazeLine(line));
        //            line = new List<Gaze>();
        //            idLine++;
        //        }
        //        else if ((deg < 45) && (gazes[i - 1].gazeX - gazes[i].gazeX > 10) && (gazes[i - 1].gazeX - gazes[i].gazeX < 200))
        //        {
        //            status[i - 1] = "reread";
        //        }
        //        else
        //        {
        //            status[i - 1] = "undefined";
        //        }

        //        gazes[i].idLine = idLine;
        //        line.Add(gazes[i]);
        //    }

        //    //Don't forget to add the last line when the process is over!
        //    lines.Add(new GazeLine(line));

        //    //Filter the "line" with one dot. It is backward or noise
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        if (lines[i].gazes.Count <= 1)
        //        {
        //            lines.RemoveAt(i);
        //            i--;
        //        }
        //    }

        //    //Up
        //    the gazes
        //    gazes.Clear();
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        foreach (var item in lines[i].gazes)
        //        {
        //            item.idLine = i + 1;
        //            gazes.Add(item);
        //        }
        //    }

        //}

        public void medianLines()
        {
            for (int i = 0; i < lines.Count; i++)
            {
                List<double> allGazeY = new List<double>();
                foreach (var item in lines[i].gazes)
                {
                    allGazeY.Add(item.gazeY);
                }

                double median = allGazeY[(allGazeY.Count + 1) / 2];
                for (int j = 0; j < lines[i].gazes.Count; j++)
                {
                    lines[i].gazes[j].gazeY = median;
                }
            }
        }


        /// <summary>
        /// I-VT algorithm from "Identifying fixations and saccades in eye-tracking protocols", Salvucci, 2000
        /// </summary>
        /// <param name="maxSpeedFixation">If the speed is inferior to maxSpeedFixation, it is considered as a fixation</param>
        public void fixationVelocity(int maxSpeedFixation = 40)
        {
            //With Tobii EyeX, the distance and velocity are similar because the data is recorded at a regular time - every x ms)
            //Compute the velocity
            for (int i = 1; i < gazes.Count; i++)
            {
                gazes[i].squaredVelocity = (gazes[i - 1].gazeX - gazes[i].gazeX) * (gazes[i - 1].gazeX - gazes[i].gazeX) + (gazes[i - 1].gazeY - gazes[i].gazeY) * (gazes[i - 1].gazeY - gazes[i].gazeY);
            }
            List<Gaze> fixations = new List<Gaze>();
            for (int i = 0; i < gazes.Count; i++)
            {
                List<Gaze> microFixations = new List<Gaze>();
                int j = 0;
                while (i + j < gazes.Count && gazes[i + j].squaredVelocity < maxSpeedFixation * maxSpeedFixation)
                {
                    microFixations.Add(gazes[i + j]);
                    j++;
                }
                if (microFixations.Count > 1)
                {
                    //Compute the centroid
                    Gaze centre = centroid(microFixations);
                    fixations.Add(centre);
                    i += j - 1;
                }
                if (microFixations.Count == 1) fixations.Add(microFixations.First());
            }
            gazes = fixations;
        }


        /// <summary>
        /// Fixation algorithm from "Eye movements as implicit relevance feedback", Busher, 2008
        /// </summary>
        /// <param name="msFixation">Minimum number of time (in ms) for considering a fixation</param>
        /// <param name="smallSquareSize">Size of the first small square of the algorithm</param>
        /// <param name="bigSquareSize">Size of the second big square of the algorithm. Allows noise from eye tracker, microsaccades and drifts.</param>
        /// <param name="consecutiveFails">If 4 successive gazes cannot be merged with the fixation, the process of fixation detection is stopped</param>
        public static List<Gaze> fixationBusher2008(List<Gaze> gazes, int msFixation = 80/*change from 100ms*/, int smallSquareSize = 30, int bigSquareSize = 50, int consecutiveFails = 4)
        {
            //First compute how many fixations we need in order to have 100ms (which is the minimum time for a fixation)
            List<Gaze> fixations = new List<Gaze>();
            for (int i = 0; i < gazes.Count; i++)
            {
                int fails = 0;
                //Let's take enought gazes for making 100ms
                double time = 0;
                List<Gaze> fixationCandidates = new List<Gaze>();
                int counter = 0;
                //while (time < msFixation && i + counter < gazes.Count )
                while (time < msFixation || fixationCandidates.Count < 4)
                {
                    if (i + counter + 1 > gazes.Count) break;
                    fixationCandidates.Add(gazes[i + counter]);
                    time = fixationCandidates[fixationCandidates.Count - 1].timestamp - fixationCandidates[0].timestamp;
                    counter++;
                }

                //If not enough gazes are gathered (at the end of the file)
                if (time < msFixation)
                    break;

                //If they are contained in a square
                if (insideSquare(fixationCandidates, smallSquareSize))
                {
                    //This gazes are considered as a fixation
                    //While next gazes are include in 50 * 50 square we add them to the fixation
                    int j = i + fixationCandidates.Count;
                    int remove_count = 0;                                             //change part
                    while (j < gazes.Count)
                    {

                        fixationCandidates.Add(gazes[j]);
                        //If the gaze is not in  the square, we remove it from the fixation and stop the process after 4 consecutive fails
                        if (!insideSquare(fixationCandidates, bigSquareSize))
                        {
                            fixationCandidates.RemoveAt(fixationCandidates.Count - 1);
                            remove_count++;                                            //change part
                            fails++;
                            if (fails >= consecutiveFails)
                                break;
                        }
                        else
                        {
                            fails = 0;
                        }
                        j++;
                    }
                    fixations.Add(centroid(fixationCandidates));
                    i += fixationCandidates.Count + remove_count;            //change part
                    // i += fixationCandidates.Count - 1;
                }
            }
            return fixations;
        }

        public static bool insideSquare(List<Gaze> gazes, int sizeSquare)
        {
            var minX = gazes.Min(p => p.gazeX);
            var minY = gazes.Min(p => p.gazeY);
            var maxX = gazes.Max(p => p.gazeX);
            var maxY = gazes.Max(p => p.gazeY);
            if (maxX - minX < sizeSquare && maxY - minY < sizeSquare)
                return true;
            else
                return false;
        }

        public static Gaze centroid(List<Gaze> gazes)
        {
            Gaze g = gazes.First();
            g.duration = gazes.Last().timestamp - g.timestamp;
            for (int i = 1; i < gazes.Count; i++)
            {
                g.gazeX += gazes[i].gazeX;
                g.gazeY += gazes[i].gazeY;
            }
            g.gazeX /= gazes.Count;
            g.gazeY /= gazes.Count;
            return g;
        }
    }
}
