using System;
using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public static partial class Book
	{
		public static IEnumerable<Field> GetDraws()
		{
			return GetFields(GetDrawString());
		}
		public static IEnumerable<Field> GetLoss()
		{
			return GetFields(GetLossString());
		}

		private static IEnumerable<Field> GetFields(string str)
		{
			var bytes = Convert.FromBase64String(str);

			for (var i = 0; i < bytes.Length; i += 16)
			{
				var subset = new byte[16];
				Array.Copy(bytes, i, subset, 0, 16);

				var field = Field.FromBytes(subset);
				yield return field;
				yield return field.Flip();
			}
		}
	}
}
