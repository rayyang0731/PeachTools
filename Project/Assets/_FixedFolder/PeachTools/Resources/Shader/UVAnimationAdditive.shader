Shader "UI/Peach/UVAnimation/Additive" {
    Properties {
        [NoScaleOffset]_MainTex ("Sprite Texture", 2D) = "white" {}
        [NoScaleOffset]_AlphaTex ("Alpha Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _Speed ("Speed", vector) = (1,1,0,0)

        //MASK SUPPORT ADD
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END
    }
    SubShader {
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
		Blend SrcAlpha One

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            half4 _MainTex_ST;
            half4 _Speed;
			fixed4 _Color;
            
            struct a2v {
                half4 vertex : POSITION;
                half4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
            };
            
            struct v2f {
                half4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                half2 uv2 : TEXCOORD1;
				fixed4 color : COLOR;
            };
            
            v2f vert (a2v IN) {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv2 = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.uv = OUT.uv2 + frac( _Speed.xy * _Time.y);
                OUT.color = IN.color;
                return OUT;
            }
            
            fixed4 frag (v2f IN) : SV_Target {
                fixed4 alphaTex = tex2D(_AlphaTex,IN.uv2);
                fixed4 col = tex2D(_MainTex, IN.uv) * _Color * IN.color;
                col.a = alphaTex.a * _Color.a;
                return col;
            }
            
            ENDCG
        }
    }
    FallBack "VertexLit"
}