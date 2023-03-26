using System;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable MemberCanBePrivate.Global

namespace Quibli {
#if DUSTYROOM_DEV
[CreateAssetMenu(fileName = "Readme", menuName = "Quibli/Internal/Readme", order = 0)]
#endif // DUSTYROOM_DEV

[ExecuteAlways]
public class Readme : ScriptableObject {
    [NonSerialized] public readonly string AssetVersion = "1.7.1";

    [NonSerialized] public bool? UrpInstalled;

    [NonSerialized] [CanBeNull] public string PackageManagerError;

    [NonSerialized] public string UrpVersionInstalled = "N/A";

    [NonSerialized] public string UnityVersion = Application.unityVersion;

    private const string UrpPackageID = "com.unity.render-pipelines.universal";

    public void Refresh() {
        UrpInstalled = false;
        PackageManagerError = null;

        PackageCollection packages = GetPackageList();
        foreach (PackageInfo p in packages) {
            if (p.name == UrpPackageID) {
                UrpInstalled = true;
                UrpVersionInstalled = p.version;
            }
        }

        UnityVersion = Application.unityVersion;
    }

    private PackageCollection GetPackageList() {
        var listRequest = Client.List(true);

        while (listRequest.Status == StatusCode.InProgress) continue;

        if (listRequest.Status == StatusCode.Failure) {
            PackageManagerError = listRequest.Error.message;
            Debug.LogWarning("<b>[Quibli]</b> Failed to get packages from Package Manager.");
            return null;
        }

        return listRequest.Result;
    }
}
}
