using UnityEditor;

namespace ExternalPropertyAttributes.Editor
{
	internal class SavedBool
	{
		private bool _value;
		private string _name;

		public bool Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value == value)
				{
					return;
				}

				_value = value;
				EditorPrefs.SetBool("Ext_" + _name, value);
			}
		}

		public SavedBool(string name, bool value)
		{
			_name = name;
			_value = EditorPrefs.GetBool("Ext_" + name, value);
		}
	}
}