

sampler ColorMapSampler : register(s0);

//Rozmiar tekstury u¿ywany do przeskalowania obrysu
float2 ScreenSize = float2(1024.0f, 1024.0f);

//Grubosc obrysu
float Thickness = 1.5f;

//Threeshold krawedzi - im mniejszy tym bardziej 'komiksowy' efekt i ostrzejsze przejscia tonalne
float Threshold = 0.2f;

//Pomocnicza funkcja do zwrocenia wartosci grayscale dla piksela
float getGray(float4 c)
{
	return(dot(c.rgb, ((0.33333).xxx)));
}

struct VertexShaderOutput
{
	float2 Tex : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//zrodlowy kolor piksela
	float4 Color = tex2D(ColorMapSampler, input.Tex);

	//ox - wektor offset X, offset bazowany na skali grubosci krawedzi
	float2 ox = float2(Thickness / ScreenSize.x, 0.0);

	//oy - wektor offset Y, offset bazowany na skali grubosci krawedzi
	float oy = float2(0.0, Thickness / ScreenSize.y);

	//current xy (uv) texture coordinate
	float2 uv = input.Tex.xy;

		//Utworzenie macierzy 3x3 by odwzrowoaæ wartoœæ grayscale dla naszego bie¿¹cego piksela oraz jego 8 s¹siadów
		// g00 g01 g02
		// g10 'g11' g12
		// g20 g21 g22

		//1 - dolny rz¹d pikseli
		// SW: uv - oy - ox 
		// S: uv - oy 
		// SE: uv - oy + ox

		float2 PP = uv - oy;
		float4 CC = tex2D(ColorMapSampler, PP - ox);	float g00 = getGray(CC);
		CC = tex2D(ColorMapSampler, PP);				float g01 = getGray(CC);
	CC = tex2D(ColorMapSampler, PP + ox);			float g02 = getGray(CC);

	// 2 - œrodkowy rz¹d pikseli
	// W: uv - ox
	// current: uv
	// E: uv + ox

	PP = uv;
	CC = tex2D(ColorMapSampler, PP - ox);			float g10 = getGray(CC);
	CC = tex2D(ColorMapSampler, PP);				float g11 = getGray(CC);
	CC = tex2D(ColorMapSampler, PP + ox);			float g12 = getGray(CC);

	// 3 - górny rz¹d pikseli
	// NW: uv + oy - ox
	// N: uv + oy
	// NE: uv + oy + ox

	PP = uv + oy;
	CC = tex2D(ColorMapSampler, PP - ox);			float g20 = getGray(CC);
	CC = tex2D(ColorMapSampler, PP);				float g21 = getGray(CC);
	CC = tex2D(ColorMapSampler, PP + ox);			float g22 = getGray(CC);

	//Sobell filter:
	// -1	-2	-1
	// 0	0	0
	// 1	2	1

	float K00 = -1;
	float K01 = -2;
	float K02 = -1;
	float K10 = 0;
	float K11 = 0;
	float K12 = 0;
	float K20 = 1;
	float K21 = 2;
	float K22 = 1;

	//Obliczenie sx - jako wyniku g.ij * K.ij
	// otrzymamy poziom¹ krawêdŸ

	float sx = 0;
	sx += g00 * K00;
	sx += g01 * K01;
	sx += g02 * K02;
	sx += g10 * K10;
	sx += g11 * K11;
	sx += g12 * K12;
	sx += g20 * K20;
	sx += g21 * K21;
	sx += g22 * K22;

	//Obliczenie sy - jako wyniku g.ij * K.ji
	//K.ji obraca filtr, daj¹c pionow¹ krawêdŸ

	float sy = 0;
	sy += g00 * K00;
	sy += g01 * K10;
	sy += g02 * K20;
	sy += g10 * K01;
	sy += g11 * K11;
	sy += g12 * K21;
	sy += g20 * K02;
	sy += g21 * K12;
	sy += g22 * K22;

	//Polaczenie wynikow (pion & poziom) poprzez obliczenie dystansu jaki tworzy ich wektor
	float contrast = sqrt(sx*sx + sy*sy);

	//zalozenie braku krawedzi
	float result = 1;

	//Jezeli dlugosc s.xy ma wartosc wieksza niz threshold -> zmiana koloru (kontrastu) w tym pikselu pokazuje ze to krawedz.
	//Wyczerniamy ten piksel wartoœci¹ 0.
	if (contrast > Threshold)
	{
		result = 0;
	}

	//Zwrócenie oryginalnego koloru przemnozonego przez wynik. 
	//Dla wartosci kontrastu wyzszych od wartosci threshold bedzie to 0 -> czarna krawêdŸ
	return Color * float4(result.xxx, 1);
}
		technique PostOutline
		{
			pass Pass0
			{
				PixelShader = compile ps_2_0 PixelShaderFunction();
			}
		}

