using AnalazingEyeGaze;
using Retrieval;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedMeter
{
    public class SpeedEstimator
    {
        //Offline estimation 
        public GazeData gd;
        private float averageWidthWordSpace;

        public SpeedEstimator(string pathGaze, PcGts layout)
        {
            this.gd = new GazeData(Path.ChangeExtension(pathGaze, null));
            averageWidthWordSpace = layout.averageWordWidth + layout.averageSpaceLength;
        }

        public List<double> speedPerFixation()
        {
            //Clone the gaze for applying fixation detection on it
            GazeData gdFixation = (GazeData)gd.Clone();
            GazeData.fixationBusher2008(gdFixation.gazes);
            List<double> speed = new List<double>();
            for (int i = 1; i < gdFixation.gazes.Count; i++)
            {
                double distX = gdFixation.gazes[i].gazeX - gdFixation.gazes[i-1].gazeX;
                //Don't compute speed for backward 
                if (distX < 0) continue;
                double estimatedWords = distX / averageWidthWordSpace;
                double elapsedtime = gdFixation.gazes[i].timestamp - gdFixation.gazes[i - 1].timestamp;
                speed.Add(60000 * estimatedWords / elapsedtime);
            }
            return speed;
        }

        public List<double> speedPerLine()
        {
            //Clone the gaze for applying fixation and line break detection on it
            GazeData gdCopy = (GazeData)gd.Clone();
            gdCopy.gazes = GazeData.fixationBusher2008(gdCopy.gazes);
            gdCopy.lines = GazeData.lineBreakDetectionSimple(ref gdCopy.gazes);
            List<double> speed = new List<double>();
            foreach (var item in gdCopy.lines)
            {
                //estimateSpeed
                speed.Add(speedLine(item));
            }
            return speed;
        }

        private double speedLine(GazeLine gl)
        {
            double estimatedWords = gl.Length / averageWidthWordSpace;
            double elapsedtime = gl.gazes.Last().timestamp - gl.gazes.First().timestamp;
            //60000 ms to minute conversion
            double speed = 60000 * estimatedWords / elapsedtime;
            return speed;
        }

        private int wordsLine(GazeLine gl)
        {
            double estimatedWords = gl.Length / averageWidthWordSpace;
            return (int)estimatedWords+1;
        }

        public int estimatedNumberOfReadWords()
        {
            GazeData gdCopy = (GazeData)gd.Clone();
            gdCopy.gazes = GazeData.fixationBusher2008(gdCopy.gazes);
            gdCopy.lines = GazeData.lineBreakDetectionSimple(ref gdCopy.gazes);
            List<int> words = new List<int>();
            foreach (var item in gdCopy.lines)
            {
                //estimateSpeed
                words.Add(wordsLine(item));
            }
            return words.Sum();
        }
    }
}
