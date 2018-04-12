using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderRetrieval
{
    class Program
    {
        /// <summary>
        /// The aim of this project is to recognize who is reading a text, based on the extraction and learning of reading features
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string folderCsv = @"C:\Users\Olivier\Documents\GitHub\eyex\ReaderRetrieval\bin\Debug\Recordings";
            string[] files = Directory.GetFiles(folderCsv);
            List<Recording> records = new List<Recording>();
            foreach (var file in files)
            {
                records.Add(new Recording(file));
            }

            int scoreDocument = 0;
            int scoreReader = 0;
            //Compare the recordings
            foreach (var record in records)
            {
                //Normalize the data
                normalize(records);

                //Compute the similarities
                Dictionary<Recording, double> similarities = new Dictionary<Recording, double>();
                foreach (var rec in records)
                {
                    if (rec == record) continue;
                    double sim = computeDist(record, rec, 2);
                    similarities.Add(rec, sim);
                }

                //Check the most similar recording
                var order = similarities.OrderBy(key => key.Value);
                var mostSimRec = order.ElementAt(0).Key;
                if (record.documentName == mostSimRec.documentName)
                    scoreDocument++;
                if (record.readerName == mostSimRec.readerName)
                    scoreReader++;
            }

            double finalScoreDoc = 100 * scoreDocument / records.Count;
            double finalScoreReader = 100 * scoreReader / records.Count;
            Console.WriteLine("Performance document = " + finalScoreDoc);
            Console.WriteLine("Performance reader = " + finalScoreReader);
        }

        private static void normalize(List<Recording> records)
        {
            double min, max;

            for (int r = 0; r < records.First().features.Keys.Count; r++ )
            {
                string item = records.First().features.Keys.ElementAt(r);
                min = records.Min(d => d.features[item]);
                max = records.Max(d => d.features[item]);
                for (int i = 0; i < records.Count; i++)
                {
                    records[i].features[item] -= min;
                    records[i].features[item] /= (max - min);
                }
            }
        }

        private static double computeDist(Recording record, Recording rec, int pow = 2)
        {
            double sim = 0;
            foreach (string item in record.features.Keys)
            {
                sim += Math.Pow( record.features[item] - rec.features[item], pow);
            }
            return sim;
        }

    }
}
