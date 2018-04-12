 using AnalazingEyeGaze;
using Retrieval;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud
{
    public class Program
    {
        //input a file recorded by tobii
        //outut the tag words as a string. This string can be use in wordle.net
        static void Main(string[] args)
        {
            Console.WriteLine("Write the path to the Tobii file:");
            string tobiiFile = readFile("Timestamp;GazeX;GazeY;LeftEye;RightEye");
            Console.WriteLine("Write the path to the layout file:");
            string layoutFile = readFile("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            GazeData gd = new GazeData(Path.ChangeExtension(tobiiFile, null));
            PcGts layout = Retrieval.Program.loadLayout(layoutFile);
            gd.gazes = GazeData.fixationBusher2008(gd.gazes);
            //For each gaze, find the closest word
            List<string> words = findClosestWords(gd, layout);
            //Outpout the words
            foreach (var item in words)
            {
                Console.WriteLine(item);                
            }
            Console.WriteLine("Write the path to export the file:");
            string pathSave = Console.ReadLine();
            File.WriteAllLines(pathSave, words);
        }

        public static List<string> findClosestWords(GazeData gd, PcGts layout)
        {
            List<string> words = new List<string>();
            foreach (var item in gd.gazes)
            {
                int dist = 0;
                words.Add(closestWord((int)item.gazeX, (int)item.gazeY, layout.Words, out dist));
            }
            return words;
        }

        public static List<string> findClosestWords(List<Gaze> gd, PcGts layout)
        {
            List<string> words = new List<string>();
            foreach (var item in gd)
            {
                int dist = 0;
                words.Add(closestWord((int)item.gazeX, (int)item.gazeY, layout.Words, out dist));
            }
            return words;
        }

        public static string closestWord(int X, int Y, List<Word> words, out int minDistSquared)
        {
            Word closest = words[0];
            minDistSquared = int.MaxValue;
            foreach (Word w in words)
            {
                int dist = (w.Center.X - X) * (w.Center.X - X) + (w.Center.Y - Y) * (w.Center.Y - Y);
                if (dist < minDistSquared)
                {
                    closest = w;
                    minDistSquared = dist;
                }
            }
            return closest.TextEquiv.Unicode;
        }


        static string readFile(string headerToCheck)
        {
            List<string> lines = new List<string>();
            string inputFile = Console.ReadLine();
            if (File.Exists(inputFile))
            {
                lines = File.ReadAllLines(inputFile).ToList();
                if (lines[0] == headerToCheck)
                {
                    //OK
                    return inputFile;
                }
                else
                {
                    Console.WriteLine("The file " + inputFile + " should start with this line: " + headerToCheck);
                    readFile(headerToCheck);
                }
            }
            else
            {
                Console.WriteLine("The file " + inputFile + " does not exist");
                readFile(headerToCheck);
            }
            return inputFile;
        }
    }
}
