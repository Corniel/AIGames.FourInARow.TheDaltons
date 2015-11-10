using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class SearchTreeSubNode<T> : SearchTreeNode where T : SearchTreeNode
	{
		public SearchTreeSubNode(Field field, byte depth, int value) : base(field, depth, value) { }

		public abstract bool IsMax { get; }
		public abstract int LosingScore { get; }
		public abstract int WinningScore { get; }
		public override int Count { get { return children == null ? -1 : children.Count; } }

		public override IEnumerable<SearchTreeNode> GetChildren()
		{
			return children == null ? Enumerable.Empty<SearchTreeNode>() : children;
		}
		protected List<T> children;

		public override int Apply(byte depth, SearchTree tree, int alpha, int beta)
		{
			if (depth < Depth || !tree.TimeLeft) { return Score; }

			// If no children, get moves.
			if (children == null)
			{
				var items = tree.Generator.GetMoves(Field, (Depth & 1) == 1);
				var childDepth = (byte)(Depth + 1);

				children = new List<T>();

				foreach (var item in items.Where(e => e != Field.Empty))
				{
					var child = tree.GetNode(item, childDepth);
					if (child is T)
					{
						children.Add((T)child);
					}
					// one of the responses is losing. So this tree is losing.
					else if (child is SearchTreeEndNode)
					{
						children.Clear();
						Score = child.Score;
						return Score;
					}
				}
			}

			// This node is final. return its score.
			if (children.Count < 2)
			{
				return Score;
			}

			Score = ApplyChildren(depth, tree, alpha, beta);
			children.Sort();
			return Score;
		}
		protected abstract int ApplyChildren(byte depth, SearchTree tree, int alpha, int beta);
		protected virtual IEnumerable<SearchTreeNode> LoopChildren()
		{
			var prev = int.MinValue;
			var count = 0;
			var max = Depth < 12 ? 7 : 3;
			foreach (var child in children)
			{
				if (count++ < max || child.Score == prev)
				{
					yield return child;
					prev = child.Score;
				}
				else
				{
					yield break;
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{3} Depth: {0}, Score: {1}, Children: {2}, {4}",
					Depth,
					Scores.GetFormatted(Score),
					children == null ? 0 : children.Count,
					IsMax ? "Red" : "Yellow",
					Field);
			}
		}
	}
}
