using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	public static class ReparentingOperations
	{
		public static void StartReparenting(Transform[] targets)
		{
			Ensure.That(nameof(targets)).IsNotNull(targets);
			Ensure.That(nameof(targets)).HasItems(targets);

			if (!TransformOperations.WarnRestructurable(targets))
			{
				return;
			}

			ReparentingPrompt.Prompt
			(
				TransformOperations.FindCommonScene(targets),
				(parentScene, parentTransform) =>
				{
					var reparented = false;

					if (parentTransform != null)
					{
						ReparentingUtility.Reparent(targets, parentTransform);
						reparented = true;
					}
					else if (parentScene != null)
					{
						ReparentingUtility.Reparent(targets, parentScene.Value);
						reparented = true;
					}

					if (reparented)
					{
						Selection.objects = targets.Select(t => t.gameObject).ToArray();

						EditorGUIUtility.PingObject(targets.OrderBy(t => t.GetSiblingIndex()).First());
					}
				}
			);
		}
	}
}