// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//////////////////////////////////////////////////////////////////////////
// GreyScale.shader
//
//  unlit transparent texture with greyscale effect.
//
// (c) 2014 hwkim
//////////////////////////////////////////////////////////////////////////
Shader "Extension/GrayScale" 
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _EffectAmount ("Effect Amount", Range (0, 1)) = 1.0
    }
 
    SubShader
    {
    	LOD 200
    	
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
 
		Cull Off
		Lighting Off
		ZWrite Off
		Offset -1, -1
		Fog { Mode Off }
		ColorMask RGB
		AlphaTest Greater .01
		Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"
           
            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float _EffectAmount;
            //float2 _ClipSharpness = float2(20.0, 20.0);
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                //float2 worldPos : TEXCOORD1;
            };
           
            fixed4 _Color;
 
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                //OUT.worldPos = TRANSFORM_TEX(IN.vertex.xy, _MainTex);
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
 
                return OUT;
            }           
 
            fixed4 frag(v2f IN) : COLOR
            {
            	//Softness factor
				//float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipSharpness;
				
                half4 texcol = tex2D (_MainTex, IN.texcoord) * IN.color;              
                texcol.rgb = lerp(texcol.rgb, dot(texcol.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount);
                texcol = texcol * IN.color;                
                //texcol.a *= clamp( min(factor.x, factor.y), 0.0, 1.0);
                
                return texcol;
            }
        ENDCG
        }
      }  
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
    Fallback "Sprites/Default"
 } 