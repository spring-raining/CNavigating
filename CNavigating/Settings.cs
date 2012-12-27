using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace CNavigating
{

    public partial class Settings : Form
    {
        public static List<Color> colorSettings = new List<Color>();
        TextBox[] textBoxes = new TextBox[9];
        Button[] checkColorBoxes = new Button[9];
        Button[] printColorBoxes = new Button[9];
        public int lastselect;

        public Settings()
        {
            InitializeComponent();

            //for (int i = 0; i < 9; i++)
            //{
            //    string fieldname = string.Format("textBox{0}", i + 1);
            //    this.TextBoxes[i] = (TextBox)this.GetType().GetField(fieldname).GetValue(this);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var CNavigating = new CNavigating();
            var CSV = new CSV();
            var Color = new Color();



            string path = CNavigating.openFile("CSVファイルを開く", "CSVファイル (*.CSV)|*.CSV");
            if (path == null) return;

            try
            {
                if (CNavigating.judgeEncode(path) == Encode.ShiftJIS)
                {
                    var parser = new TextFieldParser(path, Encoding.GetEncoding("shift_jis"));
                    foreach (Color item in Color.addColor(CSV.Open(parser), path))
                    {
                        switch (item.number)
                        {
                            case 1: checkColorBox1.BackColor = item.checkcolor; printColorBox1.BackColor = item.printcolor; textBox1.Text = item.label; break;
                            case 2: checkColorBox2.BackColor = item.checkcolor; printColorBox2.BackColor = item.printcolor; textBox2.Text = item.label; break;
                            case 3: checkColorBox3.BackColor = item.checkcolor; printColorBox3.BackColor = item.printcolor; textBox3.Text = item.label; break;
                            case 4: checkColorBox4.BackColor = item.checkcolor; printColorBox4.BackColor = item.printcolor; textBox4.Text = item.label; break;
                            case 5: checkColorBox5.BackColor = item.checkcolor; printColorBox5.BackColor = item.printcolor; textBox5.Text = item.label; break;
                            case 6: checkColorBox6.BackColor = item.checkcolor; printColorBox6.BackColor = item.printcolor; textBox6.Text = item.label; break;
                            case 7: checkColorBox7.BackColor = item.checkcolor; printColorBox7.BackColor = item.printcolor; textBox7.Text = item.label; break;
                            case 8: checkColorBox8.BackColor = item.checkcolor; printColorBox8.BackColor = item.printcolor; textBox8.Text = item.label; break;
                            case 9: checkColorBox9.BackColor = item.checkcolor; printColorBox9.BackColor = item.printcolor; textBox9.Text = item.label; break;
                        }
                    }
                }
                else if (CNavigating.judgeEncode(path) == Encode.UTF8)
                {
                    var parser = new TextFieldParser(path);
                    foreach (Color item in Color.addColor(CSV.Open(parser), path))
                    {
                        switch (item.number)
                        {
                            case 1: checkColorBox1.BackColor = item.checkcolor; printColorBox1.BackColor = item.printcolor; textBox1.Text = item.label; break;
                            case 2: checkColorBox2.BackColor = item.checkcolor; printColorBox2.BackColor = item.printcolor; textBox2.Text = item.label; break;
                            case 3: checkColorBox3.BackColor = item.checkcolor; printColorBox3.BackColor = item.printcolor; textBox3.Text = item.label; break;
                            case 4: checkColorBox4.BackColor = item.checkcolor; printColorBox4.BackColor = item.printcolor; textBox4.Text = item.label; break;
                            case 5: checkColorBox5.BackColor = item.checkcolor; printColorBox5.BackColor = item.printcolor; textBox5.Text = item.label; break;
                            case 6: checkColorBox6.BackColor = item.checkcolor; printColorBox6.BackColor = item.printcolor; textBox6.Text = item.label; break;
                            case 7: checkColorBox7.BackColor = item.checkcolor; printColorBox7.BackColor = item.printcolor; textBox7.Text = item.label; break;
                            case 8: checkColorBox8.BackColor = item.checkcolor; printColorBox8.BackColor = item.printcolor; textBox8.Text = item.label; break;
                            case 9: checkColorBox9.BackColor = item.checkcolor; printColorBox9.BackColor = item.printcolor; textBox9.Text = item.label; break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                var title = new CNavigating();
                MessageBox.Show("カラー情報を読み込めませんでした。", title.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorSettings.Clear();
            for (int i = 0; i < 9; i++)
            {
                Color tmp = new Color();
                tmp.label = textBoxes[i].Text;
                tmp.checkcolor = checkColorBoxes[i].BackColor;
                tmp.printcolor = printColorBoxes[i].BackColor;
                tmp.number = i + 1;
                colorSettings.Add(tmp);
            }

            this.Close();
        }


        void button3_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        void color_Click(object sender, System.EventArgs e)
        {
            var cd = new ColorDialog();
            Button button = (Button)sender;
            if (cd.ShowDialog() == DialogResult.OK)
                button.BackColor = cd.Color;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBoxes[0] = textBox1;
            textBoxes[1] = textBox2;
            textBoxes[2] = textBox3;
            textBoxes[3] = textBox4;
            textBoxes[4] = textBox5;
            textBoxes[5] = textBox6;
            textBoxes[6] = textBox7;
            textBoxes[7] = textBox8;
            textBoxes[8] = textBox9;
            checkColorBoxes[0] = checkColorBox1;
            checkColorBoxes[1] = checkColorBox2;
            checkColorBoxes[2] = checkColorBox3;
            checkColorBoxes[3] = checkColorBox4;
            checkColorBoxes[4] = checkColorBox5;
            checkColorBoxes[5] = checkColorBox6;
            checkColorBoxes[6] = checkColorBox7;
            checkColorBoxes[7] = checkColorBox8;
            checkColorBoxes[8] = checkColorBox9;
            printColorBoxes[0] = printColorBox1;
            printColorBoxes[1] = printColorBox2;
            printColorBoxes[2] = printColorBox3;
            printColorBoxes[3] = printColorBox4;
            printColorBoxes[4] = printColorBox5;
            printColorBoxes[5] = printColorBox6;
            printColorBoxes[6] = printColorBox7;
            printColorBoxes[7] = printColorBox8;
            printColorBoxes[8] = printColorBox9;
        }

    }
}
