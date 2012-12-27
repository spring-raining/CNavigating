using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CNavigating
{
    public class Color
    {
        public int number;
        public System.Drawing.Color checkcolor;
        public System.Drawing.Color printcolor;
        public string label;

        public List<Color> addColor(string[][] data, string path)
        {
            var result = new List<Color>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (data[i][0] == "Color" && int.Parse(data[i][1]) >= 1 && int.Parse(data[i][1]) <= 9)
                {
                    var tmp = new Color();
                    tmp.number = int.Parse(data[i][1]);
                    tmp.checkcolor = ToSystemColor(data[i][2]);
                    tmp.printcolor = ToSystemColor(data[i][3]);
                    tmp.label = data[i][4];
                    result.Add(tmp);
                }
            }

            return result;
        }

        /// <summary>
        /// チェックリスト形式の色指定をColor構造体に変換する。
        /// </summary>
        /// <param name="BGRColor"></param>
        /// <returns></returns>
        public System.Drawing.Color ToSystemColor(string BGRColor)
        {
            return ColorTranslator.FromHtml("#" + BGRColor.Substring(4, 2) + BGRColor.Substring(2, 2) + BGRColor.Substring(0, 2));
        }

        /// <summary>
        /// Color構造体の色指定をチェックリスト形式に変換する。
        /// </summary>
        /// <param name="SystemColor"></param>
        /// <returns></returns>
        public string ToBGRColor(System.Drawing.Color SystemColor)
        {
            var tmp = ColorTranslator.ToHtml(SystemColor);
            return tmp.Substring(5, 2) + tmp.Substring(3, 2) + tmp.Substring(1, 2);
        }

    }
}
