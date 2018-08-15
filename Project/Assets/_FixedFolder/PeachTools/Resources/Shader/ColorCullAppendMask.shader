Shader "UI/Peach/ColorCullWithMask"
 {  
     Properties
     {
         [PerRendererData]_MainTex ("Main Texture", 2D) = "white" {}
        _Color ("要剔除的目标颜色", Color) = (0,0,0,1)
        _Range("与目标颜色相差的范围",Range (0,1.01))=0.01
        _Mask("遮罩", 2D) = "white" {}
     }
     SubShader
     {
         Tags 
         { 
			"Queue"="Transparent" 
         	"IgnoreProjector"="True" 
         	"RenderType"="Transparent" 
         	"PreviewType"="Plane"
         	"CanUseSpriteAtlas"="True"
         }
         Pass
         {
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha


             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag

             sampler2D _MainTex;
             float4 _Color;
             half _Range;
             sampler2D _Mask;
             struct Vertex
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float2 uv2 : TEXCOORD1;
             };

             struct Fragment
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float2 uv2 : TEXCOORD1;
             };

             Fragment vert(Vertex v)
             {
                 Fragment o;

                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.uv_MainTex = v.uv_MainTex;
                 o.uv2 = v.uv2;

                 return o;
             }

             float4 frag(Fragment IN) : COLOR
             {
                 float4 o = float4(1, 0, 0, 0.2);

                 half4 c = tex2D (_MainTex, IN.uv_MainTex);
                 half4 m = tex2D (_Mask, IN.uv_MainTex);
                 o.rgb = c.rgb;
                 if(abs(c.r-_Color.r)<_Range && abs(c.g-_Color.g)<_Range && abs(c.b-_Color.b)<_Range)
                 {
                    o.a = 0;
                 }
                 else
                 {
                     o.a = 1 * m.a;
                 }

                 return o;
             }

             ENDCG
         }
     }
 }