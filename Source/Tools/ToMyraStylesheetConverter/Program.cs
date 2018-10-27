using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Myra.Tools.ToMyraStylesheetConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("ToMyraStylesheetConverter converts LibGDX UI stylesheet to Myra UI stylesheet.");
				Console.WriteLine("Usage: ToMyraStylesheetConverter <input.json> <output.json>");

				return;
			}

			try
			{
				var inputFile = args[0];
				var outputFile = args[1];

				var inputData = File.ReadAllText(inputFile);

				var inputObject = JObject.Parse(inputData);

				var k = 5;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
