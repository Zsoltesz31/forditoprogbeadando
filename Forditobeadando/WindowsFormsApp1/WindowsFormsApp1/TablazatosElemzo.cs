using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;



namespace WindowsFormsApp1
{
    class TablazatosElemzo
    {
        

        private string input;
        private string convertedInput;
        private string path = "";
        private Stack<string> verem = new Stack<string>();
        private List<string> lepesek = new List<string>();

        public List<string> Lepesek
        {
            get { return lepesek; }
            set { lepesek = value; }
        }


        public string ConvertedInput
        {
            get { return convertedInput; }
            set { convertedInput = value; }
        }

        public Stack<string> Verem
        {
            get { return verem; }
            set { verem = value; }
        }

        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

   
        public void convertInput(string expressionstring)
        {

            ConvertedInput = Regex.Replace(expressionstring, "[0-9]+", "i");

        }

        public string getVeremErtekek()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Verem)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
        public string getListaErtekek()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Lepesek)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }

    }
}
