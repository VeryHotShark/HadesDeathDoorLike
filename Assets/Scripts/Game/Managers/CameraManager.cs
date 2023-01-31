using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class CameraManager : Singleton<CameraManager>
    {
        //Cinemachine
        
        private static Camera _cameraInstance;
        public static Camera CameraInstance => _cameraInstance;

        public static void RegisterCamera(Player player) => _cameraInstance = player.Camera;
    }
}
