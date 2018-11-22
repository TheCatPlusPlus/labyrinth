/*
Original C implementation of xoshiro256**:

	Written in 2018 by David Blackman and Sebastiano Vigna (vigna@acm.org)

	To the extent possible under law, the author has dedicated all copyright
	and related and neighboring rights to this software to the public domain
	worldwide. This software is distributed without any warranty.

	See <http://creativecommons.org/publicdomain/zero/1.0/>.

Original C implementation of splitmix64:
	 Written in 2015 by Sebastiano Vigna (vigna@acm.org)

	To the extent possible under law, the author has dedicated all copyright
	and related and neighboring rights to this software to the public domain
	worldwide. This software is distributed without any warranty.

	See <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using System;
using System.Runtime.Serialization;

namespace Labyrinth.Utils
{
	[DataContract]
	public sealed class Xoshiro256StarStar
	{
		private struct SplitMix64
		{
			private ulong _x;

			public SplitMix64(ulong x)
			{
				_x = x;
			}

			public ulong Next()
			{
				var z = _x += 0x9E3779B97F4A7C15ul;
				z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9ul;
				z = (z ^ (z >> 27)) * 0x94D049BB133111EBul;
				return z ^ (z >> 31);
			}
		}

		[DataMember]
		private ulong _s0;
		[DataMember]
		private ulong _s1;
		[DataMember]
		private ulong _s2;
		[DataMember]
		private ulong _s3;

		public static Xoshiro256StarStar FromSeed(ulong? seed = null)
		{
			var rng = new Xoshiro256StarStar();
			var mix = new SplitMix64(seed ?? (ulong)DateTime.UtcNow.Ticks);

			rng._s0 = mix.Next();
			rng._s1 = mix.Next();
			rng._s2 = mix.Next();
			rng._s3 = mix.Next();

			return rng;
		}

		public ulong NextULong()
		{
			var result = RotL(_s1 * 5ul, 7) * 9ul;
			var t = _s1 << 17;

			_s2 ^= _s0;
			_s3 ^= _s1;
			_s1 ^= _s2;
			_s0 ^= _s3;

			_s2 ^= t;

			_s3 = RotL(_s3, 45);
			return result;
		}

		public double NextDouble()
		{
			var x = NextULong();
			var bits = (0x3FFL << 52) | (x >> 12);
			return BitConverter.Int64BitsToDouble((long)bits) - 1.0;
		}

		private static ulong RotL(ulong x, int k)
		{
			return (x << k) | (x >> (64 - k));
		}
	}
}
