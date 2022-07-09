using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	public static class ReparentingMenuIntegration
	{
		[MenuItem("Edit/Reparent Selection %m", true)]
		private static bool ValidateReparentSelection(MenuCommand menuCommand)
		{
			return ValidateTarget(menuCommand);
		}
		
		[MenuItem("Edit/Reparent Selection %m", false, 200)]
		private static void ReparentSelection(MenuCommand menuCommand)
		{
			var targets = GetTargets(menuCommand);
			ReparentingOperations.StartReparenting(targets);
		}

		private static bool ValidateTarget(MenuCommand menuCommand)
		{
			return Selection.transforms.Length > 0;
		}

		private static Transform[] GetTargets(MenuCommand menuCommand)
		{
			return Selection.transforms;
		}
	}
}
