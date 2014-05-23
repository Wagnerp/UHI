using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace UHI
{
	public class IndexParser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UHI.IndexParser"/> class.
		/// </summary>
		public IndexParser()
		{

		}

		/// <summary>
		/// Parse the specified indexStringLines.
		/// </summary>
		/// <param name="indexStringLines">Index string lines.</param>
		public FileInfo[] Parse(string[] indexStringLines)
		{
			List<FileInfo> filesInfo = new List<FileInfo>();

			foreach (string line in indexStringLines)
			{
				string tempLine = line.Trim();

				if (tempLine == "")
				{
					continue;
				}

				string[] parts = tempLine.Split(new char[] { '§' }, StringSplitOptions.RemoveEmptyEntries);


				if (parts.Length != 3)
				{
					throw new MissingFieldException("One or more lines were not correct");
				}

				FileInfo fi = new FileInfo()
				{
					FileNumber = int.Parse(parts[0]),
					FilePath = Tools.DecodeFromBase64(parts[1]),
					HashString = parts[2]
				};

				filesInfo.Add(fi);
			}

			return filesInfo.ToArray();
		}

		/// <summary>
		/// Parse the specified indexString.
		/// </summary>
		/// <param name="indexString">Index string.</param>
		public FileInfo[] Parse(string indexString)
		{
			return Parse(indexString.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// Parse the specified filePath with fileEncoding.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <param name="fileEncoding">File encoding.</param>
		public FileInfo[] Parse(string filePath, Encoding fileEncoding)
		{
			return Parse(File.ReadAllLines(filePath, fileEncoding));
		}
	}
}

