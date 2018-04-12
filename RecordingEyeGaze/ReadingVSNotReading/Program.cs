using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReadingVSNotReading
{
    class Program
    {
        static void Main(string[] args)
        {
            //load a file
            //string namefile = @"C:\Users\Olivier\Documents\GitHub\eyex\RecordingEyeGaze\RecordingEyeGaze\bin\Debug\readingNature_screenshots\readingNaturep1-913words";
            ////string namefile = @"C:\Users\Olivier\Documents\GitHub\eyex\RecordingEyeGaze\RecordingEyeGaze\bin\Debug\notreading_screenshots\notreading";

            //GazeData gd = new GazeData(namefile);
            //gd.gazes = GazeData.fixationBusher2008(gd.gazes);
            //List<bool> reading = scoreReading1(gd.gazes);

            string folder = @"C:\Users\Olivier\Documents\GitHub\eyex\RecordingEyeGaze\ReadingVSNotReading\bin\Debug\verticalJapanese";
            //string folder = @"C:\Users\Olivier\Documents\GitHub\eyex\RecordingEyeGaze\ReadingVSNotReading\bin\Debug\English";
            string[] files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                Console.WriteLine(file);
                GazeData gd = new GazeData(file);
                gd.gazes = GazeData.fixationBusher2008(gd.gazes);
                List<int> reading = scoreReading2(gd.gazes, 5, 20);
                double average = reading.Average();
                Console.WriteLine(average);
                Console.WriteLine("\n");

                //If reading, apply the wordometer
                //if (average > 0.5)
                //{
                //    gd.lines = GazeData.lineBreakDetectionSimple(ref gd.gazes);
                //    int nbWords = (int)(gd.TotalLinesLength / 70);
                //    Console.WriteLine("Nb of words = " + nbWords);
                //}
            }
            Console.WriteLine("\n\nPress a key to exit...");
            Console.ReadLine();
        }

        public static List<int> scoreReading1(List<Gaze> fixations)
        {
            Console.WriteLine();
            List<int> readingDetection = new List<int>();
            int currentScore = 0;
            int count = fixations.Count;
            if (count < 3) return readingDetection;
            //If the 3 last fixations are aligned 
            for (int i = 3; i < fixations.Count; i++)
            {
                var A = fixations[i - 1];
                var B = fixations[i - 2];
                var C = fixations[i - 3];
                if (A.gazeX > B.gazeX && B.gazeX > C.gazeX && Math.Abs(A.gazeY - B.gazeY) < 50 && Math.Abs(B.gazeY - C.gazeY) < 50)
                {
                    currentScore = 20;
                }
                else
                    currentScore -= 20;
                if (currentScore > 0)
                {
                    readingDetection.Add(1);
                    Console.Write("*");
                }
                else
                {
                    readingDetection.Add(0);
                    Console.Write("-");
                }
            }
            Console.WriteLine();
            return readingDetection;
        }

        //Algorithm independent from English and Japanese reading. The saccades are alligned
        public static List<int> scoreReading2(List<Gaze> fixations, int bufferFixationSize = 5, int angleThreshold = 20)
        {
            Vector horizontal = new Vector(100, 0);
            Console.WriteLine();
            List<int> readingDetection = new List<int>();
            int count = fixations.Count;
            if (count < bufferFixationSize) return readingDetection;
            for (int i = 0; i < fixations.Count-bufferFixationSize; i++)
            {

                List<double> angles = new List<double>();
                for (int j = 0; j < bufferFixationSize-1; j++)
                {
                    //compute the saccade vector
                    Vector vec = new Vector(fixations[i + j+1].gazeX - fixations[i + j].gazeX, fixations[i + j+1].gazeY - fixations[i + j].gazeY);
                    //Compute the angle
                    angles.Add(Vector.AngleBetween(horizontal, vec));
                }
                angles.Sort();
                //Filter the list by removing the angle wich is the most different. 
                double median = angles[angles.Count/2];
                List<double> devAngle = new List<double>();
                for(int j=0; j<angles.Count; j++)
                {
                    devAngle.Add(Math.Abs(median - angles[j]));
                }
                devAngle.Sort();
                //Remove the last (the biggest). It can corresponds to on line break or one noise
                devAngle.RemoveAt(devAngle.Count - 1);
                double mean = devAngle.Average();
                //Make decision. If the average of difference is smaller than a threshold, it's reading. Else not reading. 
                if (mean < angleThreshold)
                {
                    readingDetection.Add(1);
                    Console.Write("*");
                }
                else
                {
                    readingDetection.Add(0);
                    Console.Write("-");
                }
            }
            Console.WriteLine();
            return readingDetection;
        }
    }
}
