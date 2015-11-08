using System.Diagnostics;
namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SearchTreeEndNode : SearchTreeNode
	{
		public SearchTreeEndNode(Field field, byte depth, bool odd) : base(field, depth) 
		{
			Score = odd ? Scores.Red : Scores.Yel;
		}

		public override int Count { get { return 0; } }
		public override Field Best { get { return Field; } }

		public override int Apply(byte depth, SearchTree tree, int alpha, int beta) { return Score; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}, Score: {1}", Depth, Score);
			}
		}
	}
}
