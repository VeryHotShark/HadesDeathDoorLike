#ifndef SEETHROUGHSHADER_FUNCTION
#define SEETHROUGHSHADER_FUNCTION

#ifndef UNITY_MATRIX_I_M
        #define UNITY_MATRIX_I_M   unity_WorldToObject
#endif


void DoSeeThroughShading(
                                    float3 l0,
                                    float3 ll0, float3 lll0,
                                    float3 llll0,
                                    float lllll0, float llllll0, float lllllll0, float llllllll0,
                                    float lllllllll0, float llllllllll0,
                                    float lllllllllll0,
                                    float4 llllllllllll0, float lllllllllllll0, float llllllllllllll0,
                                    float lllllllllllllll0, float llllllllllllllll0, float lllllllllllllllll0, float llllllllllllllllll0,
                                    bool lllllllllllllllllll0,
                                    float llllllllllllllllllll0,
                                    float lllllllllllllllllllll0,
                                    float llllllllllllllllllllll0, float lllllllllllllllllllllll0,
                                    float llllllllllllllllllllllll0, float lllllllllllllllllllllllll0,
                                    float llllllllllllllllllllllllll0, float lllllllllllllllllllllllllll0,
                                    float llllllllllllllllllllllllllll0, float lllllllllllllllllllllllllllll0,
                                    float llllllllllllllllllllllllllllll0,
                                    float lllllllllllllllllllllllllllllll0,
                                    float l1,
                                    float ll1,
                                    float lll1, float llll1, float lllll1,
                                    float llllll1, float lllllll1, float llllllll1, float lllllllll1, float llllllllll1, float lllllllllll1,
                                    float llllllllllll1, float lllllllllllll1, float llllllllllllll1, float lllllllllllllll1, float llllllllllllllll1,
                                    float lllllllllllllllll1, float llllllllllllllllll1,
                                    float lllllllllllllllllll1,
                                    float llllllllllllllllllll1, float lllllllllllllllllllll1, float llllllllllllllllllllll1, float lllllllllllllllllllllll1,
                                    float llllllllllllllllllllllll1, float lllllllllllllllllllllllll1,
                                    float llllllllllllllllllllllllll1,
                                    float lllllllllllllllllllllllllll1,
#ifdef USE_UNITY_TEXTURE_2D_TYPE
                                    UnityTexture2D llllllllllllllllllllllllllll1,
                                    UnityTexture2D lllllllllllllllllllllllllllll1,
                                    UnityTexture2D llllllllllllllllllllllllllllll1,
#else
                                    sampler2D llllllllllllllllllllllllllll1,
                                    sampler2D lllllllllllllllllllllllllllll1,
                                    sampler2D llllllllllllllllllllllllllllll1,
                                    float4 lllllllllllllllllllllllllllllll1,
                                    float4 l2,
#endif
                                    out half3 ll2,
                                    out half3 lll2,
                                    out float llll2
)
{
        ShaderData d;
        d.worldSpaceNormal = llll0;
        d.worldSpacePosition = lll0;
        Surface o;
        o.Normal = ll0;
        o.Albedo = half3(0, 0, 0) + l0;
        o.Emission = half3(0, 0, 0);
        ll2 = half3(0, 0, 0);
        lll2 = half3(0, 0, 0);
        llll2 = 1;
        bool lllll2;
        lllll2 = (lllll0 > 0 || llllll0 == -1 && _Time.y - lllllll0 < lllllllllllllllllll1) || (lllll0 >= 0 && llllll0 == 1);
        bool llllll2 = !lllllllll0 && !llllllllll0;
        float lllllll2 = 0;
        half4 llllllll2 = half4(0, 0, 0, 0);
        if (!lllllllllll0 && (lllll2 || llllll2))
        {
        float4 lllllllll2;
        float4 llllllllll2;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
        llllllllll2 = llllllllllllllllllllllllllllll1.texelSize;
        lllllllll2 = lllllllllllllllllllllllllllll1.texelSize;
#else
        llllllllll2 = l2;
        lllllllll2 = lllllllllllllllllllllllllllllll1;
#endif
            if (l1 < 0)
            {
                l1 = 0;
            }
            float3 lllllllllll2;
            float3 llllllllllll2 = d.worldSpacePosition / (-1.0 * abs(llllllllllllll0));
            if (lllllllllllllllll1)
            {
                llllllllllll2 = llllllllllll2 + abs(((_Time.y) * llllllllllllllllll1));
            }
            float3 lllllllllllll2 = float3(0, 0, 0);
            float3 llllllllllllll2 = float3(0, 0, 0);
            float3 lllllllllllllll2 = float3(0, 0, 0);
            lllllllllllll2 = tex2D(llllllllllllllllllllllllllll1, llllllllllll2.yz).rgb;
            llllllllllllll2 = tex2D(llllllllllllllllllllllllllll1, llllllllllll2.xz).rgb;
            lllllllllllllll2 = tex2D(llllllllllllllllllllllllllll1, llllllllllll2.xy).rgb;
            float llllllllllllllll2 = abs(d.worldSpaceNormal.x);
            float lllllllllllllllll2 = abs(d.worldSpaceNormal.z);
            float3 llllllllllllllllll2 = lerp(llllllllllllll2, lllllllllllll2, llllllllllllllll2).rgb;
            lllllllllll2 = lerp(llllllllllllllllll2, lllllllllllllll2, lllllllllllllllll2).rgb;
            half lllllllllllllllllll2 = lllllllllll2.r;
            float3 llllllllllllllllllll2 = UNITY_MATRIX_V[2].xyz;
#ifdef _HDRP
                llllllllllllllllllll2 =  mul(UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2]).xyz;
#else
        llllllllllllllllllll2 = mul(UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2]).xyz;
#endif
            float lllllllllllllllllllll2 = 0;
            float llllllllllllllllllllll2 = 1;
            bool lllllllllllllllllllllll2 = false;
            float llllllllllllllllllllllll2 = 0;
            float lllllllllllllllllllllllll2 = 0;
            float llllllllllllllllllllllllll2 = 0;
            float lllllllllllllllllllllllllll2 = 0;
            float llllllllllllllllllllllllllll2 = 0;
            float lllllllllllllllllllllllllllll2 = 0;
