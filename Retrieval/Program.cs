using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Retrieval
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Load a list of layout
            string folderPathGT = @"C:\Users\Olivier\Documents\GitHub\eyex\Retrieval\bin\Debug\GT";
            List<PcGts> pages = loadLayouts(folderPathGT);

            //Load the eye gaze
            string folderPathEG = @"C:\Users\Olivier\Documents\GitHub\eyex\Retrieval\bin\Debug\Data";
            List<GazeData> readings = loadEyeGazes(folderPathEG);
            estimateNbLinesWords(readings, pages);
            //matchLinesWithDTW(readings, pages);
            Console.ReadLine();
        }

        private static void matchLinesWithDTW(List<GazeData> readings, List<PcGts> pages)
        {
            foreach (var read in readings)
            {
                //create the array for the DTW
                float[] readLines = new float[read.lines.Count];
                float[] pageLines = new float[pages[read.groundTruthID].Lines.Count];
                for (int i = 0; i < read.lines.Count; i++)
                {
                    readLines[i] = (float)read.lines[i].Length + pages[read.groundTruthID].averageWordWidth;
                }
                for (int i = 0; i < pages[read.groundTruthID].Lines.Count; i++)
                {
                    pageLines[i] = pages[read.groundTruthID].Lines[i].LineLength;
                }

                //Compute the DTW
                float[,] dtw = AnalazingEyeGaze.Program.LocalDTW(readLines, pageLines);
                float score = dtw[readLines.Length - 1, pageLines.Length - 1];
                List<KeyValuePair<int, int>> matchs = AnalazingEyeGaze.Program.Backtrack(dtw);
                Console.WriteLine(read.filename);
                Console.WriteLine(score);
                Console.WriteLine();
            }
        }

        private static void estimateNbLinesWords(List<GazeData> readings, List<PcGts> pages)
        {
            List<float> errorsWordsLine = new List<float>();
            List<float> errorsWordsFixation = new List<float>();
            List<int> errorsLines = new List<int>();
            foreach (var read in readings)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(read.filename));
                int nbWords = pages[read.groundTruthID].Words.Count;
                //float minutes = (read.gazes.Last().timestamp - read.gazes.First().timestamp) / 60000;
                Console.WriteLine("Nb of words: " + nbWords + ", nb of lines: " + pages[read.groundTruthID].Lines.Count);// + ", wpm: " + nbWords/minutes);
                //", GT estimation: " + pages[gd.groundTruthID].totalLineLength/ (pages[gd.groundTruthID].averageWordWidth + pages[gd.groundTruthID].averageSpaceLength));
                //Estimate the number of wordss (line length / (words + space)
                float estimatedWordsBasedOnFixation = read.gazes.Count;
                float estimatedWordsBasedOnLineLength = (float)read.TotalLinesLength / (pages[read.groundTruthID].averageWordWidth + pages[read.groundTruthID].averageSpaceLength);
                //I had one word per line in the estimation because the fixation are found at the beggining of the words
                estimatedWordsBasedOnLineLength += read.lines.Count;
                float errorWordsLine = Math.Abs(nbWords - estimatedWordsBasedOnLineLength) * 100 / nbWords;
                float errorWordsFixation = 1f * Math.Abs(nbWords - estimatedWordsBasedOnFixation) * 100 / nbWords;
                Console.WriteLine("Estimated words (fixations): " + estimatedWordsBasedOnFixation + " (" + errorWordsFixation + "%)");
                Console.WriteLine("Estimated words (line length): " + estimatedWordsBasedOnLineLength + " (" + errorWordsLine + "%)" + ", lines: " + read.lines.Count);
                Console.WriteLine();
                errorsWordsLine.Add(errorWordsLine);
                errorsWordsFixation.Add(errorWordsFixation);
                errorsLines.Add(Math.Abs(pages[read.groundTruthID].Lines.Count - read.lines.Count));
            }
            errorsWordsLine.Sort();
            errorsLines.Sort();
            errorsWordsFixation.Sort();
            Console.WriteLine("Average word error: " + errorsWordsFixation.Average() + ", Median: " + errorsWordsFixation[errorsWordsFixation.Count / 2]);
            Console.WriteLine("Average word error: " + errorsWordsLine.Average() + ", Median: " + errorsWordsLine[errorsWordsLine.Count / 2]);
            Console.WriteLine("Average line error: " + errorsLines.Average() + ", Median: " + errorsLines[errorsLines.Count / 2]);
        }

        public static List<PcGts> loadLayouts(string folderPathGT)
        {
            string[] filesGT = Directory.GetFiles(folderPathGT, "*.xml");
            List<PcGts> pages = new List<PcGts>();
            foreach (var item in filesGT)
            {
                pages.Add(loadLayout(item));
            }
            return pages;
        }

        public static PcGts loadLayout(string path)
        {
            PcGts p;
            XmlSerializer xs = new XmlSerializer(typeof(PcGts));
            using (StreamReader rd = new StreamReader(path))
            {
                p = xs.Deserialize(rd) as PcGts;
                Console.WriteLine();
                Console.WriteLine(path);
                Console.WriteLine("Nb of lines: " + p.Lines.Count);
                Console.WriteLine("Nb of words: " + p.Words.Count);
                Console.WriteLine("Average width of words: " + p.averageWordWidth);
                float totalLengthLines = 0;
                foreach (var l in p.Lines)
                {
                    totalLengthLines += l.LineLength;
                }
                //Console.WriteLine("Nb of words (total line length / (words + space)): " + totalLengthLines / (p.averageWordWidth + p.averageSpaceLength));
                //Console.WriteLine("Estimated nb of words (total line length / Median width of words): " + totalLengthLines / p.MedianWordWidth);
            }
            return p;
        }

        public static List<GazeData> loadEyeGazes(string folderPathEG)
        {
            List<GazeData> gazes = new List<GazeData>();
            List<string> files = Directory.GetFiles(folderPathEG, "*.csv").ToList();
            files.AddRange(Directory.GetFiles(folderPathEG, "*.txt"));
            foreach (var item in files)
            {
                //Get the ID of the groundtruth
                int groundTruthID = int.Parse(Path.GetFileNameWithoutExtension(item).Split('_')[1]);
                //I don't take the first text as it seems not relevant
                //if (groundTruthID == 1) continue;
                GazeData gd = new GazeData(Path.ChangeExtension(item, null), groundTruthID - 1);
                gd.gazes =  GazeData.fixationBusher2008(gd.gazes, 100);
                gd.lines = GazeData.lineBreakDetectionSimple(ref gd.gazes);
                //gd.lineBreakDetectionOkoso();
                //gd.rowDataLineBreak(-80);
                gazes.Add(gd);
            }
            return gazes;
        }
    }
}
