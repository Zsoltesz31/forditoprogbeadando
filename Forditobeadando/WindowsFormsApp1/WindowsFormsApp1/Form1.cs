using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;

namespace WindowsFormsApp1
{ 


    public partial class Form1 : Form
    {
        TablazatosElemzo te = new TablazatosElemzo();
        string[,] tablazatMatrix;
        int veremIndex=-1;
        int inputIndex=-1;
        int SzalagIndex = 0;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
                openFileDialog1.ShowDialog();
                filenameTxtBox.Text = openFileDialog1.FileName;
                te.Path = filenameTxtBox.Text;
                loadExcelToTable(te.Path); 
        }

        private void atalakitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (bemenetTxtBox.Text == "")
                {
                    throw new missingInputException(String.Format("Hiányzó input!"));
                }
                te.Input = bemenetTxtBox.Text + "#";
                te.convertInput(te.Input);
                atalakitottInputTxtBox.Text = te.ConvertedInput;
                te.Verem.Push("#");
                te.Verem.Push("E");
            }
            catch (Exception ex)
            {
                label4.ForeColor = Color.Red;
                label4.Text = ex.Message;
            }
           

        }
        
        public void loadExcelToTable(string path)
        {
            try
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string[] columns = sr.ReadLine().Split(';');
                    foreach (var col in columns)
                    {
                        dataGridView1.Columns.Add(col, col);
                    }
                    dataGridView1.Invalidate();
                    while (!sr.EndOfStream)
                    {
                        string[] row = sr.ReadLine().Split(';');
                        dataGridView1.Rows.Add(row);
                    }
                
                }  

            }
            catch (Exception ex)
            {

                label4.ForeColor = Color.Red;
                label4.Text = ex.Message;
            }
        }

        public void Elemez()
        {
            while (true)
            {
                if (dataGridView1.Rows.Count==0)
                {
                    throw new missingFileException(String.Format("Hiányzik az elemző táblázat"));
                }

                if (te.ConvertedInput=="")
                {
                    throw new missingInputException(String.Format("Hiányzó ellenőrízendő input"));
                }

                veremIndex = -1;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (te.Verem.Peek() == dataGridView1.Rows[i].Cells[0].FormattedValue.ToString())
                    {
                        veremIndex = i;
                        break;
                    }
                }

                inputIndex = -1;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (te.ConvertedInput[SzalagIndex].ToString() == dataGridView1.Columns[i].Name)
                    {
                        inputIndex = i;
                        break;
                    }
                }

                //Hibás lépés esetén

                if (dataGridView1[inputIndex,veremIndex].Value.ToString() == "")
                {
                    throw new incorectException(String.Format("Hibás input. A hiba pozíciója: {0}", SzalagIndex));
                    break;
                } //elfogadó állapot vizsgálata
                else if (dataGridView1[inputIndex, veremIndex].Value.ToString() == "elfogad")
                {
                    //sikeres a végrehajtás
                    label4.ForeColor = Color.Green;
                    label4.Text = "Sikeres ellenőrzés!";
                    break;
                }
                else if (dataGridView1[inputIndex, veremIndex].Value.ToString() == "pop")
                {
                    te.Verem.Pop();
                    SzalagIndex++;
                }
                else
                {
                    string[] rule = dataGridView1[inputIndex, veremIndex].FormattedValue.ToString().Substring(1, dataGridView1[inputIndex, veremIndex].FormattedValue.ToString().Length - 2).Split(',');
                    te.Verem.Pop();

                    for (int i = rule[0].Length -1 ; i >= 0; i--)
                    {
                        if (rule[0] == "e")
                        {
                            listBox1.Items.Add($"{te.ConvertedInput.Substring(SzalagIndex)}, {te.getVeremErtekek()}, {te.getListaErtekek()}");
                            continue;
                        }
                        if (rule[0][i] == '\'')
                        {
                                te.Verem.Push(rule[0].Substring(i - 1, 2));
                            i--;
                            continue;
                        }
                        te.Verem.Push(rule[0][i].ToString());
                        
                        listBox1.Items.Add($"{te.ConvertedInput.Substring(SzalagIndex)}, {te.getVeremErtekek()}, {te.getListaErtekek()}");
                        
                    }
                }
            }

          

        }

        public void Reset()
        {
            te.Verem.Clear();
            listBox1.Items.Clear();
            bemenetTxtBox.Text = "";
            atalakitottInputTxtBox.Text = "";
            te.Input = "";
            te.ConvertedInput = "";
            SzalagIndex = 0;
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Elemez();
            }
            catch (Exception ex)
            {
                label4.ForeColor =Color.Red;
                label4.Text=ex.Message;
            }
            
        }
        [Serializable]
        class incorectException : Exception
        {
            public incorectException() { }
            public incorectException(string message) : base(message) { }
            public incorectException(string message, Exception inExcp) : base(message, inExcp) { }
            public incorectException(SerializationInfo info, StreamingContext contect) : base(info, contect) { }

        }
        class missingInputException : Exception
        {
            public missingInputException() { }
            public missingInputException(string message) : base(message) { }
            public missingInputException(string message, Exception inExcp) : base(message, inExcp) { }
            public missingInputException(SerializationInfo info, StreamingContext contect) : base(info, contect) { }

        }

        class missingFileException : Exception
        {
            public missingFileException() { }
            public missingFileException(string message) : base(message) { }
            public missingFileException(string message, Exception inExcp) : base(message, inExcp) { }
            public missingFileException(SerializationInfo info, StreamingContext contect) : base(info, contect) { }

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
