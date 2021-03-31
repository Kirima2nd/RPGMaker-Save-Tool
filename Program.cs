using System;
using System.IO;
using LZStringCSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace RPGMV_Save_Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("                                  ");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("| RPGMaker Save Converter v0.1.2 |");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("                                  ");
            ///(file)(\d+)[.](rpgsave)
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: Invalid path file.");
                Console.WriteLine("----------------------------------");
                return;
            }

            
            if (!Regex.Match(Path.GetFileName(args[0]), @"(file)(\d+)").Success)
            {
                Console.WriteLine("ERROR: Invalid file name");
                Console.WriteLine("----------------------------------");
                return;
            }

            if (Path.GetExtension(args[0]) == ".rpgsave")
            {
                Console.WriteLine("INFO: Decoding the file...");
                DecodeSave(args[0]);
            }
            else if (Path.GetExtension(args[0]) == ".json")
            {
                Console.WriteLine("INFO: Encoding the file...");
                EncodeSave(args[0]);
            }
            else {
                Console.WriteLine("ERROR: Invalid file name");
                Console.WriteLine("----------------------------------");
                return;
            }
        }
        static void DecodeSave(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string textFile = LZString.DecompressFromBase64(sr.ReadToEnd());
            sr.Close();

            string savePath = Path.GetDirectoryName(filePath)
                                + Path.DirectorySeparatorChar
                                + Path.GetFileNameWithoutExtension(filePath)
                                + ".json"
                                ;

            File.WriteAllText(savePath, PretifyJSON(textFile));

            Console.WriteLine("INFO: Success decoding the file.");
            Console.WriteLine("----------------------------------");
        }
        static void EncodeSave(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            string textFile = sr.ReadToEnd();
            sr.Close();

            string EncodeFile = LZString.CompressToBase64(MinifyJSON(textFile));

            string savePath = Path.GetDirectoryName(filePath)
                                + Path.DirectorySeparatorChar
                                + Path.GetFileNameWithoutExtension(filePath)
                                + ".rpgsave"
                                ;

            File.WriteAllText(savePath, EncodeFile);
 
            Console.WriteLine("INFO: Success encoding the file.");
            Console.WriteLine("----------------------------------");
        }
        private static string PretifyJSON(string str)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(str);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private static string MinifyJSON(string str)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(str);
            return JsonConvert.SerializeObject(parsedJson, Formatting.None);
        }
}
}
