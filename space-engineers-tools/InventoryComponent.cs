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
		public enum ComponentType
		{
			SteelPlate
		};

		public class InventoryComponent
		{
			Dictionary<ComponentType, decimal> components = new Dictionary<ComponentType, decimal>();

			public InventoryComponent()
			{
				components.Add(ComponentType.SteelPlate, 0);
			}

			public decimal GetValue(ComponentType type)
			{
				return components[type];
			}

			public void SetValue(ComponentType type, decimal value)
			{
				components[type] = value;
			}

			public IEnumerable<ComponentType> GetKeys()
			{
				return components.Keys;
			}

			int missileContainer = 0;
			int ammoContainer = 0;
			int magazine= 0;
			int bulletproofGlass = 0;
			int canvas = 0;
			//int components = 0;
			int computer = 0;
			int constructionComponent = 0;
			int detectorComponents = 0;
			int display = 0;
			int explosives = 0;
			int girder = 0;
			int gravityGeneratorComponents = 0;
			int interiorPlate = 0;
			int largeSteelTube = 0;
			int medicalComponents = 0;
			int metalGrid = 0;
			int motor = 0;
			int powerCell= 0;
			int radioCommunicationComponents = 0;
			int reactorComponents = 0;
			int smallStellTube = 0;
			int solarCell = 0;
			int steelPlate = 0;
			int superconductorComponent = 0;
			int thrusterComponent = 0;
			
		}
	}
}
