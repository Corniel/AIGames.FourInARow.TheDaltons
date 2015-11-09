namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		public static int GetScore(Field field, byte ply)
		{
			ulong red = field.GetRed();
			ulong yel = field.GetYellow();

			var redDbl = 0;
			var redTri = 0;
			var yelDbl = 0;
			var yelTri = 0;

			foreach (var mask in Field.Scores)
			{
				var matchRed = red & mask;
				if (matchRed == mask) { return Scores.Red - ply; }
				
				var matchYel= yel & mask;
				if (matchYel == mask) { return Scores.Yel +ply; }

				var countRed = Bits.Count(matchRed);
				var countYel = Bits.Count(matchYel);
				// we have at least two of the combo, and the rest is free.
				if (countRed > 1 && countYel == 0)
				{
					if (countRed == 2) { redDbl++; }
					else { redTri++; }
				}
				else if (countYel > 1 && countRed == 0)
				{
					if (countYel == 2) { yelDbl++; }
					else { yelTri++; }
				}
			}
			var redScore = redDbl +redTri* redTri* redTri * 100;
			var yelScore = yelDbl +yelTri* yelTri* yelTri * 100;
			return redScore - yelScore;
		}
	}
}
