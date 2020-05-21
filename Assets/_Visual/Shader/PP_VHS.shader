Shader "ACalmPostProcess/VHS-effect"
{
	Properties
	{
		_Blur("Blur Intensity", Range(0,0.01)) = 0.005 //(min = 0.0005)
		_BlurForColor("Blue for each color",Vector) = (0,0,0,0)

		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_OffsetBlue("Decalage du bleu", Vector) = (0.001,0.001,0,0) //_OffsetBlue store noise offset in z and w 
		_OffsetRed("Decalage du rouge", Vector) = (0.001,0.001,0,0) //_OffsetBlue store noise offset in z and w 
		_OffsetGreen("Decalage du vert", Vector) = (0.001,0.001,0,0) //_OffsetBlue store noise offset in z and w 
		//_OffsetBluRound("Decalage du bleu  rond", float) = 0 //_OffsetBlue store noise offset in z and w 
		//_OffsetRedRound("Decalage du rouge rond", float) = 0 //_OffsetBlue store noise offset in z and w 
		//_OffsetGreRound("Decalage du vert  rond", float) = 0 //_OffsetBlue store noise offset in z and w 
		_Saturation("Saturation", Range(0,2)) = 0.7
		_NbIntensity("NoirEtBlanc", Range(0,2)) = 0.5

		_Hauteur("Hauteur du bug", Range(0,1)) = 0.5
		_Taille("Taille du bug", Range(0,1)) = 0.02
		_Decalage("Decalage dans le bug", float) = 0.005
		_Speed("Vitesse du bug", float) = 1
		_typeOfBug("0 mean only decalage", float) = 0

		_BlackNoiseTex("Texture", 2D) = "white" {}
		_ToleranceBWNoise("BW Tolerance", Range(0,1)) = 0.1
		_NoiseColor("Noise Color", Color) = (0,0,0.15,1)
		_NoiseBool("If we use offset or time", Range(0,1)) = 0

		_Tramage("Tramage", float) = 2
		_Tram2("Tram2", float) = 2
		_TramIntensity("Intensity", float) = 1
		_TramColor("TramColor", Color) = (1,1,1,1)
			
		_Dilatation("Dilation", Range(-1,2)) = 0

		_FloatEffect("Float Effect Intensity", Range(0,1)) = 0
		_FloatNoiseDensity("Noise Density", Range(0,12)) = 4

	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

			sampler2D _MainTex;
			float4 _Color;

			float _Blur;
			float4 _BlurForColor;

			//COLOR PART 
			float4 _OffsetBlue;
			float4 _OffsetRed;
			float4 _OffsetGreen;
			float _Saturation;
			float _NbIntensity;

			//BUG PART
			//Bug large
			half _Hauteur;
			half _Taille;
			float _Decalage;
			float _Speed;
			//Type du bug
			float _typeOfBug;



			//BLACK NOISE
			sampler2D _BlackNoiseTex;
			float _ToleranceBWNoise;
			float4 _NoiseColor;
			half _NoiseBool;

			//TRAME
			float _Tramage;
			float _Tram2;
			float _TramIntensity;
			float4 _TramColor;

			float _Dilatation;
			float _FloatEffect;
			float _FloatNoiseDensity;

			fixed4 frag(v2f i) : SV_Target
			{
				//I.UV PART :
				float2 offsetFromCenter = i.uv - float2(0.5,0.5);

				i.uv += _FloatEffect * sin((offsetFromCenter + _Time.y) * _FloatNoiseDensity) * 0.02f;

				i.uv += _Dilatation * offsetFromCenter;
			    
				//DECALAGE PART
				//i.uv = float2(frac(i.uv.x), frac(i.uv.y));

				half positionLow = frac(_Hauteur + _Time * _Speed);
				half positionUp = frac(positionLow + _Taille);

				//if (positionUp > positionLow) {
				float value = max(sign(i.uv.y - positionLow), 0) * (1 - max(sign(i.uv.y - positionUp), 0));
				float doTrait = _typeOfBug * value;//need to be between 0 and 1
				i.uv.y = i.uv.y * (1 - doTrait) + positionLow * doTrait;
				i.uv.x += _Decalage * value;
				//}
				/*else {
					if (i.uv.y > positionLow || i.uv.y < positionUp) {
						if (_typeOfBug != 0)
							i.uv.y = positionLow;
						i.uv.x += _Decalage;
					}
				}*/
				//END DECALAGE PART

				//COLOR PART : 
				fixed4 col = tex2D(_MainTex, i.uv);

				float blur = _Blur;
				////BLUR BUT LOL
				//float4 totalValue = col;
				///*totalValue += tex2D(_MainTex, float2(i.uv.x - blur, i.uv.y - blur));
				//totalValue += tex2D(_MainTex, float2(i.uv.x - blur, i.uv.y));
				//totalValue += tex2D(_MainTex, float2(i.uv.x - blur, i.uv.y + blur));
				//totalValue += tex2D(_MainTex, float2(i.uv.x       , i.uv.y - blur));
				//totalValue += tex2D(_MainTex, float2(i.uv.x       , i.uv.y + blur));
				//totalValue += tex2D(_MainTex, float2(i.uv.x + blur, i.uv.y - blur));
				//totalValue += tex2D(_MainTex, float2(i.uv.x + blur, i.uv.y));
				//totalValue += tex2D(_MainTex, float2(i.uv.x + blur, i.uv.y + blur));*/
				////col = totalValue / 9;
				////BLUR PART END

				blur = _BlurForColor.r;
				//Decal Green;
				float redValue = tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x, i.uv.y + _OffsetRed.y)).r;
				/*redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x - blur, i.uv.y + _OffsetRed.y - blur)).r;
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x - blur, i.uv.y + _OffsetRed.y)).r;		r
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x - blur, i.uv.y + _OffsetRed.y + blur)).r;
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x,        i.uv.y + _OffsetRed.y - blur)).r;
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x,        i.uv.y + _OffsetRed.y + blur)).r;
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x + blur, i.uv.y + _OffsetRed.y - blur)).r;
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x + blur, i.uv.y + _OffsetRed.y)).r;		r
				redValue += tex2D(_MainTex, float2(i.uv.x + _OffsetRed.x + blur, i.uv.y + _OffsetRed.y + blur)).r;*/
				//redValue /= 9;

				blur = _BlurForColor.g;
				//Decal Green;
				float greenValue = tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x, i.uv.y + _OffsetGreen.y)).g;
				/*greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x - blur, i.uv.y + _OffsetGreen.y - blur)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x - blur, i.uv.y + _OffsetGreen.y)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x - blur, i.uv.y + _OffsetGreen.y + blur)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x,        i.uv.y + _OffsetGreen.y - blur)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x,        i.uv.y + _OffsetGreen.y + blur)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x + blur, i.uv.y + _OffsetGreen.y - blur)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x + blur, i.uv.y + _OffsetGreen.y)).g;
				greenValue += tex2D(_MainTex, float2(i.uv.x + _OffsetGreen.x + blur, i.uv.y + _OffsetGreen.y + blur)).g;*/
				//greenValue /= 9;

				blur = _BlurForColor.b;
				//Decal Green;
				float blueValue = tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x, i.uv.y + _OffsetBlue.y)).b;
				/*blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x - blur, i.uv.y + _OffsetBlue.y - blur)).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x - blur, i.uv.y + _OffsetBlue.y)       ).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x - blur, i.uv.y + _OffsetBlue.y + blur)).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x,        i.uv.y + _OffsetBlue.y - blur)).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x,        i.uv.y + _OffsetBlue.y + blur)).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x + blur, i.uv.y + _OffsetBlue.y - blur)).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x + blur, i.uv.y + _OffsetBlue.y)       ).b;
				blueValue += tex2D(_MainTex, float2(i.uv.x + _OffsetBlue.x + blur, i.uv.y + _OffsetBlue.y + blur)).b;*/
				//blueValue /= 9;

				fixed4 colDecal = fixed4(redValue * _Saturation,
					greenValue * _Saturation,
					blueValue * _Saturation, col.a);

				col = colDecal;

				//COLOR PART : Noir et Blanc
				float greyValue = (col.r + col.g + col.b) / 3;
				fixed4 colNoiEtBla = fixed4(greyValue, greyValue, greyValue, col.a);
				fixed4 colResultat = lerp(col, colNoiEtBla, _NbIntensity);

				//Final color tint
				colResultat.rgb *= _Color.rgb;

				//END COLOR PART

				//BLACK NOISE
				float offsetHere = (float2(20 * _Time.x + _Time.y, 36 * _Time.x - _Time.y) * (1 -_NoiseBool)) + (_NoiseBool * _OffsetBlue.zw);
				float4 colBW = tex2D(_BlackNoiseTex, i.uv + offsetHere);
				float blackNoiseVal = max(sign(colBW - _ToleranceBWNoise), 0);
				float tram = frac(fmod(frac(i.uv.y + _Time.x) * pow(10, _Tram2), _Tramage));
				colResultat.rgb = blackNoiseVal * colResultat + (1 - blackNoiseVal) * (float4(_NoiseColor.r, _NoiseColor.g, _NoiseColor.b, colBW.r));
				if(_TramIntensity == 0)
					tram = 0;
				colResultat += (_TramColor * tram) * _TramIntensity;
				//END BLACK DOT NOISE AND TRAM
				
				return colResultat;
			}
			ENDCG
		}
		}


		
}
