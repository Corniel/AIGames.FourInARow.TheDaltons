using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIGames.FourInARow.TheDaltons
{
	public partial struct Field : IEquatable<Field>, IComparable<Field>
	{
		public static readonly Field Empty = default(Field);

		private readonly ulong red;
		private readonly ulong yel;

		private Field(ulong r, ulong y)
		{
			red = r;
			yel = y;
		}

		/// <summary>Gets the mask for the occupied cells.</summary>
		public ulong Occupied { get { return (red | yel) & Field.Mask; } }
		
		/// <summary>Get the count of played discs.</summary>
		public int Count { get { return Bits.Count(Occupied); } }

		public Field MoveRed(ulong move)
		{
			var update = (red | move) & Mask;
			return new Field(update | GetHashCode(update), yel);
		}
		public Field MoveYellow(ulong move)
		{
			var update = (yel | move) & Mask;
			return new Field(red, update | GetHashCode(update));
		}

		public ulong GetRed() { return red & Mask; }
		public ulong GetYellow() { return yel & Mask; }

		public bool IsScoreRed() { return IsScore(red); }
		public bool IsScoreYellow() { return IsScore(yel); }

		private static bool IsScore(ulong color)
		{
			foreach (var mask in Field.Connect4)
			{
				if ((mask & color) == mask) { return true; }
			}
			return false;
		}

		public Field Flip()
		{
			var r = Flip(red);
			var y = Flip(yel);
			return new Field(r | GetHashCode(r), y | GetHashCode(y));
		}
		public static ulong Flip(ulong color)
		{
			var flipped =
				((color & 0x010101010101) << 6) |
				((color & 0x020202020202) << 4) |
				((color & 0x040404040404) << 2) |
				((color & 0x080808080808) << 0) |
				((color & 0x101010101010) >> 2) |
				((color & 0x202020202020) >> 4) |
				((color & 0x404040404040) >> 6);
			return flipped;
		}

		/// <summary>Gets the hash code for the field.</summary>
		/// <remarks>
		/// The hash per color is stored at the end of their ulong value.
		/// </remarks>
		public override int GetHashCode()
		{
			unchecked
			{
				var hash = red >> 48;
				hash |= (yel >> 32) & 0xFFFF0000;
				return (int)hash;
			}
		}
		/// <summary>Gets the hash code part for a color.</summary>
		/// <remarks>
		/// The idea is to prevent collisions from happening.
		/// 
		/// 0000000000111111111122222222223333333333444444444455555555556666
		/// 0123456789012345678901234567890123456789012345678901234567890123
		///                                                 ================
		/// ................................................xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx. >> 48
		/// .........................xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx. >> 25
		/// ..............................xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx.xxxxxxx. >> 30
		/// </remarks>
		private static ulong GetHashCode(ulong color)
		{
			var hash = (color << 48) ^ (color << 25) ^ (color << 30);

			// Only returns the last 16 bit.
			return hash & 0xFFFF000000000000;
		}

		public override bool Equals(object obj) { return Equals((Field)obj); }
		public bool Equals(Field other)
		{
			return red == other.red && yel == other.yel;
		}
		public static bool operator ==(Field l, Field r) { return l.Equals(r); }
		public static bool operator !=(Field l, Field r) { return !(l == r); }

		public int CompareTo(Field other)
		{
			var c = red.CompareTo(other.red);
			if (c == 0)
			{
				c = yel.CompareTo(other.yel);
			}
			return c;
		}
		public static bool operator <(Field l, Field r) { return l.CompareTo(r) < 0; }
		public static bool operator >(Field l, Field r) { return l.CompareTo(r) > 0; }
		public static bool operator <=(Field l, Field r) { return l.CompareTo(r) <= 0; }
		public static bool operator >=(Field l, Field r) { return l.CompareTo(r) >= 0; }

		public override string ToString()
		{
			var sb = new StringBuilder((7 + 6) * 6 + 6);

			for (var row = 5; row >= 0; row--)
			{
				var r =( red & Mask) >> (row << 3);
				var y = (yel & Mask) >> (row << 3);

				for (var col = 6; col >= 0; col--)
				{
					if ((r & (1UL << col)) != 0)
					{
						sb.Append('1');
					}
					else if ((y & (1UL << col)) != 0)
					{
						sb.Append('2');
					}
					else { sb.Append('0'); }

					if (col > 0)
					{
						sb.Append(',');
					}
				}
				if (row > 0)
				{
					sb.Append(';');
				}
			}

			return sb.ToString();
		}

		/// <summary>Debugger helper to read the field.</summary>
		private string[] Rows
		{
			get
			{
				return ToString()
					.Split(';')
					.Select(row => row.Replace(",", ""))
					.ToArray();
			}
		}

		public static Field Parse(String str)
		{
			var sb = new StringBuilder(7 * 6);
			foreach (var ch in str)
			{
				if ("012".Contains(ch))
				{
					sb.Append(ch);
				}
			}
			var stripped = sb.ToString();
			var r = ToColored(stripped, 2);
			var y = ToColored(stripped, 1);

			return new Field(r | GetHashCode(r), y | GetHashCode(y));
		}
		private static ulong ToColored(string str, int remove)
		{
			var lines = str.Replace(remove.ToString(), "0").Replace("2", "1");
			var field = 0UL;

			for (var r = 5; r >= 0; r--)
			{
				var line = lines.Substring(r * 7, 7);
				var row = ParseRows[line];
				field |= row << ((5 - r) << 3);
			}
			return field;
		}

		private static readonly Dictionary<string, ulong> ParseRows = GetParseRows();
		public static Dictionary<string, ulong> GetParseRows()
		{
			var dict = new Dictionary<string, ulong>();
			for (var i = 0; i < 128; i++)
			{
				var key = "000000" + Convert.ToString(i, 2);
				key = key.Substring(key.Length - 7);
				dict[key] = (ulong)i;
			}
			return dict;
		}

		public byte[] GetBytes()
		{
			var bytes = new byte[16];
	
			var r = BitConverter.GetBytes(red);
			var y = BitConverter.GetBytes(yel);

			Array.Copy(r, 0, bytes, 0, 8);
			Array.Copy(y, 0, bytes, 8, 8);

			return bytes;
		}

		public static Field FromBytes(byte[] bytes)
		{
			var r = BitConverter.ToUInt64(bytes, 0);
			var y = BitConverter.ToUInt64(bytes, 8);
			return new Field(r, y);
		}
	}
}
