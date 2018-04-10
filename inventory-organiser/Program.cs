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
		// The container name filters that are matched against when running the organisation logic.
		string oreStorageFilter = "Ore / Ingot Storage";
		string componentStorageFilter = "Component Storage";
		IteratingDeferredUpdate deferredUpdate = new IteratingDeferredUpdate();

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
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

			var allInventories = GetBlocks<IMyTerminalBlock>()
				.Where(block => block.HasInventory);

			TransferOres(allInventories);
			TransferComponents(allInventories);

			// TODO: sort items.
		}

		void TransferOres(IEnumerable<IMyTerminalBlock> allInventories)
		{
			var oreSources = allInventories
				.Where(block => !block.CustomName.Contains(oreStorageFilter))
				.Where(block => !(block is IMyReactor || block is IMyOxygenFarm || block is IMyGasGenerator || block is IMyGasTank || block is IMyOxygenFarm || block is IMyOxygenGenerator ||block is IMyOxygenTank))
				.Select(block => block.GetInventory(block.InventoryCount - 1)) // Assemblers have two inventories, get the second containing components.
				.Where(inventory => inventory.GetItems().Count > 0);

			// Get destination storages and randomise the order so we fill the containers up as evenly as possible, mitigating damage.
			Random random = new Random();
			var oreDestinations = allInventories
				.Where(block => block.CustomName.Contains(oreStorageFilter))
				.Select(block => block.GetInventory(0))
				.Where(inventory => !inventory.IsFull)
				.OrderBy(inventory => random.Next());

			MoveToAny(
				oreSources, 
				oreDestinations, 
				item => 
					(item.Content.TypeId.ToString() == "MyObjectBuilder_Ore"
							|| item.Content.TypeId.ToString() == "MyObjectBuilder_Ingot")
					&& (!item.Content.SubtypeName.Contains("Uranium")));
			SortInventoryByName(oreDestinations);
		}

		private void TransferComponents(IEnumerable<IMyTerminalBlock> allInventories)
		{
			var componentSources = allInventories
				.Where(block => !block.CustomName.Contains(componentStorageFilter))
				.Where(block => !(block is IMyReactor || block is IMyOxygenFarm || block is IMyGasGenerator || block is IMyGasTank || block is IMyOxygenFarm || block is IMyOxygenGenerator || block is IMyOxygenTank))
				.Select(block => block.GetInventory(block.InventoryCount - 1)) // Assemblers have two inventories, get the second containing components.
				.Where(inventory => inventory.GetItems().Count > 0);

			// Get destination storages and randomise the order so we fill the containers up as evenly as possible, mitigating damage.
			Random random = new Random();
			var componentDestinations = allInventories
				.Where(block => block.CustomName.Contains(componentStorageFilter))
				.Select(block => block.GetInventory(0))
				.Where(inventory => !inventory.IsFull)
				.OrderBy(inventory => random.Next());

			MoveToAny(componentSources, componentDestinations, item => item.Content.TypeId.ToString() == "MyObjectBuilder_Component");
			SortInventoryByName(componentDestinations);
		}

		IEnumerable<T> GetBlocks<T>() where T : class
		{
			List<T> blocks = new List<T>();
			GridTerminalSystem.GetBlocksOfType<T>(blocks);
			return blocks;
		}

		void MoveToAny(IEnumerable<IMyInventory> sources, IEnumerable<IMyInventory> destinations, Func<IMyInventoryItem, bool> filter)
		{
			if (sources.Count() == 0 || destinations.Count() == 0) return;

			foreach (IMyInventory sourceInventory in sources)
			{
				int index = 0;
				foreach (IMyInventoryItem sourceItem in sourceInventory.GetItems())
				{
					if (!filter(sourceItem))
					{
						++index;
						continue;
					}
					Echo(sourceItem.Content.SubtypeName);
					foreach (IMyInventory destinationInventory in destinations)
					{
						sourceInventory.TransferItemTo(destinationInventory, index, stackIfPossible: true);
					}
				}
			}
		}

		private void SortInventoryByName(IEnumerable<IMyInventory> inventories)
		{
			if (inventories == null) return;

			foreach (IMyInventory inventory in inventories) {
				inventory
					.GetItems()
					.OrderBy(item => item.Content.SubtypeName);
			};
		}
	}
}