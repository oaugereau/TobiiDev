using LINQtoCSV;
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

namespace GroundTruth
{
    public partial class Form1 : Form
    {
        private List<EyeGaze> lines = new List<EyeGaze>();
        private Bitmap original;
        private static List<Color> colors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Yellow, Color.Orange, Color.Cyan, Color.DarkBlue, Color.HotPink };
        private int _keyValue;
        private Boolean _checkKeyValue = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxCSVIn.Text = ofd.FileName;
                string pathImage = ofd.FileName.Replace(".csv", "_back.png");
                textBoxCSVOut.Text = ofd.FileName.Replace(".csv", "_GT.csv");
                lines = ReadFromCsv(textBoxCSVIn.Text).ToList();
                dataGridView1.DataSource = lines;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                if (File.Exists(pathImage)) textBoxImage.Text = pathImage;
            }
        }

        public Bitmap displayGaze(Bitmap original)
        {
            using (Graphics g = Graphics.FromImage(original))
            {
                foreach (var item in lines)
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    if (item.LineNumber != 0)
                        brush = new SolidBrush(colors[item.LineNumber % colors.Count]);
                    g.FillEllipse(brush, new Rectangle((int)item.GazeX - 5, (int)item.GazeY - 5, 10, 10));
                }
            }
            return original;
        }

        public IEnumerable<EyeGaze> ReadFromCsv(string csvFile)
        {
            //Here you set some properties. Check the documentation.
            var csvFileDescription = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                SeparatorChar = ';' //Specify the separator character.
            };

            var csvContext = new CsvContext();

            return csvContext.Read<EyeGaze>(csvFile, csvFileDescription);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxImage.Text = ofd.FileName;
            }
        }

        private void textBoxImage_TextChanged(object sender, EventArgs e)
        {
            original = new Bitmap(textBoxImage.Text);
            original = displayGaze(original);
            pictureBox1.Image = original;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (lines.Count != 0)
            {
                var csvFileDescription = new CsvFileDescription
                {
                    FirstLineHasColumnNames = true,
                    SeparatorChar = ';' //Specify the separator character.
                };

                var csvContext = new CsvContext();
                csvContext.Write<EyeGaze>(lines, textBoxCSVOut.Text, csvFileDescription);
                labelSave.Text = "saved";
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = displayGaze(original);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
            {
                dataGridView1.SelectedCells[i].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && original != null)
            {
                //erase the label saved
                labelSave.Text = "";
                //print the gaze
                Bitmap copy = (Bitmap)original.Clone();
                using (Graphics g = Graphics.FromImage(copy))
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    float x = (float)dataGridView1.CurrentRow.Cells[1].Value;
                    float y = (float)dataGridView1.CurrentRow.Cells[2].Value;
                    g.FillEllipse(brush, new Rectangle((int)x - 5, (int)y - 5, 10, 10));
                }
                pictureBox1.Image = copy;
            }
        }
    }
}
