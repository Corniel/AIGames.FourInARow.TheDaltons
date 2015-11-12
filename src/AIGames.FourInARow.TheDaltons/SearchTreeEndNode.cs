using System;
using System.Diagnostics;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SearchTreeEndNode : SearchTreeNode
	{
		public SearchTreeEndNode(Field field, byte depth, int value) : base(field, depth, value)
		{
			IsFinal = true;
		}

		public override void Add(MoveCandidates candidates) { throw new NotSupportedException(); }
		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta) { return Score; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}, Score: {1}", Depth,Scores.GetFormatted( Score));
			}
		}

		
	}
}
