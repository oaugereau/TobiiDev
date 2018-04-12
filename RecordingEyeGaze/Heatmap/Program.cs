using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalazingEyeGaze;
using System.Drawing;


namespace Heatmap
{
    class Program
    {
        static string name = "001_raw";

        static string folder = @"V:\manga-eyegaze\l3i";

        static void Main(string[] args)
        {
            //Load recordings
            string[] files = Directory.GetFiles(folder, name + ".csv", SearchOption.AllDirectories);

            //Load image
            Bitmap bmp = new Bitmap(@"V:\manga-eyegaze\l3i\2015-10-06_10-26-41\001_back.png");
            Graphics g = Graphics.FromImage(bmp);

            //Compute fixations
            foreach (var file in files)
            {
                GazeData gd = new GazeData(file.Replace(".csv", ""));
                //List<Gaze> gazes = new List<Gaze>();
                List<Gaze> fixations = GazeData.fixationBusher2008(gd.gazes);
                //normalize the time 
                double sum = fixations.Sum(x => x.duration);
                for (int i = 0; i < fixations.Count; i++)
                {
                    // 360/25
                    fixations[i].duration *= 100 / sum;
                    //Add to the hue , the duration in 50 pixels around the fixations
                    Pen p = new Pen(Color.Red);
                    g.DrawEllipse(p, new Rectangle((int)fixations[i].gazeX, (int)fixations[i].gazeY, 50, 50)); // Should be a gaussian
                }
                //gd.gazes = fixations;
            }

            
        }


    }
}
