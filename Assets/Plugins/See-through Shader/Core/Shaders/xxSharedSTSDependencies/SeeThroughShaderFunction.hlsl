#ifndef SEETHROUGHSHADER_FUNCTION
#define SEETHROUGHSHADER_FUNCTION
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
                llllllllllllllllllll2 =  mul(UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V)) [2].xyz);
#else
        llllllllllllllllllll2 = mul(UNITY_MATRIX_M, transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))[2].xyz);
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
                                lllllllllllllllllllllllll2 = min(llllllllll3,lllllllll3);
                                lllllllllllllllllllllllll2 = min(lllllllllllllllllllllllll2,lllllllllll3);
                                if(lllllllllllllllllllllllll2<0) {
                                    lllllllllllllllllllllllll2 = 0;
                                }
                            }
                        }
                        if(lllllllllllllllllllllllllllllll2) {
                            llllllllllllllllllllllll2 = _ZDFA[l3];
                            lllllllllllllllllllllll2 = true;    
                            lllllllllllllllllllllllllll2 = _ZDFA[l3 + 17];
                            lllllllllllllllllllllllllllll2 = _ZDFA[l3 + 18];
                            llllllllllllllllllllllllllll2 = _ZDFA[l3 + 19];
                            break;                        
                        }
                    }
                }
#endif
            float llllllllllll3 = 0;
            float lllllllllllll3 = lllllllllllllllllllllll2;
#if !defined(_PLAYERINDEPENDENT)
#if defined(_ZONING)
                    if(lllllllllllllllllllllll2 && llllllllllll1 == 1 && lllllllllllll1 == 1 && llllllllllllllllllllllll1) {
                        float llllllllllllll3 = 0;
                        bool lllllllllllllll3 = false;
                        for (int i = 0; i < _ArrayLength; i++){
                            float llllllllllllllll3 = _PlayersDataFloatArray[llllllllllllll3+1]; 
                            float3 lllllllllllllllll3 = _PlayersPosVectorArray[llllllllllllllll3].xyz - _WorldSpaceCameraPos;               
                            if(dot(llllllllllllllllllll2,lllllllllllllllll3) <= 0) {       
                                if(!llllll2) {
                                    float llllllllllllllllll3 = llllllllllllll3 + 3;
                                    float lllllllllllllllllll3 = 4;
                                    for (int llllllllllllllllllll6 = 0; llllllllllllllllllll6 < _PlayersDataFloatArray[llllllllllllll3 + 2]; llllllllllllllllllll6++){
                                        float llllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 2];
                                        if (llllllllllllllllllll3 != 0 && llllllllllllllllllll3 == llllllll0) {
                                            float lllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 ];
                                            float llllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 1];
                                            if ((llllllllllllllllllllll3 == -1 && _Time.y - lllllllllllllllllllll3 < lllllllllllllllllll1 )|| (llllllllllllllllllllll3 == 1) ) {
                                                float lllllllllllllllllllllll3 = _PlayersPosVectorArray[llllllllllllllll3].y+ lllllllllllllll1;
                                                if(llllllllllllllllllllllllll1) {
                                                    if(i==0) {
                                                        llllllllllll3 = lllllllllllllllllllllll3;
                                                    } else {
                                                        llllllllllll3 = max(llllllllllll3,lllllllllllllllllllllll3);
                                                    }
                                                }
                                                bool llllllllllllllllllllllll3 = llllllllllllllllllllllllll2 >= lllllllllllllllllllllll3 + lllllllllllllllllllllllll1; 
                                                if(!llllllllllllllllllllllll3) {
                                                    lllllllllllllll3 = true;
                                                } 
                                            }                        
                                        }
                                    }
                                } else if (distance(_PlayersPosVectorArray[llllllllllllllll3].xyz, d.worldSpacePosition.xyz) < ll1) {
                                    float lllllllllllllllllllllll3 = _PlayersPosVectorArray[llllllllllllllll3].y+ lllllllllllllll1;
                                    if(llllllllllllllllllllllllll1) {
                                        if(i==0) {
                                            llllllllllll3 = lllllllllllllllllllllll3;
                                        } else {
                                            llllllllllll3 = max(llllllllllll3,lllllllllllllllllllllll3);
                                        }
                                    }
                                    bool llllllllllllllllllllllll3 = llllllllllllllllllllllllll2 >= lllllllllllllllllllllll3 + lllllllllllllllllllllllll1; 
                                    if(!llllllllllllllllllllllll3) {
                                        lllllllllllllll3 = true;
                                    } 
                                }
                                llllllllllllll3 = llllllllllllll3 + _PlayersDataFloatArray[llllllllllllll3 + 2]*4 + 3; 
                                llllllllllllll3 = llllllllllllll3 + _PlayersDataFloatArray[llllllllllllll3]*4 + 1; 
                            }
                        }
                        if(!lllllllllllllll3) {
                            lllllllllllllllllllllll2 = false;
                        }
                    }
