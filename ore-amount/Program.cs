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
	partial class Program : MyGridProgram
	{
		// This list of LCD types defines the supported LCD names. E.g. when given LCD block's name contains 
		// either "Ore LCD", "Ingot LCD" or "Component LCD", the amounts with given type will then be updated 
		// to the LCD in question.
		List<string> lcdTypes = new List<string> { "Ore", "Ingot", "Component" };

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
		}

		public void Save()
		{
		}

		public void Main(string argument, UpdateType updateSource)
		{
			var allItems = GetBlocks<IMyTerminalBlock>()
				.Where(block => block.HasInventory)
				.SelectMany(AllInventories)
				.SelectMany(inventory => inventory.GetItems());

			foreach (string type in lcdTypes) {
				// Get the aggregate sum of items in each type and build the display string.
				string displayString = allItems
					.Where(item => item.Content.TypeId.ToString() == "MyObjectBuilder_" + type)
					.GroupBy(item => item.Content.SubtypeName)
					.OrderBy(group => group.Key)
					.Select(group => new { Name = group.Key, Value = group.Aggregate(new VRage.MyFixedPoint(), (fixedPoint, item) => fixedPoint + item.Amount) })
					.Aggregate(
							new StringBuilder(type + "s:").AppendLine(), 
							(builder, items) => builder
									.Append(items.Name)
									.Append(": ").Append(Math.Round(((double) items.Value / 1000), 2).ToString())
									.Append(" t")
									.AppendLine())
					.ToString();

				// Update LCD displays named IngotLCD, OreLCD, and ComponentLCD
				foreach (var panel in GetBlocks<IMyTextPanel>().Where(x => x.CustomName.Contains(type + " LCD")))
				{
					panel.WritePublicText(displayString);
					panel.ShowTextureOnScreen();
					panel.ShowPublicTextOnScreen();
				}
			}
		}

		IEnumerable<T> GetBlocks<T>() where T : class
		{
			List<T> blocks = new List<T>();
			GridTerminalSystem.GetBlocksOfType<T>(blocks);
			return blocks;
		}

		// Get all inventories of a block. Most have just one. Refineries and Assemblers have two.
		static IEnumerable<IMyInventory> AllInventories(IMyTerminalBlock block)
		{
			for (int i = 0; i < block.InventoryCount; i++)
			{
				yield return block.GetInventory(i);
			}
		}
	}
}