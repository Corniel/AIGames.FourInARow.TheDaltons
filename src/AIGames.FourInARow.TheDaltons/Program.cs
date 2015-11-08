using System;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.FourInARow.TheDaltons
{
	public class Program
	{
		[ExcludeFromCodeCoverage]
		public static void Main(string[] args)
		{
			Communication.ConsolePlatform.Run(new TheDaltonsBot());
		}
	}
}
