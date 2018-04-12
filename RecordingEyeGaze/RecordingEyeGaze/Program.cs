using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EyeXFramework;
using Tobii.EyeX.Framework;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using ModelUtpal;
using AnalazingEyeGaze;

namespace RecordingEyeGaze
{
    public class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Write the output file name (without extension): ");
            string filenameWithoutExtention = Console.ReadLine();

            //Console.WriteLine("Taking screenshot during the recording? Yes (Y) No (N)");
            //string screenShots = Console.ReadLine();
            bool takeScreenShots = false;
            //if (screenShots.ToLower() == "y") takeScreenShots = true;

            //Console.WriteLine("Chose the Gaze Point Data mode : Filtered (F) or Unfiltered (U)");
            //string mode = Console.ReadLine();
            bool filtered = true;
            //if (mode.ToLower() == "u") filtered = false;
            //else filtered = true;
            Record record = new Record(filtered, filenameWithoutExtention, takeScreenShots);

            Console.WriteLine("Listening for gaze data, press 'q' to quit or any key to start/stop the recording...");
            // Let it run until a key is pressed.
            record.recordLoop();
            
            //Save the files
            record.saveFiles();

            //Dispose
            record.Dispose();

            //Process the fix file
            string fileOutput = AnalazingEyeGaze.Program.processOneFile(filenameWithoutExtention);
            ModelUtpal.Program.processOnefile(fileOutput);
        }

 
    }
}
