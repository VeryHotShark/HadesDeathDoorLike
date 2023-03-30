using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    public class TransitionUtils
    {

        public static float CalculateTValue(float tValue, float duration)
        {
            float time = 0;

            RenderPipeline renderPipeline = GeneralUtils.getCurrentRenderPipeline();
            if (renderPipeline == RenderPipeline.BiRP)
            {
                time = Time.timeSinceLevelLoad;
            }
            else
            {
#if UNITY_EDITOR
                time = Application.isPlaying ? Time.time : Time.realtimeSinceStartup;
#else
                time = Time.time;
#endif
            }

            if (tValue != 0 && time - tValue < duration)
            {
                float lastTValue = tValue;
                tValue = 2 * time - lastTValue - duration;
            }
            else
            {
                tValue = time;
            }


            return tValue;
        }

    }

}
