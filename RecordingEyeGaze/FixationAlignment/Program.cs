using AnalazingEyeGaze;
using Retrieval;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FixationAlignment
{
    class Program
    {
        const int nbNeareastLines = 15;

        static void Main(string[] args)
        {
            //string folder = @"X:\Olivier Augereau\CsvFixationsLineSave";
            string folder = @"X:\Olivier Augereau\FichiersCsv_ApresCorrection _T50_15Lines";
            //string folder = @"X:\Olivier Augereau\NewCsvFolderPath";
            foreach (var item in Directory.GetFiles(folder, "*.csv"))
            {
                nearestXMLLines(item, @"X:\Olivier Augereau\benediktXML");  
            }
        }

        //creating a csv. name : lines_Benedick_3.csv, content: X;y;timestamp;duration;lineID

        //reading the csv and alanyse each different lineID. For each fixation line, computing the y median position, 
        //taking the n neareast text line of the corresponding xml (the xml name correspond to the last letter of the csv file)
        //Save 1 file per fixation line
        //The file contain: n+1 lines (the first is the fixation line, and next ones are the nearest text lines with the potition of the words) : lineID; x1; x2; x3 …
        static void nearestXMLLines(string fixationLinefilename, string xmlFolder)
        {
            #region serialize the XMLs
            string[] xmlFiles = Directory.GetFiles(xmlFolder, "*.xml");
            int xmlNumber = int.Parse(fixationLinefilename[fixationLinefilename.Length - 5].ToString());
            //select the good xml file
            string xmlPath = "";
            foreach (var item in xmlFiles)
            {
                if (int.Parse(item[item.Length - 5].ToString()) == xmlNumber)
                {
                    xmlPath = item;
                    break;
                }
            }
            if (xmlPath == "")
            {
                Console.WriteLine("Error, the XML file corresponding to the csv cannot be found.");
                Console.ReadLine();
                return;
            }
            PcGts xml;
            XmlSerializer xs = new XmlSerializer(typeof(PcGts));
            using (StreamReader rd = new StreamReader(xmlPath))
            {
                xml = xs.Deserialize(rd) as PcGts;
            }
            #endregion
            #region Analyse the fixations and the lines, create corresponding objects
            List<string> fixations = File.ReadAllLines(fixationLinefilename).ToList();
            fixations.RemoveAt(0); //remove the header
            List<GazeLine> lines = new List<GazeLine>();
            int idLine = -1;
            GazeLine curentLine = new GazeLine();
            for (int i = 0; i < fixations.Count; i++)
            {
                Gaze g = new Gaze();
                string[] split = fixations[i].Split(';');
                g.gazeX = float.Parse(split[0]);
                g.gazeY = float.Parse(split[1]);
                g.timestamp = float.Parse(split[2]);
                g.duration = float.Parse(split[3]);
                g.idLine = int.Parse(split[4]);
                if (i == 0)
                {
                    idLine = g.idLine;
                    curentLine.idLine = g.idLine;
                }
                else if (idLine != g.idLine)
                {
                    //save the line
                    lines.Add(curentLine);
                    //start a new one
                    curentLine = new GazeLine();
                    idLine = g.idLine;
                    curentLine.idLine = g.idLine;
                }
                curentLine.gazes.Add(g);
            }
            //add the last group
            lines.Add(curentLine);
            #endregion
            //Foreach line take the 5 nearest text line (in y)
            foreach (var line in lines)
            {
                //get median y of line
                double y = line.MedianY;
                Dictionary<int, TextLine> nnLines = nearestLines(y, nbNeareastLines, xml);
                //save the fixation x pos and the nearest text lines
                string nameFile = fixationLinefilename.Replace(".csv", "_" + line.idLine + ".txt");
                List<string> linesToWrite = new List<string>();
                linesToWrite.Add("idLine; x1; x2;...");
                string lineFixation = line.idLine.ToString();
                foreach (var item in line.gazes)
                {
                    lineFixation += ";" + item.gazeX + ";" + item.gazeY;
                }
                linesToWrite.Add(lineFixation);
                foreach (var item in nnLines)
                {
                    string lineTxtToWrite = item.Key.ToString();
                    foreach (var word in item.Value.Words)
                    {
                        lineTxtToWrite += ";" + word.Center.X + ";" + word.Center.Y;
                    }
                    linesToWrite.Add(lineTxtToWrite);
                }
                File.WriteAllLines(nameFile, linesToWrite);
            }
        }

        static Dictionary<int, TextLine> nearestLines(double value, int n, PcGts p)
        {
            List<TxtLine> txtLines = new List<TxtLine>();
            Dictionary<int, TextLine> neareastLines = new Dictionary<int, TextLine>(); // int is the ID of the textLine
            //compute the distance with every line
            for (int i = 0; i < p.Lines.Count; i++)
            {
                double linePosY = p.Lines[i].Coords.Rectangle.Y + p.Lines[i].Coords.Rectangle.Height / 2;
                double dist = Math.Abs(value - linePosY);
                TxtLine txt = new TxtLine();
                txt.distance = dist;
                txt.lineID = i;
                txt.textline = p.Lines[i];
                txtLines.Add(txt);
            }
            //sort distances
            txtLines = txtLines.OrderBy(x => x.distance).ToList();
            //take the n nearest ones
            for (int i = 0; i < n; i++)
            {
                if (i >= txtLines.Count) break;
                neareastLines.Add(txtLines[i].lineID, txtLines[i].textline);
            }
            return neareastLines;
        }

        //Compare/align the fixation line with the n text lines and compute a distance/score. Then compute the overwall performance of the system.

    }

    class TxtLine
    {
        public TextLine textline;
        public double distance;
        public int lineID;
    };
}
