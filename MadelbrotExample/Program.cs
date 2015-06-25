using System;
using System.Drawing;
using FractalLibrary;

namespace MadelbrotExample
{
	class MainClass
	{
		private static MandelbrotFractal _fractal = new MandelbrotFractal();
		public static void Main (string[] args)
		{
			_fractal.UseGaussianSmooth = true;
			_fractal.Iterations = 100;
			_fractal.SetDataSize (1024, 1024);
			_fractal.SetBounds (-1f, -1f, 1f, 1f);
			_fractal.SetIteratingFunction (FractalUtility.QuadMandelbrotIterate);
			_fractal.SetInitialIterationPoint (0.3f, 0.6f);
			_fractal.SetCenter (0f, 0f);
			_fractal.OnDataGenerated += OnSamplesGenerated;
			_fractal.RefreshDataSamples ();

			Console.ReadKey ();
			//Console.WriteLine ("Hello World!");
		}

		private static void OnSamplesGenerated()
		{
			Color referenceColor = Color.Firebrick;

			Bitmap bitmap = new Bitmap (1024, 1024);
			for (int y = 0; y < bitmap.Height; ++y) {
				for (int x = 0; x < bitmap.Width; ++x) {
					int value = (int)(_fractal.Data [x, y] * 255f);

					bitmap.SetPixel (x, y, Color.FromArgb(value, referenceColor));
				}
			}
			bitmap.Save ("mandelBrotTest2.png", System.Drawing.Imaging.ImageFormat.Png);
			Console.WriteLine ("Completed");
		}
	}
}
