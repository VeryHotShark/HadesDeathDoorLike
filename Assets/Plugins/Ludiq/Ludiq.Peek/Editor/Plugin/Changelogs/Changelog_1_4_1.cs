using System;
using System.Collections.Generic;
using Ludiq.Peek;
using Ludiq.PeekCore;

[assembly: MapToPlugin(typeof(Changelog_1_4_1), PeekPlugin.ID)]

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	internal class Changelog_1_4_1 : PluginChangelog
	{
		public Changelog_1_4_1(Plugin plugin) : base(plugin) { }

		public override SemanticVersion version => "1.4.1";

		public override DateTime date => new DateTime(2021, 11, 09);

		public override IEnumerable<string> changes
		{
			get
			{
				yield return "[Fixed] Compatibility with Unity 2021.1 and 2021.2";
			}
		}
	}
}