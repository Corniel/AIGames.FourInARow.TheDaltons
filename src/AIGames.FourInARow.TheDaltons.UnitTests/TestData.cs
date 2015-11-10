﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	public static class TestData
	{
		public static readonly Field[] Fields = new Field[]
		{
			Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,0,0,0
				0,0,0,1,0,0,0
				0,0,0,1,2,0,0
				0,0,2,1,2,0,0"),

			Field.Parse(@"
				2,2,1,1,2,1,0
				1,1,2,1,2,1,0
				2,2,1,2,2,1,0
				1,2,1,1,1,2,0
				2,1,2,2,2,1,0
				1,2,2,1,1,2,0"),

			Field.Parse(@"
				0,0,0,1,0,0,0
				0,0,2,1,0,0,0
				0,0,2,1,0,0,0
				0,0,1,2,0,0,0
				0,0,1,1,1,0,0
				2,0,2,1,2,2,2"),
			
			Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,2,0,0")
		};
	}
}
