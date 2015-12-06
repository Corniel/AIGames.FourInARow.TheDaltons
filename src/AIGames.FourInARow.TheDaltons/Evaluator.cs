namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		private const int NotSet = short.MaxValue;
		
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

				var score = 0;
				var redTri = 0;
				var yelTri = 0;

				var forcedRed = NotSet;
				var forcedYel = NotSet;
				var lowestRed = NotSet;
				var lowestYel = NotSet;
				var lowestRedCol = NotSet;
				var lowestYelCol = NotSet;

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
						var base3 = index << 2;
						for (var i = 0; i < 4; i++)
						{
							var lookup = base3 | i;
							if (Field.Connect3Out4[lookup] == matchRed)
							{
								redTri++;
								score += Scores.Threat3[index];
								threatRed |= Field.Connect4Threat[lookup];
								break;
							}
						}
					}
					else
					{
						var base3 = index << 2;
						for (var i = 0; i < 4; i++)
						{
							var lookup = base3 | i;
							if (Field.Connect3Out4[lookup] == matchYel)
							{
								yelTri++;
								score -= Scores.Threat3[index];
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

					for (var col = 0; col < 7; col++)
					{
						var colRed = threatRed & Field.ColumnMasks[col];
						var colYel = threatYel & Field.ColumnMasks[col];

						if (colRed != 0 || colYel != 0)
						{
							var colMix = mixed & Field.ColumnMasks[col];

							var min = 0;
							var rowR = NotSet;
							var rowY = NotSet;

							for (var row = 0; row < 6; row++)
							{
								var shift = row << 3 | col;
								ulong mask = 1UL << shift;
								if /**/ ((colMix & mask) != 0)
								{
									min = row;
								}
								else
								{
									if ((colRed & mask) != 0)
									{
										if (rowR + 1 == row)
										{
											var forced = row - min;
											if (rowY >= rowR && forced < forcedRed)
											{
												forcedRed = forced;
												break;
											}
										}
										rowR = row;
										if ((row & 1) == 0 && row < lowestRed)
										{
											lowestRedCol = col;
											lowestRed = row;
										}
										leftRed--;
									}
									if ((colYel & mask) != 0)
									{
										if (rowY + 1 == row)
										{
											var forced = row - min;
											if (rowR >= row && forced < forcedYel)
											{
												forcedYel = forced;
												break;
											}
										}
										rowY = row;
										// Zero based: s
										if ((row & 1) == 1 && row < lowestYel)
										{
											lowestYelCol = col;
											lowestYel = row;
										}
										leftYel--;
									}
									if (leftRed == 0 && leftYel == 0) { break; }
								}
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
							return Scores.RedWins(ply + forcedRed + 1);
						}
						else
						{
							return Scores.YelWins(ply + forcedYel + 2);
						}
					}
					else
					{
						if (forcedRed < forcedYel)
						{
							return Scores.RedWins(ply + forcedRed + 2);
						}
						else
						{
							return Scores.YelWins(ply + forcedYel + 1);
						}
					}
				}

				if (lowestRed != NotSet)
				{
					// Lowest threat is red
					if (lowestRed < lowestYel)
					{
						score += Scores.StrongThreat;
					}
					// Lowest threat in same column for yellow.
					else if (lowestRedCol == lowestYelCol)
					{
						score -= Scores.StrongThreat;
						
					}
					// Lowest threat for yellow but different column.
					else
					{
						score += Scores.StrongThreat >> 1;
					}
				}
				// Only yellow has a strong threat.
				else if(lowestYel != NotSet)
				{
					score -= Scores.StrongThreat;
				}
				return score;
#if !DEBUG
			}
#endif
		}
	}
}
