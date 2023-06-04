using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class NpcStatusUI : MonoBehaviour {
        [SerializeField] private UIStatusIcon _statusIconPrefab;

        private List<UIStatusIcon> _statusIcons = new();
        private Dictionary<Status, UIStatusIcon> _statusIconsDict = new();


        public void AddStatus(Status status) {
            UIStatusIcon statusIcon = Instantiate(_statusIconPrefab, transform);
            statusIcon.SetStatus(status);
            _statusIcons.Add(statusIcon);
            _statusIconsDict.Add(status, statusIcon);
        }

        public  void RemoveStatus(Status status) {
            UIStatusIcon statusIcon = _statusIconsDict[status];
            _statusIconsDict.Remove(status);
            _statusIcons.Remove(statusIcon);
            Destroy(statusIcon.gameObject);
        }

        private void Update() {
            if(_statusIcons.Count == 0)
                return;
            
            UpdateStatusIcons();
        }

        private void UpdateStatusIcons() {
            foreach (UIStatusIcon statusIcon in _statusIcons) {
                statusIcon.UpdateStatusDuration();
            }            
        }
    }
}