#if defined(_ZONING)
                if(llllllllllllllllllll1) {
                    float llllllllllllllllllllllllllllll2 = 0;
                    for (int z = 0; z < _ZonesDataCount; z++){
                        bool lllllllllllllllllllllllllllllll2 = false;
                        float l3 = llllllllllllllllllllllllllllll2;
                        if (_ZDFA[llllllllllllllllllllllllllllll2 + 1] == 0) {  
                            float ll3 = llllllllllllllllllllllllllllll2 + 2; 
                            float3 lll3 = d.worldSpacePosition - float3(_ZDFA[ll3],_ZDFA[ll3+1], _ZDFA[ll3+2]);
                            float3 llll3 =     float3(_ZDFA[ll3+ 3],_ZDFA[ll3+ 4], _ZDFA[ll3+ 5]);
                            float3 lllll3 =     float3(_ZDFA[ll3+ 6],_ZDFA[ll3+ 7], _ZDFA[ll3+ 8]);
                            float3 llllll3 =     float3(_ZDFA[ll3+ 9],_ZDFA[ll3+10], _ZDFA[ll3+11]);
                            float3 lllllll3 = float3(_ZDFA[ll3+12],_ZDFA[ll3+13], _ZDFA[ll3+14]);
                            lllllllllllllllllllllllllllllll2 =    abs(dot(lll3, llll3)) <= lllllll3.x &&
                                        abs(dot(lll3, lllll3)) <= lllllll3.y &&
                                        abs(dot(lll3, llllll3)) <= lllllll3.z;
                            if(lllllllllllllllllllllllllllllll2 && llllllllllll1 == 1 && llllllllllllllllllllllll1) {
                                llllllllllllllllllllllllll2 = _ZDFA[ll3+1] - _ZDFA[ll3+13];  
                                if(lllllllllllll1 == 0) {                                    
                                    bool llllllll3 = ((llllllllllllllllllllllllll2 - lllllllllllllllllllllllll1)  <= llllllllllllll1); 
                                    if(!llllllll3) {
                                        lllllllllllllllllllllllllllllll2 = false;
                                    }
                                }
                            }
                            llllllllllllllllllllllllllllll2 = llllllllllllllllllllllllllllll2 + 17 + 3; 
                            if(lllllllllllllllllllllllllllllll2) {
                                float lllllllll3 = lllllll3.x - abs(dot(lll3, llll3));
                                float llllllllll3 = lllllll3.y - abs(dot(lll3, lllll3));
                                float lllllllllll3 = lllllll3.z - abs(dot(lll3, llllll3));
                                float llllllllllll3 = min(llllllllll3,lllllllll3);
                                llllllllllll3 = min(llllllllllll3,lllllllllll3);
                                lllllllllllllllllllllllll2 = max(llllllllllll3,lllllllllllllllllllllllll2);
                                if(llllllllllll3<0) {
                                    lllllllllllllllllllllllll2 = 0;
                                }
                            }
                        }
                        if(lllllllllllllllllllllllllllllll2) {
                            if(lllllllllllllllllllllll2 == false) {
                                llllllllllllllllllllllll2 = _ZDFA[l3];
                                lllllllllllllllllllllll2 = true;    
                                lllllllllllllllllllllllllll2 = _ZDFA[l3 + 17];
                                lllllllllllllllllllllllllllll2 = _ZDFA[l3 + 18];
                                llllllllllllllllllllllllllll2 = _ZDFA[l3 + 19];
                            } 
                        }
                    }
                }
#endif
            float lllllllllllll3 = 0;
            float llllllllllllll3 = lllllllllllllllllllllll2;
#if !defined(_PLAYERINDEPENDENT)
#if defined(_ZONING)
                    if(lllllllllllllllllllllll2 && llllllllllll1 == 1 && lllllllllllll1 == 1 && llllllllllllllllllllllll1) {
                        float lllllllllllllll3 = 0;
                        bool llllllllllllllll3 = false;
                        for (int i = 0; i < _ArrayLength; i++){
                            float lllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllll3+1]; 
                            float3 llllllllllllllllll3 = _PlayersPosVectorArray[lllllllllllllllll3].xyz - _WorldSpaceCameraPos;               
                            if(dot(llllllllllllllllllll2,llllllllllllllllll3) <= 0) {       
                                if(!llllll2) {
                                    float lllllllllllllllllll3 = lllllllllllllll3 + 3;
                                    float llllllllllllllllllll3 = 4;
                                    for (int lllllllllllllllllllll6 = 0; lllllllllllllllllllll6 < _PlayersDataFloatArray[lllllllllllllll3 + 2]; lllllllllllllllllllll6++){
                                        float lllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 2];
                                        if (lllllllllllllllllllll3 != 0 && lllllllllllllllllllll3 == llllllll0) {
                                            float llllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 ];
                                            float lllllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 1];
                                            if ((lllllllllllllllllllllll3 == -1 && _Time.y - llllllllllllllllllllll3 < lllllllllllllllllll1 )|| (lllllllllllllllllllllll3 == 1) ) {
                                                float llllllllllllllllllllllll3 = _PlayersPosVectorArray[lllllllllllllllll3].y+ lllllllllllllll1;
                                                if(llllllllllllllllllllllllll1) {
                                                    if(i==0) {
                                                        lllllllllllll3 = llllllllllllllllllllllll3;
                                                    } else {
                                                        lllllllllllll3 = max(lllllllllllll3,llllllllllllllllllllllll3);
                                                    }
                                                }
                                                bool lllllllllllllllllllllllll3 = llllllllllllllllllllllllll2 >= llllllllllllllllllllllll3 + lllllllllllllllllllllllll1; 
                                                if(!lllllllllllllllllllllllll3) {
                                                    llllllllllllllll3 = true;
                                                } 
                                            }                        
                                        }
                                    }
                                } else if (distance(_PlayersPosVectorArray[lllllllllllllllll3].xyz, d.worldSpacePosition.xyz) < ll1) {
                                    float llllllllllllllllllllllll3 = _PlayersPosVectorArray[lllllllllllllllll3].y+ lllllllllllllll1;
                                    if(llllllllllllllllllllllllll1) {
                                        if(i==0) {
                                            lllllllllllll3 = llllllllllllllllllllllll3;
                                        } else {
                                            lllllllllllll3 = max(lllllllllllll3,llllllllllllllllllllllll3);
                                        }
                                    }
                                    bool lllllllllllllllllllllllll3 = llllllllllllllllllllllllll2 >= llllllllllllllllllllllll3 + lllllllllllllllllllllllll1; 
                                    if(!lllllllllllllllllllllllll3) {
                                        llllllllllllllll3 = true;
                                    } 
                                }
                                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3 + 2]*4 + 3; 
                                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3]*4 + 1; 
                            }
                        }
                        if(!llllllllllllllll3) {
                            lllllllllllllllllllllll2 = false;
                        }
                    }
