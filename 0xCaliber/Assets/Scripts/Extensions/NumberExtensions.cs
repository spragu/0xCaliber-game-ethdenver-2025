﻿namespace Projectiles
{
	public static class NumberExtensions
	{
		public static bool IsBitSet(this byte flags, int bit)
		{
			return (flags & (1 << bit)) == (1 << bit);
		}

		public static byte SetBit(ref this byte flags, int bit, bool value)
		{
			if (value == true)
				return flags |= (byte)(1 << bit);

			return flags &= unchecked((byte)~(1 << bit));
		}

		public static bool IsBitSet(this int flags, int bit)
		{
			return (flags & (1 << bit)) == (1 << bit);
		}

		public static int SetBit(ref this int flags, int bit, bool value)
		{
			if (value == true)
				return flags |= 1 << bit;

			return flags &= ~(1 << bit);
		}
	}
}
