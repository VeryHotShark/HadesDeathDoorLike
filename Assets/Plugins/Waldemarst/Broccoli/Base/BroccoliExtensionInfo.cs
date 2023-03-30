using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace Broccoli.Base
{
	/// <summary>
	/// Broccoli extension information holder.
	/// </summary>
	public static class BroccoliExtensionInfo
	{
		#region Vars
		/// <summary>
		/// The name of the extension.
		/// </summary>
		public static string extensionName = "Broccoli Tree Creator";
		/// <summary>
		/// The major version.
		/// </summary>
		public static string majorVersion = "1";
		/// <summary>
		/// The minor version.
		/// </summary>
		public static string minorVersion = "7";
		/// <summary>
		/// The patch version.
		public static string patchVersion = "1";
		/// <summary>
		/// Version compound string with major, minor and patch version.
		/// </summary>
		private static string _fullVersion = string.Empty;
		/// <summary>
		/// Version compound string with the extension name, major, minor and patch version.
		/// </summary>
		private static string _fullNamedVersion = string.Empty;
		/// </summary>
		/// <summary>
		/// The complete version string of this extension.
		/// </summary>
		public static string version {
			get { return GetVersion(); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Complete version string of this extension.
		/// </summary>
		/// <returns>Complete version string of this extension.</returns>
		public static string GetVersion () {
			if (string.IsNullOrEmpty (_fullVersion)) {
				_fullVersion = majorVersion + "." + minorVersion + "." + patchVersion;
			}
			return _fullVersion; 
		}
		/// <summary>
		/// Complete version string of this extension.
		/// </summary>
		/// <returns>Complete version string of this extension.</returns>
		public static string GetNamedVersion () {
			if (string.IsNullOrEmpty (_fullNamedVersion)) {
				_fullNamedVersion = extensionName + " v" + GetVersion ();
			}
			return _fullNamedVersion;
		}
		#endregion
	}
}