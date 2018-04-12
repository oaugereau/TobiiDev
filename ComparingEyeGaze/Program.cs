using AForge.Video.FFMPEG;
using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ComparingEyeGaze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path of the csv file");
            string path = Console.ReadLine();
            if (File.Exists(path))
            {
                //open csv and compute fixation
                GazeData gd = openAndFilter(path);
                //save the fixations in a file
                //gd.saveFixations(path.Replace(".csv", "_fix.csv"));
            }
        }

        public static void scanMatch()
        {
            Console.WriteLine("Enter the path of the first csv file");
            string path1 = Console.ReadLine();
            //open csv and filter fixation/saccade
            GazeData gd1 = openAndFilter(path1);
            //create spacial binning
            //create temporal binning
            GazeData gd1bin = createTemporalBinning(gd1);


            Console.WriteLine("Enter the path of the second csv file");
            string path2 = Console.ReadLine();
            //open csv and filter fixation/saccade
            GazeData gd2 = openAndFilter(path2);
            //create spacial binning
            //create temporal binning
            GazeData gd2bin = createTemporalBinning(gd2);


            //compare two temporal binning
            //create substitution matrix 
            int[,] subsMat = computeSubstitutionMatrix(gd1bin, gd2bin);
            //track back for computing the score
            int score = computeScore(subsMat);
        }

        public static GazeData openAndFilter(string path)
        {
            GazeData gd = new GazeData(Path.ChangeExtension(Path.GetFullPath(path), null));
            gd.gazes =  GazeData.fixationBusher2008(gd.gazes);
            return gd;
        }

        public static GazeData createTemporalBinning(GazeData gd, int timeBin = 50)
        {
            GazeData tempBin = new GazeData();
            //process
            return tempBin;
        }

        public static int[,] computeSubstitutionMatrix(GazeData gd1bin, GazeData gd2bin)
        {
            int[,] subsMat = new int[gd1bin.gazes.Count, gd2bin.gazes.Count];
            //create the matrix
            return subsMat;
        }

        public static int computeScore(int[,] subsMat)
        {
            int score = 0;
            //compute the score
            return score;
        }

    }
}
