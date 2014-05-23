using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Numerics;
using System.Threading;

namespace UHI
{
	internal class Tools
	{
		static List<FileInfo> _tempList = new List<FileInfo>();
		static BigInteger _tempStarted = 0;
		static BigInteger _tempDone = 0;

		/// <summary>
		/// Gets the files.
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="directory">Directory.</param>
		/// <param name="pattern">Search pattern.</param>
		internal static string[] GetFiles(string directory, string pattern = "*")
		{
			if (Directory.Exists(directory))
			{
				return Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
			}
			else
			{
				throw new DirectoryNotFoundException("Could not find directory to index");
			}
		}

		/// <summary>
		/// Gets the files's info.
		/// </summary>
		/// <returns>The files info.</returns>
		/// <param name="files">Files paths.</param>
		/// <param name="hash">Hash.</param>
		/// <param name="multiThreadHashing">If set to <c>true</c> multi thread hashing. For a few files or small files is better to use <c>false</c>.</param>
		internal static FileInfo[] GetFilesInfo(string[] files, Hash hash, bool multiThreadHashing)
		{
			List<FileInfo> filesInfo = new List<FileInfo>();

			foreach (string file in files)
			{
				if (multiThreadHashing)
				{
					BigInteger _lastTempStarted = _tempStarted;
					BackgroundWorker bw = new BackgroundWorker();
					
					bw.DoWork += (sender, e) =>
					{
						_tempStarted++;
						_tempList.Add(GetFileInfo(hash, file));
					};

					bw.RunWorkerCompleted += (sender, e) =>
					{
						_tempDone++;
					};

					bw.RunWorkerAsync();
					while (_lastTempStarted == _tempStarted)
					{
						Thread.Sleep(1);
					}
				}
				else
				{
					filesInfo.Add(GetFileInfo(hash, file));
				}
			}

			if (multiThreadHashing)
			{
				while (_tempStarted != _tempDone)
				{
					if (_tempStarted <= _tempDone)
					{
						break;
					}

					Thread.Sleep(10);
				}

				filesInfo = _tempList;
				_tempList = new List<FileInfo>();
			}

			return filesInfo.ToArray();
		}

		/// <summary>
		/// Gets the file's info.
		/// </summary>
		/// <returns>The file info.</returns>
		/// <param name="hash">Hash.</param>
		/// <param name="path">File path.</param>
		internal static FileInfo GetFileInfo(Hash hash, string path)
		{
			string hashString = GetChecksum(path, hash);

			return new FileInfo()
			{
				FilePath = path,
				HashString = hashString
			};
		}

		/// <summary>
		/// Encodes to base64.
		/// </summary>
		/// <returns>Base64 string.</returns>
		/// <param name="input">Input string.</param>
		internal static string EncodeToBase64(string input)
		{
			return Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(input));
		}

		/// <summary>
		/// Decodes from base64.
		/// </summary>
		/// <returns>Base64 string.</returns>
		/// <param name="input">Input string.</param>
		public static string DecodeFromBase64(string input)
		{
			return UTF8Encoding.UTF8.GetString(Convert.FromBase64String(input));
		}

		/// <summary>
		/// Gets the file's checksum.
		/// </summary>
		/// <returns>The checksum.</returns>
		/// <param name="file">File path.</param>
		/// <param name="hash">Hash.</param>
		private static string GetChecksum(string file, Hash hash)
		{
			using (FileStream stream = File.OpenRead(file))
			{
				byte[] checksum = null;

				switch (hash)
				{
					case Hash.RIPEMD160:
						checksum = new SHA256Managed().ComputeHash(stream);
						break;
					case Hash.SHA1:
						checksum = new SHA1Managed().ComputeHash(stream);
						break;
					case Hash.SHA256:
						checksum = new SHA256Managed().ComputeHash(stream);
						break;
					case Hash.SHA384:
						checksum = new SHA384Managed().ComputeHash(stream);
						break;
					case Hash.SHA512:
						checksum = new SHA512Managed().ComputeHash(stream);
						break;
					case Hash.MD5:
						checksum = new MD5CryptoServiceProvider().ComputeHash(stream);
						break;
					default:
						throw new InvalidEnumArgumentException("Hash function was not found");
				}

				return BitConverter.ToString(checksum).Replace("-", String.Empty);
			}
		}
	}
}

