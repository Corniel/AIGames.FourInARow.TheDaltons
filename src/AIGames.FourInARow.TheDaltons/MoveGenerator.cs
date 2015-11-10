namespace AIGames.FourInARow.TheDaltons
{
	public class MoveGenerator
	{
		private const ulong RowMask = 0x010101010101;
		private static readonly ulong[,] Moves = GenerateMoves();

		public Field[] GetMoves(Field field, bool IsRed)
		{
			var moves = new Field[7];

			var occupied = field.Occupied;

			for (var col = 0; col < 7; col++)
			{
				var test = (occupied >> col) & RowMask;
				var row = 0;
				switch (test)
				{
					case 0x0000000000: row = 0; break;
					case 0x0000000001: row = 1; break;
					case 0x0000000101: row = 2; break;
					case 0x0000010101: row = 3; break;
					case 0x0001010101: row = 4; break;
					case 0x0101010101: row = 5; break;
					default: row = -1; break;
				}
				if (row != -1)
				{
					var move = Moves[row, col];
					moves[col] = IsRed ? field.MoveRed(move) : field.MoveYellow(move);
				}
			}
			return moves;
		}

		private static ulong[,] GenerateMoves()
		{
			var moves = new ulong[6, 7];
			for (var row = 0; row < 6; row++)
			{
				for (var col = 0; col < 7; col++)
				{
					moves[row, col] = 1UL << (col + (row << 3));
				}
			}
			return moves;
		}
	}
}
