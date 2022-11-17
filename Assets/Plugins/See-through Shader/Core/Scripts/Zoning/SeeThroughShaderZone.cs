using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu("")]
    public class SeeThroughShaderZone : MonoBehaviour
    {

        public ZoneData zoneData;

        public TransformContainer tempTransform;

        private ZoneDataStorage zoneDataStorage = ZoneDataStorage.Instance;

        public ZoneType type;

        public int numOfPlayersInside = 0;
        public int transitionDuration = 2;

        bool isInitialized = false;

        public bool isActivated = false;

        private void init()
        {
            if (!isInitialized)
            {
                if (type == ZoneType.Box)
                {
                    //zoneData = new ZoneBox(transform);
                    zoneData = ZoneBox.CreateInstance(transform);
                }
                zoneData.id = IdGenerator.Instance.Id;
                zoneData.zoneInstanceID = GetInstanceID();
                zoneDataStorage.AddOrUpdateZoneData(zoneData);
                isInitialized = true;
            }

        }

        public void ForceInit()
        {
            init();
        }

        private void OnTransformParentChanged()
        {
            zoneData.id = IdGenerator.Instance.Id;
            zoneDataStorage.AddOrUpdateZoneData(zoneData);
            PlayersDataStorage playersDataStorage = PlayersDataStorage.Instance;
            playersDataStorage.RemoveAllTransitionDatasFromZone(zoneData);
        }

        void Start()
        {
            init();
        }

        private void OnDisable()
        {
            if (isInitialized)
            {
                zoneDataStorage.RemoveZoneData(zoneData);
                zoneDataStorage.UpdateZonesDatasShaderFloatArray();
            }
        }

        private void OnEnable()
        {
            if (isInitialized)
            {
                zoneDataStorage.AddOrUpdateZoneData(zoneData);
                zoneDataStorage.UpdateZonesDatasShaderFloatArray();
            }
        }

        void Update()
        {
            if (zoneData != null && zoneData.UpdateNecessary())
            {
                zoneDataStorage.AddOrUpdateZoneData(zoneData);
                zoneDataStorage.UpdateZonesDatasShaderFloatArray();
            }

        }

        public void UpdateZoneData()
        {
            zoneDataStorage.AddOrUpdateZoneData(zoneData);
            zoneDataStorage.UpdateZonesDatasShaderFloatArray();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
            {
                TransitionController.transitionEffectZones(1, other.transform, this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
            {
                TransitionController.transitionEffectZones(-1, other.transform, this);
            }
        }

        public void toggleZoneActivation()
        {
            if(this.isActiveAndEnabled)
            {

                if (isActivated)
                {
                    TransitionController.transitionEffectZones(-1, this);
                    isActivated = false;
                }
                else
                {
                    TransitionController.transitionEffectZones(1, this);
                    isActivated = true;
                }

            }

        }

    }
}