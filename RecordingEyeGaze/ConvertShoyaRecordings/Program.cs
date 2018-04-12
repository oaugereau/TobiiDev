using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertShoyaRecordings
{
    class Program
    {
        static void Main(string[] args)
        {

            string folderInput = @"V:\manga-eyegaze\l3i";
            string folderOutput = @"V:\manga-eyegaze\documentRetrievalMatlab";

            string[] files = Directory.GetFiles(folderInput, "*fixation.csv", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                List<string> newLines = new List<string>();
                List<string> lines = File.ReadAllLines(files[i]).ToList();
                lines.RemoveAt(0); // remove the header
                foreach (var item in lines)
                {
                    string[] split = item.Split(',');
                    newLines.Add(split[1] + ";" + split[2] + ";0");
                }

                string folderName = Path.GetDirectoryName(files[i]).Split(Path.DirectorySeparatorChar).Last().Replace("_", "");
                string filename = folderOutput + "\\" + folderName + "_" + Path.GetFileNameWithoutExtension(files[i]).Split('_')[0] + ".csv";
                File.WriteAllLines(filename, newLines);
            }

            //save for matlab


        }
    }
}
