using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApplication2
{
    class Program
    {
        private static readonly XNamespace ttmlns = "http://www.w3.org/ns/ttml";

        static void Main(string[] args)
        {
            Console.WriteLine("Here we go");
            ParseTTML("C:\\Temp\\AATEMP\\Abiola.mp4 - Indexed\\Abiola.mp4.ttml");
            Console.ReadLine();
        }
            private static string ParseTTML(string ttmlFile)
        {
            // This will extract all the spoken text from a TTML file into a single string
            var d = XDocument.Load(ttmlFile);
            var b = d.Element(ttmlns + "tt")
                     .Element(ttmlns + "body")
                     .Element(ttmlns + "div")
                     .Elements(ttmlns + "p");
            string t;
            IEnumerable<XAttribute> segmentAtts;
            foreach (XElement x in b)
            {
                segmentAtts = x.Attributes();
                foreach (XAttribute x2 in segmentAtts)
                    t = x2.ToString();
            }
            return b.ToString();
        }
    }
    
}
