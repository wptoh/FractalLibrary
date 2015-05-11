using System;

namespace FractalLibrary
{
	public class CircleInversionFractal : BaseFractal
	{
		private class Circle
		{
			public FractalVector2 Center;
			public float Radius;
		}

		private Random mRandom;

		public int Iterations = 100000;
			
		private int mNumberOfMainCircles = 3;
		private float mViewportExtents = 1f;

		protected FractalVector2 mInitialPoint = new FractalVector2(-5f, -5f);

		private Circle[] mMainCircles;

		public CircleInversionFractal()
		{
			mRandom = new Random (System.DateTime.Now.Millisecond);
		}

		public void SetInitialIterationPoint(float x, float y)
		{
			mInitialPoint.x = x;
			mInitialPoint.y = y;
		}

		public CircleInversionFractal(int mainCircleCount)
		{
			mNumberOfMainCircles = mainCircleCount;
		}

		public void SetNumberOfCircles(int mainCircleCount)
		{
			mNumberOfMainCircles = mainCircleCount;
		}

		private void PopulateCircles()
		{
			float startAngle = (float)Math.PI * 2f / mNumberOfMainCircles;
			float radiusOfMainCircles = (float)(Math.Sin (startAngle) / Math.Sin (((float)Math.PI - startAngle) * 0.5f)) * 0.5f;
			mViewportExtents = (float)Math.Sqrt (1f - (radiusOfMainCircles * radiusOfMainCircles));
			mMainCircles = new Circle[mNumberOfMainCircles + 1];
			mMainCircles [0] = new Circle () {
				Center = new FractalVector2(0,0),
				Radius = 1f - radiusOfMainCircles,
			};

			for (int i = 0; i < mNumberOfMainCircles; ++i) {
				mMainCircles [i + 1] = new Circle () {
					Center = new FractalVector2 ((float)Math.Sin (i * startAngle), (float)Math.Cos (i * startAngle)),
					Radius = radiusOfMainCircles,
				};
			}
		}

		private void PopulateData()
		{
			FractalVector2 interpolatedPoint = mInitialPoint;
			for (int i = 0; i < Iterations; ++i) {
				int selectedCircle = mRandom.Next (mNumberOfMainCircles + 1);
				FractalVector2 delta = interpolatedPoint - mMainCircles [selectedCircle].Center;
				float deltaMagnitude = delta.Magnitude;
				FractalVector2 deltaNormalized = delta.Normalize ();
				float deltaRadius = mMainCircles [selectedCircle].Radius * mMainCircles [selectedCircle].Radius / deltaMagnitude;
				interpolatedPoint = deltaNormalized * deltaRadius + mMainCircles [selectedCircle].Center;
				int coordX = (int)(mData.GetUpperBound (0) * (interpolatedPoint.x + mViewportExtents) / (mViewportExtents * 2f));
				int coordY = (int)(mData.GetUpperBound (1) * (interpolatedPoint.y + mViewportExtents) / (mViewportExtents * 2f));
				try
				{
					mData[coordX,coordY] = 1f;
				}
				catch(Exception) {
					continue;
				}
			}
		}

		public override void RefreshDataSamples ()
		{
			PopulateCircles ();
			PopulateData ();
			//throw new NotImplementedException ();
		}
	}
}

