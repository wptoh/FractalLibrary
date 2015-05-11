using System;

namespace FractalLibrary
{
	public abstract class BaseFractal
	{
		protected float[,] mData = new float[0,0];
		public float[,] Data
		{
			get {
				return mData;
			}
		}

		protected FractalVector2 mInitialPoint = new FractalVector2(-5f, -5f);

		public BaseFractal ()
		{
		}

		public void SetInitialIterationPoint(float x, float y)
		{
			mInitialPoint.x = x;
			mInitialPoint.y = y;
		}

		public void SetDataSize(int sizeX, int sizeY)
		{
			mData = new float[sizeX, sizeY];
		}

		public abstract void RefreshDataSamples ();
	}
}

