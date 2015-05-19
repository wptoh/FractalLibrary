﻿using System;
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
			fractal.SetIterator (mandelbrotIterator);
			fractal.SetInitialIterationPoint (0.285f, 0.01f);
			fractal.SetCenter (1.5f, 1.5f);
			fractal.RefreshDataSamples ();

			Color referenceColor = Color.Firebrick;

			Bitmap bitmap = new Bitmap (2048, 2048);
			for (int y = 0; y < bitmap.Height; ++y) {
				for (int x = 0; x < bitmap.Width; ++x) {
					bitmap.SetPixel (x, y, Color.FromArgb ((int)(fractal.Data [x, y] * 255f), referenceColor));
				}
			}
			bitmap.Save ("mandelBrotTest.png", System.Drawing.Imaging.ImageFormat.Png);
			//Console.WriteLine ("Hello World!");
		}
	}
}
