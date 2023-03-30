using UnityEngine;
using UnityEditor;

using Broccoli.Base;
using Broccoli.Factory;
using Broccoli.Pipe;
using Broccoli.Utils;

namespace Broccoli.TreeNodeEditor
{
    /// <summary>
	/// Base class for node editors.
	/// </summary>
	public abstract class PipelineElementEditor : Editor {
		#region Vars
		/// <summary>
		/// The base pipeline element of this editor.
		/// </summary>
		protected PipelineElement pipelineElement = null;
		/// <summary>
		/// The serialized property.
		/// </summary>
		protected SerializedProperty serializedProperty;
		/// <summary>
		/// The seconds to wait to update the pipeline.
		/// </summary>
		private float secondsToUpdatePipeline = 0;
		/// <summary>
		/// The editor delta time.
		/// </summary>
		double editorDeltaTime = 0f;
		/// <summary>
		/// The last time since startup.
		/// </summary>
		double lastTimeSinceStartup = 0f;
		/// <summary>
		/// True if the waiting is for the UpdatePipeline method.
		/// </summary>
		bool waitingToUpdatePipeline = false;
		/// <summary>
		/// True if the waiting is for the UpdatePipelineUpstream method.
		/// </summary>
		bool waitingToUpdatePipelineFromUpstream = false;
		/// <summary>
		/// True if the waiting is for the UpdateComponent method.
		/// </summary>
		bool waitingToUpdateComponent = false;
		/// <summary>
		/// The waiting command passed to the update pipeline method.
		/// </summary>
		int waitingCmd = 0;
		/// <summary>
		/// The waiting class type passed to the update pipeline method.
		/// </summary>
		PipelineElement.ClassType waitingClassType;
		/// <summary>
		/// Flag to rebuild the preview tree from anew.
		/// </summary>
		public bool rebuildTreePreview = false;
		/// <summary>
		/// The show field help flag.
		/// </summary>
		public bool showFieldHelp = false;
		/// <summary>
		/// The current undo group to check for undoable actions.
		/// </summary>
		public int currentUndoGroup = 0;
		/// <summary>
		/// Flag to show the node description.
		/// </summary>
		protected bool showNodeDescription = false;
		/// <summary>
		/// Color to use on the display header.
		/// </summary>
		protected Color headerColor = Color.gray;
		protected static GUIContent isSeedFixedLabel = new GUIContent ("Use Fixed Seed", 
			"Using a fixed seed guarantees this element will produce the same output across tree processing iterations.");
		protected static GUIContent fixedSeedLabel = new GUIContent ("Seed", 
			"Fixed seed to use on the randomized processing for this element.");
		protected static GUIContent presetLabel = new GUIContent ("Preset Mode", 
			"Select a Preset so that this editor display values and ranges for sliders more tuned with the scale of " +
			"the vegetation you are working with (like a large tree vs a small plant).");
		protected static GUIContent showFieldHelpLabel = new GUIContent ("Show Fields Description", 
			"Displays a description for each field on this editor.");
		protected static GUIContent useKeyNameLabel = new GUIContent ("Use Key Name", 
			"Uses a key name (label) to mark his element with a name on the pipeline node editor.");
		protected static GUIContent keyNameLabel = new GUIContent ("Key Name", 
			"Key name for this element.");
		#endregion

