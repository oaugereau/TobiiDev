using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReaderRetrieval
{
    public class Saccade
    {
        public double vectorX;
        public double vectorY;
        public double length;
        public double duration;
        public double velocity;

        public Saccade(Gaze start, Gaze end)
        {
            vectorX = end.gazeX - start.gazeX;
            vectorY = end.gazeY - start.gazeY;
            length = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            duration = end.timestamp - start.timestamp - start.duration;
            if (duration <= 0)
            {
                duration = 0;
                Console.WriteLine(duration + " The saccade duration shouldn't be negative");
                velocity = 0;
            }
            else velocity = length / duration;
        }
    }
}
