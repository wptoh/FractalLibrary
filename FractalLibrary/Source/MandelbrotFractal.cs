using System;
using System.Threading;

namespace FractalLibrary
{
	public class MandelbrotFractal : IteratingFractal
	{
		public delegate void MandelbrotFractalIterateFunction(int numberOfIterations, out int returnValue, params FractalComplexNumber[] complexNos);
		private MandelbrotFractalIterateFunction mCurrentIterateFunction = null;

		private FractalVector2 mCenter = new FractalVector2(1.5f, 1.5f);

		private int mFinishedThreadCount = 0;

		private Mutex mMutex = null;

		public MandelbrotFractal ()
		{
		}

		public void SetIteratingFunction(MandelbrotFractalIterateFunction func)
		{
			mCurrentIterateFunction = func;
		}

		public void SetCenter(float x, float y)
		{
			mCenter.x = x;
			mCenter.y = y;
		}

		private int Iterate(params FractalComplexNumber[] complexNos)
		{
			/*
			if (mIterator != null) {
				mIterator.Iterate (Iterations, complexNos);
				return mIterator.ReturnValue;
			}
			*/
			int returnVal = 0;
			if (mCurrentIterateFunction != null) {
				mCurrentIterateFunction (Iterations, out returnVal, complexNos);
			}
			return returnVal;
		}

		public override void RefreshDataSamples ()
		{
			
			mFinishedThreadCount = 0;
			if (mMutex == null) {
				mMutex = new Mutex (false);
			}

			int cores = Math.Min (System.Environment.ProcessorCount, mData.GetUpperBound (1) + 1);
			int slice = (int)(Math.Floor((float)(mData.GetUpperBound (1) + 1) / cores));

			Console.WriteLine ("Number of parallel threads used: {0}", cores.ToString ());

			for (int i = 0; i < cores - 1; ++i) {
				FractalUtility.ThreadData td = new FractalUtility.ThreadData (slice * i, slice * (i + 1));
				ParameterizedThreadStart ts = new ParameterizedThreadStart (this.ThreadedIterate);
				Thread newthread = new Thread (ts);
				newthread.Start (td);
			}

			FractalUtility.ThreadData lasttd = new FractalUtility.ThreadData (slice * (cores - 2), mData.GetUpperBound (1) + 1);
			ThreadedIterate (lasttd);


			while (mFinishedThreadCount < cores) {
				Thread.Sleep (1);
			}


			/*
			float xStep = (mMaxX - mMinX) / (mData.GetUpperBound (0) + 1);
			float yStep = (mMaxY - mMinY) / (mData.GetUpperBound (1) + 1);
			for (int y = 0; y <= mData.GetUpperBound (1); ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					FractalComplexNumber complexPoint = new FractalComplexNumber (mMinX + (x * xStep) - mCenter.x, mMinY + (y * yStep) - mCenter.y);
					int iterated = Iterate (new FractalComplexNumber (mInitialPoint), complexPoint);
					float value = 1;
					if (iterated > -1) {
						value = (float)iterated / Iterations;
					} 
					mData [x, y] = value;
				}
			}
			*/
			//throw new NotImplementedException ();
		}

		private void ThreadedIterate(System.Object obj)
		{
			FractalUtility.ThreadData td = (FractalUtility.ThreadData)obj;
			float xStep = (mMaxX - mMinX) / (mData.GetUpperBound (0) + 1);
			float yStep = (mMaxY - mMinY) / (mData.GetUpperBound (1) + 1);
			for (int y = td.start; y < td.end; ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					FractalComplexNumber complexPoint = new FractalComplexNumber (mMinX + (x * xStep) - mCenter.x, mMinY + (y * yStep) - mCenter.y);
					int iterated = Iterate (new FractalComplexNumber (mInitialPoint), complexPoint);
					float value = 1;
					if (iterated >= 0) {
						value = (float)iterated / Iterations;
					} 

					mData [x, y] = value;
				}
			}
			mMutex.WaitOne ();
			mFinishedThreadCount++;
			mMutex.ReleaseMutex ();
		}
	}
}

