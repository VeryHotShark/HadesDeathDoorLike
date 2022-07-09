using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	// TODO: Dry with GroupNamePrompt?
	public sealed class ReparentingPrompt : LudiqEditorWindow
	{
		public static ReparentingPrompt instance { get; private set; }

		public static void Prompt(Scene? defaultParentScene, Action<Scene?, Transform> onConfirm)
		{
			if (instance == null)
			{
				var instance = CreateInstance<ReparentingPrompt>();
				instance.minSize = instance.maxSize = new Vector2(300, 88);
			}

			instance.parentScene = defaultParentScene;
			instance.onConfirm = onConfirm;
			instance.ShowUtility();
			instance.Center();
		}

		public Scene? parentScene { get; private set; }

		public Transform parentTransform { get; private set; }

		private bool focused;

		private Action<Scene?, Transform> onConfirm;

		protected override void OnEnable()
		{
			base.OnEnable();
			titleContent.text = "Reparent";
			instance = this;
			this.Center();
		}

		protected override void OnDisable()
		{
			instance = null;
			base.OnDisable();
		}

		protected override void OnGUI()
		{
			base.OnGUI();

			if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return)
			{
				Confirm();
			}
			else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return)
			{
				Cancel();
			}

			GUILayout.BeginVertical(Styles.fieldsArea);

			// Scene

			EditorGUI.BeginDisabledGroup(parentTransform != null);

			if (parentTransform != null)
			{
				parentScene = parentTransform.gameObject.scene;
			}

			GUI.SetNextControlName("ReparentSceneField");

			var sceneOptions = ListPool<string>.New();
			var selectedSceneIndex = -1;

			for (var i = 0; i < EditorSceneManager.sceneCount; i++)
			{
				var scene = EditorSceneManager.GetSceneAt(i);

				if (scene == parentScene)
				{
					selectedSceneIndex = i;
				}

				sceneOptions.Add(scene.name);
			}

			EditorGUI.BeginChangeCheck();

			var newSelectedSceneIndex = EditorGUILayout.Popup("Parent Scene", selectedSceneIndex, sceneOptions.ToArray());

			if (EditorGUI.EndChangeCheck())
			{
				parentScene = EditorSceneManager.GetSceneAt(newSelectedSceneIndex);
			}

			sceneOptions.Free();

			EditorGUI.EndDisabledGroup();

			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

			// Transform

			GUI.SetNextControlName("ReparentObjectField");
			parentTransform = EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true) as Transform;

			if (parentTransform != null && !parentTransform.IsSceneBound())
			{
				parentTransform = null;
			}

			GUILayout.EndVertical();

			if (!focused)
			{
				EditorGUI.FocusTextInControl("ReparentObjectField");
				focused = true;
			}

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal(Styles.buttonsArea);

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Cancel"))
			{
				Cancel();
			}

			GUILayout.Space(2);

			EditorGUI.BeginDisabledGroup(parentTransform == null && parentScene == null);

			if (GUILayout.Button("OK"))
			{
				Confirm();
			}

			EditorGUI.EndDisabledGroup();

			GUILayout.EndHorizontal();
		}

		private void Confirm()
		{
			Close();
			onConfirm?.Invoke(parentScene, parentTransform);
			GUIUtility.ExitGUI();
		}

		private void Cancel()
		{
			Close();
			GUIUtility.ExitGUI();
		}

		private static class Styles
		{
			static Styles()
			{
				fieldsArea = ColorPalette.unityBackgroundMid.CreateBackground();
				fieldsArea.padding = new RectOffset(8, 8, 8, 8);

				buttonsArea = ColorPalette.unityBackgroundDark.CreateBackground();
				buttonsArea.padding = new RectOffset(8, 8, 8, 8);
			}

			public static readonly GUIStyle fieldsArea;

			public static readonly GUIStyle buttonsArea;
		}
	}
}