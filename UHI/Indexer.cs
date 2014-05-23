using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Diagnostics;

namespace UHI
{
	public class Indexer
	{
		// String with saved path to directory
		private string DIRECTORY { get; set; }
		// Saved information which hash should be used
		private Hash HASH { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UHI.Indexer"/> class.
		/// </summary>
		/// <param name="directory">Indexed directory.</param>
		/// <param name="hash">Used hash.</param>
		/// <exception cref="ArgumentNullException">Throws when one of arguments is null</exception>
		/// <exception cref="DirectoryNotFoundException">Throws when directory will not be available</exception>
		public Indexer(string directory, Hash hash)
		{
			directory = directory.Trim();

			if (directory == null || directory == "")
			{
				throw new ArgumentNullException("directory");
			}

			if (!Directory.Exists(directory))
			{
				throw new DirectoryNotFoundException("Could not find directory to index");
			}

			DIRECTORY = directory.Replace("/", "\\");
			HASH = hash;
		}

		/// <summary>
		/// Index selected folder to output string.
		/// </summary>
		/// <returns>Returns <c>object[] { long totalMsElpsed, FileInfo[] filesInfo }</c>.</returns>
		/// <param name="multiThreadHashing">If set to <c>true</c> multi thread hashing. For a few files or small files is better to use <c>false</c>.</param>
		/// <param name="pattern">Search pattern.</param>
		public object[] Index(bool multiThreadHashing = false, string pattern = "*")
		{
			object[] output = new object[] { null, null };
			long stopwatchStart = DateTime.Now.Ticks;

			string[] files = Tools.GetFiles(DIRECTORY, pattern);
			output[1] = Tools.GetFilesInfo(files, HASH, multiThreadHashing);

			long stopwatchEnd = DateTime.Now.Ticks;
			TimeSpan stopwachResult = new TimeSpan(stopwatchEnd - stopwatchStart);
			output[0] = (long)stopwachResult.TotalMilliseconds;

			return output;
		}

		/// <summary>
		/// Index selected folder to output string.
		/// </summary>
		/// <returns>Returns <c>object[] { long totalMsElpsed, string filesInfoString }</c>.</returns>
		/// <param name="multiThreadHashing">If set to <c>true</c> multi thread hashing. For a few files or small files is better to use <c>false</c>.</param>
		/// <param name="pattern">Search pattern.</param>
		public object[] IndexString(bool multiThreadHashing = false, string pattern = "*")
		{
			object[] output = Index(multiThreadHashing, pattern);
			BigInteger i = 0;
			StringBuilder sb = new StringBuilder();

			foreach (FileInfo fi in (FileInfo[])output[1])
			{
				sb.AppendLine(String.Format("{0}§{1}§{2}", i, Tools.EncodeToBase64(fi.FilePath), fi.HashString));
				i++;
			}

			output[1] = sb.ToString();


			return output;
		}

		/// <summary>
		/// Index selected folder to the specified outputIndexFile.
		/// </summary>
		/// <returns>Elpsed ms.</returns>
		/// <param name="outputIndexFile">Output index file.</param>
		/// <param name="multiThreadHashing">If set to <c>true</c> multi thread hashing. For a few files or small files is better to use <c>false</c>.</param>
		/// <param name="pattern">Search pattern.</param>
		/// <exception cref="ArgumentNullException">Throws when one of arguments is null</exception>
		public long IndexToFile(string outputIndexFile, bool multiThreadHashing = false, string pattern = "*")
		{
			if (outputIndexFile == null || outputIndexFile == "")
			{
				throw new ArgumentNullException("outputIndexFile");
			}

			object[] output = IndexString(multiThreadHashing, pattern);
			File.WriteAllText(outputIndexFile, (string)output[1]);

			return (long)output[0];
		}
	}
}

