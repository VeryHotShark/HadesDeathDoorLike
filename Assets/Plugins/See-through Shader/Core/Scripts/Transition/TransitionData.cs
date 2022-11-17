using System;
using UnityEngine;

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
            //if (playersData[playerIndexToUpdate][1]!=0 && Time.timeSinceLevelLoad - playersData[playerIndexToUpdate][1] < duration)
            if (this.tValue != 0 && Time.timeSinceLevelLoad - this.tValue < this.tDuration)
            {
                //float lastTValue = playersData[playerIndexToUpdate][1];
                float lastTValue = this.tValue;
                // last stable
                //float newTValue = duration - (((Time.timeSinceLevelLoad - 1) - lastTValue) - 1);
                //tValue = (Time.timeSinceLevelLoad - newTValue + 2);
                this.tValue = 2 * Time.timeSinceLevelLoad - lastTValue - this.tDuration;

            }
            else
            {
                this.tValue = Time.timeSinceLevelLoad;
            }
        }
    }
}