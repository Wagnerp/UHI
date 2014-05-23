using System;
using System.Numerics;

namespace UHI
{
	public struct FileInfo
	{
		// File's number (May change, use it carefully ...)
		public BigInteger FileNumber { get; set; }
		// Path to the file
		public string FilePath { get; set; }
		// Hash string of the file
		public string HashString { get; set; }
	}
}

