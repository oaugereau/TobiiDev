using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundTruth
{
    public class EyeGaze
    {
        [CsvColumn(FieldIndex = 0, CanBeNull = false, Name = "Timestamp")]
        public string Timestamp { get; set; }
        [CsvColumn(FieldIndex = 1, CanBeNull = false, Name = "GazeX")]
        public float GazeX { get; set; }
        [CsvColumn(FieldIndex = 2, CanBeNull = false, Name = "GazeY")]
        public float GazeY { get; set; }
        [CsvColumn(FieldIndex = 3, CanBeNull = false, Name = "LeftEye")]
        public string LeftEye { get; set; }
        [CsvColumn(FieldIndex = 4, CanBeNull = false, Name = "RightEye")]
        public string RightEye { get; set; }
        [CsvColumn(FieldIndex = 5, CanBeNull = true, Name = "LineNumber")]
        public int LineNumber { get; set; }
    }
}