		#region Events
		/// <summary>
		/// Raises the enable event.
		/// </summary>
		void OnEnable () {
			pipelineElement = target as PipelineElement;
			OnEnableSpecific ();
			EditorApplication.update += OnEditorUpdate;
			#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui += OnSceneGUI;
			#else
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			#endif
			headerColor = BroccoEditorGUI.GetElementColor (pipelineElement);
			Undo.undoRedoPerformed -= OnUndoInternal;
			Undo.undoRedoPerformed += OnUndoInternal;
		}
		/// <summary>
		/// Raises the disable event.
		/// </summary>
		void OnDisable () {
			EditorApplication.update -= OnEditorUpdate;
			#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= OnSceneGUI;
			#else
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			#endif
			OnDisableSpecific ();
			Undo.undoRedoPerformed -= OnUndoInternal;
		}
		/// <summary>
		/// Undo or redo event detected by this editor.
		/// </summary>
		void OnUndoInternal () {
			OnUndo ();
		}
		/// <summary>
		/// Inspector GUI drawing event.
		/// </summary>
		public override void OnInspectorGUI () {
			showNodeDescription = BroccoEditorGUI.DrawHeader (
                pipelineElement.elementName, 
                pipelineElement.elementDescription, 
                pipelineElement.elementHelpURL, 
                headerColor, 
				showNodeDescription);
			OnInspectorGUISpecific ();
		}
		/// <summary>
		/// Raises the scene GUI event.
		/// </summary>
		/// <param name="sceneView">Scene view.</param>
		protected virtual void OnSceneGUI (SceneView sceneView) {}
		/// <summary>
		/// Raises the editor update event.
		/// </summary>
		void OnEditorUpdate () {
			if (secondsToUpdatePipeline > 0) {
				SetEditorDeltaTime ();
				secondsToUpdatePipeline -= (float) editorDeltaTime;
				if (secondsToUpdatePipeline < 0) {
					if (waitingToUpdatePipeline) {
						UpdatePipeline ();
					} else if (waitingToUpdatePipelineFromUpstream) {
						UpdatePipelineUpstream (waitingClassType);
					} else if (waitingToUpdateComponent) {
						UpdateComponent (waitingCmd);
					}
					secondsToUpdatePipeline = 0;
					waitingToUpdatePipeline = false;
					waitingToUpdatePipelineFromUpstream = false;
					waitingToUpdateComponent = false;
				}
			}
		}
		/// <summary>
		/// Sets the editor delta time.
		/// </summary>
		private void SetEditorDeltaTime ()
		{
			if (lastTimeSinceStartup == 0f)
			{
				lastTimeSinceStartup = EditorApplication.timeSinceStartup;
			}
			editorDeltaTime = EditorApplication.timeSinceStartup - lastTimeSinceStartup;
			lastTimeSinceStartup = EditorApplication.timeSinceStartup;
		}
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected abstract void OnEnableSpecific ();
		/// <summary>
		/// Raises the disable specific event.
		/// </summary>
		protected virtual void OnDisableSpecific () {}
		/// <summary>
		/// Raises the specific inspector GUI drawing.
		/// </summary>
		protected virtual void OnInspectorGUISpecific () {}
		#endregion

