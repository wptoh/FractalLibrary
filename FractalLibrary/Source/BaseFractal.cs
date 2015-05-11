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

		public void SetDataSize(int sizeX, int sizeY)
		{
			mData = new float[sizeX, sizeY];
		}

		public abstract void RefreshDataSamples ();
	}
}

