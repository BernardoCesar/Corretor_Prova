using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {
			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 1.png"),
				bitmapSaida = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {
				
				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saida = (byte*)bitmapSaida.GetPixels();

					long media= 0;
					int pixelsTotais = bitmapEntrada.Width * bitmapEntrada.Height;

					for (int e = 0, s = 0; s < pixelsTotais; e += 4, s++) {
						saida[s] = (byte)((entrada[e] +  entrada[e + 1] + entrada[e + 2] )/3);
						media += saida[s];
					}	
					media = (byte)(media /pixelsTotais);	

					for (int s = 0; s < pixelsTotais; s++) {
						if ( saida[s] > media){
							saida[s] = 0;
						}else{
							saida[s] = 255;
						};
					}	
					
							
				}

				using (FileStream stream = new FileStream("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto Saida.png", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}
		}
	}
}
