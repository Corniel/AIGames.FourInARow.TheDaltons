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

		public Field(ulong r, ulong y)
		{
#if DEBUG
			if ((r & y) != 0)
			{
				throw new ArgumentException("Not exclusive.");
			}
#endif
			red = r;
			yel = y;
		}

		public ulong Occupied { get { return red | yel; } }

		public Field MoveRed(ulong move)
		{
			return new Field(red | move, yel);
		}
		public Field MoveYellow(ulong move)
		{
			return new Field(red, yel | move);
		}

		public Field Flip()
		{
			return new Field(Flip(red), Flip(yel));
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

		public bool IsScoreRed() { return IsScore(red); }
		public bool IsScoreYellow() { return IsScore(yel); }

		private static bool IsScore(ulong color)
		{
			foreach (var mask in Scores)
			{
				if ((mask & color) == mask) { return true; }
			}
			return false;
		}

		public override int GetHashCode() { return (red ^ (yel << (64 - 41))).GetHashCode(); }
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
				var r = red >> (row << 3);
				var y = yel >> (row << 3);

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

			return new Field(r, y);
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
	}
}
