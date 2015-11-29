namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		private const int NotSet = short.MaxValue;
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
			var redToMove = (ply & 1) == 1;

			ulong red = field.GetRed();
			ulong yel = field.GetYellow();

			ulong threatRed = 0;
			ulong threatYel = 0;

			var redTri = 0;
			var yelTri = 0;

			var forcedRed = NotSet;
			var forcedYel = NotSet;

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
						var lookup = (index << 2) | i;
						if (Field.Connect3Out4[lookup] == matchRed)
						{
							redTri++;
							threatRed |= Field.Connect4Threat[lookup];
							break;
						}
					}
				}
				else
				{
					for (var i = 0; i < 4; i++)
					{
						var lookup = (index << 2) | i;
						if (Field.Connect3Out4[lookup] == matchYel)
						{
							yelTri++;
							threatYel |= Field.Connect4Threat[lookup];
							break;
						}
					}
				}
			}

			if (redTri > 0 || yelTri > 0)
			{
				var mixed = red | yel;

				var leftRed = redTri;
				var leftYel = yelTri;

				for (ulong col = 0; col < 7; col++)
				{
					var colRed = threatRed & Field.ColumnMasks[col];
					var colYel = threatYel & Field.ColumnMasks[col];

					if (colRed != 0 || colYel != 0)
					{
						var colMix = mixed & Field.ColumnMasks[col];

						var strR = Field.ToString(threatRed);
						var strY = Field.ToString(threatYel);

						var min = 0;
						var row = 0;
						var rowR = 0;
						var rowY = 0;

						for (ulong mask = col + 1; mask < Field.Mask; mask <<= 8)
						{
							row++;
							if /**/ ((colMix & mask) != 0)
							{
								min = row;
							}
							else if ((colRed & mask) != 0)
							{
								if (rowR + 1 == row)
								{
									var forced = row - min;
									if (rowY == 0 && forced < forcedRed)
									{
										forcedRed = forced;
										break;
									}
								}
								rowR = row;
								leftRed--;
							}
							else if ((colYel & mask) != 0)
							{
								if (rowY + 1 == row)
								{
									var forced = row - min;
									if (rowR == 0 && forced < forcedYel)
									{
										forcedYel = forced;
										break;
									}
								}
								rowR = row;
								leftYel--;
							}
							if (leftRed == 0 && leftYel == 0) { break; }
						}
					}
				}
			}

			if (forcedRed != NotSet || forcedYel != NotSet)
			{
				if (redToMove)
				{
					if (forcedRed <= forcedYel)
					{
						return Scores.RedWins(ply + forcedRed);
					}
					else
					{
						return Scores.YelWins(ply + forcedRed + 1);
					}
				}
				else
				{
					if (forcedRed < forcedYel)
					{
						return Scores.RedWins(ply + forcedRed + 1);
					}
					else
					{
						return Scores.YelWins(ply + forcedRed);
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
