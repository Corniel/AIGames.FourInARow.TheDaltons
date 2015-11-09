using System;
using System.Globalization;
using System.Text;

namespace AIGames.FourInARow.TheDaltons
{
	public struct PlyLog
	{
		public PlyLog(int ply, int move, int score, byte depth, TimeSpan elapsed)
		{
			Ply = ply;
			Move = move;
			Score = score;
			Depth = depth;
			Elapsed = elapsed;
		}

		public readonly int Ply;
		public readonly int Move;
		public readonly int Score;
		public readonly byte Depth;
		public readonly TimeSpan Elapsed;

		public override string ToString()
		{
			var sb = new StringBuilder()
				.AppendFormat(CultureInfo.InvariantCulture, "{0:00}/{1:00}. ", Ply, Depth)
				.AppendFormat(CultureInfo.InvariantCulture, "{0}: ", GetScore())
				.AppendFormat(CultureInfo.InvariantCulture, "{{{0}}} ", Move)
				.AppendFormat(CultureInfo.InvariantCulture, "{0:0.000}s", Elapsed.TotalSeconds);

			return sb.ToString();
		}

		private string GetScore()
		{
			if (Score >= Scores.RedMin)
			{
				var ply = (Scores.Red - Score) - Ply;
				return String.Format(CultureInfo.InvariantCulture, "+oo {0}", ply);
			}
			if (Score <= Scores.YelMin)
			{
				var ply = (Score- Scores.Yel) - Ply;
				return String.Format(CultureInfo.InvariantCulture, "-oo {0}", ply);
			}

			var str = "";
			if (Score > 0) { str = "+"; }
			else if (Score == 0) { str = "="; }
			str += (Score / 100m).ToString("0.00", CultureInfo.InvariantCulture);
			return str;
		}
	}
}
