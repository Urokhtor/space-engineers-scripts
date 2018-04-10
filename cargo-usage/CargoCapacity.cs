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
		public class CargoCapacity
		{
			VRage.MyFixedPoint currentVolume = 0;
			VRage.MyFixedPoint maxVolume = 0;

			public VRage.MyFixedPoint CurrentVolume
			{
				get
				{
					return currentVolume;
				}

				set
				{
					currentVolume = value;
				}
			}

			public VRage.MyFixedPoint MaxVolume
			{
				get
				{
					return maxVolume;
				}

				set
				{
					maxVolume = value;
				}
			}

			public double ToPercentage()
			{
				return ((double)currentVolume / (double)maxVolume) * 100;
			}

			public double ToCurrentVolume()
			{
				return Math.Round((double)currentVolume, 2);
			}

			public double ToMaxVolume()
			{
				return Math.Round((double)maxVolume, 2);
			}

			public void Clear()
			{
				currentVolume = 0;
				MaxVolume = 0;
			}
		}
	}
}
