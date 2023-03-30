using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// event system by JCPereira 
namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_PLAYER)]
    public class SeeThroughShaderPlayer : MonoBehaviour
    {

        /// <summary>
        /// Amount of zones currently inside
        /// </summary>
        public int zonesInsideCount => _zonesInside.Count;

        /// <summary>
        /// All current zones inside
        /// </summary>
        public List<SeeThroughShaderZone> zonesInside => _zonesInside.ToList(); //Copy of original list

        /// <summary>
        /// If true, it will send event <see cref="OnExitZoneEvent"/> for all zones in current list <see cref="_zonesInside"/>
        /// </summary>
        [Tooltip("If true it will trigger the event OnExitZoneEvent for all zones inner")]
        [SerializeField]
        bool triggerZoneEventOnDisable = false;

#if UNITY_2020_1_OR_NEWER
        [SerializeField]
        UnityEngine.Events.UnityEvent<SeeThroughShaderZone> OnEnterZoneEvent;
#else
        [System.Serializable]
        public class OnEnterZoneEventX : UnityEvent<SeeThroughShaderZone> { }
        [SerializeField]
        public OnEnterZoneEventX OnEnterZoneEvent;
#endif

#if UNITY_2020_1_OR_NEWER
        [SerializeField]
        UnityEngine.Events.UnityEvent<SeeThroughShaderZone> OnExitZoneEvent;
#else
        [System.Serializable]
        public class OnExitZoneEventX : UnityEvent<SeeThroughShaderZone> { }
        [SerializeField]
        public OnExitZoneEventX OnExitZoneEvent;
#endif



        PlayersPositionManager posManager;
        List<SeeThroughShaderZone> _zonesInside = new List<SeeThroughShaderZone>();



        private void Awake()
        {
            posManager = GameObject.FindObjectOfType<PlayersPositionManager>();
        }

        private void OnDisable()
        {

            if (posManager != null)
            {
                posManager.RemovePlayerAtRuntime(this.gameObject);
            }

            if (triggerZoneEventOnDisable)
            {
                foreach (SeeThroughShaderZone zone in _zonesInside)
                {
                    OnExitZoneEvent?.Invoke(zone);
                }
            }

            _zonesInside.Clear();
        }

        /// <summary>
        /// Trigged by <see cref="SeeThroughShaderZone"/> ever object enter at a active zones
        /// </summary>
        /// <param name="STSZone"></param>
        internal void OnEnterZone(SeeThroughShaderZone STSZone)
        {

            if (!_zonesInside.Contains(STSZone)) { return; }
            _zonesInside.Add(STSZone);
            OnEnterZoneEvent?.Invoke(STSZone);
        }

        /// <summary>
        /// Trigged by <see cref="SeeThroughShaderZone"/> ever this object exit from active zones
        /// </summary>
        /// <param name="STSZone"></param>
        internal void OnExitZone(SeeThroughShaderZone STSZone)
        {
            if (_zonesInside.Remove(STSZone))
            {
                OnExitZoneEvent?.Invoke(STSZone);
            }
        }




    }
}