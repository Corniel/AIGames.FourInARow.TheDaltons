using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SearchTreeEndNode : SearchTreeNode
	{
		public SearchTreeEndNode(Field field, byte depth, bool redToMove) : base(field, depth) 
		{
			Score = redToMove ? Scores.Yel : Scores.Red;
		}

		public override int Count { get { return 0; } }
	
		public override IEnumerable<SearchTreeNode> GetChildren() { return Enumerable.Empty<SearchTreeNode>(); }

		public override int Apply(byte depth, SearchTree tree, int alpha, int beta) { return Score; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}, Score: {1}", Depth, Score);
			}
		}
	}
}
