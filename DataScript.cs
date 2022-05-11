using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript
{
	class DataScript
	{
		static void Main(string[] args)
		{
			string path = "";
			string data;

			//if (args.Length < 2 || Array.IndexOf(args, "--file") < 0 || Array.IndexOf(args, "--file") == (args.Length - 1))
			//{
			//	Console.WriteLine("Please specify the file to use using --file <filename>");
			//	Console.ReadKey();
			//	Environment.Exit(0);
			//}
			//else if (args[Array.IndexOf(args, "--file") + 1].StartsWith("\"") && !args[Array.IndexOf(args, "--file") + 1].EndsWith("\""))
			//{
			//	for (int i = Array.IndexOf(args, "--file") + 1; !args[i-1].EndsWith("\""); i++)
			//	{
			//		path += args[i].Replace("\"", "");
			//	}


			//}
			//else
			//{
			//	path = args[Array.IndexOf(args, "--file") + 1].Replace("\"", "");
			//}
			
			data = System.IO.File.ReadAllText("format.ts");

			LexicalAnalyzer.ScanData(data).ToList().ForEach(Console.WriteLine);


		}
	}
}
