using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

using Broccoli.Factory;
using Broccoli.Utils;

namespace Broccoli.Examples 
{
    /// <summary>
    /// Descriptor utility class for Broccoli Tree Creator example scenes.
    /// </summary>
    [ExecuteInEditMode]
    public class SceneDescriptor : MonoBehaviour {
        #region Vars
        public string title = string.Empty;
        public string description = string.Empty;
        public bool autoselectOnSceneLoad = true;
        public bool develShowFields = true;
        public enum RenderPipelineType {
			Regular,
			HDRP,
			URP,
            LWRP
		}
        public RenderPipelineType scenePipelineType = RenderPipelineType.Regular;
        public RenderPipelineType detectedPipelineType = RenderPipelineType.Regular;
        bool _requiresRebuild = true;
        public bool requiresRebuild {
            get { return _requiresRebuild; }
        }
        #endregion

        #region Strings
        static string PIPELINE_REGULAR = "Built-in Render Pipeline";
        static string PIPELINE_URP = "Universal Render Pipeline (URP)";
        static string PIPELINE_HDRP = "High Definition Render Pipeline (HDRP)";
        static string PIPELINE_LWRP = "Lightweight Render Pipeline (LWRP)";
        #endregion

        #region Mono Events
        void OnEnable () {
            DetectRenderPipeline ();
            CheckRenderPipelineMismatch ();
        }
        void OnDisable () {

        }
        void Start () {
            if (autoselectOnSceneLoad) {
                #if UNITY_EDITOR
                UnityEditor.Selection.activeGameObject = this.gameObject;
                UnityEditor.Selection.activeObject = this.gameObject;
                UnityEditor.Selection.SetActiveObjectWithContext (this.gameObject, null);
                #endif
            }
        }
        #endregion

        #region Graphics
        public void CheckRenderPipelineMismatch () {
            if (scenePipelineType != detectedPipelineType) {
                _requiresRebuild = true;
            }
        }
        public void DetectRenderPipeline () {
			// LightweightPipelineAsset
			// HDRenderPipelineAsset
			// UniversalRenderPipelineAsset
			var currentRenderPipeline = GraphicsSettings.renderPipelineAsset;
			detectedPipelineType = RenderPipelineType.Regular;
			if (currentRenderPipeline != null) {
				if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains ("UniversalRenderPipelineAsset")) {
					detectedPipelineType = RenderPipelineType.URP;
				} else if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains ("LightweightPipelineAsset")) {
					detectedPipelineType = RenderPipelineType.LWRP;
				} else if (GraphicsSettings.renderPipelineAsset.GetType().Name.Contains ("HDRenderPipelineAsset")) {
					detectedPipelineType = RenderPipelineType.HDRP;
				}
			}
		}
        public string PipelineTypeToString (RenderPipelineType pipelineType) {
            if (pipelineType == RenderPipelineType.Regular) {
                return PIPELINE_REGULAR;
            } else if (pipelineType == RenderPipelineType.URP) {
                return PIPELINE_URP;
            } else if (pipelineType == RenderPipelineType.HDRP) {
                return PIPELINE_HDRP;
            } else {
                return PIPELINE_LWRP;
            }
        }
        public void RebuildTreeFactories () {
            TreeFactory[] treeFactories = GameObject.FindObjectsOfType<TreeFactory> ();
            for (int i = 0; i < treeFactories.Length; i++) {
                treeFactories [i].ProcessPipelinePreview (null, true);
            }
            scenePipelineType = detectedPipelineType;
            _requiresRebuild = false;
            Debug.Log ("Broccoli Tree Factories on the Scene have been rebuilt.");
        }
        #endregion
    }
}