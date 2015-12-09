using System;
using System.Diagnostics;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SearchTreeEndNode : ISearchTreeNode
	{
		private byte m_depth;
		private int m_score;

		public SearchTreeEndNode(byte depth, int score)
		{
			m_depth = depth;
			m_score = score;
		}

		public Field Field { get { return Field.Empty; } }
		public byte Depth { get { return m_depth; } }
		public int Score { get { return m_score; } }


		public int Apply(byte depth, ISearchTree tree, int alpha, int beta) { return Score; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}, Score: {1} (final)", Depth, Scores.GetFormatted(Score));
			}
		}

		public void Add(MoveCandidates candidates) { throw new NotImplementedException(); }
	}
}
