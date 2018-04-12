using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCQ_TOEIC
{
    public class Question
    {
        public string question;
        public string answerA;
        public string answerB;
        public string answerC;
        public string answerD;


        public Question(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            question = lines[0];
            answerA = lines[1];
            answerB = lines[2];
            answerC = lines[3];
            answerD = lines[4];
        }
    }
}