		#region Pipeline Update
		/// <summary>
		/// Updates the pipeline.
		/// </summary>
		/// <param name="secondsToWait">Seconds to wait before calling the pipeline to update.</param>
		/// <param name="rebuild">If set to <c>true</c> the whole preview tree gets rebuild from anew.</param>
		protected void UpdatePipeline (float secondsToWait = 0, bool rebuild = false) {
			if (rebuild) {
				this.rebuildTreePreview = true;
			}
			if (secondsToWait > 0) {
				this.secondsToUpdatePipeline = secondsToWait;
				waitingToUpdatePipeline = true;
				SetEditorDeltaTime ();
			} else {
				if (pipelineElement != null && pipelineElement.isOnValidPipeline) {
					if (rebuildTreePreview) {
						TreeFactory.GetActiveInstance ().ProcessPipelinePreview (null, true, true);
					} else {
						TreeFactory.GetActiveInstance ().ProcessPipelinePreviewDownstream (pipelineElement, true);
					}
				}
				this.secondsToUpdatePipeline = 0;
				this.rebuildTreePreview = false;
			}
		}
		/// <summary>
		/// Updates the pipeline from an upstream element if it is found.
		/// </summary>
		/// <param name="classType">Class type for the upstream pipeline element.</param>
		/// <param name="secondsToWait">Seconds to wait.</param>
		protected void UpdatePipelineUpstream (PipelineElement.ClassType classType, float secondsToWait = 0) {
			if (secondsToWait > 0) {
				this.secondsToUpdatePipeline = secondsToWait;
				waitingToUpdatePipelineFromUpstream = true;
				waitingClassType = classType;
				SetEditorDeltaTime ();
			} else {
				if (pipelineElement != null && pipelineElement.isOnValidPipeline) {
					TreeFactory.GetActiveInstance ().ProcessPipelinePreviewFromUpstream (pipelineElement, classType, true);
				}
				this.secondsToUpdatePipeline = 0;
			}
		}
		/// <summary>
		/// Updates the pipeline with only one component.
		/// </summary>
		/// <param name="cmd">Command passed to the component.</param>
		/// <param name="secondsToWait">Seconds to wait.</param>
		protected void UpdateComponent (int cmd, float secondsToWait = 0) {
			if (secondsToWait > 0) {
				this.secondsToUpdatePipeline = secondsToWait;
				waitingToUpdateComponent = true;
				waitingCmd = cmd;
				SetEditorDeltaTime ();
			} else {
				if (pipelineElement != null && pipelineElement.isOnValidPipeline) {
					TreeFactory.GetActiveInstance ().ProcessPipelineComponent (pipelineElement, cmd);
				}
				this.secondsToUpdatePipeline = 0;
			}
		}
		#endregion

		#region GUI and Serialization
		/// <summary>
		/// Gets the serialized property for an editable var on the pipeline element.
		/// </summary>
		/// <returns>The serialized property.</returns>
		/// <param name="propertyName">Property name on the pipeline.</param>
		protected SerializedProperty GetSerializedProperty (string propertyName) {
			return serializedObject.FindProperty (propertyName);
		}
		/// <summary>
		/// Updates the serialized object.
		/// </summary>
		protected void UpdateSerialized() {
			if (serializedObject != null) {
				serializedObject.Update ();
			}
		}
		/// <summary>
		/// Applies any pending changes to the serialized object.
		/// </summary>
		protected void ApplySerialized () {
			serializedObject.ApplyModifiedProperties ();
			pipelineElement.RaiseChangeEvent ();
		}
		/// <summary>
		/// Checks if the pipeline has changed due to an undo action.
		/// </summary>
		protected void CheckUndoRequest () {
			if (EventType.ValidateCommand == Event.current.type &&
				"UndoRedoPerformed" == Event.current.commandName) {
					Debug.Log ("DETECTED UNDO REQUEST!!");
				if (TreeFactory.GetActiveInstance ().lastUndoProcessed !=
					pipelineElement.pipeline.undoControl.undoCount) {
					OnUndo ();
					TreeFactory.GetActiveInstance ().RequestPipelineUpdate ();
				}
			}
			currentUndoGroup = Undo.GetCurrentGroup ();
		}
		protected virtual void OnUndo () {}
		/// <summary>
		/// Sets the undo control counter.
		/// This counter is collapsed with the latest change to the undo stack,
		/// then when an undo event is called the factory could check for this
		/// counter value to update the tree.
		/// </summary>
		protected void SetUndoControlCounter (bool collapseUndos = true) {
			if (pipelineElement != null && 
				pipelineElement.pipeline != null && pipelineElement.isOnValidPipeline) {
				Undo.RecordObject (pipelineElement.pipeline, "undoControl");
				pipelineElement.pipeline.undoControl.undoCount++;
				if (collapseUndos) {
					Undo.CollapseUndoOperations (currentUndoGroup);
				}
				pipelineElement.Validate ();
			}
		}
		#endregion

