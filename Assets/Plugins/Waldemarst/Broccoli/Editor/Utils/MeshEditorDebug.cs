using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Broccoli.Pipe;
using Broccoli.Model;

namespace Broccoli.BroccoEditor {
    /// <summary>
    /// Class to draw a mesh with debugging options.
    /// </summary>
    public class MeshEditorDebug {
        #region Vars
        /// <summary>
        /// Mesh to debug.
        /// </summary>
        private Mesh _mesh = null;
        /// <summary>
        /// Material to use to draw the mesh.
        /// </summary>
        private Material _material = null;
        /// <summary>
        /// Mesh preview to use as canvas to draw the mesh.
        /// </summary>
        private MeshPreview _meshPreview = null;
        private float _width = 1f;
        private float _height = 1f;
        private float _widthPivot = 0.5f;
        private float _heightPivot = 0f;
        private int _widthSegments = 3;
        private int _heightSegments = 3;
        private int _planes = 1;
        private float _depth = 0f;
        private Mesh _srcMesh = null;
        private Vector3 _pivot = Vector3.zero;
        private Vector3 _scale = Vector3.one;
        private Vector3 _rotation = Vector3.zero;
        private ScriptableObject _branchCollection = null;
        #endregion

        #region Accessors
        public Mesh mesh {
            get { return _mesh; }
        }
        public Material material {
            get { return _material; }
        }
        #endregion

        #region GUI Vars
        private static string buildPlaneBtn = "Build Plane Mesh";
        private static string buildGridMesh = "Build Grid Mesh";
        private static string buildPlaneXBtn = "Build Plane X Mesh";
        private static string buildMeshBtn = "Build Mesh";
        private static string widthLabel = "Width";
        private static string heightLabel = "Height";
        private static string widthPivotLabel = "Width Pivot";
        private static string heightPivotLabel = "Height Pivot";
        private static string widthSegmentsLabel = "Width Segments";
        private static string heightSegmentsLabel = "Height Segments";
        private static string depthLabel = "Depth";
        private static string meshLabel = "Mesh";
        private static string pivotLabel = "Pivot";
        private static string scaleLabel = "Scale";
        private static string orientationLabel = "Rotation";
        private static string planeFoldoutLabel = "Plane Mesh";
        private static string gridFoldoutLabel = "Grid Mesh";
        private static string planeXFoldoutLabel = "Plane X Mesh";
        private static string meshFoldoutLabel = "Mesh Object";
        private static string branchCollectionFoldoutLabel = "BranchCollectionSO";
        private static string planesLabel = "Planes";
        private bool planeMeshFoldout = false;
        private bool gridMeshFoldout = false;
        private bool planeXMeshFoldout = false;
        private bool meshFoldout = false;
        private bool branchCollectionFoldout = false;
        #endregion

        #region Singleton
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static MeshEditorDebug _current = null;
        /// <summary>
        /// Gets the singleton instance for this class.
        /// </summary>
        /// <returns>Singleton instance.</returns>
        public static MeshEditorDebug Current () {
            if (_current == null) {
                _current = new MeshEditorDebug ();
            }
            return _current;
        }
        #endregion

