using System;

namespace AIGames.FourInARow.TheDaltons
{
	public abstract class SearchTreeNode : IEquatable<SearchTreeNode>
	{
		protected SearchTreeNode(Field field, byte depth)
		{
			Field = field;
			Depth = depth;
		}
		public readonly Field Field;
		public readonly byte Depth;

		public abstract int Count { get; }
		public int Score { get; protected set; }

		public abstract Field Best { get; }

		public abstract int Apply(byte depth, SearchTree tree, int alpha, int beta);

		public override int GetHashCode() { return Field.GetHashCode(); }
		public override bool Equals(object obj) { return Equals((SearchTreeNode)obj); }
		public bool Equals(SearchTreeNode other) { return Field.Equals(other.Field); }
	}
}