		#region Draw
		/// <summary>
		/// Draws a horizontal line with top and bottom margins to separate sections of the GUI.
		/// </summary>
		protected virtual void DrawSeparator () {
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		}
		/// <summary>
		/// Draws the log box.
		/// </summary>
		protected virtual void DrawLogBox () {
			if (pipelineElement.log.Count > 0) {
				var enumerator = pipelineElement.log.GetEnumerator ();
				while (enumerator.MoveNext ()) {
					MessageType messageType = UnityEditor.MessageType.Info;
					switch (enumerator.Current.messageType) {
					case LogItem.MessageType.Error:
						messageType = UnityEditor.MessageType.Error;
						break;
					case LogItem.MessageType.Warning:
						messageType = UnityEditor.MessageType.Warning;
						break;
					}
					EditorGUILayout.HelpBox (enumerator.Current.message, messageType);
				}
			}
		}
		protected virtual void DrawPresetOptions () {
			pipelineElement.preset = (PipelineElement.Preset) EditorGUILayout.EnumPopup (presetLabel, pipelineElement.preset);
		}
		/// <summary>
		/// Draws the seed options.
		/// </summary>
		protected virtual void DrawSeedOptions () {
			bool isSeedFixed = EditorGUILayout.Toggle (isSeedFixedLabel, pipelineElement.isSeedFixed);
			if (isSeedFixed != pipelineElement.isSeedFixed) {
				Undo.RecordObject (pipelineElement, "Using fixed seed on " + pipelineElement.name + " element changed.");
				pipelineElement.isSeedFixed = isSeedFixed;
				NodeEditorFramework.NodeEditor.RepaintClients ();
				SetUndoControlCounter ();
			}
			if (pipelineElement.isSeedFixed) {
				int newFixedSeed = EditorGUILayout.IntSlider (fixedSeedLabel, pipelineElement.fixedSeed, 0, 10000);
				if (newFixedSeed != pipelineElement.fixedSeed) {
					Undo.RecordObject (pipelineElement, "Fixed seed on " + pipelineElement.name + " element changed.");
					pipelineElement.fixedSeed = newFixedSeed;
					SetUndoControlCounter ();
				}
			}
		}
		/// <summary>
		/// Draws the key name options.
		/// </summary>
		protected virtual void DrawKeyNameOptions () {
			bool useKeyName = EditorGUILayout.Toggle (useKeyNameLabel, pipelineElement.useKeyName);
			if (useKeyName != pipelineElement.useKeyName) {
				Undo.RecordObject (pipelineElement, "Using key name.");
				pipelineElement.useKeyName = useKeyName;
				NodeEditorFramework.NodeEditor.RepaintClients ();
				SetUndoControlCounter ();
			}
			if (pipelineElement.useKeyName) {
				string newKeyName = pipelineElement.keyName;
				newKeyName = EditorGUILayout.TextField (keyNameLabel, newKeyName);
				if (string.Compare (newKeyName, pipelineElement.keyName) != 0) {
					Undo.RecordObject (pipelineElement, "New pipeline element key name: " + pipelineElement.keyName + ".");
					pipelineElement.keyName = newKeyName;
					SetUndoControlCounter ();
				}
			}
		}
		/// <summary>
		/// Draws the field help options.
		/// </summary>
		protected virtual void DrawFieldHelpOptions () {
			bool _showFieldHelp = EditorGUILayout.Toggle (showFieldHelpLabel, showFieldHelp);
			if (_showFieldHelp != showFieldHelp) {
				showFieldHelp = _showFieldHelp;
				OnShowFieldHelpChanged ();
			}
			#if BROCCOLI_DEVEL
			if (GlobalSettings.debugEnabled) {
				DrawDebugInfo ();
			}
			#endif
		}
		/// <summary>
		/// Shows the help box.
		/// </summary>
		/// <param name="msg">Message.</param>
		protected void ShowHelpBox (string msg) {
			if (showFieldHelp)
				EditorGUILayout.HelpBox (msg, MessageType.None);
		}
		/// <summary>
		/// Called when the ShowFieldHelp flag changed.
		/// </summary>
		protected virtual void OnShowFieldHelpChanged () {}
		#endregion

