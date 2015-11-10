using System;
using System.Linq;
using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Mocking
{
	public class SearchTreeStub : ISearchTree
	{
		public SearchTreeStub()
		{
			TimeLeft = true;
			Lookup = new Dictionary<Field, ISearchTreeNode>();
		}
		public Dictionary<Field, ISearchTreeNode> Lookup { get; set; }

		public Field[] GetMoves(Field field, bool IsRed)
		{
			return Lookup.Keys.ToArray();
		}

		public ISearchTreeNode GetNode(Field search, byte ply)
		{
			return Lookup[search];
		}

		public bool TimeLeft { get; set; }

		public void Add(List<ISearchTreeNode> nodes)
		{
			foreach (var node in nodes)
			{
				Lookup[node.Field] = node;
			}
		}
	}
}
