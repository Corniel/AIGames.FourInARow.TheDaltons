using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeBookNode : SearchTreeRedNode
	{
		private int m_Value;
		private bool m_IsWinning;

		public SearchTreeBookNode(Field field, byte ply, int score) : base(field, ply, score) 
		{
			m_Value = score;
			m_IsWinning = Scores.IsWinning(score);
		}

		public override int Apply(byte depth, ISearchTree tree, int alpha, int beta)
		{
			Score = base.Apply(depth, tree, alpha, beta);

			// We want to know more.
			if (!m_IsWinning)
			{
				Score += m_Value;
			}
			return Score;
		}
	}
}
