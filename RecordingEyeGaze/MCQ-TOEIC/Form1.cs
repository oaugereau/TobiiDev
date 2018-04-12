using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCQ_TOEIC
{
    public partial class Form1 : Form
    {

        int currentQuestion = -1;
        List<Question> questions = new List<Question>();
        List<FormSure> sures = new List<FormSure>();
        List<string> logger = new List<string>();
        string[] answers = new string[0];

        public Form1()
        {
            InitializeComponent();
        }

        //Prepare the questions
        private void buttonStart_Click(object sender, EventArgs e)
        {           
            //Check if the name recording have been filled out
            if(textBoxNameRecording.Text == "")
            {
                MessageBox.Show("The text box for the name recoding is empty!");
                return;
            }

            //Get all the .txt files in the specified folder
            if (!Directory.Exists(textBoxPathQuestions.Text))
            {
                MessageBox.Show("The folder specified in the textbox don't exist!");
                return;
            }

            string[] files = Directory.GetFiles(textBoxPathQuestions.Text, "*.txt");

            if(files.Length ==0)
            {
                MessageBox.Show("No .txt file can be found in the specified folder");
                return;
            }

            foreach (var file in files)
            {
                questions.Add(new Question(file));
            }
            logger.Add(files.Length.ToString());
            answers = new string[files.Length];

            radioButtonA.Enabled = true;
            radioButtonB.Enabled = true;
            radioButtonC.Enabled = true;
            radioButtonD.Enabled = true;

            displayNextQuestion();
        }

        private void displayNextQuestion()
        {
            currentQuestion++;
            //If it was the final question
            if(currentQuestion>= questions.Count)
            {
                logger.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.FFF") + ";End");
                logger.Add(radioButtonA.RectangleToScreen(radioButtonA.ClientRectangle).ToString());
                logger.Add(radioButtonB.RectangleToScreen(radioButtonB.ClientRectangle).ToString());
                logger.Add(radioButtonC.RectangleToScreen(radioButtonC.ClientRectangle).ToString());
                logger.Add(radioButtonD.RectangleToScreen(radioButtonD.ClientRectangle).ToString());
                for (int i = 0; i < answers.Length; i++)
                {
                    logger[i + 1] = logger[i + 1] + ";" + answers[i];
                }
                //Save the log
                File.WriteAllLines(textBoxPathQuestions.Text + "\\" + textBoxNameRecording.Text + ".log", logger);
                //logger.Add(radioButtonA.ClientRectangle.ToString());
                MessageBox.Show("All questions have been processed. Thank you!");
                //Save the log
                File.WriteAllLines(textBoxPathQuestions.Text + "\\" + textBoxNameRecording.Text + ".log", logger);
                //Close the application
                this.Close();
            }
            //Else display the question
            else
            {
                logger.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.FFF") + ";Question" + (currentQuestion+1));
                labelQuestion.Text = "Question: " + (currentQuestion+1) + "/" + questions.Count ;
                richTextBox1.Text = questions[currentQuestion].question;
                radioButtonA.Text = questions[currentQuestion].answerA;
                radioButtonB.Text = questions[currentQuestion].answerB;
                radioButtonC.Text = questions[currentQuestion].answerC;
                radioButtonD.Text = questions[currentQuestion].answerD;
                radioButtonA.Checked = false;
                radioButtonB.Checked = false;
                radioButtonC.Checked = false;
                radioButtonD.Checked = false;
                answers[currentQuestion] = null;

            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            //Check if the current question has an answer
            if (answers[currentQuestion] != null)
            {
                FormSure f = new FormSure();
                f.ShowDialog();
                sures.Add(f);
                displayNextQuestion();
            }
        }

        private void radioButtonA_CheckedChanged(object sender, EventArgs e)
        {
            answers[currentQuestion] = "A";
        }

        private void radioButtonB_CheckedChanged(object sender, EventArgs e)
        {
            answers[currentQuestion] = "B";
        }

        private void radioButtonC_CheckedChanged(object sender, EventArgs e)
        {
            answers[currentQuestion] = "C";
        }

        private void radioButtonD_CheckedChanged(object sender, EventArgs e)
        {
            answers[currentQuestion] = "D";
        }
    }
}
