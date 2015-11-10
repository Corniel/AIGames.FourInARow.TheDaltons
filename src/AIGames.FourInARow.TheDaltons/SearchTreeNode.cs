using System;
using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public abstract class SearchTreeNode : ISearchTreeNode
	{
		protected SearchTreeNode(Field field, byte depth, int value)
		{
			Field = field;
			Depth = depth;
			Value = value;
			Score = value;
		}
		public Field Field { get; private set; }
		public byte Depth { get; private set; }

		public int Score { get; protected set; }
		public int Value { get; protected set; }
		public bool IsFinal { get; protected set; }

		public abstract int Apply(byte depth, ISearchTree tree, int alpha, int beta);

		public override int GetHashCode() { return Field.GetHashCode(); }
		public override bool Equals(object obj) { return Equals((ISearchTreeNode)obj); }
		public bool Equals(ISearchTreeNode other) { return Field.Equals(other.Field); }
	}
}
