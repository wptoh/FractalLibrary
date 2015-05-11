using System;

namespace FractalLibrary
{
	public class MandelbrotFractal : IteratingFractal
	{
		private MandelbrotIterator mIterator;

		private FractalVector2 mCenter = new FractalVector2(1.5, 1.5f);
		private float mScale = 3f;

		public MandelbrotFractal ()
		{
		}

		public void SetCenter(float x, float y)
		{
			mCenter.x = x;
			mCenter.y = y;
		}

		private void Iterate()
		{
			for (int i = 0; i < Iterations + 1; ++i) {

				if (mIterator.ReturnValue > -1) {
					break;
				}
				mIterator.Iterate ();
			}
			
		}

		public override void RefreshDataSamples ()
		{
			float scaleToUse = mScale / (mData.GetUpperBound (0) + 1);
			for (int y = 0; y <= mData.GetUpperBound (1); ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					
				}
			}
			//throw new NotImplementedException ();
		}
	}

	public abstract class MandelbrotIterator
	{
		public int ReturnValue = -1;
		public abstract void Iterate(params FractalVector2[] vectors);
	}
}

