using System;

namespace FractalLibrary
{
	public class IteratingFractal : BaseFractal
	{
		public int Iterations = 100000;

		protected FractalVector2 mInitialPoint = new FractalVector2(-5f, -5f);

		public void SetInitialIterationPoint(float x, float y)
		{
			mInitialPoint.x = x;
			mInitialPoint.y = y;
		}

		public override void RefreshDataSamples ()
		{
			throw new NotImplementedException ();
		}
	}
}