#endif
            float llllllllllllll3 = 0;
            for (int i = 0; i < _ArrayLength; i++)
            {
                float llllllllllllllll3 = _PlayersDataFloatArray[llllllllllllll3 + 1];
                float3 lllllllllllllllll3 = _PlayersPosVectorArray[llllllllllllllll3].xyz - _WorldSpaceCameraPos;
                float llllllllllllllllllllllllllllll3 = 0;
                float lllllllllllllllllll3 = 4;
                if (!llllll2)
                {
                    float llllllllllllllllll3 = llllllllllllll3 + 3;
                    for (int llllllllllllllllllll6 = 0; llllllllllllllllllll6 < _PlayersDataFloatArray[llllllllllllll3 + 2]; llllllllllllllllllll6++)
                    {
                        float llllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 2];
                        if (llllllllllllllllllll3 != 0 && llllllllllllllllllll3 == llllllll0)
                        {
                            float lllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3];
                            float llllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 1];
                            llllllllllllllllllllllllllllll3 = 1;
                            if (llllllllllllllllllllll3 != 0 && lllllllllllllllllllll3 != 0 && _Time.y - lllllllllllllllllllll3 < lllllllllllllllllll1)
                            {
                                if (llllllllllllllllllllll3 == 1)
                                {
                                    llllllllllllllllllllllllllllll3 = ((lllllllllllllllllll1 - (_Time.y - lllllllllllllllllllll3)) / lllllllllllllllllll1);
                                }
                                else
                                {
                                    llllllllllllllllllllllllllllll3 = ((_Time.y - lllllllllllllllllllll3) / lllllllllllllllllll1);
                                }
                            }
                            else if (llllllllllllllllllllll3 == -1)
                            {
                                llllllllllllllllllllllllllllll3 = 1;
                            }
                            else if (llllllllllllllllllllll3 == 1)
                            {
                                llllllllllllllllllllllllllllll3 = 0;
                            }
                            else
                            {
                                llllllllllllllllllllllllllllll3 = 1;
                            }
                            llllllllllllllllllllllllllllll3 = 1 - llllllllllllllllllllllllllllll3;
                        }
                    }
                }
                llllllllllllll3 = llllllllllllll3 + _PlayersDataFloatArray[llllllllllllll3 + 2] * 4 + 3;
                float lllll4 = 0;
                float llllll4 = 0;
                float lllllll4 = llllll4;
                bool llllllll4 = distance(_PlayersPosVectorArray[llllllllllllllll3].xyz, d.worldSpacePosition) > ll1;
                if ((llllllllllllllllllllllllllllll3 != 0) || ((!lllllllll0 && !llllllllll0) && !llllllll4))
                {
#if defined(_ZONING)
                            if(llllllllllllllllllll1) {
                                if(lllllllllllllllllllllll2) 
                                {
                                    if(llllllllllllllllllllll1) {
                                        float llllllllllllllllll3 = llllllllllllll3 + 1;
                                        for (int llllllllllllllllllll6 = 0; llllllllllllllllllll6 < _PlayersDataFloatArray[llllllllllllll3]; llllllllllllllllllll6++){
                                            float llllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 2];
                                            if (llllllllllllllllllll3 != 0 && llllllllllllllllllll3 == llllllllllllllllllllllll2) {
                                                float lllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 ];
                                                float llllllllllllllllllllll3 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 1];
                                                lllll4 = 1;
                                                float lllllllllllll4 = _PlayersDataFloatArray[llllllllllllllllll3 + llllllllllllllllllll6 * lllllllllllllllllll3 + 3];
                                                if( llllllllllllllllllllll3!= 0 && lllllllllllllllllllll3 != 0 && _Time.y-lllllllllllllllllllll3 < lllllllllllll4) {
                                                    if(llllllllllllllllllllll3 == 1) {
                                                        lllll4 = ((lllllllllllll4-(_Time.y-lllllllllllllllllllll3))/lllllllllllll4);
                                                    } else {
                                                        lllll4 = ((_Time.y-lllllllllllllllllllll3)/lllllllllllll4);
                                                    }
                                                } else if(llllllllllllllllllllll3 ==-1) {
                                                    lllll4 = 1;
                                                } else if(llllllllllllllllllllll3 == 1) {
                                                    lllll4 = 0;
                                                } else {
                                                    lllll4 = 1;
                                                }
                                                lllll4 = 1 - lllll4;
                                            }
                                            if(lllllllllllllllllllll1 == 0 && llllllllllllllllllllll1) {
                                                float llllllllllllll4 = 1 / lllllllllllllllllllllll1;
                                                if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)  {
                                                    float lllllllllllllll4 = ((lllllllllllllllllllllll1-lllllllllllllllllllllllll2) * llllllllllllll4);
                                                    lllll4 =  max(lllll4,lllllllllllllll4);
                                                }
                                            }
                                        }
                                    } else { 
                                    }
                                } else {
                                }
                            }
