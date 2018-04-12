using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalazingEyeGaze
{
    public class Gaze
    {
        public DateTime windowTime;
        public double timestamp;
        public double gazeX;
        public double gazeY;
        public int idLine;
        public double duration;
        public double squaredVelocity; //not used for now
    }
}
