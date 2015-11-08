using System;

namespace AIGames.FourInARow.TheDaltons.Communication
{
	public class MoveInstruction : IInstruction
	{
		public MoveInstruction(byte colomn)
		{
			// The internal representation is mirrored to the of the challenge.
			Column = (byte)(6 - colomn);
		}

		private readonly Byte Column;

		public override string ToString() { return string.Format("place_disc {0}", Column); }
	}
}