#endif
            float lllllllllllllll3 = 0;
            for (int i = 0; i < _ArrayLength; i++)
            {
            float lllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllll3 + 1];
            if (sign(_PlayersPosVectorArray[lllllllllllllllll3].w) != -1)
            {
                float3 llllllllllllllllll3 = _PlayersPosVectorArray[lllllllllllllllll3].xyz - _WorldSpaceCameraPos;
                float lllllllllllllllllllllllllllllll3 = 0;
                float llllllllllllllllllll3 = 4;
                if (!llllll2)
                {
                    float lllllllllllllllllll3 = lllllllllllllll3 + 3;
                    for (int lllllllllllllllllllll6 = 0; lllllllllllllllllllll6 < _PlayersDataFloatArray[lllllllllllllll3 + 2]; lllllllllllllllllllll6++)
                    {
                        float lllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 2];
                        if (lllllllllllllllllllll3 != 0 && lllllllllllllllllllll3 == llllllll0)
                        {
                            float llllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3];
                            float lllllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 1];
                            lllllllllllllllllllllllllllllll3 = 1;
                            if (lllllllllllllllllllllll3 != 0 && llllllllllllllllllllll3 != 0 && _Time.y - llllllllllllllllllllll3 < lllllllllllllllllll1)
                            {
                                if (lllllllllllllllllllllll3 == 1)
                                {
                                    lllllllllllllllllllllllllllllll3 = ((lllllllllllllllllll1 - (_Time.y - llllllllllllllllllllll3)) / lllllllllllllllllll1);
                                }
                                else
                                {
                                    lllllllllllllllllllllllllllllll3 = ((_Time.y - llllllllllllllllllllll3) / lllllllllllllllllll1);
                                }
                            }
                            else if (lllllllllllllllllllllll3 == -1)
                            {
                                lllllllllllllllllllllllllllllll3 = 1;
                            }
                            else if (lllllllllllllllllllllll3 == 1)
                            {
                                lllllllllllllllllllllllllllllll3 = 0;
                            }
                            else
                            {
                                lllllllllllllllllllllllllllllll3 = 1;
                            }
                            lllllllllllllllllllllllllllllll3 = 1 - lllllllllllllllllllllllllllllll3;
                        }
                    }
                }
                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3 + 2] * 4 + 3;
                float llllll4 = 0;
                float lllllll4 = 0;
                float llllllll4 = lllllll4;
                bool lllllllll4 = distance(_PlayersPosVectorArray[lllllllllllllllll3].xyz, d.worldSpacePosition) > ll1;
                if ((lllllllllllllllllllllllllllllll3 != 0) || ((!lllllllll0 && !llllllllll0) && !lllllllll4))
                {
#if defined(_ZONING)
                            if(llllllllllllllllllll1) {
                                if(lllllllllllllllllllllll2) 
                                {
                                    if(llllllllllllllllllllll1) {
                                        float lllllllllllllllllll3 = lllllllllllllll3 + 1;
                                        for (int lllllllllllllllllllll6 = 0; lllllllllllllllllllll6 < _PlayersDataFloatArray[lllllllllllllll3]; lllllllllllllllllllll6++){
                                            float lllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 2];
                                            if (lllllllllllllllllllll3 != 0 && lllllllllllllllllllll3 == llllllllllllllllllllllll2) {
                                                float llllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 ];
                                                float lllllllllllllllllllllll3 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 1];
                                                llllll4 = 1;
                                                float llllllllllllll4 = _PlayersDataFloatArray[lllllllllllllllllll3 + lllllllllllllllllllll6 * llllllllllllllllllll3 + 3];
                                                if( lllllllllllllllllllllll3!= 0 && llllllllllllllllllllll3 != 0 && _Time.y-llllllllllllllllllllll3 < llllllllllllll4) {
                                                    if(lllllllllllllllllllllll3 == 1) {
                                                        llllll4 = ((llllllllllllll4-(_Time.y-llllllllllllllllllllll3))/llllllllllllll4);
                                                    } else {
                                                        llllll4 = ((_Time.y-llllllllllllllllllllll3)/llllllllllllll4);
                                                    }
                                                } else if(lllllllllllllllllllllll3 ==-1) {
                                                    llllll4 = 1;
                                                } else if(lllllllllllllllllllllll3 == 1) {
                                                    llllll4 = 0;
                                                } else {
                                                    llllll4 = 1;
                                                }
                                                llllll4 = 1 - llllll4;
                                            }
                                            if(lllllllllllllllllllll1 == 0 && llllllllllllllllllllll1) {
                                                float lllllllllllllll4 = 1 / lllllllllllllllllllllll1;
                                                if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)  {
                                                    float llllllllllllllll4 = ((lllllllllllllllllllllll1-lllllllllllllllllllllllll2) * lllllllllllllll4);
                                                    llllll4 =  max(llllll4,llllllllllllllll4);
                                                }
                                            }
                                        }
                                    } else { 
                                    }
                                } else {
                                }
                            }
#endif
                    if (dot(llllllllllllllllllll2, llllllllllllllllll3) <= 0)
                    {
                        if (llllllllllllllllllll0 == 2 || llllllllllllllllllll0 == 3 || llllllllllllllllllll0 == 4 || llllllllllllllllllll0 == 5 || llllllllllllllllllll0 == 6 || llllllllllllllllllll0 == 7)
                        {
                            float4 lllllllllllllllll4 = float4(0, 0, 0, 0);
                            float4 llllllllllllllllll4 = float4(0, 0, 0, 0);
                            float lllllllllllllllllll4 = 0;
                            if (lllllllllllllllllllllllllllllll0 || llllllllllllllllllll0 == 6)
                            {
                                float llllllllllllllllllll4 = _ScreenParams.x / _ScreenParams.y;
#ifdef _HDRP
                                        float4 lllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(GetCameraRelativePositionWS(_PlayersPosVectorArray[lllllllllllllllll3].xyz), 1.0));
                                        llllllllllllllllll4 = ComputeScreenPos(lllllllllllllllllllll4 , _ProjectionParams.x);
#else
                                float4 lllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(_PlayersPosVectorArray[lllllllllllllllll3].xyz, 1.0));
                                llllllllllllllllll4 = ComputeScreenPos(lllllllllllllllllllll4);
#endif
                                llllllllllllllllll4.xy /= llllllllllllllllll4.w;
                                llllllllllllllllll4.x *= llllllllllllllllllll4;
#ifdef _HDRP
                                        float4 lllllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(GetCameraRelativePositionWS(d.worldSpacePosition.xyz), 1.0));
                                        lllllllllllllllll4 = ComputeScreenPos(lllllllllllllllllllllll4 , _ProjectionParams.x);
