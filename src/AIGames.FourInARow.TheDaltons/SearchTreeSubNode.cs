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
				Score = 7-children.Count;
			}
			
			// This node is final. return its score.
			if (children.Count < 2) 
			{
				return Score; 
			}
			
			if (ApplyChildren(depth, tree, alpha, beta))
			{
				children.Clear();
				return Score;
			}
			children.Sort();

			
			
			// only update if the test <> 0.
			// Otherwise the 'options score' is best.
			var test = children[0].Score;
			if(test != 0)
			{
				Score = test;
			}
			
			if (Score == WinningScore)
			{
				for (var i = children.Count - 1; i > 0; i--)
				{
					var child = children[i];
					if (child.Score != WinningScore)
					{
						children.Remove(child);
					}
					else
					{
						break;
					}
				}
			}
			else if (Score != LosingScore)
			{
				for (var i = children.Count - 1; i > 0; i--)
				{
					var child = children[i];
					if (child.Score == LosingScore)
					{
						children.Remove(child);
					}
					else
					{
						break;
					}
				}
			}
			return Score;
		}
		protected abstract bool ApplyChildren(byte depth, SearchTree tree, int alpha, int beta);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{3} Depth: {0}, Score: {1}, Children: {2}, {4}",
					Depth, 
					Score, 
					children == null ? 0 : children.Count,
					IsMax ? "Red": "Yellow",
					Field);
			}
		}
	}
}
