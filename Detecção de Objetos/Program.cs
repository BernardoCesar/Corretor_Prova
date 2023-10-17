using SkiaSharp;

namespace Projeto {
	class Program {
		static void Main(string[] args) {
			using (SKBitmap bitmap = SKBitmap.Decode("Caminho da imagem de entrada")) {
				Console.WriteLine(bitmap.ColorType);

				unsafe {
					byte* ptr = (byte*)bitmap.GetPixels();

					ptr[0] = 0; // B
					ptr[1] = 255; // G
					ptr[2] = 255; // R
					ptr[3] = 255; // A
				}

				using (FileStream stream = new FileStream("Caminho da imagem de saída", FileMode.OpenOrCreate, FileAccess.Write)) {
					bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
				}
			}
		}
	}
}
