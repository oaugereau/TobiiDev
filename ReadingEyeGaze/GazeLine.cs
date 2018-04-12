using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalazingEyeGaze
{
    public class GazeLine
    {
        public List<Gaze> gazes = new List<Gaze>();
        private double length = -1;
        public double Length 
        { 
            get
            {
                if(length ==-1)
                {
                    double xmin = gazes[0].gazeX;
                    double xmax = gazes[gazes.Count - 1].gazeX;
                    foreach (var item in gazes)
                    {
                        if (item.gazeX < xmin) xmin = item.gazeX;
                        if (item.gazeX > xmax) xmax = item.gazeX;
                    }
                    length = xmax - xmin;
                }
                return length;
            }
        }

        public int idLine;
        private double medianY = -1;
        public double MedianY
        {
            get
            {
                if (medianY == -1)
                {
                    List<double> y = new List<double>();
                    foreach (var fix in gazes)
                    {
                        y.Add(fix.gazeY);
                    }
                    y.Sort();
                    medianY = y[y.Count / 2];
                }
                return medianY;
            }
        }
        public GazeLine(List<Gaze> gazes)
        {
            this.gazes = gazes;
        }
        public GazeLine()
        {

        }
    }
}
