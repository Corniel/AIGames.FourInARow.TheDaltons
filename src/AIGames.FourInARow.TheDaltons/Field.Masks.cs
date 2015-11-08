using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.FourInARow.TheDaltons
{
	public partial struct Field
	{
		public const ulong Mask = 0x00007f7f7f7f7f7f;

		public static readonly ulong[] ColumnMasks = 
		{
			0x0000010101010101,
			0x0000020202020202,
			0x0000040404040404,
			0x0000080808080808,
			0x0000101010101010,
			0x0000202020202020,
			0x0000404040404040,
		};

		public static readonly ulong[] Scores = GetScores();

		public static ulong[] GetScores()
		{
			var scores = new HashSet<ulong>();

			// row scores.
			for (var col = 0; col < 4; col++)
			{
				for (var row = 0; row < 6; row++)
				{
					ulong line = 0x0F;
					line <<= col + (row << 3);
					scores.Add(line);
				}
			}

			// column scores.
			for (var col = 0; col < 7; col++)
			{
				for (var row = 0; row < 3; row++)
				{
					ulong line = 0x01010101;
					line <<= col + (row << 3);
					scores.Add(line);
				}
			}
			// diagonal scores.
			for (var col = 0; col < 4; col++)
			{
				for (var row = 0; row < 3; row++)
				{
					ulong dig0 = 0x08040201;
					ulong dig1 = 0x01020408;
					dig0 <<= col + (row << 3);
					dig1 <<= col + (row << 3);
					scores.Add(dig0);
					scores.Add(dig1);
				}
			}
			return scores.OrderByDescending(s => s).ToArray();
		}
	
		public static string ToString(ulong field)
		{
			var sb = new StringBuilder();

			for (var row = 5; row >= 0; row--)
			{
				var line = 127 & (field >> (row << 3));
				sb.AppendLine(ParseRows.FirstOrDefault(kvp => kvp.Value == line).Key);
			}
			return sb.ToString();
		}
	}
}
