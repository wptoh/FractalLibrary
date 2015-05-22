using System;

namespace FractalLibrary
{
	public abstract class BaseFractal
	{
		protected float[,] mData = new float[0,0];
		protected float mMinX, mMinY, mMaxX, mMaxY;
		public float Max_X
		{
			get{ return mMaxX; }
		}

		public float Min_X
		{
			get{ return mMinX; }
		}

		public float Max_Y
		{
			get{ return mMaxY; }
		}

		public float Min_Y
		{
			get{ return mMinY; }
		}

		public float[,] Data
		{
			get {
				return mData;
			}
		}

		public void SetBounds(float minX, float minY, float maxX, float maxY)
		{
			mMinX = minX;
			mMinY = minY;
			mMaxX = maxX;
			mMaxY = maxY;
		}

		public void SetDataSize(int sizeX, int sizeY)
		{
			mData = new float[sizeX, sizeY];
		}

		public abstract void RefreshDataSamples ();
	}
}

