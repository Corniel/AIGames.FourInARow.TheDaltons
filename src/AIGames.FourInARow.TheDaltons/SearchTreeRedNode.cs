using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeRedNode : SearchTreeSubNode<SearchTreeYellowNode>, IComparable<SearchTreeRedNode>
	{
		public SearchTreeRedNode(Field field, byte depth) : base(field, depth) { }

		public override bool IsMax { get { return true; } }

		public int CompareTo(SearchTreeRedNode other)
		{
			// higher scores first.
			return -other.Score.CompareTo(Score);
		}

		protected override bool ApplyChildren(byte depth, SearchTree tree, int alpha, int beta)
		{
			Score = int.MinValue;
			foreach (var child in children)
			{
				var test = child.Apply(depth, tree, alpha, beta);
				if (test > Score)
				{
					Score = test;
					if (Score > alpha)
					{
						alpha = Score;
					}
					if (test == Scores.Red)
					{
						return true;
					}
				}
				else if (beta <= alpha)
				{
					break;
				}
			}
			return false;
		}
	}
}
