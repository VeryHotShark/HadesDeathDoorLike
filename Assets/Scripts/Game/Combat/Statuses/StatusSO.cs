using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    [CreateAssetMenu(fileName = "Status", menuName = "VHS/Combat/StatusSO")]
    public class StatusSO : ScriptableObject {
        [SerializeReference] private Status _status;

        public Status Instance => _status;
    }
}