#endif
                    if (dot(llllllllllllllllllll2, lllllllllllllllll3) <= 0)
                    {
                        if (llllllllllllllllllll0 == 2 || llllllllllllllllllll0 == 3 || llllllllllllllllllll0 == 4 || llllllllllllllllllll0 == 5 || llllllllllllllllllll0 == 6 || llllllllllllllllllll0 == 7)
                        {
                            float4 llllllllllllllll4 = float4(0, 0, 0, 0);
                            float4 lllllllllllllllll4 = float4(0, 0, 0, 0);
                            float llllllllllllllllll4 = 0;
                            if (lllllllllllllllllllllllllllllll0 || llllllllllllllllllll0 == 6)
                            {
                                float lllllllllllllllllll4 = _ScreenParams.x / _ScreenParams.y;
#ifdef _HDRP
                                        float4 llllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(GetCameraRelativePositionWS(_PlayersPosVectorArray[llllllllllllllll3].xyz), 1.0));
                                        lllllllllllllllll4 = ComputeScreenPos(llllllllllllllllllll4 , _ProjectionParams.x);
#else
                                float4 llllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(_PlayersPosVectorArray[llllllllllllllll3].xyz, 1.0));
                                lllllllllllllllll4 = ComputeScreenPos(llllllllllllllllllll4);
#endif
                                lllllllllllllllll4.xy /= lllllllllllllllll4.w;
                                lllllllllllllllll4.x *= lllllllllllllllllll4;
#ifdef _HDRP
                                        float4 llllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(GetCameraRelativePositionWS(d.worldSpacePosition.xyz), 1.0));
                                        llllllllllllllll4 = ComputeScreenPos(llllllllllllllllllllll4 , _ProjectionParams.x);
#else
                                float4 llllllllllllllllllllll4 = mul(UNITY_MATRIX_VP, float4(d.worldSpacePosition.xyz, 1.0));
                                llllllllllllllll4 = ComputeScreenPos(llllllllllllllllllllll4);
#endif
                                llllllllllllllll4.xy /= llllllllllllllll4.w;
                                llllllllllllllll4.x *= lllllllllllllllllll4;
#if defined(_DISSOLVEMASK)
                                        if(lllllllllllllllllllllllllllllll0) {
                                                llllllllllllllllll4 = max(lllllllll2.z,lllllllll2.w);
                                        }
#endif
                        }
                            float3 llllllllllllllllllllllll4 = _WorldSpaceCameraPos - _PlayersPosVectorArray[llllllllllllllll3].xyz;
                            float3 lllllllllllllllllllllllll4 = normalize(llllllllllllllllllllllll4);
                            float llllllllllllllllllllllllll4 = dot(d.worldSpacePosition.xyz - _PlayersPosVectorArray[llllllllllllllll3].xyz, lllllllllllllllllllllllll4);
                            float lllllllllllllllllllllllllll4 = 0;
                            float llllllllllllllllllllllllllll4 = 0;
                            float2 lllllllllllllllllllllllllllll4 = float2(0, 0);
                            if (llllllllllllllllllll0 == 2 || llllllllllllllllllll0 == 3)
                            {
                                lllllllllllllllllllllllllll4 = llllllllllllllllllllll0;
                                float llllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz - _PlayersPosVectorArray[llllllllllllllll3].xyz) - llllllllllllllllllllllllll4 * lllllllllllllllllllllllll4);
                                float lllllllllllllllllllllllllllllll4 = length(llllllllllllllllllllllll4);
                                float l5 = lllllllllllllllllllllll0;
                                float ll5 = (llllllllllllllllllllllllll4 / lllllllllllllllllllllllllllllll4) * l5;
#if _DISSOLVEMASK
                                        float lll5 = (2*ll5) / llllllllllllllllll4;
                                        float2 llll5 = llllllllllllllll4.xy - lllllllllllllllll4.xy;
                                        llll5 =  normalize(llll5)*llllllllllllllllllllllllllllll4;
                                        lllllllllllllllllllllllllllll4 = llll5 /lll5;
#else
                                float lllll5 = llllllllllllllllllllllllllllll4 < ll5;
                                if (lllll5)
                                {
                                    float llllll5 = llllllllllllllllllllllllllllll4 / ll5;
                                    llllllllllllllllllllllllllll4 = llllll5;
                                }
                                else
                                {
                                    llllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 4 || llllllllllllllllllll0 == 5)
                            {
                                lllllllllllllllllllllllllll4 = llllllllllllllllllllllll0;
                                float llllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz - _PlayersPosVectorArray[llllllllllllllll3].xyz) - llllllllllllllllllllllllll4 * lllllllllllllllllllllllll4);
                                float llllllll5 = lllllllllllllllllllllllll0;
                                float lllllllll5 = (llllllllllllllllllllllllllllll4 < llllllll5) && llllllllllllllllllllllllll4 > 0;
#if _DISSOLVEMASK
                                        float lll5 = (2*llllllll5) / llllllllllllllllll4;
                                        float2 llll5 = llllllllllllllll4.xy - lllllllllllllllll4.xy;
                                        llll5 =  normalize(llll5)*llllllllllllllllllllllllllllll4;
                                        lllllllllllllllllllllllllllll4 = llll5 /lll5;
