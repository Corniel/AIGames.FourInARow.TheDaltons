using NUnit.Framework;
using System;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class SearchTreeTest
	{
		[Test]
		public void GetMove_Initial_()
		{
			var tree = new SearchTree();
			var act = tree.GetMove(Field.Empty, 1, TimeSpan.Zero, TimeSpan.MaxValue);
		}
	}
}
