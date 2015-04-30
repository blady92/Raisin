#define MaxBones 59

float4x4 Bones[MaxBones];
float4x4 View;
float4x4 Projection;
texture Tekstura;

sampler2D C_sampler = sampler_state {
	Texture = <Tekstura>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct VS_INPUT // VertexShader Input
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 BoneIndices : BLENDINDICES0;
	float4 BoneWeights : BLENDWEIGHT0;
};

struct VS_OUTPUT //VertexShader Output
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

VS_OUTPUT VSBasic(VS_INPUT input)
{
	VS_OUTPUT output;
	float4x4 skinTransform = 0;
	skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
	skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
	skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
	skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;

	float4 pos = mul(input.Position, skinTransform);
	pos = mul(pos, View);
	pos = mul(pos, Projection);

	output.Position = pos;
	output.TexCoord = input.TexCoord;

	return output;
}

float4 PSBasic(VS_OUTPUT input, sampler2D C_Sampler) : COLOR0
{
	float4 outColor = tex2D(C_Sampler, input.TexCoord);
	outColor.a = 1;
	return outColor;
}

technique SkinnedModelTechnique
{
	pass SkinnedModelPass
	{
		VertexShader = compile vs_1_1 VSBasic();
		PixelShader = compile ps_2_0 PSBasic();
	}
}

