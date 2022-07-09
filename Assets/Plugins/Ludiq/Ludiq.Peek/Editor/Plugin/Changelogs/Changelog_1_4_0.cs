using System;
using System.Collections.Generic;
using Ludiq.Peek;
using Ludiq.PeekCore;

[assembly: MapToPlugin(typeof(Changelog_1_4_0), PeekPlugin.ID)]

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	internal class Changelog_1_4_0 : PluginChangelog
	{
		public Changelog_1_4_0(Plugin plugin) : base(plugin) { }

		public override SemanticVersion version => "1.4.0";

		public override DateTime date => new DateTime(2021, 04, 12);

		public override IEnumerable<string> changes
		{
			get
			{
				yield return "[Added] Dialog to Reparent Selection to specific Scene or Transform (Ctrl/Cmd+M)";
			}
		}
	}
}