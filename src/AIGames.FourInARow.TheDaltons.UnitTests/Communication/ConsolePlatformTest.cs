using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Communication
{
	[TestFixture, Category(Category.IntegrationTest)]
	public class ConsolePlatformTest
	{
		[Test]
		public void DoRun_Simple_NoExceptions()
		{
			using(var platform = new ConsolePlatformTester("input.simple.txt"))
			{
				platform.DoRun(new TheDaltonsBot());
			}
		}
	}
}
