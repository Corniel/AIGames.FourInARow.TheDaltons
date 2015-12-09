namespace AIGames.FourInARow.TheDaltons
{
	public abstract class SearchTreeNode : ISearchTreeNode
	{
		protected SearchTreeNode(Field field, byte depth, int score)
		{
			Field = field;
			Depth = depth;
			Score = score;
		}
		public Field Field { get; private set; }
		public byte Depth { get; private set; }

		public int Score { get; protected set; }

		public abstract void Add(MoveCandidates candidates);
		public abstract int Apply(byte depth, ISearchTree tree, int alpha, int beta);
	}
}
