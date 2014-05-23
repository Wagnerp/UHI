using System;

namespace UHI
{
	public enum DifferenceType
	{
		/// <summary>
		/// The file hashes are different.
		/// </summary>
		FileHashesAreDifferent = 1,
		/// <summary>
		/// The file is missing in first (For ex. old).
		/// </summary>
		FileIsMissingInFirst = 2,
		/// <summary>
		/// The file is missing in second (For ex. new).
		/// </summary>
		FileIsMissingInSecond = 3
	}
}

