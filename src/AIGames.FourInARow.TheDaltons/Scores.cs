using System;
using System.Globalization;

namespace AIGames.FourInARow.TheDaltons
{
	public static class Scores
	{
		public static readonly int[] Threat3 = { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 80, 10, 10, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };

		public const int InitialAlpha = int.MinValue;
		public const int InitialBeta = int.MaxValue;

		public const int StrongThreat = 4;
		public const int Draw = 0;
		private const int Red = 1000000;
		private const int Yel = -1000000;

		public static readonly int[] RedWins = GetRedWins();
		public static readonly int[] YelWins = GetYelWins();

		public const int RedWin = Red - SearchTree.MaximumDepth;
		public const int YelWin = Yel + SearchTree.MaximumDepth - 1;

		private static int[] GetRedWins()
		{
			var sc = new int[SearchTree.MaximumDepth + 1];

			for (var ply = 0; ply < sc.Length; ply++)
			{
				sc[ply] = Red - ply;
			}
			return sc;

		}
		private static int[] GetYelWins()
		{
			var sc = new int[SearchTree.MaximumDepth];

			for (var ply = 0; ply < sc.Length; ply++)
			{
				sc[ply] = Yel + ply;
			}
			return sc;
		}

		/// <summary>Returns the required ply for winning.</summary>
		/// <returns>
		/// returns 255 if no win could be given else the ply required for winning.
		/// </returns>
		public static byte GetPlyToWinning(int score)
		{
			if (score >= RedWin)
			{
				return (byte)(Red - score);
			}
			if (score <= YelWin)
			{
				return (byte)(score - Yel);
			}
			return Byte.MaxValue;
		}

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
			str += (score / 100m).ToString("0.00", CultureInfo.InvariantCulture);
			return str;
		}
	}
}
