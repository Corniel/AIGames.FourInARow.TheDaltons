namespace AIGames.FourInARow.TheDaltons
{
	public class MoveGenerator
	{
		private const ulong RowMask = 0x010101010101;

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
					case 0x000000000000: break;
					case 0x000000000001: row = 1; break;
					case 0x000000000101: row = 2; break;
					case 0x000000010101: row = 3; break;
					case 0x000001010101: row = 4; break;
					case 0x000101010101: row = 5; break;
					case 0x010101010101: row = 6; break;
					default: break;
				}
				if (row != 6)
				{
					var move = 1UL << ((row << 3) | col);
					moves[col] = IsRed ? field.MoveRed(move) : field.MoveYellow(move);
				}
			}
			return moves;
		}
	}
}
