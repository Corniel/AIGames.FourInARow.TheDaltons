using System;
namespace AIGames.FourInARow.TheDaltons.UnitTests.Mocking
{
	public class SearchTreeNodeStub: ISearchTreeNode
	{
		public Field Field { get; set; }
		public byte Depth { get; set; }
		public bool IsFinal { get; set; }
		public int Score { get; set; }
		public int Value { get; set; }

		public void Add(MoveCandidates candidates) { throw new NotImplementedException(); }
		public int Apply(byte depth, ISearchTree tree, int alpha, int beta) { return Score; }

		public bool Equals(ISearchTreeNode other) { return false; }

		public override string ToString()
		{
			return string.Format("SubNode: {0}, {1}", Scores.GetFormatted(Score), Field);
		}


		
	}
}
