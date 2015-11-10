using System;
namespace AIGames.FourInARow.TheDaltons
{
	public interface ISearchTree
	{
		Field[] GetMoves(Field field, bool IsRed);
		ISearchTreeNode GetNode(Field search, byte ply);

		bool TimeLeft { get; }
	}
}