		#region Property Fields
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		protected bool FloatRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, float minRangeValue, float maxRangeValue, string label, int decimals = 2) {
			return FloatRangePropertyField (propMinValue, propMaxValue, minRangeValue, maxRangeValue, label, null, decimals);
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		protected bool FloatRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, float minRangeValue, float maxRangeValue, GUIContent label, int decimals = 2) {
			return FloatRangePropertyField (propMinValue, propMaxValue, minRangeValue, maxRangeValue, string.Empty, label, decimals);
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="contentLabel">Content label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		private bool FloatRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, float minRangeValue, float maxRangeValue, string label, GUIContent contentLabel, int decimals = 2) {
			float minValue = propMinValue.floatValue;
			float maxValue = propMaxValue.floatValue;
			EditorGUILayout.BeginHorizontal ();
			if (contentLabel == null) {
				EditorGUILayout.MinMaxSlider (label, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (contentLabel, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (minValue.ToString("F" + decimals) + "/" + maxValue.ToString("F" + decimals), GUILayout.Width (72));
			EditorGUILayout.EndHorizontal ();
			if (minValue != propMinValue.floatValue || maxValue != propMaxValue.floatValue) {
				propMinValue.floatValue = minValue;
				propMaxValue.floatValue = maxValue;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		protected bool IntRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, int minRangeValue, int maxRangeValue, string label) {
			return IntRangePropertyField (propMinValue, propMaxValue, minRangeValue, maxRangeValue, label, null);
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		protected bool IntRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, int minRangeValue, int maxRangeValue, GUIContent label) {
			return IntRangePropertyField (propMinValue, propMaxValue, minRangeValue, maxRangeValue, string.Empty, label);
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="contentLabel">Content label to display on the field.</param>
		/// <returns>True if the range was changed.</returns>
		private bool IntRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, int minRangeValue, int maxRangeValue, string label, GUIContent contentLabel) {
			float minValue = propMinValue.intValue;
			float maxValue = propMaxValue.intValue;
			EditorGUILayout.BeginHorizontal ();
			if (contentLabel == null) {
				EditorGUILayout.MinMaxSlider (label, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (contentLabel, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (minValue.ToString("F0") + "-" + maxValue.ToString("F0"), GUILayout.Width (60));
			EditorGUILayout.EndHorizontal ();
			if (Mathf.RoundToInt (minValue) != propMinValue.intValue || Mathf.RoundToInt (maxValue) != propMaxValue.intValue) {
				propMinValue.intValue = Mathf.RoundToInt (minValue);
				propMaxValue.intValue = Mathf.RoundToInt (maxValue);
				return true;
			}
			return false;
		}
		#endregion

		#region Debug
		#if BROCCOLI_DEVEL
		protected void DrawDebugInfo () {
			string debugInfo = string.Format ("PipelineElement [{0}]\n", pipelineElement.elementName);
			if (pipelineElement.pipeline == null) {
				debugInfo += string.Format ("Not in a pipeline...");
			} else {
				debugInfo += string.Format ("In {0} pipeline with {1} elements.\n", 
					(pipelineElement.pipeline.IsValid ()?"valid":"not valid"), pipelineElement.pipeline.GetElementsCount ());
			}
			debugInfo += string.Format ("Src Id: {0} [{1}]\n", pipelineElement.srcElementId, pipelineElement.srcElement==null?"null":pipelineElement.srcElement.elementName);
			debugInfo += string.Format ("Sink Id: {0} [{1}]\n", pipelineElement.sinkElementId, pipelineElement.sinkElement==null?"null":pipelineElement.sinkElement.elementName);
			debugInfo += string.Format ("Graph Pos: {0}", pipelineElement.nodePosition);
			EditorGUILayout.HelpBox (debugInfo, MessageType.None);
		}
		#endif
		#endregion
	}
}