        #region Draw
        public bool DrawEditorOptions (MeshPreview meshPreview) {
            _meshPreview = meshPreview;
            _material = (Material) EditorGUILayout.ObjectField (_material, typeof (Material), true);
            bool result = false;

            // Plane mesh.
            planeMeshFoldout = EditorGUILayout.Foldout (planeMeshFoldout, planeFoldoutLabel);
            if (planeMeshFoldout) {
                DrawSizeOptions ();
                DrawPivotOptions ();
                DrawPlaneOptions ();
                if (GUILayout.Button (buildPlaneBtn)) {
                    DebugAddPlaneMesh ();
                    result = true;
                }
                EditorGUILayout.Space ();
            }

            // Grid mesh.
            gridMeshFoldout = EditorGUILayout.Foldout (gridMeshFoldout, gridFoldoutLabel);
            if (gridMeshFoldout) {
                DrawSizeOptions ();
                DrawPivotOptions ();
                DrawSegmentOptions ();
                DrawPlaneOptions ();
                if (GUILayout.Button (buildGridMesh)) {
                    DebugAddGridMesh ();
                    result = true;
                }
            }

            // Plane X mesh.
            planeXMeshFoldout = EditorGUILayout.Foldout (planeXMeshFoldout, planeXFoldoutLabel);
            if (planeXMeshFoldout) {
                DrawSizeOptions ();
                DrawPivotOptions ();
                DrawDepthOptions ();
                if (GUILayout.Button (buildPlaneXBtn)) {
                    DebugAddPlaneXMesh ();
                    result = true;
                }
                EditorGUILayout.Space ();
            }

            // Source mesh.
            meshFoldout = EditorGUILayout.Foldout (meshFoldout, meshFoldoutLabel);
            if (meshFoldout) {
                DrawMeshOptions ();
                if (GUILayout.Button (buildMeshBtn)) {
                    DebugAddMesh ();
                    result = true;
                }
                EditorGUILayout.Space ();
            }

            // Branch Collection SO.
            branchCollectionFoldout = EditorGUILayout.Foldout (branchCollectionFoldout, branchCollectionFoldoutLabel);
            if (branchCollectionFoldout) {
                DrawBranchCollectionOptions ();
                if (GUILayout.Button (buildMeshBtn)) {
                    DebugAddBranchCollection ();
                    result = true;
                }
                EditorGUILayout.Space ();
            }

            _meshPreview = null;
            return result;
        }
        public void DrawSizeOptions () {
            _width = EditorGUILayout.FloatField (widthLabel, _width);
            _height = EditorGUILayout.FloatField (heightLabel, _height);
        }
        public void DrawPivotOptions () {
            _widthPivot = EditorGUILayout.FloatField (widthPivotLabel, _widthPivot);
            _heightPivot = EditorGUILayout.FloatField (heightPivotLabel, _heightPivot);
        }
        public void DrawSegmentOptions () {
            _widthSegments = EditorGUILayout.IntField (widthSegmentsLabel, _widthSegments);
            _heightSegments = EditorGUILayout.IntField (heightSegmentsLabel, _heightSegments);
        }
        public void DrawPlaneOptions () {
            _planes = EditorGUILayout.IntSlider (planesLabel, _planes, 1, 3);
        }
        public void DrawDepthOptions () {
            _depth = EditorGUILayout.FloatField (depthLabel, _depth);
        }
        public void DrawMeshOptions () {
            _srcMesh = (Mesh)EditorGUILayout.ObjectField (meshLabel, _srcMesh, typeof(Mesh), true);
            _pivot = EditorGUILayout.Vector3Field (pivotLabel, _pivot);
            _scale = EditorGUILayout.Vector3Field (scaleLabel, _scale);
            _rotation = EditorGUILayout.Vector3Field (orientationLabel, _rotation);
        }
        public void DrawBranchCollectionOptions () {
            _branchCollection = (BranchDescriptorCollectionSO) EditorGUILayout.ObjectField (
                _branchCollection,
                typeof (BranchDescriptorCollectionSO), 
                true);
        }
        #endregion

        #region Mesh Cases
        private void DebugAddPlaneMesh () {
            Broccoli.Builder.PlaneSproutMeshBuilder.SetUVData (0f, 0f, 1f, 1f, 0);
			Mesh planeMesh = 
                Broccoli.Builder.PlaneSproutMeshBuilder.GetPlaneMesh (
                    _width, _height, _widthPivot, _heightPivot, _planes
                );
			DebugAddMesh (planeMesh);
		}
		private void DebugAddGridMesh () {
            Broccoli.Builder.GridSproutMeshBuilder.SetUVData (0f, 0f, 1f, 1f, 0);
			Mesh gridMesh = 
                Broccoli.Builder.GridSproutMeshBuilder.GetGridMesh (
                    _width, _height, _widthSegments, _heightSegments, _widthPivot, _heightPivot, _planes
                );
			DebugAddMesh (gridMesh);
		}
        private void DebugAddPlaneXMesh () {
            Broccoli.Builder.PlaneXSproutMeshBuilder.SetUVData (0f, 0f, 1f, 1f, 0);
			Mesh planeMesh = 
                Broccoli.Builder.PlaneXSproutMeshBuilder.GetPlaneXMesh (
                    _width, _height, _widthPivot, _heightPivot, _depth
                );
			DebugAddMesh (planeMesh);
		}
        private void DebugAddMesh () {
			Mesh mesh = 
                Broccoli.Builder.MeshSproutMeshBuilder.GetMesh (
                    _srcMesh, _scale, _pivot, Quaternion.Euler (_rotation)
                );
			DebugAddMesh (mesh);
		}
        private void DebugAddBranchCollection () {
			Mesh mesh = 
                Broccoli.Builder.BranchCollectionSproutMeshBuilder.GetMesh (
                    ((BranchDescriptorCollectionSO)_branchCollection).branchDescriptorCollection, 
                    0, 0, _scale, _pivot, Quaternion.Euler (_rotation)
                );
			DebugAddMesh (mesh);
		}
		private void DebugAddMesh (Mesh mesh) {
			Material material;
			if (_material == null) {
				material = new Material(Shader.Find ("Hidden/Broccoli/WireframeUnlit"));
				material.SetColor ("_BaseColor", new Color (0f, 0.25f, 0.5f));
				material.SetColor ("_WireColor", new Color (0.9f, 0.9f, 0.9f));
				material.SetFloat ("_WireThickness", 600f);
			} else {
				material = _material;
			}
            _mesh = mesh;
            _material = material;
			//meshPreview.AddMesh (0, planeMesh, false);
			_meshPreview.ShowAxisHandles (true, 0.5f, Vector3.zero);
			_meshPreview.hasSecondPass = false;
			_meshPreview.SetTargetRotation (Quaternion.identity);
			_meshPreview.SetOffset (new Vector3 (0f, 0.15f, -5.5f));
            _meshPreview.SetDirection (new Vector2 (135f, -15f));
		}
        #endregion
    }
}