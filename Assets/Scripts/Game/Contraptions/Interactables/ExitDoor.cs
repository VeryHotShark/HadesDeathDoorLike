using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VHS {
    public class ExitDoor : Interactable {
        // TODO Change this so LevelManager has all the scenes
        [ValueDropdown("SelectScene", DropdownTitle = "Scene Selection")]
        [SerializeField] private string _nextScene;
        
        private bool _isLocked = false;

        public void SetLocked(bool state) => _isLocked = state;

        public override bool IsInteractable(Player player) => _isLocked == false && base.IsInteractable(player);

        public override void OnInteract(Player player) {
            base.OnInteract(player);
            LevelManager.LoadScenes(_nextScene);
        }
        
        private static IEnumerable SelectScene()
        {
            var filesPath = Directory.GetFiles("Assets/Scenes");
            var fileNameList = filesPath
                .Select(Path.GetFileName)
                .Select(file => file.Split(".")[0])
                .Distinct()
                .ToList();

            return fileNameList;
        }
    }
}