#else
                                float4 lllllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(d.worldSpacePosition.xyz, 1.0));
                                lllllllllllllllll4 = ComputeScreenPos(lllllllllllllllllllllll4);
#endif
                                lllllllllllllllll4.xy /= lllllllllllllllll4.w;
                                lllllllllllllllll4.x *= llllllllllllllllllll4;
#if defined(_DISSOLVEMASK)
                                        if(lllllllllllllllllllllllllllllll0) {
                                                lllllllllllllllllll4 = max(lllllllll2.z,lllllllll2.w);
                                        }
#endif
                            }
                            float3 lllllllllllllllllllllllll4 = _WorldSpaceCameraPos - _PlayersPosVectorArray[lllllllllllllllll3].xyz;
                            float3 llllllllllllllllllllllllll4 = normalize(lllllllllllllllllllllllll4);
                            float lllllllllllllllllllllllllll4 = dot(d.worldSpacePosition.xyz - _PlayersPosVectorArray[lllllllllllllllll3].xyz, llllllllllllllllllllllllll4);
                            float llllllllllllllllllllllllllll4 = 0;
                            float lllllllllllllllllllllllllllll4 = 0;
                            float2 llllllllllllllllllllllllllllll4 = float2(0, 0);
                            if (llllllllllllllllllll0 == 2 || llllllllllllllllllll0 == 3)
                            {
                                llllllllllllllllllllllllllll4 = llllllllllllllllllllll0;
                                float lllllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz - _PlayersPosVectorArray[lllllllllllllllll3].xyz) - lllllllllllllllllllllllllll4 * llllllllllllllllllllllllll4);
                                float l5 = length(lllllllllllllllllllllllll4);
                                float ll5 = lllllllllllllllllllllll0;
                                float lll5 = (lllllllllllllllllllllllllll4 / l5) * ll5;
#if _DISSOLVEMASK
                                        float llll5 = (2*lll5) / lllllllllllllllllll4;
                                        float2 lllll5 = lllllllllllllllll4.xy - llllllllllllllllll4.xy;
                                        lllll5 =  normalize(lllll5)*lllllllllllllllllllllllllllllll4;
                                        llllllllllllllllllllllllllllll4 = lllll5 /llll5;
#else
                                float llllll5 = lllllllllllllllllllllllllllllll4 < lll5;
                                if (llllll5)
                                {
                                    float lllllll5 = lllllllllllllllllllllllllllllll4 / lll5;
                                    lllllllllllllllllllllllllllll4 = lllllll5;
                                }
                                else
                                {
                                    lllllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 4 || llllllllllllllllllll0 == 5)
                            {
                                llllllllllllllllllllllllllll4 = llllllllllllllllllllllll0;
                                float lllllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz - _PlayersPosVectorArray[lllllllllllllllll3].xyz) - lllllllllllllllllllllllllll4 * llllllllllllllllllllllllll4);
                                float lllllllll5 = lllllllllllllllllllllllll0;
                                float llllllllll5 = (lllllllllllllllllllllllllllllll4 < lllllllll5) && lllllllllllllllllllllllllll4 > 0;
#if _DISSOLVEMASK
                                        float llll5 = (2*lllllllll5) / lllllllllllllllllll4;
                                        float2 lllll5 = lllllllllllllllll4.xy - llllllllllllllllll4.xy;
                                        lllll5 =  normalize(lllll5)*lllllllllllllllllllllllllllllll4;
                                        llllllllllllllllllllllllllllll4 = lllll5 /llll5;
#else
                                if (llllllllll5)
                                {
                                    float lllllll5 = lllllllllllllllllllllllllllllll4 / lllllllll5;
                                    lllllllllllllllllllllllllllll4 = lllllll5;
                                }
                                else
                                {
                                    lllllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 6)
                            {
                                llllllllllllllllllllllllllll4 = llllllllllllllllllllllllll0;
                                float llllllllllllll5 = length(lllllllllllllllllllllllll4);
                                float llllllllllllllllllll4 = _ScreenParams.x / _ScreenParams.y;
                                float llllllllllllllll5 = min(1, llllllllllllllllllll4);
                                float lllllllllllllllll5 = distance(lllllllllllllllll4.xy, llllllllllllllllll4.xy) < lllllllllllllllllllllllllll0 / llllllllllllll5 * llllllllllllllll5;
                                float llllllllllllllllll5 = (lllllllllllllllll5) && lllllllllllllllllllllllllll4 > 0;
#if _DISSOLVEMASK
                                        float lllllllllllllllllll5 = lllllllllllllllllllllllllll0/llllllllllllll5*llllllllllllllll5;
                                        float llll5 = (2*lllllllllllllllllll5) / lllllllllllllllllll4;
                                        float2 lllll5 = lllllllllllllllll4.xy - llllllllllllllllll4.xy;
                                        llllllllllllllllllllllllllllll4 = lllll5 /llll5;
#else
                                if (llllllllllllllllll5)
                                {
                                    float llllllllllllllllllllll5 = (distance(lllllllllllllllll4.xy, llllllllllllllllll4.xy) / (lllllllllllllllllllllllllll0 / llllllllllllll5 * llllllllllllllll5));
                                    lllllllllllllllllllllllllllll4 = llllllllllllllllllllll5;
                                }
                                else
                                {
                                    lllllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 7)
                            {
#if _OBSTRUCTION_CURVE
                                        llllllllllllllllllllllllllll4 = llllllllllllllllllllllllllll0;
                                        float lllllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz  - _PlayersPosVectorArray[lllllllllllllllll3].xyz) - lllllllllllllllllllllllllll4 * llllllllllllllllllllllllll4);
                                        float llllllllllllll5 = length(lllllllllllllllllllllllll4);
                                        float4 lllllllllllllllllllllllll5 = float4(0,0,0,0);
                                        float llllllllllllllllllllllllll5 = llllllllll2.z;
                                        float lllllllllllllllllllllllllll5 = (lllllllllllllllllllllllllll4/llllllllllllll5) * llllllllllllllllllllllllll5;
                                        float4 llllllllllllllllllllllllllll5 = float4(0,0,0,0);
                                        llllllllllllllllllllllllllll5 = llllllllll2;
                                        float2 lllllllllllllllllllllllllllll5 = (lllllllllllllllllllllllllll5+0.5) * llllllllllllllllllllllllllll5.xy;
                                            lllllllllllllllllllllllll5 = tex2D(llllllllllllllllllllllllllllll1, lllllllllllllllllllllllllllll5);
                                        float llllllllllllllllllllllllllllll5 = lllllllllllllllllllllllll5.r * lllllllllllllllllllllllllllll0;
                                        float lllllllllllllllllllllllllllllll5 = (lllllllllllllllllllllllllllllll4 < llllllllllllllllllllllllllllll5) && lllllllllllllllllllllllllll4 > 0 ;
