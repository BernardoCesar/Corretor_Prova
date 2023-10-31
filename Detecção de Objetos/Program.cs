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
			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Errado 1.png"),
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

				using (FileStream stream = new FileStream("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Errado Limiarizado.png", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}

			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Errado Limiarizado.png"),
				bitmapSaidaAritmetica = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {

				int largura = bitmapEntrada.Width;
				int altura = bitmapEntrada.Height;
				int tamanhoErosao = 5;
				List<(int x0, int y0 , int x1 ,int y1)> coordenadas = new List<(int x0, int y0 , int x1 ,int y1)> {(34, 34, 80, 80), (641, 34, 687, 80), (34, 498, 80, 543), (641, 498, 687, 543), (34, 961, 80, 1007), (641, 961, 687, 1007)};

				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saidaAritmetica = (byte*)bitmapSaidaAritmetica.GetPixels();
				    bool considerar8vizinhos = true;
					List<Forma> formasIndividuais = new List<Forma>();
					bool teste;

					for (int y = 0; y < altura -1 ; y++) {
						for (int x= 0; x < largura-1 ; x++) {
                            saidaAritmetica [y * largura + x ] = Erodir(entrada, largura,altura, x, y, tamanhoErosao);
						}
					}


					formasIndividuais=Forma.DetectarFormas(saidaAritmetica, largura, altura, considerar8vizinhos);
					for(int y=0; y<formasIndividuais.Count; y++){
						Console.WriteLine(formasIndividuais[y]);
						if(y<formasIndividuais.Count-1){
							teste=formasIndividuais[y].FazInterseccao(formasIndividuais[y+1].X0, formasIndividuais[y+1].Y0,formasIndividuais[y+1].X1, formasIndividuais[y+1].Y1);
							if(teste==true){
								Console.WriteLine("Gabarito errado");
							}
						}
					}

				for( int y=0; y<coordenadas.Count; y++){
					bool valido = Forma.ValidarGabarito(saidaAritmetica, largura, altura,coordenadas[y].x0, coordenadas[y].y0, coordenadas[y].x1, coordenadas[y].y1, considerar8vizinhos);
					if(valido == false){
						Console.WriteLine("Gabarito não é valido");
						break;
					}
				}



				}
					using (FileStream stream = new FileStream("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Errado Saida.png", FileMode.OpenOrCreate, FileAccess.Write)) {
						bitmapSaidaAritmetica.Encode(stream, SKEncodedImageFormat.Png, 100);
					}
			}
		}
	}
}
