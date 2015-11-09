﻿using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeRedNode : SearchTreeSubNode<SearchTreeYellowNode>, IComparable<SearchTreeRedNode>
	{
		public SearchTreeRedNode(Field field, byte depth) : base(field, depth) { }

		public override bool IsMax { get { return true; } }
		public override int LosingScore { get { return Scores.Yel; } }
		public override int WinningScore { get { return Scores.Red; } }

		/// <summary>Sort nodes on score.</summary>
		public int CompareTo(SearchTreeRedNode other)
		{
			// Lower scores first, it is Yellow that may choose.
			var compare = Score.CompareTo(other.Score);
			if (compare == 0)
			{
				// Less children first. More forcing.			
				compare = Count.CompareTo(other.Count);
			}
			// lower discs first.
			if (compare == 0)
			{
				compare = Field.Occupied.CompareTo(other.Field.Occupied);
			}
			return compare;
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
