using SkiaSharp;

namespace Projeto {
	class Program {

		static unsafe byte Erodir(byte* entrada, int largura, int altura, int x , int y , int tamanhoErosao){
			int yInicial = y - tamanhoErosao;
			int xInicial = x - tamanhoErosao;
			int yFinal = y + tamanhoErosao;
			int xFinal = x + tamanhoErosao;

			if (xInicial < 0) {
				xInicial = 0;
			}
			if (yInicial < 0) {
				yInicial = 0;
			}
			if (xFinal > largura - 1) {
				xFinal = largura - 1;
			}
			if (yFinal > altura - 1) {
				yFinal = altura - 1;
			}

			byte menor = entrada[(yInicial * largura) + xInicial];

			for(y = yInicial; y <= yFinal; y++){
				for(x = xInicial; x <= xFinal; x++){
					int i = (y * largura) + x;
					if( entrada[i] < menor ){
						menor = entrada[i];
					}
				}
			}
			return menor;
		}

		static void Main(string[] args) {
			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 1.png"),
				bitmapSaida = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {
				
				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saida = (byte*)bitmapSaida.GetPixels();

					int largura = bitmapEntrada.Width;
					int altura = bitmapEntrada.Height;
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

				using (FileStream stream = new FileStream("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto Limiarizado.png", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}

			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto Limiarizado.png"),
				bitmapSaidaAritmetica = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {
					
				int largura = bitmapEntrada.Width;
				int altura = bitmapEntrada.Height;
				int tamanhoErosao = 5;

				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saidaAritmetica = (byte*)bitmapSaidaAritmetica.GetPixels();		

					for (int y = 0; y < altura -1 ; y++) {
						for (int x= 0; x < largura-1 ; x++) {
                            saidaAritmetica [y * largura + x ] = Erodir(entrada, largura,altura, x, y, tamanhoErosao);
						}
					}					
				}
					using (FileStream stream = new FileStream("C:\\Users\\HENRIQUE.CORT\\Desktop\\ComputaçãoCognitiva\\ATV_Grupo_1\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto Saida.png", FileMode.OpenOrCreate, FileAccess.Write)) {
						bitmapSaidaAritmetica.Encode(stream, SKEncodedImageFormat.Png, 100);
					}
			}	
		}
	}
}