#else
                                if (lllllllll5)
                                {
                                    float llllll5 = llllllllllllllllllllllllllllll4 / llllllll5;
                                    llllllllllllllllllllllllllll4 = llllll5;
                                }
                                else
                                {
                                    llllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 6)
                            {
                                lllllllllllllllllllllllllll4 = llllllllllllllllllllllllll0;
                                float lllllllllllll5 = length(llllllllllllllllllllllll4);
                                float lllllllllllllllllll4 = _ScreenParams.x / _ScreenParams.y;
                                float lllllllllllllll5 = min(1, lllllllllllllllllll4);
                                float llllllllllllllll5 = distance(llllllllllllllll4.xy, lllllllllllllllll4.xy) < lllllllllllllllllllllllllll0 / lllllllllllll5 * lllllllllllllll5;
                                float lllllllllllllllll5 = (llllllllllllllll5) && llllllllllllllllllllllllll4 > 0;
#if _DISSOLVEMASK
                                        float llllllllllllllllll5 = lllllllllllllllllllllllllll0/lllllllllllll5*lllllllllllllll5;
                                        float lll5 = (2*llllllllllllllllll5) / llllllllllllllllll4;
                                        float2 llll5 = llllllllllllllll4.xy - lllllllllllllllll4.xy;
                                        lllllllllllllllllllllllllllll4 = llll5 /lll5;
#else
                                if (lllllllllllllllll5)
                                {
                                    float lllllllllllllllllllll5 = (distance(llllllllllllllll4.xy, lllllllllllllllll4.xy) / (lllllllllllllllllllllllllll0 / lllllllllllll5 * lllllllllllllll5));
                                    llllllllllllllllllllllllllll4 = lllllllllllllllllllll5;
                                }
                                else
                                {
                                    llllllllllllllllllllllllllll4 = -1;
                                }
#endif
                            }
                            else if (llllllllllllllllllll0 == 7)
                            {
#if _OBSTRUCTION_CURVE
                                        lllllllllllllllllllllllllll4 = llllllllllllllllllllllllllll0;
                                        float llllllllllllllllllllllllllllll4 = length((d.worldSpacePosition.xyz  - _PlayersPosVectorArray[llllllllllllllll3].xyz) - llllllllllllllllllllllllll4 * lllllllllllllllllllllllll4);
                                        float lllllllllllll5 = length(llllllllllllllllllllllll4);
                                        float4 llllllllllllllllllllllll5 = float4(0,0,0,0);
                                        float lllllllllllllllllllllllll5 = llllllllll2.z;
                                        float llllllllllllllllllllllllll5 = (llllllllllllllllllllllllll4/lllllllllllll5) * lllllllllllllllllllllllll5;
                                        float4 lllllllllllllllllllllllllll5 = float4(0,0,0,0);
                                        lllllllllllllllllllllllllll5 = llllllllll2;
                                        float2 llllllllllllllllllllllllllll5 = (llllllllllllllllllllllllll5+0.5) * lllllllllllllllllllllllllll5.xy;
                                            llllllllllllllllllllllll5 = tex2D(llllllllllllllllllllllllllllll1, llllllllllllllllllllllllllll5);
                                        float lllllllllllllllllllllllllllll5 = llllllllllllllllllllllll5.r * lllllllllllllllllllllllllllll0;
                                        float llllllllllllllllllllllllllllll5 = (llllllllllllllllllllllllllllll4 < lllllllllllllllllllllllllllll5) && llllllllllllllllllllllllll4 > 0 ;
#if _DISSOLVEMASK
                                            float lll5 = (2*lllllllllllllllllllllllllllll5) / llllllllllllllllll4;
                                            float2 llll5 = llllllllllllllll4.xy - lllllllllllllllll4.xy;
                                            llll5 =  normalize(llll5)*llllllllllllllllllllllllllllll4;
                                            lllllllllllllllllllllllllllll4 = llll5 /lll5;
#else
                                            if(llllllllllllllllllllllllllllll5){
                                                float llllll5 = llllllllllllllllllllllllllllll4/lllllllllllllllllllllllllllll5;
                                                llllllllllllllllllllllllllll4 = llllll5;
                                            } else {
                                                llllllllllllllllllllllllllll4 = -1;
                                            }
#endif
#endif
                            }
#if defined(_DISSOLVEMASK)
                                    if(lllllllllllllllllllllllllllllll0) {
                                        float4 lll6 = float4(0,0,0,0);
                                        lll6 = lllllllll2;
                                        float2 llll6 = float2(lll6.z/2,lll6.w/2);
                                        float2 lllll6 = llll6 + lllllllllllllllllllllllllllll4;
                                        float2 llllll6 = (lllll6+0.5) * lll6.xy;
                                        float4 lllllll6 = float4(0,0,0,0);
                                            lllllll6 = tex2D(lllllllllllllllllllllllllllll1, llllll6);
                                        float llllllll6 = -1;
                                        if(lllll6.x <= lll6.z && lllll6.x >= 0 && lllll6.y <= lll6.w && lllll6.y >= 0 && lllllll6.x <= 0 && llllllllllllllllllllllllll4 > 0 ){
                                            float lllllllll6 = sqrt(pow(lll6.z,2)+pow(lll6.w,2))/2;
                                            float llllllllll6 = 40;
                                            float lllllllllll6 = lllllllll6/llllllllll6;
                                            float llllllllllll6 = 0;
                                            llllllll6 = 0;     
                                                for (int i = 0; i < llllllllll6; i++){
                                                    float2 lllllllllllll6 = llll6 + (lllllllllllllllllllllllllllll4 + ( normalize(lllllllllllllllllllllllllllll4)*lllllllllll6*i));
                                                    float2 llllllllllllll6 = (lllllllllllll6+0.5) * lll6.xy;
                                                    float4 lllllllllllllll6 = tex2Dlod(lllllllllllllllllllllllllllll1, float4(llllllllllllll6, 0.0, 0.0)); 
                                                    float2 llllllllllllllll6 = step(float2(0,0), lllllllllllll6) - step(float2(lll6.z,lll6.w), lllllllllllll6);
                                                    if(lllllllllllllll6.x <= 0) {
                                                        llllllllllll6 +=  (1/llllllllll6) * (llllllllllllllll6.x * llllllllllllllll6.y);
                                                    }                                            
                                                }   
                                            llllllll6 = 1-llllllllllll6;  
                                        }         
                                        llllllllllllllllllllllllllll4 = llllllll6;
                                    }