#if _DISSOLVEMASK
                                            float llll5 = (2*llllllllllllllllllllllllllllll5) / lllllllllllllllllll4;
                                            float2 lllll5 = lllllllllllllllll4.xy - llllllllllllllllll4.xy;
                                            lllll5 =  normalize(lllll5)*lllllllllllllllllllllllllllllll4;
                                            llllllllllllllllllllllllllllll4 = lllll5 /llll5;
#else
                                            if(lllllllllllllllllllllllllllllll5){
                                                float lllllll5 = lllllllllllllllllllllllllllllll4/llllllllllllllllllllllllllllll5;
                                                lllllllllllllllllllllllllllll4 = lllllll5;
                                            } else {
                                                lllllllllllllllllllllllllllll4 = -1;
                                            }
#endif
#endif
                            }
#if defined(_DISSOLVEMASK)
                                    if(lllllllllllllllllllllllllllllll0) {
                                        float4 llll6 = float4(0,0,0,0);
                                        llll6 = lllllllll2;
                                        float2 lllll6 = float2(llll6.z/2,llll6.w/2);
                                        float2 llllll6 = lllll6 + llllllllllllllllllllllllllllll4;
                                        float2 lllllll6 = (llllll6+0.5) * llll6.xy;
                                        float4 llllllll6 = float4(0,0,0,0);
                                            llllllll6 = tex2D(lllllllllllllllllllllllllllll1, lllllll6);
                                        float lllllllll6 = -1;
                                        if(llllll6.x <= llll6.z && llllll6.x >= 0 && llllll6.y <= llll6.w && llllll6.y >= 0 && llllllll6.x <= 0 && lllllllllllllllllllllllllll4 > 0 ){
                                            float llllllllll6 = sqrt(pow(llll6.z,2)+pow(llll6.w,2))/2;
                                            float lllllllllll6 = 40;
                                            float llllllllllll6 = llllllllll6/lllllllllll6;
                                            float lllllllllllll6 = 0;
                                            lllllllll6 = 0;     
                                                for (int i = 0; i < lllllllllll6; i++){
                                                    float2 llllllllllllll6 = lllll6 + (llllllllllllllllllllllllllllll4 + ( normalize(llllllllllllllllllllllllllllll4)*llllllllllll6*i));
                                                    float2 lllllllllllllll6 = (llllllllllllll6+0.5) * llll6.xy;
                                                    float4 llllllllllllllll6 = tex2Dlod(lllllllllllllllllllllllllllll1, float4(lllllllllllllll6, 0.0, 0.0)); 
                                                    float2 lllllllllllllllll6 = step(float2(0,0), llllllllllllll6) - step(float2(llll6.z,llll6.w), llllllllllllll6);
                                                    if(llllllllllllllll6.x <= 0) {
                                                        lllllllllllll6 +=  (1/lllllllllll6) * (lllllllllllllllll6.x * lllllllllllllllll6.y);
                                                    }                                            
                                                }   
                                            lllllllll6 = 1-lllllllllllll6;  
                                        }         
                                        lllllllllllllllllllllllllllll4 = lllllllll6;
                                    }
