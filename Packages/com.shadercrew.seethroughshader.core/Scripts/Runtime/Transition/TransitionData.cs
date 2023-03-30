using System;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    public class TransitionData
    {
        public float tValue { get; set; }
        public int direction { get; set; }
        public int matID { get; set; }
        public float tDuration { get; set; }


        //public TransitionData() { }

        public TransitionData(int matID)
        {
            this.matID = matID;
        }

        public TransitionData(float tValue, int direction, int matID, float tDuration)
        {
            this.tValue = tValue;
            this.direction = direction;
            this.matID = matID;
            this.tDuration = tDuration;
        }

        public float[] ToShaderFloatArraySegment()
        {
            return new float[] { tValue, (float)direction, (float)matID, tDuration };
        }

        public float[] ToShaderFloatArraySegmentZone()
        {
            return new float[] { tValue, (float)direction, tDuration };
        }

        public void CalculateTValue()
        {

//            float time = 0;

//            RenderPipeline renderPipeline = GeneralUtils.getCurrentRenderPipeline();
//            if(renderPipeline == RenderPipeline.BiRP)
//            {
//                time = Time.timeSinceLevelLoad;
//            }
//            else
//            {
//#if UNITY_EDITOR
//                time = Application.isPlaying ? Time.time : Time.realtimeSinceStartup;
//#else
//                time = Time.time;
//#endif
//            }

//            if (this.tValue != 0 && time - this.tValue < this.tDuration)
//            {
//                float lastTValue = this.tValue;
//                this.tValue = 2 * time - lastTValue - this.tDuration;
//            }
//            else
//            {
//                this.tValue = time;
//            }

            this.tValue = TransitionUtils.CalculateTValue(this.tValue, this.tDuration);


        }
    }
}