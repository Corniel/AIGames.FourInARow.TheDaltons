﻿using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeYellowNode : SearchTreeSubNode<SearchTreeRedNode>, IComparable<SearchTreeYellowNode>
	{
		public SearchTreeYellowNode(Field field, byte depth) : base(field, depth) { }

		public override bool IsMax { get { return false; } }
		public override int LosingScore { get { return Scores.Red; } }
		public override int WinningScore { get { return Scores.Yel; } }

		public int CompareTo(SearchTreeYellowNode other)
		{
			// Higher scores first, it is Red that may choose.
			return other.Score.CompareTo(Score);
		}

		protected override bool ApplyChildren(byte depth, SearchTree tree, int alpha, int beta)
		{
			Score = int.MaxValue;
			foreach (var child in children)
			{
				var test = child.Apply(depth, tree, alpha, beta);
				if (test < Score)
				{
					Score = test;
					if (Score < beta)
					{
						beta = Score;
					}
					if (test == Scores.Yel)
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