#endif
                            if (llllllllllllllllllllllllllllll0 <= 1)
                            {
                                if (lllllllllllllllllllllllllllll4 != -1)
                                {
                                    float llllllllllllllllll6 = max(llllllllllllllllllllllllllllll0, 0.00001);
                                    float lllllllllllllllllll6 = 1 - llllllllllllllllllllllllllll4;
                                    float llllllllllllllllllll6 = exp(llllllllllllllllll6 * 6);
                                    float lllllllllllllllllllll6 = lllllllllllllllllllllllllllll4;
                                    float llllllllllllllllllllll6 = lllllllllllllllllll6 / (llllllllllllllllll6 / (llllllllllllllllll6 * lllllllllllllllllll6 - 0.15 * (llllllllllllllllll6 - lllllllllllllllllll6)));
                                    float lllllllllllllllllllllll6 = ((lllllllllllllllllllll6 - llllllllllllllllllllll6) / (llllllllllllllllllll6 * (1 - lllllllllllllllllllll6) + lllllllllllllllllllll6)) + llllllllllllllllllllll6;
                                    lllllllllllllllllllllll6 = 1 - lllllllllllllllllllllll6;
                                    lllllll4 = lllllllllllllllllllllll6 * sign(llllllllllllllllllllllllllll4);
                                }
                            }
                            else
                            {
                                lllllll4 = lllllllllllllllllllllllllllll4;
                            }
                        }
                        if (llllllllllllllllllll0 == 1 || llllllllllllllllllll0 == 3 || llllllllllllllllllll0 == 5)
                        {
                            float llllllllllllllllllllllll6 = distance(_WorldSpaceCameraPos, _PlayersPosVectorArray[lllllllllllllllll3].xyz);
                            float lllllllllllllllllllllllll6 = distance(_WorldSpaceCameraPos, d.worldSpacePosition.xyz);
                            float3 llllllllllllllllllllllllll6 = d.worldSpacePosition.xyz - _PlayersPosVectorArray[lllllllllllllllll3].xyz;
                            float3 lllllllllllllllllllllllllll6 = d.worldSpaceNormal;
                            float llllllllllllllllllllllllllll6 = acos(dot(llllllllllllllllllllllllll6, lllllllllllllllllllllllllll6) / (length(llllllllllllllllllllllllll6) * length(lllllllllllllllllllllllllll6)));
                            if (llllllllllllllllllllllllllll6 <= 1.5 && llllllllllllllllllllllll6 > lllllllllllllllllllllllll6)
                            {
                                float lllllllllllllllllllllllllllll6 = (sqrt((llllllllllllllllllllllll6 - lllllllllllllllllllllllll6)) * 25 / llllllllllllllllllllllllllll6) * lllllllllllllllllllll0;
                                lllllll4 += max(0, log(lllllllllllllllllllllllllllll6 * 0.2));
                            }
                        }
                    }
                    lllllll4 = min(lllllll4 + (1 * l1), 1);
                    if (lllllllllllllllllllllll2)
                    {
                        if (lllllllllllllllllllll1 == 1)
                        {
                            float lllllllllllllll4 = 1 / lllllllllllllllllllllll1;
                            if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                            {
                                float lllllllllllllllllllllllllllllll6 = 1 - ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * lllllllllllllll4);
                                lllllll4 = min(lllllll4, lllllllllllllllllllllllllllllll6);
                            }
                        }
                        else if (lllllllllllllllllllll1 == 0 && !llllllllllllllllllllll1)
                        {
                            float lllllllllllllll4 = lllllll4 / lllllllllllllllllllllll1;
                            if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                            {
                                float lllllllllllllllllllllllllllllll6 = ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * lllllllllllllll4);
                                lllllll4 = max(0, lllllllllllllllllllllllllllllll6);
                            }
                            else
                            {
                                lllllll4 = 0;
                            }
                        }
                    }
                    float lll7 = lllllll4 / lllll1;
                    if (lll1)
                    {
                        float3 llllllllllllllllll3 = _PlayersPosVectorArray[lllllllllllllllll3].xyz - _WorldSpaceCameraPos;
                        float3 lllll7 = d.worldSpacePosition.xyz - _WorldSpaceCameraPos;
                        float llllll7 = dot(lllll7, normalize(llllllllllllllllll3));
                        if (llllll7 - llll1 >= length(llllllllllllllllll3))
                        {
                            float lllllll7 = llllll7 - llll1 - length(llllllllllllllllll3);
                            if (lllllll7 < 0)
                            {
                                lllllll7 = 0;
                            }
                            if (lllllll7 < lllll1)
                            {
                                lllllll4 = (lllll1 - lllllll7) * lll7;
                            }
                            else
                            {
                                lllllll4 = 0;
                            }
                        }
                    }
                    if (llllllllllllllllllll1 && !lllllllllllllllllllllll2)
                    {
                        if (lllllllllllllllllllll1 == 1)
                        {
                            lllllll4 = 0;
                        }
                    }
                    if (llllll1 == 1)
                    {
                        float llllllll7 = 0;
                        if (llllllll1 == 0)
                        {
                            llllllll7 = (lllllll4) / lllllllllll1;
                        }
                        else if (llllllll1 == 1)
                        {
                            float lllllllll7 = 1 - lllllll4;
                            if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                            {
                                lllllllll7 = max(1 - lllllll4, 1 - (lllllll4 * llllll4));
                            }
                            llllllll7 = lllllllll7 / lllllllllll1;
                        }
                        if (lllllll1 == 1)
                        {
                            if (d.worldSpacePosition.y > (_PlayersPosVectorArray[lllllllllllllllll3].y + llllllllll1))
                            {
                                float lllllll7 = d.worldSpacePosition.y - (_PlayersPosVectorArray[lllllllllllllllll3].y + llllllllll1);
                                if (lllllll7 < 0)
                                {
                                    lllllll7 = 0;
                                }
                                if (llllllll1 == 0)
                                {
                                    if (lllllll7 < lllllllllll1)
                                    {
                                        lllllll4 = ((lllllllllll1 - lllllll7) * llllllll7);
                                    }
                                    else
                                    {
                                        lllllll4 = 0;
                                    }
                                }
                                else
                                {
                                    if (lllllll7 < lllllllllll1)
                                    {
                                        lllllll4 = 1 - ((lllllllllll1 - lllllll7) * llllllll7);
                                    }
                                    else
                                    {
                                        lllllll4 = 1;
                                    }
                                    llllll4 = 1;
                                }
                            }
                        }
                        else
                        {
                            if (d.worldSpacePosition.y > lllllllll1)
                            {
                                float lllllll7 = d.worldSpacePosition.y - lllllllll1;
                                if (lllllll7 < 0)
                                {
                                    lllllll7 = 0;
                                }
                                if (llllllll1 == 0)
                                {
                                    if (lllllll7 < lllllllllll1)
                                    {
                                        lllllll4 = ((lllllllllll1 - lllllll7) * llllllll7);
                                    }
                                    else
                                    {
                                        lllllll4 = 0;
                                    }
                                }
                                else
                                {
                                    if (lllllll7 < lllllllllll1)
                                    {
                                        lllllll4 = 1 - ((lllllllllll1 - lllllll7) * llllllll7);
                                    }
                                    else
                                    {
                                        lllllll4 = 1;
                                    }
                                    llllll4 = 1;
                                }
                            }
                        }
                    }
                    if (llllllllllll1 == 1)
                    {
                        float llllllllllll7 = lllllll4 / llllllllllllllll1;
                        if (lllllllllllll1 == 1)
                        {
                            if (d.worldSpacePosition.y < (_PlayersPosVectorArray[lllllllllllllllll3].y + lllllllllllllll1))
                            {
                                float lllllll7 = (_PlayersPosVectorArray[lllllllllllllllll3].y + lllllllllllllll1) - d.worldSpacePosition.y;
                                if (lllllll7 < 0)
                                {
                                    lllllll7 = 0;
                                }
                                if (lllllll7 < llllllllllllllll1)
                                {
                                    lllllll4 = (llllllllllllllll1 - lllllll7) * llllllllllll7;
                                }
                                else
                                {
                                    lllllll4 = 0;
                                }
                            }
                        }
                        else
                        {
                            if (d.worldSpacePosition.y < llllllllllllll1)
                            {
                                float lllllll7 = llllllllllllll1 - d.worldSpacePosition.y;
                                if (lllllll7 < 0)
                                {
                                    lllllll7 = 0;
                                }
                                if (lllllll7 < llllllllllllllll1)
                                {
                                    lllllll4 = (llllllllllllllll1 - lllllll7) * llllllllllll7;
                                }
                                else
                                {
                                    lllllll4 = 0;
                                }
                            }
                        }
                    }
                    if (!lllllllll0 && !llllllllll0)
                    {
                        if (distance(_PlayersPosVectorArray[lllllllllllllllll3].xyz, d.worldSpacePosition) > ll1)
                        {
                            lllllll4 = 0;
                        }
                    }
                }
                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3] * 4 + 1;
                if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                {
                    lllllllllllllllllllllllllllllll3 = lllllllllllllllllllllllllllllll3 * llllll4;
                }
                if (lllllllll0 || llllllllll0)
                {
                    lllllll4 = lllllllllllllllllllllllllllllll3 * lllllll4;
                }
                else
                {
                    lllllll4 = lllllll4;
                    if (llllllllllllllllllll1)
                    {
                        if (lllllllllllllllllllllll2)
                        {
                            if (llllllllllllllllllllll1)
                            {
                                lllllll4 = llllll4 * lllllll4;
                            }
                        }
                        else
                        {
                            if (lllllllllllllllllllll1 == 1)
                            {
                                lllllll4 = llllll4 * lllllll4;
                            }
                        }
                    }
                }
                lllllllllllllllllllll2 = max(lllllllllllllllllllll2, lllllll4);
            }
            else
            {
                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3 + 2] * 4 + 3;
                lllllllllllllll3 = lllllllllllllll3 + _PlayersDataFloatArray[lllllllllllllll3] * 4 + 1;
            }
            }
