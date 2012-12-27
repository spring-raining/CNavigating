using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace CNavigating
{
    public partial class CNavigating : Form
    {
        const string VERSION = "CNavigating 1.0";

        public CNavigating()
        {
            InitializeComponent();
            memberDataColumnLoad();
            toolStripStatusLoad();
        }

        public string openFile(string title, string filter)
        {
            var path = new OpenFileDialog();
            path.Title = title;
            path.Filter = filter;

            if (path.ShowDialog() == DialogResult.OK) return path.FileName;
            else return null;
        }

        public string saveFile(string title, string filter)
        {
            var path = new SaveFileDialog();
            path.Title = title;
            path.Filter = filter;

            if (path.ShowDialog() == DialogResult.OK) return path.FileName;
            else return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var CSV = new CSV();

            string path = openFile("CSVファイルを開く", "CSVファイル (*.CSV)|*.CSV");
            if (path == null) return;

            try
            {
                if (judgeEncode(path) == Encode.ShiftJIS)
                {
                    TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding(932));
                    addMember(CSV.Open(parser), path);
                }
                else if (judgeEncode(path) == Encode.EUCJP)
                {
                    TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding(51932));
                    addMember(CSV.Open(parser), path);
                }
                else if (judgeEncode(path) == Encode.ISO2022JP)
                {
                    TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding(50220));
                    addMember(CSV.Open(parser), path);
                }
                else if (judgeEncode(path) == Encode.UTF8)
                {
                    TextFieldParser parser = new TextFieldParser(path);
                    addMember(CSV.Open(parser), path);
                }
            }
            catch (Exception)
            {
                var title = new CNavigating();
                MessageBox.Show(Path.GetFileName(path) + " を開くことができませんでした。", title.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = saveFile("CSVファイルを保存", "CSVファイル (*.CSV)|*.CSV");
            if (path == null) return;

            Cursor preCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.AppStarting;
                string[][] tmp = mergeMemberC83();
                CSV.Save(tmp, path);
            }
            catch (Exception)
            {
                var title = new CNavigating();
                MessageBox.Show("チェックリストを統合することができませんでした。", title.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = preCursor;
            }
            memberList.Clear();
            memberData.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (memberList.SelectedIndices.Count == 0) return;

            foreach (ListViewItem item in memberList.SelectedItems)
            {
                memberList.Items.Remove(item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Settings setting = new Settings();
            setting.ShowDialog(this);
        }

        private void selectedMemberChanged(object sender, EventArgs e)
        {
            var selectedMember = new ListViewItem();
            var Color = new Color();

            memberData.Clear();
            memberDataColumnLoad();
            if ( memberList.SelectedItems.Count == 0) return;

            selectedMember = memberList.SelectedItems[0];
            DataContainer obj = selectedMember.Tag as DataContainer;
            if (obj == null) return;

            System.Drawing.Color[] colorlist = new System.Drawing.Color[9];
            for (int i = 0; i < obj.data.GetLength(0); i++)
            {
                if (obj.data[i][0] != "Color") continue;
                try
                {
                    colorlist[int.Parse(obj.data[i][1]) - 1] = Color.ToSystemColor(obj.data[i][2]);
                }
                catch (Exception) { }
            }

            for (int i = 0; i < obj.data.GetLength(0); i++)
            {
                if (obj.data[i][0] != "Circle") continue;
                var tmp = new ListViewItem();
                var c = new System.Drawing.Color();
                if (int.Parse(obj.data[i][2]) == 0) c = System.Drawing.Color.White;
                else c = colorlist[int.Parse(obj.data[i][2]) - 1];
                string s = "";
                if (obj.data[i][21] == "0") s = "a";
                else if (obj.data[i][21] == "1") s = "b";
                tmp.UseItemStyleForSubItems = false;
                tmp.Text = "";
                tmp.BackColor = c;
                tmp.SubItems.Add(obj.data[i][5] + " " + obj.data[i][6] + " " + obj.data[i][7] + "-" + obj.data[i][8] + s);
                tmp.SubItems.Add(obj.data[i][10]);
                tmp.SubItems.Add(obj.data[i][17]);
                memberData.Items.Add(tmp);
            }
        }

        private void addMember(string[][] data, string path)
        {
            var memberListData = new ListViewItem();
            var dataContainer = new DataContainer();
            dataContainer.path = path;
            dataContainer.data = data;

            memberListData.Text = Path.GetFileNameWithoutExtension(path);
            memberListData.Tag = dataContainer;

            memberList.Items.Add(memberListData);
        }

        /// <summary>
        /// チェックリストを統合し、string[][]型で返す。
        /// </summary>
        /// <returns></returns>
        private string[][] mergeMemberC83()
        {
            var result = new System.Collections.Generic.List<string[]>();
            var recordCircle = new System.Collections.Generic.List<int>();
            var Color = new Color();

            string[] header = { "Header", "ComicMarketCD-ROMCatalog", "ComicMarket83", "UTF-8", VERSION };
            result.Add(header);

            foreach (ListViewItem item in memberList.Items)
            {
                DataContainer obj = item.Tag as DataContainer;
                if (obj == null) continue;

                for (int i = 0; i < obj.data.GetLength(0); i++)
                {
                    if (obj.data[i][0] != "Circle") continue;
                    try
                    {
                        if (recordCircle.IndexOf(int.Parse(obj.data[i][1])) < 0)
                        {
                            result.Add(obj.data[i]);
                            recordCircle.Add(int.Parse(obj.data[i][1]));
                        }
                        else
                        {
                            int m = recordCircle.IndexOf(int.Parse(obj.data[i][1])) + 1;
                            string[] tmp = result.ToArray()[m];
                            int tmp1 = int.Parse(tmp[2]);
                            int tmp2 = int.Parse(obj.data[i][2]);
                            if (tmp1 == 0) tmp1 = 10;
                            if (tmp2 == 0) tmp2 = 10;
                            if (tmp1 > tmp2)
                                tmp1 = tmp2;
                            if (tmp1 >= 10) tmp1 = 0;
                            tmp[2] = tmp1.ToString();

                            tmp[17] += obj.data[i][17];
                            result.RemoveAt(result.Count);
                            result.Add(tmp);
                        }
                    }
                    catch (Exception) { continue; }
                }
            }

            foreach (Color item in Settings.colorSettings)
            {
                if (item.label == null) continue;
                string[] tmpcolor = { "Color", item.number.ToString(), Color.ToBGRColor(item.checkcolor), Color.ToBGRColor(item.printcolor), item.label };
                result.Add(tmpcolor);
            }

            return result.ToArray();
        }

        /// <summary>
        /// 与えられたパスのCSVファイルのエンコードを返す。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Encode judgeEncode(string path)
        {
            TextFieldParser parser = new TextFieldParser(path);
            string[][] data = CSV.Open(parser);
            var result = Encode.UTF8;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (data[i][0] == "Header" && data[i][3] == "Shift_JIS") result = Encode.ShiftJIS;
                else if (data[i][0] == "Header" && data[i][3] == "ISO-2022-JP") result = Encode.ISO2022JP;
                else if (data[i][0] == "Header" && data[i][3] == "EUC-JP") result = Encode.EUCJP;
            }

            return result;
        }

        private void memberDataColumnLoad()
        {
            memberData.Columns.Add("色", 25);
            memberData.Columns.Add("配置", 85);
            memberData.Columns.Add("サークル名", 150);
            memberData.Columns.Add("コメント", -2);
        }

        private void toolStripStatusLoad()
        {
            toolStripStatusLabel1.Text = VERSION;
        }
    }
}
