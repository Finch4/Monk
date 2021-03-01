using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using F23.StringSimilarity;
using Microsoft.VisualBasic;


namespace Monk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string jsonz = "";

        //https://stackoverflow.com/users/975724/nicholas-miller
        public static string ReplaceAt(string str, int index, string replace)
        {
            return str.Remove(index, 1)
                    .Insert(index, replace);
        }

        void database_search(string bytes, bool jaro_state = false, double jaro_rate = 0)
        {
            var jw = new JaroWinkler();
            string[] bytes_splitted = bytes.Split(',');
            StreamReader database = new StreamReader(jsonz);
            string json = database.ReadToEnd();
            dynamic DynamicData = JsonConvert.DeserializeObject(json);
            foreach (string bytex in bytes_splitted)
            {
                for (int i = 0; i < DynamicData.Data.Count; i++)
                {
                    for (int x = 0; x < DynamicData.Data[i].Bytes.Count; x++)
                    {
                        if (jaro_state == true)
                        {
                            double jaro = jw.Similarity(Convert.ToString(DynamicData.Data[i].Bytes[x]), bytex);
                            if (jaro > jaro_rate)
                            {
                                //MessageBox.Show(String.Format("Found a match: {0} and {1} with jaro rate {2} in {3}", bytex, DynamicData.Data[i].Bytes[x], jaro_rate, DynamicData.Data[i].Name));
                                DataGridViewRow row = (DataGridViewRow)gunaDataGridView2.Rows[0].Clone();
                                row.Cells[0].Value = bytex;
                                row.Cells[1].Value = DynamicData.Data[i].Bytes[x];
                                row.Cells[2].Value = Math.Round(jaro,2);
                                row.Cells[3].Value = DynamicData.Data[i].Name;
                                gunaDataGridView2.Invoke(new Action(() => { gunaDataGridView2.Rows.Add(row); }));
                            }
                        }
                    }
                        if (jaro_state == false)
                        {
                            if (Convert.ToString(DynamicData.Data[i].Bytes).Contains(bytex))
                            {
                                //MessageBox.Show(String.Format("Found a match: {0} in {1}", bytex, DynamicData.Data[i].Name));
                                 DataGridViewRow row = (DataGridViewRow)gunaDataGridView2.Rows[0].Clone();
                                row.Cells[0].Value = bytex;
                                row.Cells[3].Value = DynamicData.Data[i].Name;
                                gunaDataGridView2.Invoke(new Action(() => { gunaDataGridView2.Rows.Add(row); }));
                            }
                        }
                }
            }
        }
        void analyze(string file_1, string file_2 = "", int increaser = 8, double jaro_rate = 0, string combobox_text = "", bool filter_state = false, bool database_bool = false, string filterx = "")
        {
            Random random = new Random();
            var jw = new JaroWinkler();
            string report_name = String.Format("{0}_report.txt", random.Next(0, 1000));
            increaser *= 2;
            byte[] file = System.IO.File.ReadAllBytes(file_1);
            byte[] file2 = System.IO.File.ReadAllBytes(file_2);
            string s1 = BitConverter.ToString(file).Replace("-", "");
            string s2 = BitConverter.ToString(file2).Replace("-", "");
            int temp_value = increaser;
            int counter_1 = 0;
            int counter_2 = increaser;
            int counter_3 = 0;
            StreamWriter writeout = new StreamWriter(String.Format(String.Format("{0}",report_name),file_1,file_2), true); //true indicates appending
            List<string> bytes_written = new List<string>();
            for (int i = 0; i <= s1.Length; i+=increaser)
            {
                char[] taken = s1.Substring(counter_1,counter_2).ToArray();
                int temp = counter_1;
                string converted = new String(taken);
                string temp_converted = converted;
                while (counter_3 <= s2.Length-counter_2)
                {
                    if (counter_3 == s2.Length - counter_2)
                    {
                        counter_2 = s2.Length - counter_3;
                    }
                    char[] taken2 = s2.Substring(counter_3, counter_2).ToArray();
                    string converted2 = new String(taken2);
                    double jw_similarity = jw.Similarity(temp_converted.Replace("0", ""), converted2.Replace("0", ""));
                    if (jw_similarity >= jaro_rate)
                    {
                        int x = 0;
                        if (jw_similarity >= jaro_rate-2)
                        {
                            while (x < temp_converted.Length)
                            {
                                if (temp_converted[x] == converted2[x] || x == 0 || x == 1)
                                {
                                    x++;
                                }
                                else
                                {
                                    temp_converted = ReplaceAt(temp_converted, x, "?");
                                    converted2 = ReplaceAt(converted2, x, "?");
                                    x++;
                                }
                            }
                        }
                        if (temp_converted.Count(f => f == '?') > temp_converted.Length / 2 || temp_converted.Count(f => f == '0') > temp_converted.Length/2 || converted2.Count(f => f == '0') > converted2.Length/2 || converted2.Count(f => f == '0') > converted2.Length/2 || jw_similarity < jaro_rate)
                        {
                            temp_converted = converted;
                        }
                        else
                        {
                            if (bytes_written.Contains(temp_converted+"("+converted2+")"))
                            {
                                //
                            }
                            else
                            {
                                writeout.WriteLine(temp_converted+"("+converted2+")");
                                bytes_written.Add(temp_converted+"("+converted2+")");
                                if (database_bool == true)
                                {
                                    database_search(temp_converted.Replace("0", ""), true, jaro_rate);
                                }
                                else
                                {
                                    //
                                }
                            }

                        }

                    }
                    counter_3 += increaser;         
                }
                counter_1 = temp+increaser;
                counter_3 = 0;
                i++;
            }
            DataGridViewRow row = (DataGridViewRow)gunaDataGridView1.Rows[0].Clone();
            row.Cells[0].Value = file_1;
            row.Cells[1].Value = file_2;
            row.Cells[2].Value = report_name;
            row.Cells[3].Value = combobox_text;
            
            gunaDataGridView1.Invoke(new Action(() => { gunaDataGridView1.Rows.Add(row); }));
            writeout.Close();
            if (filter_state == true)
            {
                filter(report_name, filterx,true);
                row.Cells[4].Value = true;
                row.Cells[5].Value = report_name + "_filtered.txt";
            }
        }

        void filter(string file_1, string filter, bool called_by_analyze)
        {
            string[] file1 = File.ReadAllLines(file_1);
            string[] file2 = File.ReadAllLines(filter);
            StreamWriter writeout = new StreamWriter(String.Format(String.Format(file_1+"_filtered.txt"), file_1, filter), true); //true indicates appending
            foreach (string bytes in file1)
            {
                if (file2.Contains(bytes.Split('(')[0]))
                {
                    //
                }
                else
                {
                    writeout.WriteLine(bytes);
                }            
            }
            if (called_by_analyze == false)
            {
                DataGridViewRow row = (DataGridViewRow)gunaDataGridView1.Rows[0].Clone();
                row.Cells[0].Value = file_1;
                row.Cells[4].Value = true;
                row.Cells[5].Value = file_1 + "_filtered.txt";
                gunaDataGridView1.Invoke(new Action(() => { gunaDataGridView1.Rows.Add(row); }));
                writeout.Close();
            }
            else
            {
                //
            }
        }

        void compare(string report_1, string report_2)
        {
            string[] file1 = File.ReadAllLines(report_1);
            string[] file2 = File.ReadAllLines(report_2);
            StreamWriter writeout = new StreamWriter(String.Format(String.Format(report_1+"_compared.txt"), report_1, report_2), true); //true indicates appending
            foreach (string bytes in file1)
            {
                if (file2.Contains(bytes))
                {
                    writeout.WriteLine(bytes);
                }
                else
                {
                    //
                }
            }
            DataGridViewRow row = (DataGridViewRow)gunaDataGridView1.Rows[0].Clone();
            row.Cells[0].Value = report_1;
            row.Cells[1].Value = report_2;
            row.Cells[6].Value = true;
            row.Cells[7].Value = report_1 + "_compared.txt";
            gunaDataGridView1.Invoke(new Action(() => { gunaDataGridView1.Rows.Add(row); }));
            writeout.Close();
        }
        // Window Move
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel1_MouseUp_1(object sender, MouseEventArgs e)
        {
            dragging = false;
        }





        private void gunaButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox4.Text = openFileDialog1.FileName;
        }

        private void gunaButton5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void gunaButton6_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            bool working = true;
            double jaro_rate = 0;
            gunaDataGridView2.Rows.Clear();

            if (comboBox1.Text == "Compare Reports")
            {
                Thread childreft4 = new Thread(new ThreadStart(() => compare(textBox1.Text, textBox2.Text)));
                childreft4.Start();
            }
            else
            {
                string combobox_text = "";
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("Please select the operation type!");
                    working = false;
                }
                else
                {
                    combobox_text = comboBox1.Text;
                }
                if (textBox3.Text.Contains(".") ||textBox3.Text == "")
                {
                    MessageBox.Show(String.Format("The Jaro rate value {0} contains a \".\" please write \",\" or is empty", jaro_rate));
                    working = false;
                }
                else
                {
                    jaro_rate = Convert.ToDouble(textBox3.Text);
                }
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select the first file!");
                    working = false;
                }
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Please select the second file!");
                    working = false;
                }
                if (working == true && comboBox1.Text != "Compare Reports")
                {
                    if (checkBox1.Checked == true)
                    {
                        Thread childreft = new Thread(new ThreadStart(() => analyze(textBox1.Text, textBox2.Text, Convert.ToInt32(numericUpDown1.Value), jaro_rate, combobox_text, true, false, textBox4.Text)));
                        childreft.Start();
                    }
                    else if (!(label12.Text == "."))
                    {
                        Thread childreft = new Thread(new ThreadStart(() => analyze(textBox1.Text, textBox2.Text, Convert.ToInt32(numericUpDown1.Value), jaro_rate, combobox_text, false, true)));
                        childreft.Start();
                    }
                    else
                    {
                        Thread childreft = new Thread(new ThreadStart(() => analyze(textBox1.Text, textBox2.Text, Convert.ToInt32(numericUpDown1.Value), jaro_rate, combobox_text, false, false)));
                        childreft.Start();
                    }
                }
            }

           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void gunaButton8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string jsonx = openFileDialog1.FileName;
            if (!jsonx.Contains(".json"))
            {
                MessageBox.Show("Please select a database!");
            }
            else
            {
                jsonz = jsonx;
                StreamReader database = new StreamReader(jsonx);
                string json = database.ReadToEnd();
                dynamic DynamicData = JsonConvert.DeserializeObject(json);
                label12.Text = Convert.ToString(DynamicData.Data.Count);
                label13.Text = Convert.ToString(DynamicData.Author);
                label14.Text = Convert.ToString(DynamicData.Version);
            }
        }

        private void gunaButton7_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true && (Convert.ToString(textBox3.Text).Contains(".") || Convert.ToString(textBox3.Text) == ""))
            {
                MessageBox.Show("Search failed, please insert a jaro rate if you enabled jaro checkbox");
            }
            if (checkBox2.Checked == true)
            {
                Thread childreft2 = new Thread(new ThreadStart(() => database_search(textBox5.Text, true, Convert.ToDouble(textBox3.Text))));
                childreft2.Start();
            }
            if (checkBox2.Checked == false)
            {
                Thread childreft2 = new Thread(new ThreadStart(() => database_search(textBox5.Text)));
                childreft2.Start();
            }
        }

        private void gunaButton9_Click(object sender, EventArgs e)
        {
                openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == "")
            {
                MessageBox.Show("Please select a file!");
            }
            else
            {
                string file = openFileDialog1.FileName;
                Thread childreft3 = new Thread(new ThreadStart(() => filter(file, textBox4.Text, false)));
                childreft3.Start();
            }
        }

        private void gunaButton10_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

    }
}
