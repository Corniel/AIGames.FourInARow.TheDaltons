using System;
using System.Globalization;

namespace AIGames.FourInARow.TheDaltons
{
	public static class Scores
	{
		public const int Draw = 0;
		public const int Red = 1000000;
		public const int Yel = -1000000;
		public const int RedMin = Red - 42;
		public const int YelMin = Yel + 42;

		public static string GetFormatted(int score)
		{
			if (score >= Scores.RedMin)
			{
				var ply = (Scores.Red - score);
				return String.Format(CultureInfo.InvariantCulture, "+oo {0}", ply);
			}
			if (score <= Scores.YelMin)
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
