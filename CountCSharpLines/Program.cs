using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace CountCSharpLines {
	public class Program {
		public static void Main(string[] args) {
			Console.WriteLine("Enter in the path you'd like to look inside.");
			string path = Console.ReadLine();

			while (!Directory.Exists(path)) {
				Console.WriteLine("The path you supplied was invalid. Please enter in a valid path:");
				path = Console.ReadLine();
			}

			Console.WriteLine("What file extensions you'd like to include in the line count? (Enter in \"done\" when you are done)");
			string input;
			List<string> fileExtensions = new List<string>(20);
			while ((input = Console.ReadLine().Trim().ToLower()) != "done") {
				if (input.StartsWith(".")) {
					fileExtensions.Add(input);
					Console.WriteLine("Successfully added " + input + " to the list of file extensions!");
				} else
					Console.WriteLine("Unable to add " + input + " to the list of file extensions.");
			}

			int total = 0;
			List<FileLineCount> lineCounts = null;
			Task task = Task.Run(() => {
				lineCounts = CountLines(path, fileExtensions);
				total = 0;
				for (int i = 0; i < lineCounts.Count; i++)
					total += lineCounts[i].lineCount;
			});

			Thread.Sleep(100);
			Console.WriteLine("\nNow counting the lines in your files...");
			Thread.Sleep(1000);
			task.Wait();

			task = Task.Run(() => {
				lineCounts.Sort();
			});
			Thread.Sleep(100);
			Console.WriteLine("\n" + lineCounts.Count + " files were found!");
			Console.WriteLine("Now sorting the results...");
			Thread.Sleep(1000);

			int delay = 350;
			Console.WindowWidth = 170;
			Console.WindowHeight = 40;

			Console.Write(".");
			Thread.Sleep(delay);
			Console.Write(".");
			Thread.Sleep(delay);
			Console.WriteLine(".\n");
			Thread.Sleep(delay);
			task.Wait();

			for (int i = 0; i < lineCounts.Count; i++) {
				Console.WriteLine(lineCounts[i].ToString());
				Thread.Sleep(15);
			}

			Console.Write(".");
			Thread.Sleep(delay);
			Console.Write(".");
			Thread.Sleep(delay);
			Console.Write(".");
			Thread.Sleep(delay);

			Console.Write(" ");
			Thread.Sleep(delay);

			Thread.Sleep(delay);
			Console.Write(".");
			Thread.Sleep(delay);
			Console.Write(".");
			Thread.Sleep(delay);
			Console.WriteLine(".");
			Thread.Sleep(delay);
			Console.WriteLine(total + " lines were counted!");

			ExitConsole();
		}

		private static void ExitConsole() {
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey(true);
		}

		private static List<FileLineCount> CountLines(string path, List<string> fileExtensions) {
			List<FileLineCount> lineCounts = new List<FileLineCount>(64);
			CountLines(path, fileExtensions, lineCounts);
			return lineCounts;
		}

		private static void CountLines(string path, List<string> fileExtensions, List<FileLineCount> lineCounts) {
			try {
				string[] absoluteFilePaths = Directory.GetFiles(path);
				for (int i = 0; i < absoluteFilePaths.Length; i++) {
					for (int j = 0; j < fileExtensions.Count; j++) {
						if (absoluteFilePaths[i].EndsWith(fileExtensions[j])) {
							int fileLines = File.ReadAllLines(absoluteFilePaths[i]).Length;
							if (fileLines > 0)
								lineCounts.Add(new FileLineCount(absoluteFilePaths[i], fileLines));
							break;
						}
					}
				}

				string[] directories = Directory.GetDirectories(path);
				for (int i = 0; i < directories.Length; i++) {
					CountLines(directories[i], fileExtensions, lineCounts);
				}
			} catch (UnauthorizedAccessException e) {
				//Lol.. ignore this directory.
			}
		}
	}
}
