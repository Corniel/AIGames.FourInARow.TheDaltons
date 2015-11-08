using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons.Communication
{
	public class GameState
	{
		public int Round { get; set; }
		public Field Field { get; set; }

		public bool Apply(IInstruction instruction)
		{
			if (Mapping.ContainsKey(instruction.GetType()))
			{
				Mapping[instruction.GetType()].Invoke(instruction, this);
				return true;
			}
			return false;
		}

		public static GameState Create(IEnumerable<IInstruction> instructions)
		{
			var state = new GameState();

			foreach(var instruction in instructions.Where(i => Mapping.ContainsKey(i.GetType())))
			{
				Mapping[instruction.GetType()].Invoke(instruction, state);
			}
			return state;
		}

		private static Dictionary<Type, Action<IInstruction, GameState>> Mapping = new Dictionary<Type, Action<IInstruction, GameState>>()
		{
			{ 
				typeof(FieldInstruction), (instruction, state) =>
				{
					var inst = (FieldInstruction)instruction;
					state.Field = inst.Field;
				}
			},
			{ 
				typeof(RoundInstruction), (instruction, state) => { state.Round = ((RoundInstruction)instruction).Value; }
			},
		};
	}
}
