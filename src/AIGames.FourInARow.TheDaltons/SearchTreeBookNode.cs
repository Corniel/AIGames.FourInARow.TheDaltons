using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeBookNode : SearchTreeRedNode
	{
		public SearchTreeBookNode(Field field, byte ply, int value) : base(field, ply, value) { }

		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			Score = base.Apply(depth, tree, alpha, beta);

			// We want to 
			if (!Scores.IsWinning(Score))
			{
				Score += Value;
			}
			return Score;
		}
	}
}
