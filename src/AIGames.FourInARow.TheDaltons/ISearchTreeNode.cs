using System;

namespace AIGames.FourInARow.TheDaltons
{
	public interface ISearchTreeNode: IEquatable<ISearchTreeNode>
	{
		Field Field { get; }
		byte Depth { get;  }
		bool IsFinal { get; }
		int Score { get; }
		int Value { get; }

		void Add(MoveCandidates candidates);
		int Apply(byte depth, ISearchTree tree, int alpha, int beta);
	}
}
