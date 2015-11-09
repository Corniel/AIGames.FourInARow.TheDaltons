using System;

namespace AIGames.FourInARow.TheDaltons
{
	public class Book
	{
		public const byte NoMove = byte.MaxValue;

		public Book() : this(unchecked((int)DateTime.UtcNow.Ticks)) { }
		
		public Book(int seed)
		{
			Console.Error.WriteLine("Seed: {0}", seed);
			Rnd = new Random(seed);
		}
		public Random Rnd { get; protected set; }

		public byte GetMove(Field field, int ply)
		{
			if (ply == 1) { return 3; }
			if (ply < 4) { return Rnd.Next(0, 2) == 0 ? (byte)2 : (byte)4; }

			return Book.NoMove;
		}
	}
}
