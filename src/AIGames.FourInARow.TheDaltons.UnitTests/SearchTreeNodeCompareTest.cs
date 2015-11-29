using AIGames.FourInARow.TheDaltons.UnitTests.Mocking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class SearchTreeNodeCompareTest
	{
		[Test]
		public void Compare_YellowBiggerLeftThenRight_IsPositive()
		{
			var node = new SearchTreeYellowNode(Field.Empty, 0, 0);

			var l = new SearchTreeNodeStub() { Score = 10 };
			var r = new SearchTreeNodeStub() { Score = 2 };

			var act = node.Compare(l, r);
			Assert.IsTrue(act > 0);
		}

		[Test]
		public void Compare_YellowBiggerLeftThenRight_IsNegative()
		{
			var node = new SearchTreeRedNode(Field.Empty, 0, 0);

			var l = new SearchTreeNodeStub() { Score = 10 };
			var r = new SearchTreeNodeStub() { Score = 2 };

			var act = node.Compare(l, r);
			Assert.IsTrue(act < 0);
		}
	}
}
