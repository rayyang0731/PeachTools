Shader "UI/Peach/UVAnimation/Addtive With Gradient" {
    Properties {
        [NoScaleOffset]_MainTex ("Sprite Texture", 2D) = "white" {}
        _Speed ("Speed", vector) = (1,1,0.5,1)
        _Color01 ("Color01",Color) = (1,1,1,1)
        _Color02 ("Color02",Color) = (1,1,1,1)
        _Gradient ("Dir&Center&Range",Vector) = (1,0.5,1,0)

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
            half4 _MainTex_ST;
            half4 _Speed;
			fixed4 _Color01;
			fixed4 _Color02;
			fixed4 _Gradient;
            
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
                fixed4 col = tex2D(_MainTex, IN.uv) * IN.color;
                fixed offset;
                if(_Gradient.x > 0){
                    offset = saturate((IN.uv2.x - (_Gradient.y - _Gradient.z * 0.5))/_Gradient.z);
                }
                else{
                    offset = saturate((IN.uv2.y - (_Gradient.y - _Gradient.z * 0.5))/_Gradient.z);
                }
                // fixed offset = saturate((IN.uv2.x - (_Gradient.y - _Gradient.z * 0.5))/_Gradient.z);
                fixed4 gradient = lerp(_Color01,_Color02,offset);
                col *= gradient;
                col.a = gradient.a;
                return col;
            }
            
            ENDCG
        }
    }
    FallBack "VertexLit"
}