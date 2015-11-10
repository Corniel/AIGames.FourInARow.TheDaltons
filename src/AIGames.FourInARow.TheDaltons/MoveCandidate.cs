using System.Diagnostics;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class MoveCandidate
	{
		public MoveCandidate(byte move, ISearchTreeNode node)
		{
			this.Move = move;
			this.Node = node;
		}

		public byte Move { get; private set; }
		public ISearchTreeNode Node { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{{{0}}} {1}", Move, Scores.GetFormatted(Node.Score));
			}
		}

	}
}
