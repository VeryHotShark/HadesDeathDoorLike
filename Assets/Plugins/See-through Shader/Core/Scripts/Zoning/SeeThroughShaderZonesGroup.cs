using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu("")]
    public class SeeThroughShaderZonesGroup : MonoBehaviour
    {
        bool isInitialized = false;
        int id;

        private ZoneDataStorage zoneDataStorage = ZoneDataStorage.Instance;
        private PlayersDataStorage playersDataStorage = PlayersDataStorage.Instance;
        private void Init()
        {
            if (!isInitialized)
            {
                id = IdGenerator.Instance.Id;
                isInitialized = true;
            }

        }

        void Start()
        {
            Init();

            AddZonesToZonesGroup();
        }


        private void OnTransformChildrenChanged()
        {
            AddZonesToZonesGroup();
        }

        private void AddZonesToZonesGroup()
        {
            SeeThroughShaderZone[] zones = transform.GetComponentsInChildren<SeeThroughShaderZone>();
            foreach (SeeThroughShaderZone zone in zones)
            {
                zone.ForceInit();
                playersDataStorage.RemoveAllTransitionDatasFromZone(zone.zoneData);
                zone.zoneData.id = id;

                zoneDataStorage.AddOrUpdateZoneData(zone.zoneData);
            }
        }

    }
}