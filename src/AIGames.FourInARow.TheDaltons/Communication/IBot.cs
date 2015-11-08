using System;

namespace AIGames.FourInARow.TheDaltons.Communication
{
	public interface IBot
	{
		void ApplySettings(Settings settings);
		void Update(GameState state);
		BotResponse GetResponse(TimeSpan time);
	}
}
