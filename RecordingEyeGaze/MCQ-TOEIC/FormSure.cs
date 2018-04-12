using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCQ_TOEIC
{
    public partial class FormSure : Form
    {
        public bool sure = false;
        public bool A = false;
        public bool B = false;
        public bool C = false;
        public bool D = false;

        public FormSure()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sure = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            sure = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            A = checkBox1.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            B = checkBox2.Enabled;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            C = checkBox3.Enabled;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            D = checkBox4.Enabled;
        }
    }
}
