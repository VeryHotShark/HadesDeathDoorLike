using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Broccoli.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class PrefabBuilder {
        #region Vars
        public bool saveAssetsInsidePrefab = false;
        public bool lodAnimateCrossFading = true;
        public LODFadeMode lodFadeMode = LODFadeMode.None;
        /// <summary>
        /// Flag to check if a Prefab creation process has been initiated.
        /// </summary>
        bool _init = false;
        GameObject _prefabGO = null;
        GameObject _prefabAsset = null;
        string _prefabName = string.Empty;
        string _prefabPath = string.Empty;
        string _prefabSubfolder = string.Empty;
        string _prefabFullPath = string.Empty;
        string _prefabSubfolderFullPath = string.Empty;
        bool _overwrite = false;
        List<GameObject> _lodGOs = new List<GameObject> ();
        List<float> _lodPcts = new List<float> ();
        List<Material> _processedMaterials = new List<Material> ();
        #endregion

        #region Singleton
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static PrefabBuilder _instance = null;
        /// <summary>
        /// Singleton getter.
        /// </summary>
        /// <value>Singleton instance of PrefabBuilder.</value>
        public static PrefabBuilder GetClear () {
            if (_instance == null) _instance = new PrefabBuilder ();
            _instance.Clear ();
            return _instance;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// <c>True</c> if the Prefab build process has begun.
        /// </summary>
        public bool isInit {
            get { return _init; }
        }
        /// <summary>
        /// The Prefab GameObject instance.
        /// </summary>
        /// <value>The Prefab GameObject instance</value>
        public GameObject PrefabGO {
            get { return _prefabGO; }
        }
        /// <summary>
        /// Full path to the Prefab file.
        public string prefabFullPath {
        /// </summary>
            get { return _prefabFullPath; }
        }
        /// <summary>
        /// Full path to the Prefab subfolder.
        /// </summary>
        public string prefabSubfolderFullPath {
            get { return _prefabSubfolderFullPath; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the Prefab build process.
        /// </summary>
        /// <param name="prefabName">Name for the Prefab asset file.</param>
        /// <param name="prefabPath">Parent folder that contains or will contain the Prefab asset file.</param>
        /// <param name="prefabSubfolder">Folder at the same level of the Prefab asset file to be created if any external assets are going to be written as assets, ex: materials.</param>
        /// <param name="overwrite"><c>True</c> to write a new Prefab if one already exists, same with subassets created (ex. materials).</param>
        /// <returns><c>True</c> if the Prefab creation process begins and the path is valid, <c>false</c> is a process has already been initiated.</returns>
        public bool BeginBuild (string prefabName, string prefabPath, string prefabSubfolder, bool overwrite = false) {
            #if UNITY_EDITOR
            if (!_init) {
                Clear ();
                _init = Broccoli.Utils.FileUtils.IsValidFolder (prefabPath);
                if (_init) {
                    _prefabName = prefabName;
                    _prefabPath = prefabPath;
                    _prefabSubfolder = prefabSubfolder;
                    _prefabFullPath = System.IO.Path.Combine (_prefabPath, prefabName) + ".prefab";
                    _prefabSubfolderFullPath = System.IO.Path.Combine (_prefabPath, prefabSubfolder);
                    _overwrite = overwrite;
                    // Create the prefab GameObject.
			        _prefabGO = new GameObject ();
                    _prefabAsset = PrefabUtility.SaveAsPrefabAsset (_prefabGO, prefabFullPath);
                } else {
                    Debug.LogWarning ("Base path to create the Prefab is not valid: " + prefabPath);
                }
            }
            #endif
            return _init;
        }
        /// <summary>
        /// Ends the Prefab build process.
        /// </summary>
        /// <returns><c>True</c> if the Prefab creation process ends successfully.</returns>
        public bool EndBuild () {
            #if UNITY_EDITOR
            if (_init) {
                // LODs.
                if (_lodGOs.Count > 0) {
                    // Create the LOD Group.
                    LODGroup lodGroup = _prefabGO.AddComponent<LODGroup> ();
                    lodGroup.animateCrossFading = lodAnimateCrossFading;
                    lodGroup.fadeMode = lodFadeMode;
                    LOD[] lods = new LOD[_lodGOs.Count];
                    // Create levels of detail.
					float lodGroupAccum = 0f;
					for (int i = 0; i < _lodGOs.Count; i++) {
                        GameObject lodGO = _lodGOs [i];
                        lodGO.transform.parent = _prefabGO.transform;
                        lodGO.name = "LOD_" + i;
                        Renderer[] renderers = new Renderer[1];
                        renderers[0] = lodGO.GetComponent<Renderer> ();
                        lodGroupAccum += _lodPcts [i];
                        lods [i] = new LOD (1f - lodGroupAccum, renderers);
                        lods [i].fadeTransitionWidth = 0.3f;
                    }
                    // Set the LODs.
                    lodGroup.SetLODs (lods);
                }
                PrefabUtility.SaveAsPrefabAsset (_prefabGO, _prefabFullPath);
                Object.DestroyImmediate (_prefabGO);
            }
            Clear ();
            #endif
            return _init;
        }
        /// <summary>
        /// Adds a single mesh to the Prefab being created.
        /// </summary>
        /// <param name="mesh">Mesh to add.</param>
        /// <param name="mats">Materials to apply to the mesh.</param>
        /// <returns>The GO created for the mesh.</returns>
        public GameObject AddMesh (Mesh mesh, List<Material> mats) {
            if (_init) {
                InitMeshGO (_prefabGO, mesh, mats);
                return _prefabGO;
            } else {
                Debug.LogWarning ("The Prefab building process has not been initialized. Call BeginBuild to add a mesh.");
            }
            return null;
        }
        /// <summary>
        /// Adds a LOD mesh to the Prefab being created.
        /// </summary>
        /// <param name="mesh">Mesh to add to the LOD Group.</param>
        /// <param name="mats">Materials to apply to the mesh.</param>
        /// <param name="lod">Value for the detail range in the LOD Group. From 0 to 1.</param>
        /// <returns>The GO created for the mesh.</returns>
        public GameObject AddMesh (Mesh mesh, List<Material> mats, float lod) {
            if (_init) {
                GameObject go = CreateMeshGO (mesh, mats);
                _lodGOs.Add (go);
                _lodPcts.Add (lod);
                return go;
            } else {
                Debug.LogWarning ("The Prefab building process has not been initialized. Call BeginBuild to add meshes.");
            }
            return null;
        }
        /// <summary>
        /// Adds a GameObject to the Prefab being created.
        /// </summary>
        /// <param name="go">GameObject instance to add to the LOD Group.</param>
        /// <param name="lod">Value for the detail range in the LOD Group. From 0 to 1.</param>
        /// <returns><c>True</c> if the GameObject instance was added to the LOD Group.</returns>
        public bool AddGO (GameObject go, float lod) {
            bool added = _init;
            if (_init) {
                _lodGOs.Add (go);
                _lodPcts.Add (lod);
            } else {
                Debug.LogWarning ("The Prefab building process has not been initialized. Call BeginBuild to add GameObjects.");
            }
            return added;
        }
        /// <summary>
        /// Adds a Material to the Prefab being created.
        /// </summary>
        /// <param name="mat">Material to add to the Prefab.</param>
        /// <returns><c>True</c> if the material gets added to the Prefab.</returns>
        public bool AddMaterial (Material mat) {
            #if UNITY_EDITOR
            bool requiresDatabaseRefresh = false;
            if (!_processedMaterials.Contains (mat)) {
                // Check if the material has a path, thus is saved as a file asset.
                string matPath = AssetDatabase.GetAssetPath (mat);
                // If it is not found as a file, see if it's going to be saved into the prefab or to the prefabs subfolder.
                if (string.IsNullOrEmpty (matPath)) {
                    if (saveAssetsInsidePrefab) {
                        AssetDatabase.AddObjectToAsset (mat, _prefabAsset);
                    } else {
                        if (!Broccoli.Utils.FileUtils.IsValidFolder (_prefabSubfolderFullPath)) {
                            Broccoli.Utils.FileUtils.CreateSubfolder (_prefabPath, _prefabSubfolder);
                        }
                        AssetDatabase.CreateAsset (mat, _prefabSubfolderFullPath + "/" + _prefabSubfolder + GetMatSuffix () + ".mat");
                        requiresDatabaseRefresh = true;
                    }
                    _processedMaterials.Add (mat);
                }
            }
            return requiresDatabaseRefresh;
            #else
            return false;
            #endif
        }
        /// <summary>
        /// Clears all the variables of any previous Prefab build process.
        /// </summary>
        public void Clear () {
            _init = false;
            _prefabGO = null;
            _prefabName = string.Empty;
            _prefabPath = string.Empty;
            _prefabSubfolder = string.Empty;
            _prefabFullPath = string.Empty;
            _prefabSubfolderFullPath = string.Empty;
            _lodGOs.Clear ();
            _lodPcts.Clear ();
            _processedMaterials.Clear ();
        }
        #endregion

        #region Processing
        private GameObject CreateMeshGO (Mesh mesh, List<Material> mats) {
            GameObject go = new GameObject ();
            InitMeshGO (go, mesh, mats);
            return go;
        }
        private void InitMeshGO (GameObject go, Mesh mesh, List<Material> mats) {
            #if UNITY_EDITOR
            // Get or create the MeshFilter.
            MeshFilter mf = go.GetComponent<MeshFilter> ();
            if (mf == null) mf = go.AddComponent<MeshFilter> ();
            // Get the MeshRenderer.
            MeshRenderer mr = go.GetComponent<MeshRenderer> ();
            if (mr == null) mr = go.AddComponent<MeshRenderer> ();
            // Set the mesh.
            AssetDatabase.AddObjectToAsset (mesh, _prefabAsset);
            if (string.IsNullOrEmpty (mesh.name)) mesh.name = "LOD_" + _lodGOs.Count;
            mf.sharedMesh = mesh;
            // Process the materials.
            bool requiresDatabaseRefresh = ProcessMaterials (mats);
            if (requiresDatabaseRefresh) {
                AssetDatabase.Refresh ();
            }
            // Set the materials.
            mr.sharedMaterials = mats.ToArray ();
            #endif
        }
        bool ProcessMaterials (List<Material> mats) {
            #if UNITY_EDITOR
            bool requiresDatabaseRefresh = false;
            for (int i = 0; i < mats.Count; i++) {
                requiresDatabaseRefresh |= AddMaterial (mats [i]);
            }
            return requiresDatabaseRefresh;
            #else
            return false;
            #endif
        }
        string GetMatSuffix () {
            string suffix = "_mat_";
            if (_processedMaterials.Count < 10) {
                suffix += "0" + _processedMaterials.Count;
            } else {
                suffix += _processedMaterials.Count.ToString ();
            }
            return suffix;
        }
        #endregion
    }
}