using System;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeRedNode : SearchTreeSubNode
	{
		public SearchTreeRedNode(Field field, byte depth, int value) : base(field, depth, value) { }

		public override bool IsWinning(int score) { return score > Scores.RedWin; }
		public override bool IsLosing(int score) { return score < Scores.YelWin; }
		
		/// <summary>Sort nodes on score.</summary>
		public override int Compare(ISearchTreeNode x, ISearchTreeNode y)
		{
			// Higher scores first.
			return unchecked(y.Score - x.Score);
		}

		protected override int ApplyChildren(byte depth, ISearchTree tree, int alpha, int beta)
		{
			Score = Scores.YelWins(Depth);
			for (var i = 0; i < Children.Count; i++)
			{
				var child = Children[i];

				var sdepth = depth > 6 ? (byte)unchecked(depth - i) : depth;

				var test = child.Apply(sdepth, tree, alpha, beta);
				if (test > Score)
				{
					Score = test;
					if (Score > alpha)
					{
						alpha = Score;
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
