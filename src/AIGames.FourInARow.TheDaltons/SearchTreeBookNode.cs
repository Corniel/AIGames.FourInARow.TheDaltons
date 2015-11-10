using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeBookNode: SearchTreeRedNode
	{
		public SearchTreeBookNode(Field field, int value) : base(field, 9, value) { }

		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			base.Apply(depth, tree, alpha, beta);

			// Those nodes are important.
			if (Value == Scores.RedWin || Value == Scores.YelWin)
			{
				Score = Value;
			}
			return Score;
		}
	}
}
