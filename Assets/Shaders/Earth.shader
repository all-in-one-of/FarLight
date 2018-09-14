Shader "HDynamics/EarthLight"
{

	//Earth Shader created by Julien Lynge @ Fragile Earth Studios
	//Upgrade of a shader originally put together in Strumpy Shader Editor by Clamps
	//Feel free to use and share this shader, but please include this attribution

	Properties 
	{
		_MainTex("_MainTex", 2D) = "black" {}
		_Normals("_Normals", 2D) = "black" {}
		_Lights("_Lights", 2D) = "black" {}
		_LightScale("_LightScale", Float) = 1
		_AtmosNear("_AtmosNear", Color) = (0.1686275,0.7372549,1,1)
		_AtmosFar("_AtmosFar", Color) = (0.4557808,0.5187039,0.9850746,1)
		_AtmosFalloff("_AtmosFalloff", Float) = 3
	}

	SubShader 
	{
		Tags
		{
			"Queue"="Geometry"
			"IgnoreProjector"="False"
			"RenderType"="Opaque"
		}

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Fog{
		}


		CGPROGRAM
		#pragma surface surf BlinnPhongEditor
		#pragma target 2.0

		sampler2D _MainTex;
		sampler2D _Normals;
		sampler2D _Lights;
		float _LightScale;
		float4 _AtmosNear;
		float4 _AtmosFar;
		float _AtmosFalloff;

		struct EditorSurfaceOutput 
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
			half4 Custom;
		};
		
		// Устанавливаем RGB и д.р. на определившейся тёмной стороне.
		inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
		{
			// Ночная сторона земли всегда немного ненасыщена (серо-выведенная) и окрашена в синий цвет. 
			// Оба этих эффекта очень тонкие, поэтому не стесняйтесь играть с этими цифрами.
			
			half3 spec = light.a * s.Gloss;
			half4 c;
			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
			c.g -= .01 * s.Alpha; //lower green by .01 on the dark side - desaturate (нижний зеленый .01 на темной стороне - обесцветить)
			c.r -= .03 * s.Alpha; //lower red by .03 on the dark side - desaturate (нижний красный .01 на темной стороне - обесцветить)
			// Lights are tinted slightly yellow. The two lines below apply the rgb of the lights to the world:
			// (Огни слегка окрашены в желтый цвет. В приведенных ниже двух строках применим rgb огней к миру)
			c.rg += min(s.Custom, s.Alpha);
			c.b += 0.75 * min(s.Custom, s.Alpha);
			// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			c.b = saturate(c.b + s.Alpha * .02); //raise blue by .02; saturate() clamps the value between 0 and 1 (поднять синий на .02; saturate () фиксирует значение от 0 до 1)
			c.a = 1.0;
			return c;
		}
		// Устанавливаем RGB и д.р. на определившейся светлой стороне.
		inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize (lightDir + viewDir);

			half diff = max (0, dot ( lightDir, s.Normal ));

			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0);

			half4 res;
			res.rgb = _LightColor0.rgb * diff;
			res.w = spec * Luminance (_LightColor0.rgb);
			res *= atten * 2.0;

			//s.Alpha is set to 1 where the earth is dark.  The value of night lights has been saved to Custom
			//s.Aphpha установлен в 1, где земля темная. Значение ночных огней было сохранено в пользовательских
			half invdiff = 1 - saturate(16 * diff); // change the '16' to adjust how far dusk should extend)
			s.Alpha = invdiff;

			return LightingBlinnPhongEditor_PrePass( s, res );
		}

		struct Input 
		{
			float3 viewDir;
			float2 uv_MainTex;
			float2 uv_Normals;
			float2 uv_Lights;
		};

		void surf (Input IN, inout EditorSurfaceOutput o) 
		{
			o.Gloss = 0.0;
			o.Specular = 0.0;
			o.Custom = 0.0;
			o.Alpha = 1.0;

			float4 Fresnel0_1_NoInput = float4(0, 0, 1, 1);
			float4 Fresnel0 = (1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
			float4 Pow0 = pow(Fresnel0, _AtmosFalloff.xxxx);
			float4 Saturate0 = saturate(Pow0);
			float4 Lerp0 = lerp(_AtmosNear, _AtmosFar, Saturate0);
			float4 Multiply1 = Lerp0 * Saturate0;
			float4 Sampled2D2 = tex2D(_MainTex, IN.uv_MainTex.xy);	// Получение текстуры !!!
			float4 Add0 = Multiply1 + Sampled2D2;					// Склейка тумана и поверхности
			float4 Sampled2D0 = tex2D(_Normals, IN.uv_Normals.xy);
			float4 UnpackNormal0 = float4(UnpackNormal(Sampled2D0).xyz, 1.0);

			o.Albedo = Add0;
			o.Normal = UnpackNormal0; // Какие-то ху*вые нормали. Отвечают за правильность теней.
			//o.Normal = 0.0; // ОШИБКА!!!
			o.Emission = 0.0;

			o.Custom = tex2D(_Lights,IN.uv_Lights.xy).r * _LightScale;

			o.Normal = normalize(o.Normal);
		}
	ENDCG
	}
	Fallback "Diffuse"
}