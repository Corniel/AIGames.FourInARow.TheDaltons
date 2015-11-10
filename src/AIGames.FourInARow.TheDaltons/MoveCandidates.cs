using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public class MoveCandidates : List<MoveCandidate>, IComparer<MoveCandidate>
	{
		public MoveCandidates(bool redToMove)
		{
			RedToMove = redToMove;
		}
		public bool RedToMove { get; private set; }

		public void Add(Field field, byte ply, ISearchTree tree)
		{
			var moves = tree.GetMoves(field, RedToMove);
			for (byte col = 0; col < moves.Length; col++)
			{
				var child = moves[col];
				if (child != Field.Empty)
				{
					var node = tree.GetNode(child, (byte)(ply + 1));
					Add(new MoveCandidate(col, node));
				}
			}
		}

		public byte GetMove()
		{
			Sort(this);
			return this[0].Move;
		}
		
		public int Compare(MoveCandidate x, MoveCandidate y)
		{
			var compare = x.Node.Score.CompareTo(y.Node.Score);
			return RedToMove ? -compare : compare;
		}
	}
}
