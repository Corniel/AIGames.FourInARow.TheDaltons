using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class SearchTreeSubNode : SearchTreeNode
	{
		public SearchTreeSubNode(Field field, byte depth, int value) : base(field, depth, value) { }

		public SearchTreeNodes Children { get; protected set; }

		public override void Add(MoveCandidates candidates)
		{
			// Just set.
			Children = new SearchTreeNodes();
			Children.AddRange(candidates.Select(candidate => candidate.Node));
		}
		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			if (depth < Depth || !tree.TimeLeft) { return Score; }

			// If no children, get moves.
			if (Children == null)
			{
				var items = tree.GetMoves(Field, (Depth & 1) == 1);
				var childDepth = (byte)(Depth + 1);

				Children = new SearchTreeNodes();

				var item0 = items[0];
				var item1 = items[1];
				var item2 = items[2];
				var item3 = items[3];
				var item4 = items[4];
				var item5 = items[5];
				var item6 = items[6];
				
				if (item2 != Field.Empty) { Children.Add(tree.GetNode(item2, childDepth)); }
				if (item4 != Field.Empty) { Children.Add(tree.GetNode(item4, childDepth)); }

				if (item3 != Field.Empty) { Children.Add(tree.GetNode(item3, childDepth)); }

				if (item1 != Field.Empty) { Children.Add(tree.GetNode(item1, childDepth)); }
				if (item5 != Field.Empty) { Children.Add(tree.GetNode(item5, childDepth)); }

				if (item0 != Field.Empty) { Children.Add(tree.GetNode(item0, childDepth)); }
				if (item6 != Field.Empty) { Children.Add(tree.GetNode(item6, childDepth)); }
			}
			Score = ApplyChildren(depth, tree, alpha, beta);
			return Score;
		}

		protected abstract int ApplyChildren(byte depth, ISearchTree tree, int alpha, int beta);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{3} Depth: {0}, Score: {1}, Children: {2}, {4}",
					Depth,
					Scores.GetFormatted(Score),
					Children == null ? 0 : Children.Count,
					GetType().Name.Substring("SearchTree".Length),
					Field);
			}
		}
	}
}
