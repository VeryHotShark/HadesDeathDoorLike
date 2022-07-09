using System;
using System.Collections.Generic;
using Ludiq.Peek;
using Ludiq.PeekCore;

[assembly: MapToPlugin(typeof(Changelog_1_4_2), PeekPlugin.ID)]

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	internal class Changelog_1_4_2 : PluginChangelog
	{
		public Changelog_1_4_2(Plugin plugin) : base(plugin) { }

		public override SemanticVersion version => "1.4.2";

		public override DateTime date => new DateTime(2021, 11, 29);

		public override IEnumerable<string> changes
		{
			get
			{
				yield return "[Removed] Odin pre-build automation for faster builds";
				yield return "[Fixed] Error caused by Odin pre-build automation in Unity 2021";
				yield return "[Fixed] CS108 warning in KeyedCollection";
			}
		}
	}
}