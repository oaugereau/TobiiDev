using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ModelUtpal
{
    public class Program
    {
        static void Main(string[] args)
        {
            string pathFolder = @"C:\Users\Olivier\Desktop\Okoso_Data\2nd\rawdata2016_test";
            string[] files = Directory.GetFiles(pathFolder, "*_fix.csv");
            foreach (var f in files)
            {
                processOnefile(f);
            }
        }

        public static void processOnefile(string f)
        {
            Console.WriteLine("Processing the file: " + f);
            //read file
            List<string> lines = File.ReadAllLines(f).ToList();
            lines.RemoveAt(0);
            Recording r = new Recording();
            FixationLine fl = new FixationLine();
            foreach (var l in lines)
            {
                Fixation fix = new Fixation(l);
                if (r.fixationLines.Count == 0 && fl.fixations.Count == 0)
                    fl.fixations.Add(fix);
                else if (fix.idLine == fl.fixations[0].idLine)
                {
                    fl.fixations.Add(fix);
                }
                else
                {
                    r.fixationLines.Add(fl);
                    fl = new FixationLine();
                    fl.fixations.Add(fix);
                }
            }
            //Add the last line
            r.fixationLines.Add(fl);

            double xReg = 0;
            //Process the label for the fixations
            for (int i = 0; i < r.fixationLines.Count; i++)
            {
                r.fixationLines[i].fixations[0].label = "F";
                xReg = 0;
                for (int j = 1; j < r.fixationLines[i].fixations.Count; j++)
                {
                    var cur = r.fixationLines[i].fixations[j];
                    var prev = r.fixationLines[i].fixations[j - 1];
                    if (cur.x < prev.x)
                    {
                        xReg = prev.x;
                        cur.label = "B";
                    }
                    else if (cur.x > xReg)
                    {
                        cur.label = "F";
                    }
                    else
                    {
                        cur.label = "B";
                    }

                    r.fixationLines[i].saccades.Add(new Saccade(prev, cur));
                }
            }

            //Write everything in a new file
            string newPath = f.Replace("_fix.csv", "_fixSac.csv");
            List<string> allLines = new List<string>();
            string header = "timestamp; x; y; duration/length; fixation/saccade; Forward/Backward; idLine";
            allLines.Add(header);
            foreach (var item in r.fixationLines)
            {
                foreach (var item1 in item.fixations)
                {
                    string lineToWrite = item1.timestamp + ";" + item1.x + ";" + item1.y + ";" + item1.duration + ";f;" + item1.label + ";" + item1.idLine;
                    allLines.Add(lineToWrite);
                }
                foreach (var item2 in item.saccades)
                {
                    string lineToWrite = item2.timestamp + ";" + item2.x + ";" + item2.y + ";" + item2.length + ";s;" + item2.label + ";" + item2.idLine;
                    allLines.Add(lineToWrite);
                }
            }

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            File.WriteAllLines(newPath, allLines);
        }
    }
}

public class Fixation
{
    public double timestamp;
    public double x;
    public double y;
    public double duration;
    public double idLine;
    public string label; //F or B
    public Fixation(string l)
    {
        l = l.Replace(',', '.');
        string[] split = l.Split(';');
        timestamp = double.Parse(split[0], new CultureInfo("en-US"));
        x = double.Parse(split[1], new CultureInfo("en-US"));
        y = double.Parse(split[2], new CultureInfo("en-US"));
        duration = double.Parse(split[3], new CultureInfo("en-US"));
        idLine = double.Parse(split[4], new CultureInfo("en-US"));
    }
}
public class Saccade
{
    public double timestamp;
    public double duration;
    public double x;
    public double y;
    public double length;
    public double idLine;
    public string label; //F or B
    public Saccade(Fixation cur, Fixation next)
    {
        duration = next.timestamp - cur.timestamp + cur.duration;
        timestamp = cur.timestamp + cur.duration;
        length = Math.Sqrt((next.x - cur.x)* (next.x - cur.x) + (next.y - cur.y) * (next.y - cur.y));
        x = cur.x;
        y = cur.y;
        idLine = cur.idLine;
        label = cur.label;
    }
}

public class FixationLine
{
    public List<Fixation> fixations = new List<Fixation>();
    public List<Saccade> saccades = new List<Saccade>();
}

public class Recording
{
    public List<FixationLine> fixationLines = new List<FixationLine>();
}