#endif
                            if (llllllllllllllllllllllllllllll0 <= 1)
                            {
                                if (llllllllllllllllllllllllllll4 != -1)
                                {
                                    float lllllllllllllllll6 = max(llllllllllllllllllllllllllllll0, 0.00001);
                                    float llllllllllllllllll6 = 1 - lllllllllllllllllllllllllll4;
                                    float lllllllllllllllllll6 = exp(lllllllllllllllll6 * 6);
                                    float llllllllllllllllllll6 = llllllllllllllllllllllllllll4;
                                    float lllllllllllllllllllll6 = llllllllllllllllll6 / (lllllllllllllllll6 / (lllllllllllllllll6 * llllllllllllllllll6 - 0.15 * (lllllllllllllllll6 - llllllllllllllllll6)));
                                    float llllllllllllllllllllll6 = ((llllllllllllllllllll6 - lllllllllllllllllllll6) / (lllllllllllllllllll6 * (1 - llllllllllllllllllll6) + llllllllllllllllllll6)) + lllllllllllllllllllll6;
                                    llllllllllllllllllllll6 = 1 - llllllllllllllllllllll6;
                                    llllll4 = llllllllllllllllllllll6 * sign(lllllllllllllllllllllllllll4);
                                }
                            }
                            else
                            {
                                llllll4 = llllllllllllllllllllllllllll4;
                            }
                        }
                        if (llllllllllllllllllll0 == 1 || llllllllllllllllllll0 == 3 || llllllllllllllllllll0 == 5)
                        {
                            float lllllllllllllllllllllll6 = distance(_WorldSpaceCameraPos, _PlayersPosVectorArray[llllllllllllllll3].xyz);
                            float llllllllllllllllllllllll6 = distance(_WorldSpaceCameraPos, d.worldSpacePosition.xyz);
                            float3 lllllllllllllllllllllllll6 = d.worldSpacePosition.xyz - _PlayersPosVectorArray[llllllllllllllll3].xyz;
                            float3 llllllllllllllllllllllllll6 = d.worldSpaceNormal;
                            float lllllllllllllllllllllllllll6 = acos(dot(lllllllllllllllllllllllll6, llllllllllllllllllllllllll6) / (length(lllllllllllllllllllllllll6) * length(llllllllllllllllllllllllll6)));
                            if (lllllllllllllllllllllllllll6 <= 1.5 && lllllllllllllllllllllll6 > llllllllllllllllllllllll6)
                            {
                                float llllllllllllllllllllllllllll6 = (sqrt((lllllllllllllllllllllll6 - llllllllllllllllllllllll6)) * 25 / lllllllllllllllllllllllllll6) * lllllllllllllllllllll0;
                                llllll4 += max(0, log(llllllllllllllllllllllllllll6 * 0.2));
                            }
                        }
                    }
                    llllll4 = min(llllll4 + (1 * l1), 1);
                    if (lllllllllllllllllllllll2)
                    {
                        if (lllllllllllllllllllll1 == 1)
                        {
                            float llllllllllllll4 = 1 / lllllllllllllllllllllll1;
                            if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                            {
                                float llllllllllllllllllllllllllllll6 = 1 - ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * llllllllllllll4);
                                llllll4 = min(llllll4, llllllllllllllllllllllllllllll6);
                            }
                        }
                        else if (lllllllllllllllllllll1 == 0 && !llllllllllllllllllllll1)
                        {
                            float llllllllllllll4 = llllll4 / lllllllllllllllllllllll1;
                            if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                            {
                                float llllllllllllllllllllllllllllll6 = ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * llllllllllllll4);
                                llllll4 = max(0, llllllllllllllllllllllllllllll6);
                            }
                            else
                            {
                                llllll4 = 0;
                            }
                        }
                    }
                    float ll7 = llllll4 / lllll1;
                    if (lll1)
                    {
                        float3 lllllllllllllllll3 = _PlayersPosVectorArray[llllllllllllllll3].xyz - _WorldSpaceCameraPos;
                        float3 llll7 = d.worldSpacePosition.xyz - _WorldSpaceCameraPos;
                        float lllll7 = dot(llll7, normalize(lllllllllllllllll3));
                        if (lllll7 - llll1 >= length(lllllllllllllllll3))
                        {
                            float llllll7 = lllll7 - llll1 - length(lllllllllllllllll3);
                            if (llllll7 < 0)
                            {
                                llllll7 = 0;
                            }
                            if (llllll7 < lllll1)
                            {
                                llllll4 = (lllll1 - llllll7) * ll7;
                            }
                            else
                            {
                                llllll4 = 0;
                            }
                        }
                    }
                    if (llllllllllllllllllll1 && !lllllllllllllllllllllll2)
                    {
                        if (lllllllllllllllllllll1 == 1)
                        {
                            llllll4 = 0;
                        }
                    }
                    if (llllll1 == 1)
                    {
                        float lllllll7 = 0;
                        if (llllllll1 == 0)
                        {
                            lllllll7 = (llllll4) / lllllllllll1;
                        }
                        else if (llllllll1 == 1)
                        {
                            float llllllll7 = 1 - llllll4;
                            if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                            {
                                llllllll7 = max(1 - llllll4, 1 - (llllll4 * lllll4));
                            }
                            lllllll7 = llllllll7 / lllllllllll1;
                        }
                        if (lllllll1 == 1)
                        {
                            if (d.worldSpacePosition.y > (_PlayersPosVectorArray[llllllllllllllll3].y + llllllllll1))
                            {
                                float llllll7 = d.worldSpacePosition.y - (_PlayersPosVectorArray[llllllllllllllll3].y + llllllllll1);
                                if (llllll7 < 0)
                                {
                                    llllll7 = 0;
                                }
                                if (llllllll1 == 0)
                                {
                                    if (llllll7 < lllllllllll1)
                                    {
                                        llllll4 = ((lllllllllll1 - llllll7) * lllllll7);
                                    }
                                    else
                                    {
                                        llllll4 = 0;
                                    }
                                }
                                else
                                {
                                    if (llllll7 < lllllllllll1)
                                    {
                                        llllll4 = 1 - ((lllllllllll1 - llllll7) * lllllll7);
                                    }
                                    else
                                    {
                                        llllll4 = 1;
                                    }
                                    lllll4 = 1;
                                }
                            }
                        }
                        else
                        {
                            if (d.worldSpacePosition.y > lllllllll1)
                            {
                                float llllll7 = d.worldSpacePosition.y - lllllllll1;
                                if (llllll7 < 0)
                                {
                                    llllll7 = 0;
                                }
                                if (llllllll1 == 0)
                                {
                                    if (llllll7 < lllllllllll1)
                                    {
                                        llllll4 = ((lllllllllll1 - llllll7) * lllllll7);
                                    }
                                    else
                                    {
                                        llllll4 = 0;
                                    }
                                }
                                else
                                {
                                    if (llllll7 < lllllllllll1)
                                    {
                                        llllll4 = 1 - ((lllllllllll1 - llllll7) * lllllll7);
                                    }
                                    else
                                    {
                                        llllll4 = 1;
                                    }
                                    lllll4 = 1;
                                }
                            }
                        }
                    }
                    if (llllllllllll1 == 1)
                    {
                        float lllllllllll7 = llllll4 / llllllllllllllll1;
                        if (lllllllllllll1 == 1)
                        {
                            if (d.worldSpacePosition.y < (_PlayersPosVectorArray[llllllllllllllll3].y + lllllllllllllll1))
                            {
                                float llllll7 = (_PlayersPosVectorArray[llllllllllllllll3].y + lllllllllllllll1) - d.worldSpacePosition.y;
                                if (llllll7 < 0)
                                {
                                    llllll7 = 0;
                                }
                                if (llllll7 < llllllllllllllll1)
                                {
                                    llllll4 = (llllllllllllllll1 - llllll7) * lllllllllll7;
                                }
                                else
                                {
                                    llllll4 = 0;
                                }
                            }
                        }
                        else
                        {
                            if (d.worldSpacePosition.y < llllllllllllll1)
                            {
                                float llllll7 = llllllllllllll1 - d.worldSpacePosition.y;
                                if (llllll7 < 0)
                                {
                                    llllll7 = 0;
                                }
                                if (llllll7 < llllllllllllllll1)
                                {
                                    llllll4 = (llllllllllllllll1 - llllll7) * lllllllllll7;
                                }
                                else
                                {
                                    llllll4 = 0;
                                }
                            }
                        }
                    }
                    if (!lllllllll0 && !llllllllll0)
                    {
                        if (distance(_PlayersPosVectorArray[llllllllllllllll3].xyz, d.worldSpacePosition) > ll1)
                        {
                            llllll4 = 0;
                        }
                    }
                }
                llllllllllllll3 = llllllllllllll3 + _PlayersDataFloatArray[llllllllllllll3] * 4 + 1;
                if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                {
                    llllllllllllllllllllllllllllll3 = llllllllllllllllllllllllllllll3 * lllll4;
                }
                if (lllllllll0 || llllllllll0)
                {
                    llllll4 = llllllllllllllllllllllllllllll3 * llllll4;
                }
                else
                {
                    llllll4 = llllll4;
                    if (llllllllllllllllllll1)
                    {
                        if (lllllllllllllllllllllll2)
                        {
                            if (llllllllllllllllllllll1)
                            {
                                llllll4 = lllll4 * llllll4;
                            }
                        }
                        else
                        {
                            if (lllllllllllllllllllll1 == 1)
                            {
                                llllll4 = lllll4 * llllll4;
                            }
                        }
                    }
                }
                lllllllllllllllllllll2 = max(lllllllllllllllllllll2, llllll4);
            }
