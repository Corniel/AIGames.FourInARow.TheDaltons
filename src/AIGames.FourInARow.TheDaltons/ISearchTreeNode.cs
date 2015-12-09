using System;

namespace AIGames.FourInARow.TheDaltons
{
	public interface ISearchTreeNode
	{
		Field Field { get; }
		byte Depth { get;  }
		int Score { get; }

		void Add(MoveCandidates candidates);
		int Apply(byte depth, ISearchTree tree, int alpha, int beta);
	}
}
