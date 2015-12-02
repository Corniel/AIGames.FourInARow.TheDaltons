using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public abstract class SearchTreeSubNode : SearchTreeNode, IComparer<ISearchTreeNode>
	{
		public SearchTreeSubNode(Field field, byte depth, int value) : base(field, depth, value) { }

		public abstract bool IsWinning(int score);
		public abstract bool IsLosing(int score);

		public List<ISearchTreeNode> Children { get; protected set; }
		protected byte LastDepth { get; set; }

		public abstract int Compare(ISearchTreeNode x, ISearchTreeNode y);

		public override void Add(MoveCandidates candidates)
		{
			// Just set.
			Children = candidates.Select(candidate => candidate.Node).ToList();
		}
		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			if (IsFinal || depth < Depth || depth == LastDepth || !tree.TimeLeft) { return Score; }
			
			// We don't want to do this more than once per depth.
			LastDepth = depth;

			// If no children, get moves.
			if (Children == null)
			{
				var items = tree.GetMoves(Field, (Depth & 1) == 1);
				var childDepth = (byte)(Depth + 1);

				Children = new List<ISearchTreeNode>();

				foreach (var item in items.Where(e => e != Field.Empty))
				{
					var child = tree.GetNode(item, childDepth);
					Children.Add(child);
				}
			}
			Score = ApplyChildren(depth, tree, alpha, beta);
			Children.Sort(this);
			IsFinal = Children.Count == 1 || Children.All(ch => ch.IsFinal);
		
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