#else
        float llllllllllllllllllllllllllllll3 = 0;
        if (!llllll2)
        {
            llllllllllllllllllllllllllllll3 = 1;
            if (llllll0 != 0 && lllllll0 != 0 && _Time.y - lllllll0 < lllllllllllllllllll1)
            {
                if (llllll0 == 1)
                {
                    llllllllllllllllllllllllllllll3 = ((lllllllllllllllllll1 - (_Time.y - lllllll0)) / lllllllllllllllllll1);
                }
                else
                {
                    llllllllllllllllllllllllllllll3 = ((_Time.y - lllllll0) / lllllllllllllllllll1);
                }
            }
            else if (llllll0 == -1)
            {
                llllllllllllllllllllllllllllll3 = 1;
            }
            else if (llllll0 == 1)
            {
                llllllllllllllllllllllllllllll3 = 0;
            }
            else
            {
                llllllllllllllllllllllllllllll3 = 1;
            }
            llllllllllllllllllllllllllllll3 = 1 - llllllllllllllllllllllllllllll3;
        }
        float llllll4 = 0;
        float lllll4 = 0;
        bool llllllll4 = distance(_WorldSpaceCameraPos, d.worldSpacePosition) > ll1;
        llllllll4 = false;
        if ((llllllllllllllllllllllllllllll3 != 0) || ((!lllllllll0 && !llllllllll0) && !llllllll4))
        {
#if defined(_ZONING)
                        if(llllllllllllllllllll1) {
                            if(lllllllllllllllllllllll2) 
                            {
                                if(llllllllllllllllllllll1) {
                                    float lllllllllllllllllllll3 = lllllllllllllllllllllllllll2;
                                    float llllllllllllllllllllll3 = lllllllllllllllllllllllllllll2;
                                    lllll4 = 1;
                                    float lllllllllllll4 = llllllllllllllllllllllllllll2;
                                    if( llllllllllllllllllllll3!= 0 && lllllllllllllllllllll3 != 0 && _Time.y-lllllllllllllllllllll3 < lllllllllllll4) {
                                        if(llllllllllllllllllllll3 == 1) {
                                            lllll4 = ((lllllllllllll4-(_Time.y-lllllllllllllllllllll3))/lllllllllllll4);
                                        } else {
                                            lllll4 = ((_Time.y-lllllllllllllllllllll3)/lllllllllllll4);
                                        }
                                    } else if(llllllllllllllllllllll3 ==-1) {
                                        lllll4 = 1;
                                    } else if(llllllllllllllllllllll3 == 1) {
                                        lllll4 = 0;
                                    } else {
                                        lllll4 = 1;
                                    }
                                    lllll4 = 1 - lllll4;
                                    if(lllllllllllllllllllll1 == 0 && llllllllllllllllllllll1) {
                                        float llllllllllllll4 = 1 / lllllllllllllllllllllll1;
                                        if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)  {
                                            float lllllllllllllll4 = ((lllllllllllllllllllllll1-lllllllllllllllllllllllll2) * llllllllllllll4);
                                            lllll4 =  max(lllll4,lllllllllllllll4);
                                        }
                                    }
                                } else { 
                                }
                            } else {
                            }
                        }
