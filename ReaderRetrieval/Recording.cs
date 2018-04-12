using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderRetrieval
{

    public class Recording
    {
        public List<Gaze> gaze;
        public List<Gaze> fixations;
        public List<Saccade> saccades;
        public List<GazeLine> fixationLines;
        public string documentName;
        public string readerName;
        public string path;

        //set of features 
        public Dictionary<string, double> features = new Dictionary<string, double>();

        public Recording(string path)
        {
            this.path = path;
            gaze = GazeData.readTobiiCsv(path);
            fixations = GazeData.fixationBusher2008(gaze);
            fixationLines = GazeData.lineBreakDetectionSimple(ref fixations);
            string[] split = Path.GetFileNameWithoutExtension(path).Split('_');
            readerName = split[0];
            documentName = split[1];
            saccades = computeSaccades(fixations);
            computeFeatures();
        }

        public static List<Saccade> computeSaccades(List<Gaze> fixations)
        {
            List<Saccade> sacc = new List<Saccade>();
            for (int i = 0; i < fixations.Count - 1; i++)
            {
                sacc.Add(new Saccade(fixations[i], fixations[i + 1]));
            }
            return sacc;
        }



        public void computeFeatures()
        {
            double fixationCount;
            double averageFixationDuration;
            double averageSaccadeAmplitude;
            double averageHSaccadeAmplitude;
            double averageVSaccadeAmplitude;
            double averageSaccadeVelocity;
            double stdDevFixationDuration;
            double stdDevSaccadeAmplitude;
            double stdDevHSaccadeAmplitude;
            double stdDevVSaccadeAmplitude;
            double stdDevSaccadeVelocity;
            double averageLineLength;
            double mediumFixationCount = 0;
            double longFixationCount = 0;
            double sSaccadeCount = 0;
            double lSaccadeCount = 0;
            //double lReverseSaccadeCount = 0;
            //double sReverseSaccadeCount = 0;

            double sum;

            fixationCount = fixations.Count;

            averageFixationDuration = fixations.Average(f => f.duration);
            sum = fixations.Sum(d => (d.duration - averageFixationDuration) * (d.duration - averageFixationDuration));
            stdDevFixationDuration = Math.Sqrt(sum / fixationCount);

            averageHSaccadeAmplitude = saccades.Average(f => f.vectorX);
            sum = saccades.Sum(d => (d.vectorX - averageHSaccadeAmplitude) * (d.vectorX - averageHSaccadeAmplitude));
            stdDevHSaccadeAmplitude = Math.Sqrt(sum / saccades.Count);

            averageSaccadeAmplitude = saccades.Average(f => f.length);
            sum = saccades.Sum(d => (d.length - averageSaccadeAmplitude) * (d.length - averageSaccadeAmplitude));
            stdDevSaccadeAmplitude = Math.Sqrt(sum / saccades.Count);

            averageVSaccadeAmplitude = saccades.Average(f => f.vectorY);
            sum = saccades.Sum(d => (d.vectorY - averageVSaccadeAmplitude) * (d.vectorY - averageVSaccadeAmplitude));
            stdDevVSaccadeAmplitude = Math.Sqrt(sum / saccades.Count);

            averageSaccadeVelocity = saccades.Average(f => f.velocity);
            sum = saccades.Sum(d => (d.velocity - averageSaccadeVelocity) * (d.velocity - averageSaccadeVelocity));
            stdDevSaccadeVelocity = Math.Sqrt(sum / saccades.Count);

            averageLineLength = fixationLines.Average(f => f.Length);



            //features.Add(Features.fixationCount, fixationCount);
            features.Add("averageFixationDuration", averageFixationDuration);
            features.Add("averageSaccadeAmplitude", averageSaccadeAmplitude);
            features.Add("averageHSaccadeAmplitude", averageHSaccadeAmplitude);
            features.Add("averageVSaccadeAmplitude", averageVSaccadeAmplitude);
            features.Add("averageSaccadeVelocity", averageSaccadeVelocity);
            features.Add("stdDevFixationDuration", stdDevFixationDuration);
            features.Add("stdDevSaccadeAmplitude", stdDevSaccadeAmplitude);
            features.Add("stdDevHSaccadeAmplitude", stdDevHSaccadeAmplitude);
            features.Add("stdDevVSaccadeAmplitude", stdDevVSaccadeAmplitude);
            features.Add("stdDevSaccadeVelocity", stdDevSaccadeVelocity);
            features.Add("nbLines", fixationLines.Count);
            features.Add("averageLineLength", averageLineLength);

            foreach (var f in fixations)
            {
                if (f.duration >= 200 && f.duration <= 300) mediumFixationCount++;
                else if (f.duration >= 500) longFixationCount++;
            }
            
            foreach (var s in saccades)
            {
                if (s.length > 0 && s.length <= 100) sSaccadeCount++;
                //else if (s.length <= -500) lReverseSaccadeCount++;
                //else if (s.length < 0 && s.length > -100) sReverseSaccadeCount++;
                else if (s.length > 100 & s.length <= 200) lSaccadeCount++;
            }

            features.Add("mediumFixationCount", mediumFixationCount);
            //features.Add("longFixationCount", longFixationCount);

            features.Add("sSaccadeCount", sSaccadeCount);
            features.Add("lSaccadeCount", lSaccadeCount);
            //features.Add("sReverseSaccadeCount", sReverseSaccadeCount);
            //features.Add("lReverseSaccadeCount", lReverseSaccadeCount);
        }
    }
}
