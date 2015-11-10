using System;
using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public abstract class SearchTreeNode : IEquatable<SearchTreeNode>
	{
		public const int InitialAlpha = int.MinValue;
		public const int InitialBeta = int.MaxValue;

		protected SearchTreeNode(Field field, byte depth, int value)
		{
			Field = field;
			Depth = depth;
			Value = value;
			Score = value;
		}
		public readonly Field Field;
		public readonly byte Depth;

		public abstract int Count { get; }
		public int Score { get; protected set; }
		public int Value { get; protected set; }

		public abstract int Apply(byte depth, SearchTree tree, int alpha, int beta);

		public abstract IEnumerable<SearchTreeNode> GetChildren();

		public override int GetHashCode() { return Field.GetHashCode(); }
		public override bool Equals(object obj) { return Equals((SearchTreeNode)obj); }
		public bool Equals(SearchTreeNode other) { return Field.Equals(other.Field); }
	}
}