#endif
            llllll4 = min(llllll4 + (1 * l1), 1);
            if (lllllllllllllllllllllll2)
            {
                if (lllllllllllllllllllll1 == 1)
                {
                    float llllllllllllll4 = 1 / lllllllllllllllllllllll1;
                    if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                    {
                        float llllllllllllllllllllllllllllll6 = 1 - ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * llllllllllllll4);
                        llllll4 = min(llllll4, llllllllllllllllllllllllllllll6);
                    }
                }
                else if (lllllllllllllllllllll1 == 0 && !llllllllllllllllllllll1)
                {
                    float llllllllllllll4 = llllll4 / lllllllllllllllllllllll1;
                    if (lllllllllllllllllllllllll2 < lllllllllllllllllllllll1)
                    {
                        float llllllllllllllllllllllllllllll6 = ((lllllllllllllllllllllll1 - lllllllllllllllllllllllll2) * llllllllllllll4);
                        llllll4 = max(0, llllllllllllllllllllllllllllll6);
                    }
                    else
                    {
                        llllll4 = 0;
                    }
                }
            }
            if (llllllllllllllllllll1 && !lllllllllllllllllllllll2)
            {
                if (lllllllllllllllllllll1 == 1)
                {
                    llllll4 = 0;
                }
            }
            if (llllll1 == 1 && lllllll1 == 0)
            {
                float lllllll7 = 0;
                if (llllllll1 == 0)
                {
                    lllllll7 = (llllll4) / lllllllllll1;
                }
                else if (llllllll1 == 1)
                {
                    float llllllll7 = 1 - llllll4;
                    if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
                    {
                        llllllll7 = max(1 - llllll4, 1 - (llllll4 * lllll4));
                    }
                    lllllll7 = llllllll7 / lllllllllll1;
                }
                if (d.worldSpacePosition.y > lllllllll1)
                {
                    float llllll7 = d.worldSpacePosition.y - lllllllll1;
                    if (llllll7 < 0)
                    {
                        llllll7 = 0;
                    }
                    if (llllllll1 == 0)
                    {
                        if (llllll7 < lllllllllll1)
                        {
                            llllll4 = ((lllllllllll1 - llllll7) * lllllll7);
                        }
                        else
                        {
                            llllll4 = 0;
                        }
                    }
                    else
                    {
                        if (llllll7 < lllllllllll1)
                        {
                            llllll4 = 1 - ((lllllllllll1 - llllll7) * lllllll7);
                        }
                        else
                        {
                            llllll4 = 1;
                        }
                        lllll4 = 1;
                    }
                }
            }
            if (llllllllllll1 == 1 && lllllllllllll1 == 0)
            {
                float lllllllllll7 = llllll4 / llllllllllllllll1;
                if (d.worldSpacePosition.y < llllllllllllll1)
                {
                    float llllll7 = llllllllllllll1 - d.worldSpacePosition.y;
                    if (llllll7 < 0)
                    {
                        llllll7 = 0;
                    }
                    if (llllll7 < llllllllllllllll1)
                    {
                        llllll4 = (llllllllllllllll1 - llllll7) * lllllllllll7;
                    }
                    else
                    {
                        llllll4 = 0;
                    }
                }
            }
        }
        if (llllllllllllllllllll1 && lllllllllllllllllllllll2 && llllllllllllllllllllll1)
        {
            llllllllllllllllllllllllllllll3 = llllllllllllllllllllllllllllll3 * lllll4;
        }
        if (lllllllll0 || llllllllll0)
        {
            llllll4 = llllllllllllllllllllllllllllll3 * llllll4;
        }
        else
        {
            llllll4 = llllll4;
            if (llllllllllllllllllll1)
            {
                if (lllllllllllllllllllllll2)
                {
                    if (llllllllllllllllllllll1)
                    {
                        llllll4 = lllll4 * llllll4;
                    }
                }
                else
                {
                    if (lllllllllllllllllllll1 == 1)
                    {
                        llllll4 = lllll4 * llllll4;
                    }
                }
            }
        }
        lllllllllllllllllllll2 = max(lllllllllllllllllllll2, llllll4);
