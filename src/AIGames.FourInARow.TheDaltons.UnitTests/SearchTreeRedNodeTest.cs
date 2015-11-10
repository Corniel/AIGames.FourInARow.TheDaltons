using AIGames.FourInARow.TheDaltons.UnitTests.Mocking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class SearchTreeRedNodeTest
	{
		[Test]
		public void Apply_4ChildrenWith1LosingNode_3ChildrenInOrder()
		{
			var nodes = new List<ISearchTreeNode>()
			{
				new SearchTreeNodeStub(){ Field = TestData.Fields[0], Score = -30},
				new SearchTreeNodeStub(){ Field = TestData.Fields[1], Score = +3},
				new SearchTreeNodeStub(){ Field = TestData.Fields[2], Score = -10},
				new SearchTreeNodeStub(){ Field = TestData.Fields[3], Score = Scores.YelWins(3)},
			};
			var tree = new SearchTreeStub();
			tree.Add(nodes);

			var node = new SearchTreeRedNode(Field.Empty, 1, 0);
			node.Apply(2, tree, Scores.InitialAlpha, Scores.InitialBeta);

			var act = node.Children.Select(ch => ch.Field).ToArray();
			var exp = new Field[] { TestData.Fields[1], TestData.Fields[2], TestData.Fields[0], TestData.Fields[3] };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Apply_4ChildrenWith3Winning_3Children()
		{
			var nodes = new List<ISearchTreeNode>()
			{
				new SearchTreeNodeStub(){ Field = TestData.Fields[0], Score = Scores.RedWins(5)},
				new SearchTreeNodeStub(){ Field = TestData.Fields[1], Score = Scores.RedWins(4)},
				new SearchTreeNodeStub(){ Field = TestData.Fields[2], Score = Scores.RedWins(4)},
				new SearchTreeNodeStub(){ Field = TestData.Fields[3], Score = -15},
			};
			var tree = new SearchTreeStub();
			tree.Add(nodes);

			var node = new SearchTreeRedNode(Field.Empty, 1, 0);
			node.Apply(2, tree, Scores.InitialAlpha, Scores.InitialBeta);

			var act = node.Children.Select(ch => ch.Field).ToArray();
			var exp = new Field[] { TestData.Fields[1], TestData.Fields[2], TestData.Fields[0], TestData.Fields[3] };

			CollectionAssert.AreEqual(exp, act);
		}
	}
}
