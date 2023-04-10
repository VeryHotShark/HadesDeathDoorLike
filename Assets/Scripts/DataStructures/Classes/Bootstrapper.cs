using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace VHS {
    public class Bootstrapper : MonoBehaviour {
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() {
            var systems = Resources.Load<GameObject>("Systems");
            Instantiate(systems);
        }
        // public static void Execute() => DontDestroyOnLoad(Addressables.InstantiateAsync("Systems.prefab").WaitForCompletion());
    }
}
