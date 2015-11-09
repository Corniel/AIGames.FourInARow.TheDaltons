namespace AIGames.FourInARow.TheDaltons
{
	/// <summary>Gets a score for the board.</summary>
	public static class Evaluator
	{
		public static int GetScore(ulong color0, ulong color1)
		{
			var doubles = 0;
			var tribble = 0;

			foreach (var mask in Field.Scores)
			{
				var match = color0 & mask;
				if (match == mask) { return Scores.Red; }

				var count = Bits.Count(match);
				// we have at least two of the combo, and the rest is free.
				if (count > 1 && (color1 & mask) == 0)
				{
					if (count == 2) { doubles++; }
					else { tribble++; }
				}
			}
			return doubles | (tribble << 5);
		}

	}
}
