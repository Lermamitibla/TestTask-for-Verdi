using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsTest
{
    class Cell
    {
        internal static int staticID { get; set; }
        public int ID { get; }
        public string Start { get; set; }
        public string End { get; set; }
        
        static Cell()
        {
            staticID = 1;           
        }

        public Cell()
        {
            ID = staticID++;
        }

        public override string ToString()
        {
            return String.Format("ID = {0} Start = {1} End = {2}", ID, Start.ToString(), End.ToString());
        }

    }
}
