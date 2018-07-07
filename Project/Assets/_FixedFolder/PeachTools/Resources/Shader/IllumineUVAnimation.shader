Shader "Peach/IllumineUVAnimation" {
    Properties {
        [NoScaleOffset]_MainTex ("Sprite Texture", 2D) = "white" {}
		[NoScaleOffset]_Illum ("Illumin (A)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _Speed ("Speed", vector) = (1,1,0,0)
        _Power ("Power",Range(0,5.0))=1
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
		Blend SrcAlpha OneMinusSrcAlpha

        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex,_Illum;
            half4 _MainTex_ST,_Illum_ST;
            half4 _Speed;
			fixed4 _Color;
            half _Power;
            
            struct a2v {
                half4 vertex : POSITION;
                half4 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
            };
            
            struct v2f {
                half4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
            };
            
            v2f vert (a2v IN) {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex) + frac( _Speed.xy * _Time.y);
                OUT.color = IN.color;
                return OUT;
            }
            
            fixed4 frag (v2f IN) : SV_Target {
				fixed4 mainTex =  tex2D(_MainTex, IN.uv);
				fixed4 illumTex = tex2D(_Illum, IN.uv);
				mainTex += illumTex * illumTex.a * _Power;
				mainTex *=  _Color * IN.color;
                return mainTex;
            }
            
            ENDCG
        }
    }
    FallBack "VertexLit"
}