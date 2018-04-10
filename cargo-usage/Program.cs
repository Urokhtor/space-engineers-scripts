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
using VRage;

namespace IngameScript
{
	partial class Program : MyGridProgram
	{
		// The string that given LCD's name must contain for this script to update its value.
		string lcdNameFilter = "Cargo Usage";

		// Stores for supported block types. We store them as fields to avoid instantiating new lists
		// during every update which is costly.
		List<IMyTerminalBlock> beacons = new List<IMyTerminalBlock>();
		List<IMyTextPanel> textPanels = new List<IMyTextPanel>();
		List<IMyTerminalBlock> containers = new List<IMyTerminalBlock>();
		List<IMyTerminalBlock> drills = new List<IMyTerminalBlock>();
		List<IMyTerminalBlock> connectors = new List<IMyTerminalBlock>();

		CargoCapacity cargoCapacity = new CargoCapacity();

		string DisplayName = "";

		public Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
		}

		public void Save()
		{

		}

		public void Main(string argument, UpdateType updateSource)
		{
			// First get all the beacons and panels in current grid and return doing nothing if both are empty as then we have nowhere to display our data.
			GridTerminalSystem.GetBlocksOfType<IMyBeacon>(beacons);
			GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(textPanels);
			
			if (beacons.Count == 0 && textPanels.Count == 0) return;

			GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(containers);
			CalculateInventory(cargoCapacity, containers);

			GridTerminalSystem.GetBlocksOfType<IMyShipDrill>(drills);
			CalculateInventory(cargoCapacity, drills);

			GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(connectors);
			CalculateInventory(cargoCapacity, connectors);

			string capacityStr = new StringBuilder(cargoCapacity.ToCurrentVolume().ToString())
				.Append("Kl of ")
				.AppendLine(cargoCapacity.ToMaxVolume().ToString() + "Kl")
				.AppendLine(cargoCapacity.ToPercentage().ToString("0.00") + "%")
				.ToString();

			IMyBeacon beacon = FindBeacon(beacons);
			IMyTextPanel textPanel = FindTextPanel(textPanels);

			if (beacon != null) SetBeaconName(beacon, DisplayName + "\n" + capacityStr);
			if (textPanel != null) SetPanelText(textPanel, capacityStr);

			CleanUp();
		}

		// Clean up the stores since the values are no longer needed in memory.
		void CleanUp()
		{
			beacons.Clear();
			textPanels.Clear();

			containers.Clear();
			drills.Clear();
			connectors.Clear();

			cargoCapacity.Clear();
		}

		void CalculateInventory<T>(CargoCapacity capacity, List<T> entities) where T : IMyEntity
		{
			if (capacity == null || entities == null)
			{
				return;
			}

			foreach(IMyEntity container in entities)
			//for (int i = 0; i < entities.Count; i++)
			{
				//IMyEntity container = entities[i] as IMyEntity;
				IMyInventory containerInventory = container.GetInventory(0);

				capacity.CurrentVolume += containerInventory.CurrentVolume;
				capacity.MaxVolume += containerInventory.MaxVolume;
			}
		}

		IMyBeacon FindBeacon(List<IMyTerminalBlock> beacons)
		{
			if (beacons == null || beacons.Count == 0) return null;
			return beacons[0] as IMyBeacon;
		}

		IMyTextPanel FindTextPanel(IEnumerable<IMyTextPanel> textPanels)
		{
			if (textPanels == null) return null;
			return textPanels
				.Where(panel => panel.CustomName.Contains(lcdNameFilter))
				.FirstOrDefault();
		}

		void SetBeaconName(IMyTerminalBlock block, string text)
		{
			if (block != null) block.CustomName = text;
		}

		void SetPanelText(IMyTextPanel panel, string text)
		{
			if (panel != null)
			{
				panel.WritePublicText(text);
				panel.ShowTextureOnScreen();
				panel.ShowPublicTextOnScreen();
			}
		}

	}
}