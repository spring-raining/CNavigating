using System;
using System.Collections.Generic;
using System.Text;

namespace CNavigating
{
    public enum Encode
    {
        ShiftJIS,
        ISO2022JP,
        EUCJP,
        UTF8
    }

    class DataContainer
    {
        public string path
        {
            get;
            set;
        }

        public string[][] data
        {
            get;
            set;
        }
    }

}
