using System.Globalization;
using System.Runtime.CompilerServices;
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
			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 2.png"),
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

				using (FileStream stream = new FileStream("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 2 Limiarizado.png", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmapSaida.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}

			using (SKBitmap bitmapEntrada = SKBitmap.Decode("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 2 Limiarizado.png"),
				bitmapSaidaAritmetica = new SKBitmap(new SKImageInfo(bitmapEntrada.Width, bitmapEntrada.Height, SKColorType.Gray8))) {

				int largura = bitmapEntrada.Width;
				int altura = bitmapEntrada.Height;
				int tamanhoErosao = 5;
				List<(int x0, int y0 , int x1 ,int y1)> coordenadas = new List<(int x0, int y0 , int x1 ,int y1)> {(34, 34, 80, 80), (641, 34, 687, 80), (34, 498, 80, 543), (641, 498, 687, 543), (34, 961, 80, 1007), (641, 961, 687, 1007)};
				bool validacao_gabarito;

				unsafe {
					byte* entrada = (byte*)bitmapEntrada.GetPixels();
					byte* saidaAritmetica = (byte*)bitmapSaidaAritmetica.GetPixels();
				    bool considerar8vizinhos = true;
					//List<Forma> formasIndividuais_questoes = new List<Forma>();
					List<Forma> formasIndividuais = new List<Forma>();

					for (int y = 0; y < altura -1 ; y++) {
						for (int x= 0; x < largura-1 ; x++) {
                            saidaAritmetica [y * largura + x ] = Erodir(entrada, largura,altura, x, y, tamanhoErosao);
						}
					}

					//Ex5

					//formasIndividuais_questoes=Forma.DetectarFormas(entrada, largura, altura, considerar8vizinhos);
					//for(int y=0; y<formasIndividuais_questoes.Count; y++){
					//	Console.WriteLine(formasIndividuais_questoes[y]);
					//}

					formasIndividuais=Forma.DetectarFormas(saidaAritmetica, largura, altura, considerar8vizinhos);
					//Ex6
					validacao_gabarito=Forma.ValidarGabarito(formasIndividuais, coordenadas);

					if(validacao_gabarito==true){

						List<int> alternativa_x0 = new List<int> { 206, 271, 336, 401, 467 };
						List<int> alternativa_y0 = new List<int> { 329, 393, 458, 523, 587, 652, 717, 781, 846, 911 };
						List<int> alternativa_x1 = new List<int> { 253, 318, 383, 449, 514 };
						List<int> alternativa_y1 = new List<int> { 376, 441, 505, 570, 635, 699, 764, 829, 893, 958 };

						Console.WriteLine("Gabarito Correto!");
						Dictionary<int, List<string>> questoes= new Dictionary<int, List<string>>();
						for(int y = 1; y < 11; y++){
							questoes.Add(y, new List<string>());
						}

						for (int forma = 0; forma < formasIndividuais.Count; forma++){
							for (int y = 0; y < 10; y++){
								for (int x = 0; x < 5; x++){
									if(x==0 && formasIndividuais[forma].FazInterseccao(alternativa_x0[x], alternativa_y0[y], alternativa_x1[x], alternativa_y1[y]) == true){
										questoes[y+1].Add("A");
									}else if(x==1 && formasIndividuais[forma].FazInterseccao(alternativa_x0[x], alternativa_y0[y], alternativa_x1[x], alternativa_y1[y]) == true){
										questoes[y+1].Add("B");
									}else if(x==2 && formasIndividuais[forma].FazInterseccao(alternativa_x0[x], alternativa_y0[y], alternativa_x1[x], alternativa_y1[y]) == true){
										questoes[y+1].Add("C");
									}else if(x==3 && formasIndividuais[forma].FazInterseccao(alternativa_x0[x], alternativa_y0[y], alternativa_x1[x], alternativa_y1[y]) == true){
										questoes[y+1].Add("D");
									}else if(x==4 && formasIndividuais[forma].FazInterseccao(alternativa_x0[x], alternativa_y0[y], alternativa_x1[x], alternativa_y1[y]) == true){
										questoes[y+1].Add("E");
									}
								}
							}
						}

					for(int y = 1; y < 11; y++){
						if(questoes[y].Count == 0){
							Console.WriteLine("Questão " + y + ": Nenhuma alternativa");
						}else{
							Console.Write("Questão " + y + ":");
							for(int x = 0; x < questoes[y].Count; x++){
								if(x < questoes.Count-1){
									Console.Write(questoes[y][x] + " ");
								}
							}
							Console.Write("\n");
						}
					}

					}else{
						Console.WriteLine("Gabarito errado");
					}
				}
					using (FileStream stream = new FileStream("C:\\Users\\bernardo.figueiredo\\Corretor_Prova\\Detecção de Objetos\\Gabarito Correto 2 Saida.png", FileMode.OpenOrCreate, FileAccess.Write)) {
						bitmapSaidaAritmetica.Encode(stream, SKEncodedImageFormat.Png, 100);
					}
			}
		}
	}
}
