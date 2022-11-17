using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_BUILDING_AUTO_DETECTOR)]
    public class BuildingAutoDetector : MonoBehaviour
    {

        public bool showDebugRays = false;
        TransitionController seeThroughShaderController;
        string currentHouse;
        Transform lastHouse;
        //public Camera camera;  // ???

        ExponentiallyWeightedAverage coneAverage;
        ExponentiallyWeightedAverage sphereAverage;

        ExponentiallyWeightedAverage coneContainsAverage;
        ExponentiallyWeightedAverage sphereContainsAverage;

        Transform lastHit;
        float diagonalOfLastHitBounds;

        List<DetectorParameters> detectorParametersLayers;

        // Start is called before the first frame update
        void Start()
        {
            seeThroughShaderController = new TransitionController();
            coneAverage = new ExponentiallyWeightedAverage();
            sphereAverage = new ExponentiallyWeightedAverage();

            coneContainsAverage = new ExponentiallyWeightedAverage();
            sphereContainsAverage = new ExponentiallyWeightedAverage();

            //detectorParametersLayers = new List<DetectorParameters>();
            //detectorParametersLayers.Add(new DetectorParameters(0.5f, 0.5f));
            //detectorParametersLayers.Add(new DetectorParameters(0.1f, 0.4f));
            //detectorParametersLayers.Add(new DetectorParameters(0.9f, 0.5f));

        }

        void OnEnable()
        {
            if (coneContainsAverage == null)
            {
                coneContainsAverage = new ExponentiallyWeightedAverage();
            }

            if (sphereContainsAverage == null)
            {
                sphereContainsAverage = new ExponentiallyWeightedAverage();
            }

            if (coneAverage == null)
            {
                coneAverage = new ExponentiallyWeightedAverage();
            }

            if (sphereAverage == null)
            {
                sphereAverage = new ExponentiallyWeightedAverage();
            }

            if (seeThroughShaderController == null)
            {
                seeThroughShaderController = new TransitionController();
            }
        }

        // Update is called once per frame
        void Update()
        {
            coneAndSphereRaycastDetection();
        }

        private void OnDisable()
        {
            if (seeThroughShaderController != null)
            {
                seeThroughShaderController.destroy();
            }

        }

        private void OnDestroy()
        {
            if (seeThroughShaderController != null)
            {
                seeThroughShaderController.destroy();
            }
        }


        void resetAllAverages()
        {
            coneAverage.reset();
            sphereAverage.reset();
            coneContainsAverage.reset();
            sphereContainsAverage.reset();
        }

        public struct DetectorParameters
        {
            public DetectorParameters(float coneThreshold = 0.5f, float sphereThreshold = 0.5f)
            {
                this.coneThreshold = coneThreshold;
                this.sphereThreshold = sphereThreshold;
            }
            public float coneThreshold { get; }
            public float sphereThreshold { get; }
        }


        public class ExponentiallyWeightedAverage
        {
            public float exponentiallyWeightedAverage { get; set; }
            float beta;
            public ExponentiallyWeightedAverage(float beta = 0.95f)
            {
                this.beta = beta;
                this.exponentiallyWeightedAverage = 0;
            }

            public void update(float value)
            {
                this.exponentiallyWeightedAverage = beta * this.exponentiallyWeightedAverage + (1 - beta) * value;
            }

            public void reset()
            {
                this.exponentiallyWeightedAverage = 0;
            }

        }


        /**
         * Unnormalized Ray
         */
        public struct ScaledRay
        {
            public ScaledRay(Vector3 pos, Vector3 unnormalizedDirection)
            {
                position = pos;
                direction = unnormalizedDirection;
            }

            public Vector3 position { get; }

            //unnormalized
            public Vector3 direction { get; }

        }
        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 2.5f, float arrowHeadAngle = 10.0f, float duration = 100)
        {
            duration = 1;
            Debug.DrawRay(pos, direction, color, duration);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color, duration);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color, duration);
        }
        public static void DrawArrow(ScaledRay scaledRay, Color color, float arrowHeadLength = 2.5f, float arrowHeadAngle = 10.0f, float duration = 100)
        {

            duration = 1;
            Vector3 pos = scaledRay.position;
            Vector3 direction = scaledRay.direction;
            Debug.DrawRay(pos, direction, color, duration);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color, duration);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color, duration);
        }
        ScaledRay sampleScaledRayInsideCone(float coneRadius, Vector3 coneTip, Vector3 coneTipToBase, bool direction = true, bool withDebug = false)
        {
            float randRadius = Random.Range(0, coneRadius);
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);
            Vector3 orthogonalToBase;
            if (coneTipToBase.x != 0)
            {
                orthogonalToBase = new Vector3((coneTipToBase.y * y + coneTipToBase.z * z) / -coneTipToBase.x, y, z).normalized * randRadius;
            }
            else if (coneTipToBase.y != 0)
            {
                orthogonalToBase = new Vector3(x, (coneTipToBase.x * x + coneTipToBase.z * z) / -coneTipToBase.y, z).normalized * randRadius;
            }
            else if (coneTipToBase.z != 0)
            {
                orthogonalToBase = new Vector3(x, y, (coneTipToBase.x * x + coneTipToBase.y * y) / -coneTipToBase.z).normalized * randRadius;
            }
            else
            {
                orthogonalToBase = new Vector3();
                Debug.LogError("null vector for coneBase not allowed");
            }

            //from tip to base
            if (direction)
            {
                Vector3 vectorInsideCone = coneTipToBase + orthogonalToBase;
                if (withDebug)
                {
                    DrawArrow(coneTip, (vectorInsideCone), Color.cyan);
                }
                return new ScaledRay(coneTip, vectorInsideCone);
                // from base to tip
            }
            else
            {
                Vector3 coneBase = coneTipToBase + coneTip;
                Vector3 vectorInsideCone = orthogonalToBase + coneBase;
                Vector3 coneBaseToTip = coneTip - vectorInsideCone;
                if (withDebug)
                {
                    DrawArrow(coneBase + orthogonalToBase, coneBaseToTip, Color.cyan);
                }
                return new ScaledRay(coneBase + orthogonalToBase, coneBaseToTip);
            }
        }

        ScaledRay sampleScaledRayInsideSphere(Vector3 origin, float sphereRadius, bool direction = true, bool withDebug = false)
        {
            Vector3 randOnSphere = Random.onUnitSphere * sphereRadius;
            if (direction)
            {
                if (withDebug)
                {
                    DrawArrow(origin + randOnSphere, -randOnSphere, Color.blue);
                }
                return new ScaledRay(origin + randOnSphere, -randOnSphere);
            }
            else
            {
                if (withDebug)
                {
                    DrawArrow(origin, randOnSphere, Color.blue);
                }
                return new ScaledRay(origin, randOnSphere);
            }
        }

        void exitBuilding()
        {
            if (lastHouse != null)
            {
                seeThroughShaderController.notifyOnTriggerExit(lastHouse.root, this.transform);
                lastHouse = null;
                resetAllAverages();
            }
        }


        void enterBuilding(Transform transform)
        {
            Debug.Assert(lastHouse == null, "enterBuilding(), lastHouse has to be null!");
            lastHouse = transform;
            seeThroughShaderController.notifyOnTriggerEnter(transform.root, this.transform);
        }



        void setupBuilding(Transform transform)
        {
            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].materials.Length > 0)
                {
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        if (GeneralUtils.STS_SHADER_LIST.Contains(renderers[i].materials[j].shader.name))
                        {
                            renderers[i].materials[j].SetFloat("_RaycastMode", 1);
                        }
                    }
                }
            }
        }


        void doConeTestingContains(bool withDebug = false)
        {
            Vector3 vectorUp = Vector3.up * 100;
            ScaledRay scaledRay = sampleScaledRayInsideCone(5, this.transform.position, vectorUp, false, withDebug);
            RaycastHit hit;
            float value = 0;
            if (Physics.Raycast(scaledRay.position, scaledRay.direction, out hit, scaledRay.direction.magnitude))
            {
                if (hit.collider.bounds.Contains(this.transform.position) && hit.transform.root != this.transform.root && !checkIfTriggerMode(hit.transform.root))
                {
                    if (lastHit != hit.transform.root)
                    {
                        coneContainsAverage.reset();
                        lastHit = hit.transform.root;
                        diagonalOfLastHitBounds = Mathf.Sqrt(Mathf.Pow(hit.collider.bounds.size.x, 2) + Mathf.Pow(hit.collider.bounds.size.y, 2)) + 1;
                    }
                    value = 1;
                }
            }
            coneContainsAverage.update(value);
        }

        void doSphereTestingContains(bool withDebug = false)
        {
            ScaledRay scaledRay = sampleScaledRayInsideSphere(this.transform.position, 40, true, withDebug);
            RaycastHit hit;
            float value = 0;
            if (Physics.Raycast(scaledRay.position, scaledRay.direction, out hit, scaledRay.direction.magnitude) && hit.transform.root != this.transform.root && !checkIfTriggerMode(hit.transform.root))
            {
                if (hit.collider.bounds.Contains(this.transform.position))
                {
                    if (lastHit != hit.transform.root)
                    {
                        sphereContainsAverage.reset();
                        lastHit = hit.transform.root;
                        diagonalOfLastHitBounds = Mathf.Sqrt(Mathf.Pow(hit.collider.bounds.size.x, 2) + Mathf.Pow(hit.collider.bounds.size.y, 2)) + 1;
                    }
                    value = 1;
                }
            }
            sphereContainsAverage.update(value);
        }

        void doConeTestingIntersect(Transform transform, bool withDebug = false)
        {
            Vector3 vectorUp = Vector3.up * diagonalOfLastHitBounds;
            ScaledRay scaledRay = sampleScaledRayInsideCone(5, this.transform.position, vectorUp, false, withDebug);
            RaycastHit hit;
            float value = 0;
            if (Physics.Raycast(scaledRay.position, scaledRay.direction, out hit, scaledRay.direction.magnitude))
            {
                if (transform == hit.transform.root)
                {
                    value = 1;
                }
            }
            coneAverage.update(value);
        }

        void doSphereTestingIntersect(Transform transform, bool withDebug = false)
        {
            ScaledRay scaledRay = sampleScaledRayInsideSphere(this.transform.position, diagonalOfLastHitBounds, true, withDebug);
            RaycastHit hit;
            float value = 0;
            if (Physics.Raycast(scaledRay.position, scaledRay.direction, out hit, scaledRay.direction.magnitude))
            {
                if (transform == hit.transform.root)
                {
                    value = 1;
                }
            }
            sphereAverage.update(value);
        }

        //not necessary to check every material
        bool checkIfTriggerMode(Transform transform)
        {
            bool hasTriggerMode = false;

            Transform test3 = transform.root;
            Renderer[] renderers3 = test3.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers3.Length; i++)
            {
                if (renderers3[i].material.HasProperty("_TriggerMode") && renderers3[i].material.GetFloat("_TriggerMode") == 1)
                {
                    hasTriggerMode = true;
                }

                // return with first true
                if (hasTriggerMode)
                {
                    return hasTriggerMode;
                }
            }
            return hasTriggerMode;
        }

        bool buildingDetectionContains(float coneThreshold = 0.5f, float sphereThreshold = 0.5f)
        {
            bool isInside = false;
            doConeTestingContains(showDebugRays);
            if (coneContainsAverage.exponentiallyWeightedAverage > coneThreshold)
            {
                isInside = true;
            }
            else
            {
                doSphereTestingContains(showDebugRays);
                if (sphereContainsAverage.exponentiallyWeightedAverage > sphereThreshold)
                {
                    isInside = true;
                }
            }
            return isInside;
        }

        bool buildingDetectionIntersects(Transform transform, float coneThreshold = 0.5f, float sphereThreshold = 0.5f)
        {
            bool isInside = false;
            doConeTestingIntersect(transform.root, showDebugRays);
            if (coneAverage.exponentiallyWeightedAverage > coneThreshold)
            {
                isInside = true;
            }
            else
            {
                doSphereTestingIntersect(transform.root, showDebugRays);
                if (sphereAverage.exponentiallyWeightedAverage > sphereThreshold)
                {
                    isInside = true;
                }
            }
            return isInside;
        }



        void coneAndSphereRaycastDetection()
        {
            bool isContained = buildingDetectionContains(0.2f, 0.2f);

            if (isContained)
            {
                float coneThreshold = 0.2f;
                float sphereThreshold = 0.2f;
                //if (detectorParametersLayers.Count >= lastHit.gameObject.layer + 1)
                // {
                //coneThreshold = detectorParametersLayers[lastHit.gameObject.layer].coneThreshold;
                //sphereThreshold = detectorParametersLayers[lastHit.gameObject.layer].sphereThreshold;
                //}

                bool isInside = buildingDetectionIntersects(lastHit, coneThreshold, sphereThreshold);
                if (isInside)
                {
                    if (lastHouse != null && lastHouse != lastHit)
                    {
                        exitBuilding();
                    }
                    if (lastHouse != lastHit)
                    {
                        // setupBuilding(lastHit);
                        seeThroughShaderController.setupAutoDetect(lastHit);
                        enterBuilding(lastHit);
                    }
                }
                else
                {
                    exitBuilding();
                }
            }
            else
            {
                exitBuilding();
            }
        }

    }
}
