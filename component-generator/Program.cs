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
		IteratingDeferredUpdate deferredUpdate = new IteratingDeferredUpdate();
		InventoryComponent targetInventory = new InventoryComponent();

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;

			targetInventory.SetValue(ComponentType.SteelPlate, 3000);
		}

		public void Save()
		{
		}

		public void Main(string argument, UpdateType updateSource)
		{
			if (!deferredUpdate.ShouldUpdate())
			{
				return;
			}

			InventoryComponent actualInventory = BuildActualInventory();
			IEnumerable<IMyAssembler> assemblers = GetBlocks<IMyAssembler>();
			QueueAssemblyLine(targetInventory, actualInventory, assemblers);
		}

		private InventoryComponent BuildActualInventory()
		{
			var allItems = GetBlocks<IMyTerminalBlock>()
				.Where(block => block.HasInventory)
				.SelectMany(AllInventories)
				.SelectMany(inventory => inventory.GetItems())
				.GroupBy(item => item.Content.SubtypeName.ToString())
				.Select(group => new { Name = group.Key, Value = group.Aggregate(new VRage.MyFixedPoint(), (fixedPoint, item) => fixedPoint + item.Amount) }); // TODO: aggregate items of same type together?

			InventoryComponent actualInventory = new InventoryComponent();
			foreach (var item in allItems)
			{
				// TODO: how to parse by name automatically?
				if (item.Name.Contains("SteelPlate"))
				{
					actualInventory.SetValue(ComponentType.SteelPlate, item.Value.ToIntSafe());
				}
			}

			return actualInventory;
		}

		private void QueueAssemblyLine(InventoryComponent targetInventory, InventoryComponent actualInventory, IEnumerable<IMyAssembler> assemblers)
		{
			if (assemblers == null || assemblers.Count() == 0)
			{
				return;
			}

			IMyAssembler assembler = FindMasterAssembler(assemblers);

			if (assembler == null)
			{
				assembler = FindRandomAssembler(assemblers);
			}

			IEnumerable<ComponentType> componentTypes = actualInventory.GetKeys();

			foreach(ComponentType componentType in componentTypes)
			{
				var difference = targetInventory.GetValue(componentType) - actualInventory.GetValue(componentType);

				if (difference > 0)
				{
					assembler.AddQueueItem(MyDefinitionId.Parse("MyObjectBuilder_BlueprintDefinition/" + componentType.ToString()), difference);
				}
			}
		}

		private IMyAssembler FindMasterAssembler(IEnumerable<IMyAssembler> assembers)
		{
			return assembers
				.Where(assembler => !assembler.CooperativeMode)
				.First();
		}

		private IMyAssembler FindRandomAssembler(IEnumerable<IMyAssembler> assemblers)
		{
			Random random = new Random();
			return assemblers
				.Where(assembler => !assembler.OutputInventory.IsFull)
				.OrderBy(ass => random.Next())
				.First();
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