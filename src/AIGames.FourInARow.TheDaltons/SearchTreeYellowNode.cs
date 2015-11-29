using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeYellowNode : SearchTreeSubNode
	{
		public SearchTreeYellowNode(Field field, byte depth, int value) : base(field, depth, value) { }

		public override bool IsWinning(int score) { return score < Scores.YelWin; }
		public override bool IsLosing(int score) { return score > Scores.RedWin; }

		/// <summary>Sort nodes on score.</summary>
		public override int Compare(ISearchTreeNode x, ISearchTreeNode y)
		{
			// Lower scores first.
			return unchecked(x.Score - y.Score);
		}

		protected override int ApplyChildren(byte depth, ISearchTree tree, int alpha, int beta)
		{
			Score = Scores.RedWins(Depth);
			for (var i = 0; i < Children.Count;i++ )
			{
				var child = Children[i];

				var sdepth = depth > 6 ? (byte)unchecked(depth - i) : depth;

				var test = child.Apply(sdepth, tree, alpha, beta);
				if (test < Score)
				{
					Score = test;
					if (Score < beta)
					{
						beta = Score;
					}
				}
				else if (beta <= alpha)
				{
					break;
				}
			}
			return Score;
		}
	}
}
