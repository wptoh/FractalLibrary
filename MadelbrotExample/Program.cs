using System;
using System.Drawing;
using FractalLibrary;

namespace MadelbrotExample
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			QuadraticMandelbrotIterator mandelbrotIterator = new QuadraticMandelbrotIterator ();
			MandelbrotFractal fractal = new MandelbrotFractal ();
			fractal.Iterations = 100;
			fractal.SetDataSize (2048, 2048);
			fractal.SetBounds (-1f, -1f, 1f, 1f);
			fractal.SetIterator (mandelbrotIterator);
			fractal.SetInitialIterationPoint (0.3f, 0.6f);
			fractal.SetCenter (0f, 0f);
			fractal.RefreshDataSamples ();

			Color referenceColor = Color.Firebrick;

			Bitmap bitmap = new Bitmap (2048, 2048);
			for (int y = 0; y < bitmap.Height; ++y) {
				for (int x = 0; x < bitmap.Width; ++x) {
					bitmap.SetPixel (x, y, Color.FromArgb ((int)(fractal.Data [x, y] * 255f), referenceColor));
				}
			}
			bitmap.Save ("mandelBrotTest2.png", System.Drawing.Imaging.ImageFormat.Png);
			//Console.WriteLine ("Hello World!");
		}
	}
}
