// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

    Shader "SLywnow/EarthCombo"
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
    _Color("Color", Color) = (0, 0, 0, 1)
    _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
    _FalloffPlanet("Falloff Planet", Float) = 5
    _TransparencyPlanet("Transparency Planet", Float) = 1


      }

      SubShader 
      {

          Tags {"LightMode" = "ForwardBase"}
        Pass
        {
            Name "PlanetBase"
            Cull Back
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
 
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
 
                #include "UnityCG.cginc"
 
                uniform sampler2D _MainTex;
                uniform float4 _MainTex_ST;
                uniform float4 _Color;
                uniform float4 _AtmoColor;
                uniform float _FalloffPlanet;
                uniform float _TransparencyPlanet;
 
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                    float2 texcoord : TEXCOORD2;
                };
 
                v2f vert(appdata_base v)
                {
                    v2f o;
 
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    o.worldvertpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
 
                    return o;
                }
 
                float4 frag(v2f i) : COLOR
                {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(_WorldSpaceCameraPos-i.worldvertpos);
 
                    float4 atmo = _AtmoColor;
                    atmo.a = pow(1.0-saturate(dot(viewdir, i.normal)), _FalloffPlanet);
                    atmo.a *= _TransparencyPlanet*_Color;
 
                    float4 color = tex2D(_MainTex, i.texcoord)*_Color;
                    color.rgb = lerp(color.rgb, atmo.rgb, atmo.a);
 
                    return color*dot(_WorldSpaceLightPos0, i.normal);
                }
            ENDCG
        }

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

          struct EditorSurfaceOutput {
            half3 Albedo;
            half3 Normal;
            half3 Emission;
            half3 Gloss;
            half Specular;
            half Alpha;
            half4 Custom;
          };

          inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
          {
    half3 spec = light.a * s.Gloss;
    half4 c;
    c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
    c.g -= .01 * s.Alpha;
    c.r -= .03 * s.Alpha;
    c.rg += min(s.Custom, s.Alpha);
    c.b += 0.75 * min(s.Custom, s.Alpha);
    c.b = saturate(c.b + s.Alpha * .02);
    c.a = 1.0;
    return c;

          }

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
        half invdiff = 1 - saturate(16 * diff);
        s.Alpha = invdiff;

            return LightingBlinnPhongEditor_PrePass( s, res );
          }

          struct Input {
            float3 viewDir;
    float2 uv_MainTex;
    float2 uv_Normals;
    float2 uv_Lights;

          };

          void surf (Input IN, inout EditorSurfaceOutput o) {
            o.Gloss = 0.0;
            o.Specular = 0.0;
            o.Custom = 0.0;
            o.Alpha = 1.0;

        float4 Fresnel0_1_NoInput = float4(0,0,1,1);
        float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
        float4 Pow0=pow(Fresnel0,_AtmosFalloff.xxxx);
        float4 Saturate0=saturate(Pow0);
        float4 Lerp0=lerp(_AtmosNear,_AtmosFar,Saturate0);
        float4 Multiply1=Lerp0 * Saturate0;
        float4 Sampled2D2=tex2D(_MainTex,IN.uv_MainTex.xy);
        float4 Add0=Multiply1 + Sampled2D2;
        float4 Sampled2D0=tex2D(_Normals,IN.uv_Normals.xy);
        float4 UnpackNormal0=float4(UnpackNormal(Sampled2D0).xyz, 1.0);

        o.Albedo = Add0;
        o.Normal = UnpackNormal0;
        //o.Emission = Multiply0;
            o.Emission = 0.0;

            //float4 Multiply0=Sampled2D1 * _LightScale.xxxx;
        o.Custom = tex2D(_Lights,IN.uv_Lights.xy).r * _LightScale;

            o.Normal = normalize(o.Normal);
          }
        ENDCG
      }
      Fallback "Diffuse"
    }