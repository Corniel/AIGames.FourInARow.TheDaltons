namespace AIGames.FourInARow.TheDaltons.Communication
{
	public static class UpdateGameInstruction
	{
		internal static IInstruction Parse(string[] splited)
		{
			switch (splited[2])
			{
				case "round": return RoundInstruction.Parse(splited);
				case "field": return FieldInstruction.Parse(splited);
			}
			return null;
		}
	}
}
