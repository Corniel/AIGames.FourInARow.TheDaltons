using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class SearchTreeSubNode<T> : SearchTreeNode where T : SearchTreeNode
	{
		public SearchTreeSubNode(Field field, byte depth) : base(field, depth) { }

		public abstract bool IsMax { get; }
		public override Field Best
		{
			get
			{
				return children == null || children.Count == 0 ? Field.Empty : children[0].Field;
			}
		}

		public override int Count { get { return children == null ? -1 : children.Count; } }

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
			if (children.Count == 0) { return Score; }

			if (Score == 0 && (Depth + 2) == depth)
			{
				if (IsMax)
				{
					Score = children.Count(ch => ch.Count == 0);
				}
				else
				{
					Score = -children.Count(ch => ch.Count == 0);
				}
			}

			if (ApplyChildren(depth, tree, alpha, beta))
			{
				children.Clear();
				return Score;
			}
			children.Sort();
			var test = children[0].Score;
			if(test != 0)
			{
				Score = test;
			}
			return Score;
		}
		protected abstract bool ApplyChildren(byte depth, SearchTree tree, int alpha, int beta);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}, Score: {1}, Children: {2}",
					Depth, Score, children == null ? 0 : children.Count);
			}
		}
	}
}
