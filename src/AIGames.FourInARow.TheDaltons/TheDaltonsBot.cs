using AIGames.FourInARow.TheDaltons.Communication;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.FourInARow.TheDaltons
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class TheDaltonsBot : IBot
	{
		public Settings Settings { get; set; }
		public GameState State { get; set; }
		public SearchTree Tree { get; set; }

		public TheDaltonsBot()
		{
			Tree = new SearchTree();
		}

		public void ApplySettings(Settings settings)
		{
			Settings = settings;
		}

		public void Update(GameState state)
		{
			State = state;
		}

		public BotResponse GetResponse(TimeSpan time)
		{
			// Take 1/3 of the thinking time up to 1.2 seconds.
			var max = Math.Min(time.TotalMilliseconds / 3, 1200);
			// Take 500 ms or if you're really getting out of time 3/4 of the max.
			var min = Math.Min(500, (max * 3) / 4);
			
			Tree.Clear(State.Ply);

			var col = Tree.GetMove(State.Field, State.Ply, TimeSpan.FromMilliseconds(min), TimeSpan.FromMilliseconds(max));

			var move = new MoveInstruction(col);

			var response = new BotResponse()
			{
				Move = move,
				Log = "",
			};
			return response;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Round {0} ({1}): {2}", State.Round, State.Ply, State.Field);
			}
		}
	}
}
