using System;

namespace UHI
{
	public struct Difference
	{
		// Type of difference
		public DifferenceType TypeOfDifference { get; set; }
		// Path to the file
		public string[] FilePaths { get; set; }
		// Hash string of the file
		public string[] HashStrings { get; set; }
	}
}

