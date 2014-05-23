using System;
using UHI;
using System.Text;

namespace UHITest
{
	class MainClass
	{
		static bool running = true;

		public static void Main(string[] args)
		{
			while (running)
			{
				Console.WriteLine("Read file (r), Index folder (i), Differences (d), Quit (q)");
				string option = Console.ReadLine().Trim().ToLower();

				if (option == "q")
				{
					running = false;
					continue;
				}
				else if (option == "r")
				{
					try
					{
						IndexParser indexParser = new IndexParser();
						FileInfo[] output = indexParser.Parse("./test.txt", Encoding.UTF8);

						foreach (FileInfo fi in output)
						{
							Console.WriteLine(fi.FileNumber);
							Console.WriteLine(" > " + fi.FilePath);
							Console.WriteLine(" > " + fi.HashString);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error >> " + ex.Message);
					}
				}
				else if (option == "d")
				{
					try
					{
						IndexParser indexParser = new IndexParser();
						FileInfo[] output = indexParser.Parse("./test.txt", Encoding.UTF8);
						FileInfo[] output2 = indexParser.Parse("./test2.txt", Encoding.UTF8);
						Comparator comparator = new Comparator();

						foreach (Difference file in comparator.GetDifferences(output, output2))
						{
							Console.WriteLine(file.TypeOfDifference);
							Console.WriteLine(" > " + file.FilePaths[0]);
							if (file.FilePaths.Length == 2)
							{
								Console.WriteLine(" > " + file.FilePaths[1]);
							}
							Console.WriteLine("  > " + file.HashStrings[0]);
							if (file.HashStrings.Length == 2)
							{
								Console.WriteLine("  > " + file.HashStrings[1]);
							}
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error >> " + ex.Message);
					}
				}
				else if (option == "i")
				{
					Console.WriteLine("Enter directory to index");
					string dir = Console.ReadLine();
					Console.WriteLine("Enter hash algorythm number (1 - 6)");
					int hash = int.Parse(Console.ReadLine().Trim());
					Console.WriteLine("Multi thread hashing 1 / 0");
					bool multi = (Console.ReadLine().Trim() == "1") ? true : false;

					try
					{
						Indexer indexer = new Indexer(dir, (Hash)hash);
						long output = indexer.IndexToFile("./test.txt", multi);
						Console.WriteLine("Time elpsed " + output + " ms (" + (output / 1000) + " s " + (output / 1000 / 60) + " m)");
					}
					catch (Exception ex)
					{
						Console.WriteLine("Error >> " + ex.Message);
					}
				}

				Console.ReadKey();
			}
		}
	}
}
