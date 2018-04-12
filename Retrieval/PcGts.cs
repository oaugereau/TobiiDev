using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Retrieval
{
    [Serializable, XmlRoot(Namespace = "http://schema.primaresearch.org/PAGE/gts/pagecontent/2013-07-15", ElementName = "PcGts")]
    public class PcGts
    {
        private List<TextLine> lines;
        private List<Word> words;
        public float averageWordWidth = -1;
        private float medianWordWidth = -1;
        public float averageSpaceLength = -1;
        public int totalLineLength = -1;
        //private int nbLongLines =-1;
        //private int nbShortLines =-1;

        [XmlElement("Page")]
        public Page Page { get; set; }
        public List<TextLine> Lines
        {
            get
            {
                if (lines == null && Page != null)
                {
                    lines = new List<TextLine>();
                    foreach (var item in Page.TextRegions)
                    {
                        lines.AddRange(item.TextLines);
                    }
                }
                return lines;
            }
        }

        public List<Word> Words
        {
            get
            {
                initialize();
                return words;
            }
        }

        private void initialize()
        {
            if (words == null && Page != null)
            {
                words = new List<Word>();
                averageWordWidth = 0;
                averageSpaceLength = 0;
                totalLineLength = 0;
                foreach (var tr in Page.TextRegions)
                {
                    foreach (var tl in tr.TextLines)
                    {
                        totalLineLength += tl.LineLength;
                        words.AddRange(tl.Words);
                        //COmpute the length of space
                        averageSpaceLength += tl.LineLength;
                        foreach (var item in tl.Words)
                        {
                            averageSpaceLength -= item.Coords.Rectangle.Width;
                            averageWordWidth += item.Coords.Rectangle.Width;
                        }
                    }
                }
                averageSpaceLength /= words.Count;
                averageWordWidth /= Words.Count;
            }
        }

        public float MedianWordWidth
        {
            get
            {
                if (medianWordWidth == -1)
                {
                    List<float> values = new List<float>();
                    foreach (var item in Words)
                    {
                        values.Add(item.Coords.Rectangle.Width);
                    }
                    values.Sort();
                    medianWordWidth = values[values.Count / 2];
                }
                return medianWordWidth;
            }
        }

        //public int NbLongLines
        //{
        //    get
        //    {
        //        if (nbLongLines == -1)
        //        {
        //            for (int i = 0; i < lines.Count; i++)
        //            {
        //                if (lines[i].LineLength > Page.ImageWidth / 2)
        //                    nbLongLines++;
        //            }
        //        }
        //        return nbLongLines;
        //    }
        //}
        //public int NbShortLines 
        //{
        //    get
        //    {
        //        if (nbShortLines == -1)
        //        {
        //            for (int i = 0; i < lines.Count; i++)
        //            {
        //                if (lines[i].LineLength <= Page.ImageWidth / 2)
        //                    nbShortLines++;
        //            }
        //        }
        //        return nbShortLines;
        //    }
        //}
    }


    public class Page
    {
        [XmlAttribute("imageFilename")]
        public string ImageFilename { get; set; }
        [XmlAttribute("imageWidth")]
        public int ImageWidth { get; set; }
        [XmlAttribute("imageHeight")]
        public int ImageHeight { get; set; }

        [XmlElement("TextRegion")]
        public List<TextRegion> TextRegions { get; set; }
        [XmlElement("SeparatorRegion")]
        public List<SeparatorRegion> SeparatorRegions { get; set; }
        [XmlElement("ImageRegion")]
        public List<ImageRegion> ImageRegions { get; set; }

    }

    public class TextRegion
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("Coords")]
        public Coords Coords { get; set; }

        [XmlElement("TextLine")]
        public List<TextLine> TextLines { get; set; }
    }

    public class SeparatorRegion
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("Coords")]
        public Coords Coords { get; set; }
    }

    public class ImageRegion
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("Coords")]
        public Coords Coords { get; set; }
    }

    public class Coords
    {
        private List<Point> lpoints;
        private Rectangle rectangle;
        [XmlAttribute("points")]
        public string Points { get; set; }
        public List<Point> Lpoint
        {
            get
            {
                if (lpoints == null && Points != null)
                {
                    lpoints = new List<Point>();
                    List<string> pairPoints = Points.Split(' ').ToList();
                    foreach (var item in pairPoints)
                    {
                        List<string> coords = item.Split(',').ToList();
                        lpoints.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
                    }
                }
                return lpoints;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                if (rectangle == Rectangle.Empty && Lpoint.Count > 0)
                {
                        var minX = Lpoint.Min(p => p.X);
                        var minY = Lpoint.Min(p => p.Y);
                        var maxX = Lpoint.Max(p => p.X);
                        var maxY = Lpoint.Max(p => p.Y);
                        rectangle = new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
                }
                return rectangle;
            }
        }
    }

    public class TextLine
    {
        private int lineLength = 0;
        [XmlElement("Coords")]
        public Coords Coords { get; set; }

        [XmlElement("Word")]
        public List<Word> Words { get; set; }

        public int LineLength
        {
            get
            {
                if (lineLength == 0)
                {
                    string[] points = Coords.Points.Split(' ');
                    //lineLength = int.Parse(points[1].Split(',')[0]) - int.Parse(points[0].Split(',')[0]);

                    lineLength = Coords.Rectangle.Width;
                }
                return lineLength;
            }
        }
    }

    public class Word
    {
        private Point center = new Point(-1, -1);
        public Point Center
        {
            get
            {
                if (center.X == -1 && center.Y == -1)
                {
                    center.X = Coords.Rectangle.X + Coords.Rectangle.Width / 2;
                    center.Y = Coords.Rectangle.Y + Coords.Rectangle.Height / 2;
                }
                return center;
            }
        }

        [XmlElement("Coords")]
        public Coords Coords { get; set; }
        [XmlElement("TextEquiv")]
        public TextEquiv TextEquiv { get; set; } 
    }

    public class TextEquiv
    {
        [XmlElement("Unicode")]
        public string Unicode { get; set; } 
    }

}
