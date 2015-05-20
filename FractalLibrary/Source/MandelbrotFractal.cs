using System;

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

		public void SetIterator(MandelbrotIterator it)
		{
			mIterator = it;
		}

		public override void RefreshDataSamples ()
		{
			float xStep = (mMaxX - mMinX) / (mData.GetUpperBound (0) + 1);
			float yStep = (mMaxY - mMinY) / (mData.GetUpperBound (1) + 1);
			for (int y = 0; y <= mData.GetUpperBound (1); ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					FractalComplexNumber complexPoint = new FractalComplexNumber (mMinX + (x * xStep) - mCenter.x, mMinY + (y * yStep) - mCenter.y);
//					Console.WriteLine (complexPoint.ToString ());
//					Console.ReadKey ();
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
}

