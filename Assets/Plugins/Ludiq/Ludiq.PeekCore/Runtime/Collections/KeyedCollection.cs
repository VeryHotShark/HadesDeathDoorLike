namespace Ludiq.PeekCore
{
	public abstract class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>, IKeyedCollection<TKey, TItem>
	{
		// TryGetValue is defined by newer Unity/.NET versions in KeyedCollection,
		// but I can't seem to find a define to branch over to determine which version
		// of .NET is using
		// https://forum.unity.com/threads/1092205/page-11#post-7696570
		#pragma warning disable 108
		
		public bool TryGetValue(TKey key, out TItem item)
		{
			Ensure.That(nameof(key)).IsNotNull(key);

			if (Dictionary != null)
			{
				return Dictionary.TryGetValue(key, out item);
			}

			foreach (var itemInItems in Items)
			{
				var keyInItems = GetKeyForItem(itemInItems);

				if (keyInItems != null && Comparer.Equals(key, keyInItems))
				{
					item = itemInItems;
					return true;
				}
			}

			item = default;
			return false;
		}

		// TODO: Make sure duck typing works even on base class
		public new NoAllocEnumerator<TItem> GetEnumerator()
		{
			return new NoAllocEnumerator<TItem>(this);
		}
	}
}