#endif
            float lllllll4 = lllllllllllllllllllll2;
            if (!llllllllllllllllllllllllll1)
            {
                if (lllllll4 == 1)
                {
                    lllllll4 = 10;
                }
                if (!lllllllllllllllllll0 || llllllllllllllllllll0 == 6)
                {
#if defined(UNITY_PASS_SHADOWCASTER) 
#if defined(SHADOWS_DEPTH) 
                if (!any(unity_LightShadowBias))
                {
                        clip(lllllllllllllllllll2- lllllll4);
                        llll2 = lllllllllllllllllll2 - lllllll4;
                }
                else
                {
                    if(lllllllllllllllllll0 && llllllllllllllllllll0 != 6) {
                        clip(lllllllllllllllllll2- lllllll4);  
                        llll2 = lllllllllllllllllll2 - lllllll4;
                    }
                }
#endif
#else
                    clip(lllllllllllllllllll2 - lllllll4);
                    llll2 = lllllllllllllllllll2 - lllllll4;
#endif
                }
                else
                {
                    clip(lllllllllllllllllll2 - lllllll4);
                    llll2 = lllllllllllllllllll2 - lllllll4;
                }
                llll2 += 9;
            }
            if (llllllllllllllllllllllllll1)
            {
                lllllll2 = 1;
                if ((lllllllllllllllllll2 - lllllll4) < 0)
                {
                    llllllll2 = half4(1, 1, 1, 1);
                    o.Emission = 1;
                }
                else
                {
                    llllllll2 = half4(0, 0, 0, 1);
                }
                if (lllllllllllll3)
                {
                    if ((lllllllllllllllllll2 - lllllll4) < 0)
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
                    float ll8 = 0;
                    if (lllllllllllll1 == 1)
                    {
                        llllllllllll3 = llllllllllll3 + lllllllllllllllllllllllll1;
                        ll8 = llllllllllll3;
                    }
                    else
                    {
                        ll8 = llllllllllllll1 + lllllllllllllllllllllllll1;
                    }
                    if (d.worldSpacePosition.y > (ll8 - lllllllllllllllllllllllllll1) && d.worldSpacePosition.y < (ll8 + lllllllllllllllllllllllllll1))
                    {
                        llllllll2 = half4(1, 0, 0, 1);
                    }
                }
            }
            else
            {
                half3 lll8 = lerp(1, llllllllllll0, lllllllllllll0).rgb;
                if (lllllllllllllllll0)
                {
                    llllllllllllllllll0 = 0.2 + (llllllllllllllllll0 * (0.8 - 0.2));
                    o.Emission = o.Emission + min(clamp(lll8 * clamp(((lllllll4 / llllllllllllllllll0) - lllllllllllllllllll2), 0, 1), 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0), clamp(lll8 * lllllll4, 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0));
                }
                else
                {
                    o.Emission = o.Emission + clamp(lll8 * lllllll4, 0, 1) * sqrt(lllllllllllllll0 * llllllllllllllll0);
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
            float llll8 = 0;
            float lllll8 = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                lllll8 =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                lllll8 = _ProbeExposureScale;
            #endif
                float llllll8 = 0;
                float lllllll8 = lllll8;
                llllll8 = rcp(lllllll8 + (lllllll8 == 0.0));
                float3 llllllll8 = o.Emission * llllll8;
                o.Emission = lerp(llllllll8, o.Emission, llll8);
            lll2 = o.Emission;
        #endif

}

#endif
