using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    /// <summary>
    /// Only works with MonoBehaviours, not classes
    /// for Classes use Unity GenericPool<T>.Get
    /// </summary>
    public interface IPoolable {
        GameObject gameObject { get; }
    }
}