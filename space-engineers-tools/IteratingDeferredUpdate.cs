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
		public class IteratingDeferredUpdate
		{
			int iteration = 0;
			int targetIterations = 10;

			public IteratingDeferredUpdate()
			{

			}

			public IteratingDeferredUpdate(int targetIterations)
			{
				this.targetIterations = targetIterations;
			}

			public Boolean ShouldUpdate()
			{
				if (iteration++ >= targetIterations)
				{
					iteration = 0;
					return true;
				}

				return false;
			}
		}
	}
}
