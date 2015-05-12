﻿using System;

namespace FractalLibrary
{
	public class MandelbrotFractal : IteratingFractal
	{
		private MandelbrotIterator mIterator;

		private FractalVector2 mCenter = new FractalVector2(1.5f, 1.5f);
		private float mScale = 3f;

		public MandelbrotFractal ()
		{
		}

		public void SetCenter(float x, float y)
		{
			mCenter.x = x;
			mCenter.y = y;
		}

		private int Iterate(params FractalComplexNumber[] complexNos)
		{
			if (mIterator != null) {
				mIterator.Iterate (Iterations, complexNos);
				return mIterator.ReturnValue;
			}
			return 0;
		}

		public override void RefreshDataSamples ()
		{
			float scaleToUse = mScale / (mData.GetUpperBound (0) + 1);
			for (int y = 0; y <= mData.GetUpperBound (1); ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					FractalComplexNumber complexPoint = new FractalComplexNumber (x * scaleToUse - mCenter.x, y * scaleToUse - mCenter.y);
					int iterated = Iterate (new FractalComplexNumber (mInitialPoint), complexPoint);
					float value = 1;
					if (iterated > -1) {
						value = (float)iterated / Iterations;
					} 
					mData [x, y] = value;
				}
			}
			//throw new NotImplementedException ();
		}
	}

	public abstract class MandelbrotIterator
	{
		public int ReturnValue = -1;
		public abstract void Iterate(params object[] arguments);
	}

	public class QuadraticMandelbrotIterator : MandelbrotIterator
	{
		public override void Iterate (params object[] arguments)
		{
			FractalComplexNumber z = new FractalComplexNumber ();
			FractalComplexNumber c = (FractalComplexNumber)arguments [1];
			int iterations = (int)arguments [0];
			if (arguments.Length > 2)
			{
				z = (FractalComplexNumber)arguments [2];
			}
			for (int i = 0; i < iterations + 1; ++i)
			{
				z = z * z + c;
				if (z.Absolute > 2f)
				{
					ReturnValue = i;
					break;
				}
			}
			//throw new NotImplementedException ();
		}
	}
}

