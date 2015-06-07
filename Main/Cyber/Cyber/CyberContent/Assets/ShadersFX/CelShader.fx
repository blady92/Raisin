

// Camera
float4x4 View;
float4x4 Projection; 

//World
float4x4 World;
float4x4 InverseWorld;

//Lights & TextureMaps
float4 LightDirection;
texture ColorMap;
texture CelMap;

//Sampler koloru modelu - zwraca oryginalny kolor piksela dla modelu

sampler ColorMapSampler = sampler_state
{
	Texture = <ColorMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

//CelShader effect map
sampler2D CelMapSampler = sampler_state
{
	Texture = <CelMap>;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
};

//Input do VertexShader
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 Tex : TEXCOORD0;
	float3 N : NORMAL0;
};

//Output z VertexShader
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 Tex : TEXCOORD0;
	float3 L : TEXCOORD1;
	float3 N : TEXCOORD2;
};

//Obliczenia dla Vertexów - obliczenie ostatecznego output position
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.Tex = input.Tex;
	output.L = normalize(LightDirection);
	output.N = normalize(mul(InverseWorld, input.N));

	return output;
}

//Shadowanie kazdego piksela
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//pobranie koloru pixela
	float4 Color = tex2D(ColorMapSampler, input.Tex);
	//tint koloru
	float Ai = 0.8f;
	float Ac = float4(0.075, 0.075, 0.2, 1.0);

	//Skala shaderu
	float Di = 1.0f;

	//Shading light color
	float2 celTexCoord = float2(saturate(dot(input.L, input.N)), 0.0f);
		float4 CelColor = tex2D(CelMapSampler, celTexCoord);

	//ostateczny kolor piksela
	return (Ai*Ac*Color) + (Color*Di*CelColor);
}

technique ToonShader
{
	pass Pass0
	{
		Sampler[0] = (ColorMapSampler);
		Sampler[1] = (CelMapSampler);

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};