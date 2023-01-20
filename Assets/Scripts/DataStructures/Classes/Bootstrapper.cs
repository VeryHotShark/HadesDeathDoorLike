using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VHS {
    public class Bootstrapper : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() =>
            DontDestroyOnLoad(Addressables.InstantiateAsync("Systems.prefab").WaitForCompletion());
    }
}
