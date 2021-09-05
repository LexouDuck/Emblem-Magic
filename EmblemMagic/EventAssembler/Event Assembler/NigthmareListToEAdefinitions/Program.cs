using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NigthmareListToEAdefinitions
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string file in args)
            {
                if (File.Exists(file))
                {
                    string defFile = Path.ChangeExtension(file, ".event");
                    StreamWriter sw = new StreamWriter(defFile);
                    StreamReader sr = new StreamReader(file);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] splitLine = line.Split(' ');
                        if (splitLine.Length > 1)
                        {
                            string newLine = "#define ";
                            for (int i = 1; i < splitLine.Length; i++)
                            {
                                newLine += splitLine[i];
                            }
                            newLine += " " + splitLine[0];
                            sw.WriteLine(newLine); 
                        }
                    }
                    sw.Close();
                    sr.Close();
                }
            }
        }
    }
}
