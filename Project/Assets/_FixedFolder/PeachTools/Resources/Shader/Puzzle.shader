Shader "Peach/Puzzle"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Alpha ("Cutout Alpha", Range(0,1)) = 0.5
      	_CullRect("Cull Rect", vector) = (0, 0, 0, 0)
		_BlockRect("Block Rect",vector) = (0, 0, 0, 0)
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
 
		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
 
			#include "UnityUI.cginc"
			
			struct appdata_t
			{
				half4 vertex   : POSITION;
				half4 color    : COLOR;
				half2 texcoord : TEXCOORD0;
			};
 
			struct v2f
			{
				half4 vertex        : SV_POSITION;
				fixed4 color        : COLOR;
				half2 texcoord      : TEXCOORD0;
				half4 worldPosition : TEXCOORD1;
			};
			
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			fixed4 _Color;
			fixed _Alpha;
            half4 _CullRect;
            half4 _BlockRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				return OUT;
			}
 
			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
				
				half2 cullleftTop = half2(_CullRect.x - _CullRect.z/2.0, _CullRect.y + _CullRect.w/2.0);
				half2 cullrightBottom = half2(_CullRect.x + _CullRect.z/2.0, _CullRect.y - _CullRect.w/2.0);

				half2 blockleftTop = half2(_BlockRect.x - _BlockRect.z/2.0, _BlockRect.y + _BlockRect.w/2.0);
				half2 blockrightBottom = half2(_BlockRect.x + _BlockRect.z/2.0, _BlockRect.y - _BlockRect.w/2.0);
				
				//-------------------------拼图块位置---------------------------
				if(IN.worldPosition.x > blockleftTop.x && IN.worldPosition.x < blockrightBottom.x && IN.worldPosition.y > blockrightBottom.y && IN.worldPosition.y < blockleftTop.y)
				{
					float2 delta = cullleftTop - blockleftTop + IN.worldPosition.xy;
					half2 uv = half2((_MainTex_TexelSize.z /2.0 +delta.x)*_MainTex_TexelSize.x,(_MainTex_TexelSize.w /2.0 +delta.y)*_MainTex_TexelSize.y);
					color = tex2D(_MainTex, uv) * IN.color;
				}
               	//-------------------------------------------------------------

				//-------------------------剔除位置----------------------------
				else if(IN.worldPosition.x > cullleftTop.x && IN.worldPosition.x < cullrightBottom.x && IN.worldPosition.y > cullrightBottom.y && IN.worldPosition.y < cullleftTop.y)
				{
					color.a*=_Alpha;
					color.rgb*= color.a;
				}
				//-------------------------------------------------------------

				return color;
			}
			ENDCG
		}
	}
}