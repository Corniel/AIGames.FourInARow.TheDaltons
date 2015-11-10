using System;
using System.Globalization;

namespace AIGames.FourInARow.TheDaltons
{
	public static class Scores
	{
		public const int InitialAlpha = int.MinValue;
		public const int InitialBeta = int.MaxValue;

		public const int Draw = 0;
		private const int Red = 1000000;
		private const int Yel = -1000000;

		public static readonly int RedWin = RedWins(42);
		public static readonly int YelWin = YelWins(42);

		public static int RedWins(int ply) { return Red - ply; }
		public static int YelWins(int ply) { return Yel + ply; }

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
			str += (score / 100m).ToString("0.00", CultureInfo.InvariantCulture);
			return str;
		}


	}
}
