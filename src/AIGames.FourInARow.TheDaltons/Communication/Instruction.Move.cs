using System;

namespace AIGames.FourInARow.TheDaltons.Communication
{
	public class MoveInstruction : IInstruction
	{
		public MoveInstruction(byte colomn)
		{
			Column = colomn;
		}

		private readonly Byte Column;

		public override string ToString() { return string.Format("place_disc {0}", Column); }
	}
}
