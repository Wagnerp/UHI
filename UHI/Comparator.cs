using System;
using System.Collections.Generic;

namespace UHI
{
	public class Comparator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UHI.Comparator"/> class.
		/// </summary>
		public Comparator()
		{

		}

		/// <summary>
		/// Gets the differences.
		/// </summary>
		/// <returns>The differences.</returns>
		/// <param name="inFirst">In first.</param>
		/// <param name="inSecond">In second.</param>
		public Difference[] GetDifferences(FileInfo[] inFirst, FileInfo[] inSecond)
		{
			List<Difference> differences = new List<Difference>();

			FileInfoCo[] first = ConvertFileInfo(inFirst);
			FileInfoCo[] second = ConvertFileInfo(inSecond);

			List<FileInfoCo> _first = new List<FileInfoCo>(first);
			List<FileInfoCo> _second = new List<FileInfoCo>(second);

			foreach (FileInfoCo file in second)
			{
				_first.Find(delegate(FileInfoCo item)
				{
					if (file.FilePath == item.FilePath && file.HashString == item.HashString)
					{
						_first.Remove(item);
						_second.Remove(item);

						return true;
					}
					else if (file.FilePath == item.FilePath)
					{
						_first.Remove(item);
						_second.Remove(file);

						differences.Add(new Difference()
						{
							TypeOfDifference = DifferenceType.FileHashesAreDifferent,
							FilePaths = new string[] { item.FilePath, file.FilePath },
							HashStrings = new string[] { item.HashString, file.HashString }
						});

						return true;
					}

					return false;
				});
			}

			foreach (FileInfoCo file in _first)
			{
				differences.Add(new Difference()
				{
					TypeOfDifference = DifferenceType.FileIsMissingInFirst,
					FilePaths = new string[] { file.FilePath },
					HashStrings = new string[] { file.HashString }
				});
			}

			foreach (FileInfoCo file in _second)
			{
				differences.Add(new Difference()
				{
					TypeOfDifference = DifferenceType.FileIsMissingInSecond,
					FilePaths = new string[] { file.FilePath },
					HashStrings = new string[] { file.HashString }
				});
			}

			inFirst = null;
			inSecond = null;
			first = null;
			second = null;
			_first = null;
			_second = null;

			return differences.ToArray();
		}

		/// <summary>
		/// Converts the FileInfo into FileInfoCo.
		/// </summary>
		/// <returns>The FileInfoCo.</returns>
		/// <param name="input">Input FileInfo.</param>
		private FileInfoCo[] ConvertFileInfo(FileInfo[] input)
		{
			List<FileInfoCo> output = new List<FileInfoCo>();

			foreach (FileInfo fi in input)
			{
				output.Add(new FileInfoCo()
				{
					FilePath = fi.FilePath,
					HashString = fi.HashString
				});
			}

			return output.ToArray();
		}
	}
}

