using System.Collections;

using UnityEngine;

namespace Broccoli.Pipe {
	/// <summary>
	/// Interface for pipeline elements that make usage of sprout groups,
	/// thus need listening to events about sprout group changes.
	/// </summary>
	public interface ISproutGroupConsumer {
		/// <summary>
		/// Look if certain sprout group is being used in this element.
		/// </summary>
		/// <returns><c>true</c>, if sprout group is being used, <c>false</c> otherwise.</returns>
		/// <param name="sproutGroupId">Sprout group identifier.</param>
		bool HasSproutGroupUsage (int sproutGroupId);
		/// <summary>
		/// Commands the element to stop using certain sprout group.
		/// </summary>
		/// <param name="sproutGroupId">Sprout group identifier.</param>
		/// <returns><c>True</c> if sprout group has been removed, <c>false</c> otherwise.</returns>
		bool StopSproutGroupUsage (int sproutGroupId);
		/// <summary>
		/// Gets the array of ids of groups consumed by this element.
		/// </summary>
		/// <returns>Arrays of group ids.</returns>
		int[] GetGroupIds ();
		/// <summary>
		/// Gets the array of colors for the groups consumed by this element.
		/// </summary>
		/// <returns>Arrays of group colors.</returns>
		Color[] GetGroupColors ();
	}
}