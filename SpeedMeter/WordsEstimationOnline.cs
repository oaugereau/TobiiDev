using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedMeter
{
    public class WordsEstimationOnline
    {
        private float space;
        public List<Gaze> gazes = new List<Gaze>();
        public List<Gaze> fixations = new List<Gaze>();
        public int nbWords = 0;

        public WordsEstimationOnline(float averageWordWidth, float averageSpaceWidth)
        {
            space = averageWordWidth + averageSpaceWidth;
        }

        public void update(float posX, float posY, float timestamp)
        {
            Gaze g = new Gaze();
            g.gazeX = posX;
            g.gazeY = posY;
            g.timestamp = timestamp;
            gazes.Add(g);
            GazeData gd = new GazeData();
            gd.gazes = gazes;
            gd.gazes = GazeData.fixationBusher2008(gd.gazes);
            gd.lines = GazeData.lineBreakDetectionSimple(ref gd.gazes);
            fixations = gd.gazes;
            nbWords = (int)(gd.TotalLinesLength / space);
            //I had one word per line in the estimation because the fixations are found at the center of the words
            nbWords += gd.lines.Count;
        }

    }
}
