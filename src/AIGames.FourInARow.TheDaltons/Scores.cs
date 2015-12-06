using System;
using System.Globalization;

namespace AIGames.FourInARow.TheDaltons
{
	public static class Scores
	{
		public static readonly int[] Threat3 = { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };

		public const int InitialAlpha = int.MinValue;
		public const int InitialBeta = int.MaxValue;

		public const int StrongThreat = 10000;
		public const int Vertical3 = 10;
		public const int Horizontal3 = 80;
		public const int Diagonal3 = 100;
		public const int Draw = 0;
		private const int Red = 10000000;
		private const int Yel = -10000000;

		public static readonly int RedWin = RedWins(SearchTree.MaximumDepth);
		public static readonly int YelWin = YelWins(SearchTree.MaximumDepth);
		

		public static int RedWins(int ply) { return Red - ply; }
		public static int YelWins(int ply) { return Yel + ply; }

		/// <summary>Return true if the score indicates a winning (or losing) position.</summary>
		public static bool IsWinning(int score)
		{
			return score >= RedWin || score <= YelWin;
		}

		public static string GetFormatted(int score)
		{
			if (score >= Scores.RedWin)
			{
				var ply = (Scores.Red - score);
				return String.Format(CultureInfo.InvariantCulture, "+oo {0}", ply);
			}
			if (score <= Scores.YelWin)
			{
				var ply = (score - Scores.Yel);
				return String.Format(CultureInfo.InvariantCulture, "-oo {0}", ply);
			}

			var str = "";
			if (score > 0) { str = "+"; }
			else if (score == 0) { str = "="; }
			str += (score / 1000m).ToString("0.000", CultureInfo.InvariantCulture);
			return str;
		}
	}
}
