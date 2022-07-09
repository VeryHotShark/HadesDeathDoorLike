using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	public static class ReparentingUtility
	{
		public static void Reparent(Transform[] targets, Transform parent)
		{
			Ensure.That(nameof(parent)).IsNotNull(parent);

			foreach (var target in targets.OrderBy(t => t.GetSiblingIndex()))
			{
				Undo.SetTransformParent(target.transform, null, "Reparent");
				Undo.MoveGameObjectToScene(target.gameObject, parent.gameObject.scene, "Reparent");
				Undo.SetTransformParent(target.transform, parent.transform, "Reparent");
			}
		}

		public static void Reparent(Transform[] targets, Scene parentScene)
		{
			foreach (var target in targets.OrderBy(t => t.GetSiblingIndex()))
			{
				Undo.SetTransformParent(target.transform, null, "Reparent");
				Undo.MoveGameObjectToScene(target.gameObject, parentScene, "Reparent");
			}
		}
	}
}