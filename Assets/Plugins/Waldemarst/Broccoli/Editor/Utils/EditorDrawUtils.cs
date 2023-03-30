using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Broccoli.Utils
{
	public class EditorDrawUtils {
		#region Vars
		private Dictionary<Color, Texture2D> _colorToTexture = new Dictionary<Color, Texture2D> ();
		#endregion

		#region Singleton
		/// <summary>
		/// Singleton to this instance.
		/// </summary>
		private static EditorDrawUtils _editorDrawUtils;
		/// <summary>
		/// Gets this singleton.
		/// </summary>
		/// <returns>Singleton to this instance.</returns>
		public static EditorDrawUtils GetInstance () {
			if (_editorDrawUtils == null) {
				_editorDrawUtils = new EditorDrawUtils ();
			}
			return _editorDrawUtils;
		}
		#endregion

		#region Line Drawing
		#endregion

		#region Textures
		public Texture2D GetColoredTexture (Color color) {
			if (!_colorToTexture.ContainsKey (color)) {
				_colorToTexture.Add (color, TextureUtils.ColorToTex (1, color));
			}
			return _colorToTexture [color];
		}
		#endregion

		#region Handles
		/// <summary>
		/// Draws a capsule gizmo.
		/// </summary>
		/// <param name="position">Position for the center of the capsule.</param>
		/// <param name="rotation">Rotation of the capsule.</param>
		/// <param name="radius">Radius for the capsule.</param>
		/// <param name="height">Height for the capsule.</param>
		/// <param name="color">Color for the lines of the capsule.</param>
		public static void DrawWireCapsule(Vector3 position, Quaternion rotation, float radius, float height, Color color = default(Color)) {
			if (color != default(Color))
				Handles.color = color;
			Matrix4x4 angleMatrix = Matrix4x4.TRS(position, rotation, Handles.matrix.lossyScale);
			using (new Handles.DrawingScope (angleMatrix)) {
				var pointOffset = (height - (radius * 2)) / 2;
				//draw sideways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
				Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
				Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
				//draw frontways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
				Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
				Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
				//draw center
				Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
				Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
	
			}
		}
		#endregion
	}
}