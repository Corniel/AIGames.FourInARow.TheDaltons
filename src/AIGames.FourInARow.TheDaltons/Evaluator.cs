using System;
namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		private const int NotSet = short.MaxValue;

		/// <summary>The delay to score with a double threat.</summary>
		/// <remarks>
		/// <code>
		/// var delay = DoubleThreatDelay[distance | Math.Min(5, otherOptions) << 3];
		/// </code>
		/// </remarks>
		private static readonly byte[] DoubleThreatDelay = new byte[]
		{
			//      0,  1,  2,  3,  4,  5,
			/* 0 */ 0,  2,  2,  4,  4,  6,   0,0,
			/* 1 */	0,  2,  4,  4,  6,  6,   0,0,
			/* 2 */	0,  2,  4,  6,  6,  8,   0,0,
			/* 3 */	0,  2,  4,  6,  8,  8,   0,0,
			/* 4 */	0,  2,  4,  6,  8, 10,   0,0,
			/* 5 */	0,  2,  4,  6,  8, 10,   0,0,
		};

		/// <summary>Gets the score for a field.</summary>
		/// <param name="field">
		/// The field to evaluate.
		/// </param>
		/// <param name="ply">
		/// The current ply.
		/// </param>
		/// <returns>
		/// A integer representing the score.
		/// </returns>
		/// <remarks>
		/// 
		/// Winning positions
		/// 
		/// 1. Two instant threats
		/// 
		///    0,0,0,0,0,0,0
		///    0,0,0,0,0,0,0
		///    0,0,0,0,0,0,0
		///    0,0,0,2,0,0,0
		///    0,0,0,2,0,0,0
		///    0,*,1,1,1,*,0
		///    
		/// 2. Two connected threats
		/// 
		///    0,0,0,0,0,0,0
		///    0,0,0,0,0,0,0
		///    0,0,2,1,0,0,0
		///    0,0,2,2,1,0,0
		///    0,0,2,1,1,1,*
		///    0,2,1,1,2,2,*
		/// 
		/// 3. Two 'lowest' treats in different columns
		/// 
		///    0,0,0,0,0,0,0
		///    0,0,0,0,0,0,0
		///    0,0,2,1,1,0,0
		///    1,*,2,2,2,*,1
		///    1,0,1,1,2,0,1
		///    2,0,1,1,2,0,2
		/// </remarks>
		public static int GetScore(Field field, byte ply)
		{
#if !DEBUG
			unchecked
			{
#endif
				var redToMove = (ply & 1) == 1;

				ulong fieldRed = field.GetRed();
				ulong fieldYel = field.GetYellow();

				ulong threatRed = 0;
				ulong threatYel = 0;

				var trippleRed = 0;
				var tripplYel = 0;

				#region Detect threats

				for (var index = 0; index < 69; index++)
				{
					var mask = Field.Connect4[index];
					var matchRed = fieldRed & mask;
					if (matchRed == mask) { return Scores.RedWins[ply - 1]; }

					var matchYel = fieldYel & mask;
					if (matchYel == mask) { return Scores.YelWins[ply & ~1]; }

					if (matchRed != 0)
					{
						// Both have a match. Skip it.
						if (matchYel != 0) { continue; }

						var base3 = index << 2;

						// 3 out of 4, so add threat.
						if (matchRed == Field.Connect3Out4[base3] ||
							matchRed == Field.Connect3Out4[base3 | 1] ||
							matchRed == Field.Connect3Out4[base3 | 2] ||
							matchRed == Field.Connect3Out4[base3 | 3])
						{
							trippleRed++;
							threatRed |= mask;
						}
					}
					else
					{
						var base3 = index << 2;

						// 3 out of 4, so add threat.
						if (matchYel == Field.Connect3Out4[base3] ||
							matchYel == Field.Connect3Out4[base3 | 1] ||
							matchYel == Field.Connect3Out4[base3 | 2] ||
							matchYel == Field.Connect3Out4[base3 | 3])
						{
							tripplYel++;
							threatYel |= mask;
						}
					}
				}
				#endregion

				// No threats, so no score.
				if (trippleRed == 0 && tripplYel == 0) { return 0; }

				var fieldMix = fieldRed | fieldYel;

				// filled cells can not be threats, so clean that.
				threatRed &= ~fieldMix;
				threatYel &= ~fieldMix;

				// predicted forced wins based on double threat.
				var forcedWinRed = NotSet;
				var forcedWinYel = NotSet;

				// number of direct threats.
				var threatInstantRed = 0;
				var threatInstantYel = 0;

				// The row of the strongest single threat.
				var threatStrongRowRed = NotSet;
				var threatStrongRowYel = NotSet;

				// The column of the strongest single threat.
				var threatStrongColRed = NotSet;
				var threatStrongColYel = NotSet;

				var threatRedLowerYel = 0;
				var threatYelLowerRed = 0;
				var threatLowerRow0 = NotSet;
				var threatLowerRow1 = NotSet;

				for (var col = 0; col < 7; col++)
				{
					var colRed = threatRed & Field.ColumnMasks[col];
					var colYel = threatYel & Field.ColumnMasks[col];

					// Column without threats.
					if (colRed == 0 && colYel == 0) { continue; }

					var colMix = fieldMix & Field.ColumnMasks[col];

					var colHighestFilled = -1;
					var threatLowestColumnRed = NotSet;
					var threatLowestColumnYel = NotSet;

					for (var row = 0; row < 6; row++)
					{
						ulong mask = 1UL << ((row << 3) | col);

						// The cell is already filled, so no threat.
						if ((colMix & mask) != 0)
						{
							colHighestFilled = row;
							continue;
						}

						var gap = row - colHighestFilled;

						// A red threat in the cell.
						if ((colRed & mask) != 0)
						{
							// Instant threat.
							if (gap == 1)
							{
								if (redToMove)
								{
									return Scores.RedWins[ply];
								}
								else
								{
									threatInstantRed++;
								}
							}
							// we found the first threat in this column.
							if (threatLowestColumnRed == NotSet)
							{
								threatLowestColumnRed = row;

								// Strong odd-row threat (zero based).
								if ((row & 1) == 0 && row < threatStrongRowRed)
								{
									threatStrongRowRed = row;
									threatStrongColRed = col;
								}
							}
							// Two connected threats in the same column and
							// the lowest of the two is exclusive for red.
							else if (
								threatLowestColumnRed + 1 == row &&
								threatLowestColumnRed <= threatLowestColumnYel)
							{
								// the cells to fill in the turns to come.
								var delay = 0;
								var distance = threatLowestColumnRed - colHighestFilled;
								var otherOptions = SearchTree.MaximumDepth - (5 - colHighestFilled) - ply;
								if (redToMove)
								{
									distance--;
									delay = DoubleThreatDelay[distance | (Math.Min(5, otherOptions) << 3)];
								}
								else
								{
									delay = DoubleThreatDelay[distance | (Math.Min(5, otherOptions) << 3)];
									delay--;
								}

								var forced = ply + delay;

								// We found potentially a quick win.
								if (forced < forcedWinRed)
								{
									forcedWinRed = forced;
									break;
								}
							}
							// further searching is meaningless.
							else if (row >= threatStrongRowRed)
							{
								// This column is for red.
								if (gap > 1 && threatLowestColumnRed < threatLowestColumnYel)
								{
									threatRedLowerYel++;
									if (row < threatLowerRow0)
									{
										threatLowerRow1 = threatLowerRow0;
										threatLowerRow0 = row;
									}
									else if (row < threatLowerRow1)
									{
										threatLowerRow1 = row;
									}
								}
								break;
							}
						}
						// A yellow threat in the cell.
						if ((colYel & mask) != 0)
						{
							// Instant threat.
							if (gap == 1)
							{
								if (redToMove)
								{
									threatInstantYel++;
								}
								else
								{
									return Scores.YelWins[ply];
								}
							}
							// we found the first threat in this column.
							if (threatLowestColumnYel == NotSet)
							{
								threatLowestColumnYel = row;

								// Strong even-row threat (zero based).
								if ((row & 1) == 1 && row < threatStrongRowYel)
								{
									threatStrongRowYel = row;
									threatStrongColYel = col;
								}
								// This column is for yellow.
								if (gap > 1 && threatLowestColumnYel < threatLowestColumnRed)
								{
									threatYelLowerRed++;
									if (row < threatLowerRow0)
									{
										threatLowerRow1 = threatLowerRow0;
										threatLowerRow0 = row;
									}
									else if (row < threatLowerRow1)
									{
										threatLowerRow1 = row;
									}
								}
							}
							// Two connected threats in the same column and
							// the lowest of the two is exclusive for red.
							else if (
								threatLowestColumnYel + 1 == row &&
								threatLowestColumnYel <= threatLowestColumnRed)
							{
								// the cells to fill in the turns to come.
								var delay = 0;
								var distance = threatLowestColumnYel - colHighestFilled;
								var otherOptions = SearchTree.MaximumDepth - (5 - colHighestFilled) - ply;
								if (!redToMove)
								{
									distance--;
									delay = DoubleThreatDelay[distance | (Math.Min(5, otherOptions) << 3)];
								}
								else
								{
									delay = DoubleThreatDelay[distance | (Math.Min(5, otherOptions) << 3)];
									delay--;
								}

								var forced = ply + delay;

								// We found potentially a quick win.
								if (forced < forcedWinYel)
								{
									forcedWinYel = forced;
									break;
								}
							}
							// further searching is meaningless.
							else if (row >= threatStrongRowYel)
							{
								break;
							}
						}
					}
				}

				// Two spots to claim victory
				if (threatInstantRed > 1 && threatInstantYel == 0)
				{
					return Scores.RedWins[ply + 1];
				}
				if (threatInstantYel > 1 && threatInstantRed == 0)
				{
					return Scores.YelWins[ply + 1];
				}

				if (forcedWinRed != NotSet || forcedWinYel != NotSet)
				{
					if (forcedWinRed < forcedWinYel)
					{
						return Scores.RedWins[forcedWinRed];
					}
					else
					{
						return Scores.YelWins[forcedWinYel];
					}
				}

				var score = 0;

				// Double lower threat for red.
				if (threatRedLowerYel > 1 && threatYelLowerRed == 0)
				{
					//var turn = SearchTree.MaximumDepth - 12 + threatLowerRow0 + threatLowerRow1;
					//return Scores.RedWins[turn | 1];
					score += Scores.StrongThreat;
					
				}
				// Double lower threat for yel.
				if (threatYelLowerRed > 1 && threatRedLowerYel == 0)
				{
					//var turn = SearchTree.MaximumDepth - 12 + threatLowerRow0 + threatLowerRow1;
					//return Scores.RedWins[turn & ~1];
					score -= Scores.StrongThreat;
				}

				if (threatStrongRowRed != NotSet)
				{
					// Lowest threat is red
					if (threatStrongRowRed < threatStrongRowYel)
					{
						score += Scores.StrongThreat;
					}
					// Lowest threat in same column for yellow.
					else if (threatStrongColYel == threatStrongColRed)
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
				else if (threatStrongRowYel != NotSet)
				{
					score -= Scores.StrongThreat;
				}
				score += trippleRed;
				score -= tripplYel;
				return score;
#if !DEBUG
			}
#endif
		}
	}
}
