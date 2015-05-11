﻿using System;
using FractalLibrary;
using System.Drawing;
using System.IO;

namespace CircleInversionTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Random random = new Random (System.DateTime.Now.Millisecond);
			CircleInversionFractal fractal = new CircleInversionFractal ();
			fractal.SetDataSize (512, 512);
			int noOfCircles = random.Next (3, 6);
			//fractal.Iterations = 100000;
			//fractal.SetInitialIterationPoint(5f,5f);
			fractal.SetNumberOfCircles (noOfCircles);
			fractal.RefreshDataSamples ();
			Color referenceColor = Color.Lavender;

			string currentDirectory = Directory.GetCurrentDirectory ();
			string exportPath = Path.Combine (currentDirectory, "circle_inversion.png");
			Bitmap bitmap = new Bitmap (512, 512);
			for (int y = 0; y < bitmap.Height; ++y) {
				for (int x = 0; x < bitmap.Width; ++x) {
					Color colorToSet = Color.FromArgb ((int)(fractal.Data [x, y] * 255), referenceColor);
					bitmap.SetPixel (x, y, colorToSet);
				}
			}
			bitmap.Save (exportPath, System.Drawing.Imaging.ImageFormat.Png);
			bitmap.Dispose ();
		}
	}
}
