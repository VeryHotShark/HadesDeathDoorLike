using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

 /*
	Give scene objects a context menu.

	Add [ContextMenu("Path")] or [SceneViewContextMenu("Path/Subpath")] above any MonoBehaviour methods.
	
	Add GetContextMenus() method to any MonoBehaviour. It can return ContextItem[], List<ContextItem>, ContextMenuBuilder, or a MonoBehaviour.
	If it returns a MonoBehaviour, it will call GetContextMenus on that instead.
	
	// https://forum.unity.com/threads/sceneview-context-menu-script-free.525029/
 */

[AttributeUsage(AttributeTargets.Method, Inherited=true, AllowMultiple=false)]
sealed class SceneViewContextMenu : Attribute
{
	public string path;
	public SceneViewContextMenu(string path) { this.path = path; }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class SceneViewContextMenuInitializer : Editor
{
	// How far away to create an object if the raycast fails to hit anything.
	const float DEFAULT_CREATION_DISTANCE = 500f;

	// Key that's got to be pressed when right clicking, to get a context menu.
	static readonly EventModifiers modifier = EventModifiers.Shift;

	static MenuSet mainMenuOptions = null;
	static string[] creationOptions;
	static Dictionary<string, List<string[]>> contextOptions;

	static SceneViewContextMenuInitializer()
	{
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

	[MenuItem("Window/SceneView Context Menu/Enable", true)] static bool EnableTest() { return !IsRegistered(); }
	[MenuItem("Window/SceneView Context Menu/Disable", true)] static bool DisableTest() { return IsRegistered(); }

	[MenuItem("Window/SceneView Context Menu/Enable")]
	static void Enable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	[MenuItem("Window/SceneView Context Menu/Disable")]
	static void Disable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	static bool IsRegistered()
	{
		var d = SceneView.onSceneGUIDelegate.GetInvocationList();
		for (int i = 0; i < d.Length; i++)
			if (d[i].Method.Name == "OnSceneGUI")
				return true;
		return false;
	}

	// Parses EditorGUIUtility.SerializeMainMenuToString so we can use custom context menus for internal objects like Transforms. Also for copying the Creation menu drop down.
	static void InitMainMenuOptions()
	{
		mainMenuOptions = new MenuSet();

		var menuString = EditorGUIUtility.SerializeMainMenuToString();
        var menus = menuString.Split('\n');
		var pathParts = new List<string>();
		var menuPaths = new List<string>();

		contextOptions = new Dictionary<string, List<string[]>>();
		
		foreach (var m in menus)
		{
			var s = m.Split(new string[] { "    " }, StringSplitOptions.None);
			var n = s[s.Length - 1];

			// Add to path parts.
			if (pathParts.Count <= s.Length)
				pathParts.Add(n);
			else
				pathParts[s.Length - 1] = n;
			
			// Get full path.
			var path = "";
			var parts = new List<string>();
			var menuSet = mainMenuOptions;
			for (int i = 0; i < s.Length; i++)
			{
				var pp = pathParts[i];
				parts.Add(pp);
				path += pp;

				if (!menuSet.children.ContainsKey(pp))
				{
					var ms = new MenuSet();
					ms.fullPath = path;
					ms.pathPart = pp;
					menuSet.children.Add(pp, ms);
				}

				menuSet = menuSet.children[pp];

				if (i != s.Length - 1)
					path += "/";
			}

			// Context menus.
			if (path.Contains("CONTEXT"))
			{
				var cParts = path.Split('/');
				if (cParts.Length >= 3)
				{
					var component = cParts[1];
					var label = cParts[2];

					if (!contextOptions.ContainsKey(component))
						contextOptions.Add(component, new List<string[]>());
					
					contextOptions[component].Add(new string[] {
						// Nice label.
						component + "/" + label,
						// Actual menu item.
						path });
				}
			}

			menuPaths.Add(path);
		}

		// Options for creating an object.
		var go = mainMenuOptions.children["GameObject"];
		var list = new List<string>();
		list.Add("GameObject/Create Empty");
		list.AddRange(go.children["3D Object"].GetSubPaths());
		list.AddRange(go.children["2D Object"].GetSubPaths());
		list.AddRange(go.children["Effects"].GetSubPaths());
		list.AddRange(go.children["Light"].GetSubPaths());
		list.AddRange(go.children["Camera"].GetSubPaths());
		creationOptions = list.ToArray();
	}

    static void OnSceneGUI(SceneView sceneview)
	{
		// If shift + right mouse click.
        if (Event.current.modifiers == modifier && Event.current.button == 1 && Event.current.type == EventType.MouseDown)
		{
			// Make sure menu items are initialized.
			if (mainMenuOptions == null)
				InitMainMenuOptions();

			// Ray from editor camera.
			var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			// Check if object selected.
			var materialIndex = 0;
			var go = HandleUtility.PickGameObject(Event.current.mousePosition, out materialIndex);

			// Show menu.
			if (go != null)
			{
				ShowMenu(go, ray);
				return;
			}
			
			// No object selected so show the default menu.
			var menu = new GenericMenu();
			AddDefaultMenus(menu, ray);
			menu.ShowAsContext();
		}
    }

	static IEnumerable<ContextItem> CallGetContextMenus(Component mb, Ray ray, ref int recursionSafety)
	{
		var method = mb
			.GetType()
			.GetMethod("GetContextMenus", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		
		if (method == null)
			return null;
		
		var menus = method.Invoke(mb, new object[] { ray });

		if (menus is ContextItem[])
			return menus as ContextItem[];
		else if (menus is List<ContextItem>)
			return (menus as List<ContextItem>).ToArray();
		else if (menus is ContextMenuBuilder)
			return (menus as ContextMenuBuilder).items.ToArray();
		// In case one object wants to use another object as it's context menu source.
		else if (menus is MonoBehaviour)
		{
			if (recursionSafety++ >= 10)
			{
				Debug.Log("Too many recursive calls.");
				return null;
			}

			return CallGetContextMenus((menus as MonoBehaviour), ray, ref recursionSafety);
		}
		else
		{
			Debug.Log("GetContextMenus in " + mb + " didn't return ContextItems.");
			return null;
		}
	}

	static IEnumerable<ContextItem> CallGetContextMenus(Component mb, Ray ray)
	{
		var recursionSafety = 0;
		var menus = CallGetContextMenus(mb, ray, ref recursionSafety);
		if (menus != null)
			return menus;
		
		return new ContextItem[0];
	}

	static IEnumerable<ContextItem> GetMenuFromComponent(Component mb, Ray ray)
	{
		if (mb == null)
			return new ContextItem[0];
		
		// Get all the methods marked with [SceneViewContextMenu].
		var list = new List<ContextItem>();
		var name = mb.GetType().Name;
		var methods = mb
			.GetType()
			.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
		
		var head = "ContextMenu (" + mb.gameObject.name + ")/";

		// Methods marked with [SceneViewContextMenu].
		foreach (var m in methods.Where(y => y.IsDefined(typeof(SceneViewContextMenu), true)))
		{
			var atr = (SceneViewContextMenu)m.GetCustomAttributes(typeof(SceneViewContextMenu), true)[0];

			// If the method wants a ray, give it to it.
			var p = m.GetParameters();
			if (p.Length == 1 && p[0].ParameterType == typeof(Ray))
				list.Add(new ContextItem(head + atr.path, () => m.Invoke(mb, new object[] { ray })));
			else
				list.Add(new ContextItem(head + atr.path, () => m.Invoke(mb, null)));
		}

		// Methods marked with [ContextMenu].
		foreach (var m in methods.Where(y => y.IsDefined(typeof(ContextMenu), true)))
		{
			var atr = (ContextMenu)m.GetCustomAttributes(typeof(ContextMenu), true)[0];
			var path = head + atr.menuItem;
			list.Add(new ContextItem(path, () => m.Invoke(mb, null)));
		}

		// Custom context menus for internal classes. i.e. [ContextMenu("CONTEXT/Transform/Reset Position")]
		if (contextOptions.ContainsKey(name))
		{
			var cop = contextOptions[name];
			foreach (var c in cop)
			{
				var niceName = c[0];
				var command = c[1];
				list.Add(new ContextItem(head + niceName, () => { EditorApplication.ExecuteMenuItem(command); }));
			}
		}
		
		return list;
	}

	// The default menu to show at the bottom of menus, or when nothing is hovered.
	static void AddDefaultMenus(GenericMenu menu, Ray ray)
	{
		// Create GameObject at contact position.
		foreach (var path in creationOptions)
			menu.AddItem(new GUIContent(path), false, () => SpawnObject(ray, path));

		// Selected GameObject tasks.
		if (Selection.gameObjects.Length > 0)
		{
			var head = "Selection (" + (Selection.gameObjects.Length == 1 ? Selection.gameObjects[0].name : Selection.gameObjects.Length + " GameObjects") + ")";
			menu.AddItem(new GUIContent(head + "/Snap To Terrain"), false, () => SnapToTerrain(Selection.gameObjects));
			menu.AddItem(new GUIContent(head + "/Snap Down"), false, () => SnapDown(Selection.gameObjects));

			menu.AddItem(new GUIContent(head + "/Reset Position"), false, () => ForEach(Selection.gameObjects, x => { if (x.transform.parent != null) x.transform.localPosition = Vector3.zero; }));
			menu.AddItem(new GUIContent(head + "/Reset Rotation"), false, () => ForEach(Selection.gameObjects, x => { if (x.transform.parent != null) x.transform.localRotation = Quaternion.identity; }));
			menu.AddItem(new GUIContent(head + "/Reset Scale"), false, () => ForEach(Selection.gameObjects, x => { if (x.transform.parent != null) x.transform.localScale = Vector3.one; }));
		}

		var components = GameObject.FindObjectsOfType<Component>()
			.Distinct()
			.ToArray();
		
		var names = GameObject.FindObjectsOfType<Transform>()
			.Select(x => x.name.Split(' ')[0])
			.Distinct()
			.ToArray();

		// Selection by components or names.
		foreach (var c in components)
		{
			var path = "Select All/Component/" + c.GetType().Name;
			menu.AddItem(new GUIContent(path), false, () => SelectByComponent(c));
		}

		foreach (var name in names)
		{
			var path = "Select All/Name/" + name;
			menu.AddItem(new GUIContent(path), false, () => SelectByName(name));
		}
		
		foreach (var c in components)
		{
			var path = "Deselect All/Component/" + c.GetType().Name;
			menu.AddItem(new GUIContent(path), false, () => DeselectByComponent(c));
		}

		foreach (var name in names)
		{
			var path = "Deselect All/Name/" + name;
			menu.AddItem(new GUIContent(path), false, () => DeselectByName(name));
		}
	}

	// Menu from context items.
	static void ShowMenu(GameObject go, Ray ray)
	{
		// Get all MonoBehaviours in object.
		var components = go.GetComponents<Component>().Where(x => x != null).ToArray();
		
		// Items returned by a GetContextMenus() method.
		var itemsFromMethod = components
			.SelectMany(x => CallGetContextMenus(x, ray));
		
		// Items returned by attributes like [ContextMenu].
		var itemsFromAttributes = components
			// Find all Context Items in MonoBehaviours.
			.SelectMany(x => GetMenuFromComponent(x, ray))
			.ToArray();
		
		var items = new List<ContextItem>();
		// Menu items from method.
		items.AddRange(itemsFromMethod);
		// Seperator.
		if (itemsFromMethod.Count() > 0 && itemsFromAttributes.Count() > 0)
			items.Add(new ContextItem(""));
		// Menu items marked with attribute.
		items.AddRange(itemsFromAttributes);

		// Create menu.
		var menu = new GenericMenu();

		if (items.Count > 0)
		{
			// Populate.
			for (int i = 0; i < items.Count; i++)
			{
				var mi = items[i];
				// Disabled option
				if (mi.disabled)
					menu.AddDisabledItem(new GUIContent(mi.label));
				// Seperator.
				else if (mi.isSeperator)
					menu.AddSeparator(mi.label);
				// Action that doesn't require parameter.
				else if (mi.callBack != null)
					menu.AddItem(new GUIContent(mi.label), mi.selected, () => mi.callBack());
				// Action that takes a parameter.
				else
					menu.AddItem(new GUIContent(mi.label), mi.selected, () => mi.callBack2(mi.callBackData));
			}

			menu.AddSeparator("");
		}

		// Default menu.
		AddDefaultMenus(menu, ray);

		// Show as context.
		menu.ShowAsContext();
	}

	#region Util.

	static void ForEach<T>(IEnumerable<T> items, Action<T> action) where T : UnityEngine.Object
	{
		Undo.RegisterCompleteObjectUndo(items.ToArray(), "For Each");

		foreach (var i in items)
			action(i);
	}

	// Selects all objects with a component.
	static void SelectByComponent(Component component)
	{
		Selection.objects = GameObject
			.FindObjectsOfType(component.GetType())
			.Select(x => (x as Component).gameObject)
			.ToArray();
	}

	// Deselects selected objects that have a component.
	static void DeselectByComponent(Component component)
	{
		var o = GameObject.FindObjectsOfType(component.GetType());

		Selection.objects = Selection.objects
			.Where(x => x is Component && !o.Contains(x))
			.Select(x => (x as Component).gameObject)
			.ToArray();
	}

	// Select objects with a given name.
	static void SelectByName(string name)
	{
		Selection.objects = GameObject.FindObjectsOfType<Transform>()
			.Where(x => x.name.StartsWith(name))
			.Select(x => x.gameObject)
			.ToArray();
	}

	// Deselect objects with a given name.
	static void DeselectByName(string name)
	{
		Selection.objects = Selection.objects
			.Where(x => x is Component && !x.name.StartsWith(name))
			.Select(x => (x as Component).gameObject)
			.ToArray();
	}

	static void SnapDown(params GameObject[] gameObjects)
	{
		Undo.RegisterCompleteObjectUndo(gameObjects, "Snap down.");

		var ray = new Ray(Vector3.zero, Vector3.down);
		RaycastHit hitInfo;

		// Disable objects so they don't collide against themselve
		for (int i = 0; i < gameObjects.Length; i++)
		{
			var go = gameObjects[i];
			ray.origin = go.transform.position;

			// Disable object so it doesn't collider against itself.
			var wasActive = gameObjects[i].activeSelf;
			gameObjects[i].SetActive(false);

			// Check if collision.
			if (Physics.Raycast(ray, out hitInfo, 1000f))
				SetPositionBoundsOffset(go, hitInfo.point, hitInfo.normal);
			
			// Set to active again.
			gameObjects[i].SetActive(wasActive);
		}
	}

	static void SnapToTerrain(params GameObject[] gameObjects)
	{
		Undo.RegisterCompleteObjectUndo(gameObjects, "Snap to terrain.");

		// Get terrain colliders.
		var t = Terrain.activeTerrains.Select(x => x.GetComponent<TerrainCollider>()).ToArray();
		RaycastHit hitInfo;
		Ray ray = new Ray(Vector3.zero, Vector3.down);

		for (int i = 0; i < gameObjects.Length; i++)
		{
			var go = gameObjects[i];
			var pos = go.transform.position;			
			var dist = float.MaxValue;
			var hitPoint = Vector3.zero;
			var hitNormal = Vector3.up;
			var hit = false;

			for (int j = 0; j < t.Length; j++)
			{
				var terrain = t[j];
				var size = terrain.terrainData.size.y + 1f;
				pos.y = size;
				ray.origin = pos;

				if (terrain.Raycast(ray, out hitInfo, size + 1f))
				{
					hit = true;

					var d = Vector3.Distance(hitInfo.point, ray.origin);
					if (d < dist)
					{
						dist = d;
						hitPoint = hitInfo.point;
						hitNormal = hitInfo.normal;
					}
				}
			}

			// If hit terrain, move to position, and offset by bounds.
			if (hit)
				SetPositionBoundsOffset(go, hitPoint, hitNormal);
		}
	}

	// Trys to offset an object by it's boundary.
	static void SetPositionBoundsOffset(GameObject gameObject, Vector3 position, Vector3 normal)
	{
		Bounds bounds;
		if (GetFullBounds(gameObject, out bounds))
			gameObject.transform.position = position + new Vector3(0f, bounds.extents.y, 0f);
		else
			gameObject.transform.position = position;
		
		gameObject.transform.forward = normal;
	}

	static void SpawnObject(Ray ray, string command)
	{
		RaycastHit hitInfo;
		var position = Vector3.zero;
		var normal = Vector3.up;

		if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
		{
			position = hitInfo.point;
			normal = hitInfo.normal;
		}
		else
		{
			position = ray.GetPoint(DEFAULT_CREATION_DISTANCE);
			normal = Vector3.up;
		}

		// Create object.
		EditorApplication.ExecuteMenuItem(command);

		// Unity should have made it the selection, so we use Selection.activeGameObject.
		SetPositionBoundsOffset(Selection.activeGameObject, position, normal);
	}

	// Try to find the bounds of the object, based on Renderers or Colliders.
	static bool GetFullBounds(GameObject go, out Bounds bounds)
	{
		bounds = new Bounds();

		// Try to get bounds from renderers.
		var rens = go.GetComponentsInChildren<Renderer>();
		if (rens.Length > 0)
		{
			for (int i = 0; i < rens.Length; i++)
				if (i == 0)
					bounds = rens[i].bounds;
				else
					bounds.Encapsulate(rens[i].bounds);
			return true;
		}

		// Try to get bounds from colliders.
		var cols = go.GetComponentsInChildren<Collider>();
		if (cols.Length > 0)
		{
			for (int i = 0; i < cols.Length; i++)
				if (i == 0)
					bounds = cols[i].bounds;
				else
					bounds.Encapsulate(cols[i].bounds);
			return true;
		}

		return false;
	}

	#endregion

	#region Internal Classes.
	public class MenuSet
	{
		public string pathPart = null;
		public string fullPath = null;
		public Dictionary<string, MenuSet> children = new Dictionary<string, MenuSet>();

		public List<string> GetSubPaths()
		{
			var l = new List<string>();

			foreach (var c in children)
				if (c.Value.children.Count == 0)
					l.Add(c.Value.fullPath);
				else
					l.AddRange(c.Value.GetSubPaths());
			
			return l;
		}
	}
	#endregion
}
#endif

public class ContextMenuBuilder
{
	public List<ContextItem> items = new List<ContextItem>();

	public ContextMenuBuilder Item(string label, Action callBack, bool disabled = false, bool selected = false)
	{
		items.Add(new ContextItem(label, callBack, disabled, selected));
		return this;
	}
	
	public ContextMenuBuilder Item(string label, Action<object> callBack, object parameter, bool disabled = false, bool selected = false)
	{
		items.Add(new ContextItem(label, callBack, parameter, disabled, selected));
		return this;
	}
	
	public ContextMenuBuilder Seperator(string path = "")
	{
		items.Add(new ContextItem(path));
		return this;
	}
}

public struct ContextItem
{
    public string label;
    public bool disabled;
    public bool selected;
    public Action callBack;
    public Action<object> callBack2;
    public object callBackData;
    public bool isSeperator;

    public ContextItem(string label, Action callBack, bool disabled = false, bool selected = false)
    {
        this.label = label;
        this.disabled = disabled;
        this.selected = selected;
        this.callBack = callBack;
        this.callBack2 = null;
        this.callBackData = null;
        this.isSeperator = false;
    }

    public ContextItem(string label, Action<object> callBack, object callBackData, bool disabled = false, bool selected = false)
    {
        this.label = label;
        this.disabled = disabled;
        this.selected = selected;
        this.callBack2 = callBack;
        this.callBackData = callBackData;
        this.callBack = null;
        this.isSeperator = false;
    }

    public ContextItem(string seperator = "")
    {
        this.label = seperator;
        this.disabled = false;
        this.selected = false;
        this.callBack2 = null;
        this.callBackData = null;
        this.callBack = null;
        this.isSeperator = true;
    }
}