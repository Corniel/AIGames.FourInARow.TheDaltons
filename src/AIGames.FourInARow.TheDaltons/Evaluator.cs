namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		private static readonly int[] Pow2 = GetPow2();

		private static int[] GetPow2()
		{
			var pows = new int[100];
			for (var p = 0; p < 100; p++)
			{
				pows[p] = p * p;
			}
			return pows;
		}

		public static int GetScore(Field field, byte ply)
		{
#if !DEBUG
			unchecked
			{
#endif
				ulong red = field.GetRed();
				ulong yel = field.GetYellow();

				var redTri = 0;
				var yelTri = 0;

				for (var index = 0; index < 69; index++)
				{
					var mask = Field.Connect4[index];
					var matchRed = red & mask;
					if (matchRed == mask) { return Scores.RedWins(ply); }

					var matchYel = yel & mask;
					if (matchYel == mask) { return Scores.YelWins(ply); }

					// Both have a match. Skip it.
					if (matchRed != 0 && matchYel != 0) { continue; }

					if (matchRed != 0)
					{
						for (var i = 0; i < 4; i++)
						{
							if (Field.Connect3Out4[(index << 2) | i] == matchRed)
							{
								redTri++;
								break;
							}
						}
					}
					else
					{
						for (var i = 0; i < 4; i++)
						{
							if (Field.Connect3Out4[(index << 2) | i] == matchYel)
							{
								yelTri++;
								break;
							}
						}
					}
				}
				return Pow2[redTri] - Pow2[yelTri];
#if !DEBUG
			}
#endif
		}
	}
}