#else
        float lllllllllllllllllllllllllllllll3 = 0;
        if (!llllll2)
        {
            lllllllllllllllllllllllllllllll3 = 1;
            if (llllll0 != 0 && lllllll0 != 0 && _Time.y - lllllll0 < lllllllllllllllllll1)
            {
                if (llllll0 == 1)
                {
                    lllllllllllllllllllllllllllllll3 = ((lllllllllllllllllll1 - (_Time.y - lllllll0)) / lllllllllllllllllll1);
                }
                else
                {
                    lllllllllllllllllllllllllllllll3 = ((_Time.y - lllllll0) / lllllllllllllllllll1);
                }
            }
            else if (llllll0 == -1)
            {
                lllllllllllllllllllllllllllllll3 = 1;
            }
            else if (llllll0 == 1)
            {
                lllllllllllllllllllllllllllllll3 = 0;
            }
            else
            {
                lllllllllllllllllllllllllllllll3 = 1;
            }
            lllllllllllllllllllllllllllllll3 = 1 - lllllllllllllllllllllllllllllll3;
        }
        float lllllll4 = 0;
        float llllll4 = 0;
        bool lllllllll4 = distance(_WorldSpaceCameraPos, d.worldSpacePosition) > ll1;
        lllllllll4 = false;
        if ((lllllllllllllllllllllllllllllll3 != 0) || ((!lllllllll0 && !llllllllll0) && !lllllllll4))
        {
#if defined(_ZONING)
                        if(llllllllllllllllllll1) {
                            if(lllllllllllllllllllllll2) 
                            {
                                if(llllllllllllllllllllll1) {
                                    float llllllllllllllllllllll3 = lllllllllllllllllllllllllll2;
                                    float lllllllllllllllllllllll3 = lllllllllllllllllllllllllllll2;
                                    llllll4 = 1;
                                    float llllllllllllll4 = llllllllllllllllllllllllllll2;
                                    if( lllllllllllllllllllllll3!= 0 && llllllllllllllllllllll3 != 0 && _Time.y-llllllllllllllllllllll3 < llllllllllllll4) {
                                        if(lllllllllllllllllllllll3 == 1) {
                                            llllll4 = ((llllllllllllll4-(_Time.y-llllllllllllllllllllll3))/llllllllllllll4);
                                        } else {
                                            llllll4 = ((_Time.y-llllllllllllllllllllll3)/llllllllllllll4);
                                        }
                                    } else if(lllllllllllllllllllllll3 ==-1) {
                                        llllll4 = 1;
                                    } else if(lllllllllllllllllllllll3 == 1) {
                                        llllll4 = 0;
                                    } else {
                                        llllll4 = 1;
                                    }
                                    llllll4 = 1 - llllll4;
                                    if(lllllllllllllllllllll1 == 0 && llllllllllllllllllllll1) {
                                        float lllllllllllllll4 = 1 / lllllllllllllllllllllll1;
                                        if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)  {
                                            float llllllllllllllll4 = ((lllllllllllllllllllllll1-lllllllllllllllllllllllll2) * lllllllllllllll4);
                                            llllll4 =  max(llllll4,llllllllllllllll4);
                                        }
                                    }
                                } else { 
                                }
                            } else {
                            }
                        }
