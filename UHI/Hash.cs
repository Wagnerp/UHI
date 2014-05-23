using System;

namespace UHI
{
	public enum Hash
	{
		/// <summary>
		/// SHA1 Hashing !! Known to be insecure !! (+/- 1695 ms one threaded, +/- 771 ms multi threaded (180MB, 306 files))
		/// </summary>
		[Obsolete("SHA1 hashing is known to be insecure")]
		SHA1 = 1,
		/// <summary>
		/// SHA256 Hashing (Recommended) (+/- 3256 ms one threaded, +/- 1531 ms multi threaded (180MB, 306 files))
		/// </summary>
		SHA256 = 2,
		/// <summary>
		/// SHA384 Hashing (+/- 2224 ms one threaded, +/- 978 ms multi threaded (180MB, 306 files))
		/// </summary>
		SHA384 = 3,
		/// <summary>
		/// SHA512 Hashing (+/- 2162 ms one threaded, +/- 1017 ms multi threaded (180MB, 306 files))
		/// </summary>
		SHA512 = 4,
		/// <summary>
		/// RIPEMD160 Hashing (+/- 3271 ms one threaded, +/- 1461 ms multi threaded (180MB, 306 files))
		/// </summary>
		RIPEMD160 = 5,
		/// <summary>
		/// MD5 Hashing !! Known to be insecure !! - Mostly the fastest (+/- 684 ms one threaded, +/- 359 ms multi threaded (180MB, 306 files); 11'196 files - 2.7GB - +/- 283574 ms (+/- 4.7 min))
		/// </summary>
		[Obsolete("MD5 hashing is known to be insecure")]
		MD5 = 6
	}
}
