using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
	partial class Program
	{
		public class InventoryOre
		{
			double cobalt = 0;
			double gold = 0;
			double ice = 0;
			double iron = 0;
			double magnesium = 0;
			double nickel = 0;
			double platinum = 0;
			double silicon = 0;
			double silver = 0;
			double stone = 0;
			double uranium = 0;

			public InventoryOre(double cobalt, double gold, double ice, double iron, double magnesium, double nickel, double platinum, double silicon, double silver, double stone, double uranium)
			{
				this.Cobalt = cobalt;
				this.Gold = gold;
				this.Ice = ice;
				this.Iron = iron;
				this.Magnesium = magnesium;
				this.Nickel = nickel;
				this.Platinum = platinum;
				this.Silicon = silicon;
				this.Silver = silver;
				this.Stone = stone;
				this.Uranium = uranium;
			}

			public double Cobalt
			{
				get
				{
					return cobalt;
				}

				set
				{
					cobalt = value;
				}
			}

			public double Gold
			{
				get
				{
					return gold;
				}

				set
				{
					gold = value;
				}
			}

			public double Ice
			{
				get
				{
					return ice;
				}

				set
				{
					ice = value;
				}
			}

			public double Iron
			{
				get
				{
					return iron;
				}

				set
				{
					iron = value;
				}
			}

			public double Magnesium
			{
				get
				{
					return magnesium;
				}

				set
				{
					magnesium = value;
				}
			}

			public double Nickel
			{
				get
				{
					return nickel;
				}

				set
				{
					nickel = value;
				}
			}

			public double Platinum
			{
				get
				{
					return platinum;
				}

				set
				{
					platinum = value;
				}
			}

			public double Silicon
			{
				get
				{
					return silicon;
				}

				set
				{
					silicon = value;
				}
			}

			public double Silver
			{
				get
				{
					return silver;
				}

				set
				{
					silver = value;
				}
			}

			public double Stone
			{
				get
				{
					return stone;
				}

				set
				{
					stone = value;
				}
			}

			public double Uranium
			{
				get
				{
					return uranium;
				}

				set
				{
					uranium = value;
				}
			}
		}
	}
}
