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
			// Take 2/3 of the thinking time up to 6 seconds.
			var max = Math.Min(2 * time.TotalMilliseconds / 3, 6000);
			// Take 5 seconds or if you're really getting out of time 1/2 of the max.
			var min = Math.Min(5000, max / 2);

			if (State.Ply < MinTime.Length)
			{
				min = MinTime[State.Ply];
			}
			if (State.Ply < MaxTime.Length)
			{
				if (MaxTime[State.Ply] > 0)
				{
					max = MaxTime[State.Ply];
				}
			}
			Tree.Clear(State.Ply);

			var col = Tree.GetMove(State.Field, TimeSpan.FromMilliseconds(min), TimeSpan.FromMilliseconds(max));

			// Just to be sure.
			if (State.Ply == 1) { col = 3; }

			var move = new MoveInstruction(col);

			var response = new BotResponse()
			{
				Move = move,
				Log = Tree.Logger.ToString(),
			};
			return response;
		}

		private static readonly int[] MinTime =
		{
			/* 00 */ 0, 
			/* 01 */ 500,
			/* 02 */ 1000,
			/* 03 */ 500,
			/* 04 */ 1000,
			/* 05 */ 500,
			/* 06 */ 1000,
			/* 07 */ 500,
			/* 08 */ 2000,
			/* 09 */ 1000,
			/* 10 */ 5000,
			/* 11 */ 5000,
		};

		private static readonly int[] MaxTime =
		{
			/* 00 */ 0, 
			/* 01 */ 600,
			/* 02 */ 1500,
			/* 03 */ 600,
			/* 04 */ 1500,
			/* 05 */ 600,
			/* 06 */ 1500,
			/* 07 */ 600,
			/* 08 */ 1500,
			/* 09 */ 1500,
			/* 10 */ 0,
		};

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