#endif
            lllllll4 = min(lllllll4 + (1 * l1), 1);
            if (lllllllllllllllllllllll2)
            {
                if (lllllllllllllllllllll1 == 1)
                {
                    float lllllllllllllll4 = 1 / lllllllllllllllllllllll1;
                    if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                    {
                        float lllllllllllllllllllllllllllllll6 = 1 - ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * lllllllllllllll4);
                        lllllll4 = min(lllllll4, lllllllllllllllllllllllllllllll6);
                    }
                }
                else if (lllllllllllllllllllll1 == 0 && !llllllllllllllllllllll1)
                {
                    float lllllllllllllll4 = lllllll4 / lllllllllllllllllllllll1;
                    if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                    {
                        float lllllllllllllllllllllllllllllll6 = ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * lllllllllllllll4);
                        lllllll4 = max(0, lllllllllllllllllllllllllllllll6);
                    }
                    else
                    {
                        lllllll4 = 0;
                    }
                }
            }
            if (llllllllllllllllllll1 && !lllllllllllllllllllllll2)
            {
                if (lllllllllllllllllllll1 == 1)
                {
                    lllllll4 = 0;
                }
            }
            if (llllll1 == 1 && lllllll1 == 0)
            {
                float llllllll7 = 0;
                if (llllllll1 == 0)
                {
                    llllllll7 = (lllllll4) / lllllllllll1;
                }
                else if (llllllll1 == 1)
                {
                    float lllllllll7 = 1 - lllllll4;
                    if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                    {
                        lllllllll7 = max(1 - lllllll4, 1 - (lllllll4 * llllll4));
                    }
                    llllllll7 = lllllllll7 / lllllllllll1;
                }
                if (d.worldSpacePosition.y > lllllllll1)
                {
                    float lllllll7 = d.worldSpacePosition.y - lllllllll1;
                    if (lllllll7 < 0)
                    {
                        lllllll7 = 0;
                    }
                    if (llllllll1 == 0)
                    {
                        if (lllllll7 < lllllllllll1)
                        {
                            lllllll4 = ((lllllllllll1 - lllllll7) * llllllll7);
                        }
                        else
                        {
                            lllllll4 = 0;
                        }
                    }
                    else
                    {
                        if (lllllll7 < lllllllllll1)
                        {
                            lllllll4 = 1 - ((lllllllllll1 - lllllll7) * llllllll7);
                        }
                        else
                        {
                            lllllll4 = 1;
                        }
                        llllll4 = 1;
                    }
                }
            }
            if (llllllllllll1 == 1 && lllllllllllll1 == 0)
            {
                float llllllllllll7 = lllllll4 / llllllllllllllll1;
                if (d.worldSpacePosition.y < llllllllllllll1)
                {
                    float lllllll7 = llllllllllllll1 - d.worldSpacePosition.y;
                    if (lllllll7 < 0)
                    {
                        lllllll7 = 0;
                    }
                    if (lllllll7 < llllllllllllllll1)
                    {
                        lllllll4 = (llllllllllllllll1 - lllllll7) * llllllllllll7;
                    }
                    else
                    {
                        lllllll4 = 0;
                    }
                }
            }
        }
        if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
        {
            lllllllllllllllllllllllllllllll3 = lllllllllllllllllllllllllllllll3 * llllll4;
        }
        if (lllllllll0 || llllllllll0)
        {
            lllllll4 = lllllllllllllllllllllllllllllll3 * lllllll4;
        }
        else
        {
            lllllll4 = lllllll4;
            if (llllllllllllllllllll1)
            {
                if (lllllllllllllllllllllll2)
                {
                    if (llllllllllllllllllllll1)
                    {
                        lllllll4 = llllll4 * lllllll4;
                    }
                }
                else
                {
                    if (lllllllllllllllllllll1 == 1)
                    {
                        lllllll4 = llllll4 * lllllll4;
                    }
                }
            }
        }
        lllllllllllllllllllll2 = max(lllllllllllllllllllll2, lllllll4);
#endif
            float llllllll4 = lllllllllllllllllllll2;
            if (!llllllllllllllllllllllllll1)
            {
                if (llllllll4 == 1)
                {
                    llllllll4 = 10;
                }
                if (!lllllllllllllllllll0 || llllllllllllllllllll0 == 6)
                {
#if defined(UNITY_PASS_SHADOWCASTER) 
#if defined(SHADOWS_DEPTH) 
                if (!any(unity_LightShadowBias))
                {
#if !defined(NO_STS_CLIPPING)
                        clip(lllllllllllllllllll2 - llllllll4);
#endif
                    llll2 = lllllllllllllllllll2 - llllllll4;
                }
                else
                {
                    if(lllllllllllllllllll0 && llllllllllllllllllll0 != 6) {
#if !defined(NO_STS_CLIPPING)
                        clip(lllllllllllllllllll2 - llllllll4);
#endif
                        llll2 = lllllllllllllllllll2 - llllllll4;
                    }
                }
#endif
#else
#if !defined(NO_STS_CLIPPING)
                    clip(lllllllllllllllllll2 - llllllll4);
#endif
                    llll2 = lllllllllllllllllll2 - llllllll4;
#endif
                }
                else
                {
#if !defined(NO_STS_CLIPPING)
                    clip(lllllllllllllllllll2 - llllllll4);
#endif
                    llll2 = lllllllllllllllllll2 - llllllll4;
                }
                if (lllllllllllllllllll2 - llllllll4 < 0)
                {
                    llll2 = 0;
                }
                else
                {
                    llll2 = 1;
                }
            }
            if (llllllllllllllllllllllllll1)
            {
                lllllll2 = 1;
                if ((lllllllllllllllllll2 - llllllll4) < 0)
                {
                    llllllll2 = half4(1, 1, 1, 1);
                    o.Emission = 1;
                }
                else
                {
                    llllllll2 = half4(0, 0, 0, 1);
                }
                if (llllllllllllll3)
                {
                    if ((lllllllllllllllllll2 - llllllll4) < 0)
                    {
                        llllllll2 = half4(0.5, 1, 0.5, 1);
                        o.Emission = 0;
                    }
                    else
                    {
                        llllllll2 = half4(0, 0.1, 0, 1);
                    }
                }
                if (lllllllllllllllllllllll2 && llllllllllll1 == 1 && llllllllllllllllllllllll1)
                {
                    float lll8 = 0;
                    if (lllllllllllll1 == 1)
                    {
                        lllllllllllll3 = lllllllllllll3 + lllllllllllllllllllllllll1;
                        lll8 = lllllllllllll3;
                    }
                    else
                    {
                        lll8 = llllllllllllll1 + lllllllllllllllllllllllll1;
                    }
                    if (d.worldSpacePosition.y > (lll8 - lllllllllllllllllllllllllll1) && d.worldSpacePosition.y < (lll8 + lllllllllllllllllllllllllll1))
                    {
                        llllllll2 = half4(1, 0, 0, 1);
                    }
                }
            }
            else
            {
                half3 llll8 = lerp(1, llllllllllll0, lllllllllllll0).rgb;
                if (lllllllllllllllll0)
                {
                    llllllllllllllllll0 = 0.2 + (llllllllllllllllll0 * (0.8 - 0.2));
                    o.Emission = o.Emission + min(clamp(llll8 * clamp(((llllllll4 / llllllllllllllllll0) - lllllllllllllllllll2), 0, 1), 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0), clamp(llll8 * llllllll4, 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0));
                }
                else
                {
                    o.Emission = o.Emission + clamp(llll8 * llllllll4, 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0);
                }
            }
        }
        if (lllllll2)
        {
            o.Albedo = llllllll2.rgb;
        }
        ll2 = o.Albedo;
        lll2 = o.Emission;
        #ifdef _HDRP  
            float lllll8 = 0;
            float llllll8 = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                llllll8 =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                llllll8 = _ProbeExposureScale;
            #endif
                float lllllll8 = 0;
                float llllllll8 = llllll8;
                lllllll8 = rcp(llllllll8 + (llllllll8 == 0.0));
                float3 lllllllll8 = o.Emission * lllllll8;
                o.Emission = lerp(lllllllll8, o.Emission, lllll8);
            lll2 = o.Emission;
        #endif

}

#endif
