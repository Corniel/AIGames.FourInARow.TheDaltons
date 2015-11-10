using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeBookNode: SearchTreeRedNode
	{
		public SearchTreeBookNode(Field field, int value) : base(field, 9, value) { }

		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			base.Apply(depth, tree, alpha, beta);
			if (Value > Score)
			{
				Score = Value;
			}
			return Score;
		}
	}
}
