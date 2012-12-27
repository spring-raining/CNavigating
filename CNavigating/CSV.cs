using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace CNavigating
{
    class CSV
    {
        /// <summary>
        /// CSVファイルをstring[][]型に変換する。
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string[][] Open(TextFieldParser parser)
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            var data = new List<string[]>();
            while (!parser.EndOfData)
            {
                data.Add(parser.ReadFields());
            }

            return data.ToArray();
        }

        /// <summary>
        /// MemberList中のファイルを統合してCSV形式で出力する。
        /// </summary>
        public static void Save(string[][] parser, string path)
        {
            Encoding encode = new UTF8Encoding(false);
            StreamWriter result = new StreamWriter(path, false, encode);

            for (int i = 0; i < parser.Length; i++)
            {
                for (int j = 0; j < parser[i].Length; j++)
                {
                    if (j > 0)
                        result.Write(',');

                    string tmp = parser[i][j];

                    if (tmp.IndexOf('"') > -1 ||
                        tmp.IndexOf(',') > -1 ||
                        tmp.IndexOf('\r') > -1 ||
                        tmp.IndexOf('\n') > -1 ||
                        tmp.StartsWith(" ") || tmp.StartsWith("\t") ||
                        tmp.EndsWith(" ") || tmp.EndsWith("\t"))
                    {
                        if (tmp.IndexOf('"') > -1)
                        {
                            //"を""とする
                            tmp = tmp.Replace("\"", "\"\"");
                        }
                        tmp = "\"" + tmp + "\"";
                    }
                    result.Write(tmp);
                }
                result.Write("\n");
            }

            result.Close();
        }
    }
}
