using System;
using System.ComponentModel;
using System.Threading;

namespace FractalLibrary
{
	public class MandelbrotFractal : IteratingFractal
	{
		public delegate void MandelbrotFractalIterateFunction(int numberOfIterations, out float returnValue, params FractalComplexNumber[] complexNos);
		private MandelbrotFractalIterateFunction mCurrentIterateFunction = null;

		private FractalVector2 mCenter = new FractalVector2(1.5f, 1.5f);

		private int mFinishedThreadCount = 0;

		private Mutex mMutex = null;

		public bool UseGaussianSmooth = false;

		BackgroundWorker mWorker = new BackgroundWorker();

		public MandelbrotFractal ()
		{
			mWorker.WorkerSupportsCancellation = true;
			mWorker.DoWork += StartComputation;
			mWorker.RunWorkerCompleted += ComputationEnd;
		}

		~MandelbrotFractal()
		{
			mWorker.RunWorkerCompleted -= ComputationEnd;
			mWorker.DoWork -= StartComputation;
			mWorker.Dispose ();
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

		private float Iterate(params FractalComplexNumber[] complexNos)
		{
			float returnVal = -1f;
			if (mCurrentIterateFunction != null) {
				mCurrentIterateFunction (Iterations, out returnVal, complexNos);
			}
			return returnVal;
		}

		public override void RefreshDataSamples ()
		{
			if (!mWorker.IsBusy) {
				mWorker.RunWorkerAsync ();
			}
			//StartComputation ();
		}

		private void ComputationEnd(object sender, RunWorkerCompletedEventArgs args)
		{
			InvokeDataGeneratedComplete ();
		}

		private void StartComputation(object sender, DoWorkEventArgs args)
		{
			BackgroundWorker currentWorker = sender as BackgroundWorker;

			mFinishedThreadCount = 0;
			if (mMutex == null) {
				mMutex = new Mutex (false);
			}

			int coresToUse = System.Environment.ProcessorCount < 2 ? 1 : System.Environment.ProcessorCount - 1;
			int cores = Math.Min (coresToUse, mData.GetUpperBound (1) + 1);
			int slice = (int)(Math.Floor((float)(mData.GetUpperBound (1) + 1) / cores));

			//Console.WriteLine ("Number of parallel threads used: {0}", cores.ToString ());

			for (int i = 0; i < cores - 1; ++i) {
				FractalUtility.ThreadData td = new FractalUtility.ThreadData (slice * i, slice * (i + 1));
				ParameterizedThreadStart ts = new ParameterizedThreadStart (this.ThreadedIterate);
				Thread newthread = new Thread (ts);
				newthread.Start (td);
				if (currentWorker.CancellationPending)
					break;
			}

			FractalUtility.ThreadData lasttd = new FractalUtility.ThreadData (slice * (cores - 2), mData.GetUpperBound (1) + 1);
			ThreadedIterate (lasttd);


			while (mFinishedThreadCount < cores && !currentWorker.CancellationPending) {
				Thread.Sleep (1);
			}
		}

		private void ThreadedIterate(System.Object obj)
		{
			FractalUtility.ThreadData td = (FractalUtility.ThreadData)obj;
			float xStep = (mMaxX - mMinX) / (mData.GetUpperBound (0) + 1);
			float yStep = (mMaxY - mMinY) / (mData.GetUpperBound (1) + 1);
			for (int y = td.start; y < td.end; ++y) {
				for (int x = 0; x <= mData.GetUpperBound (0); ++x) {
					if (mWorker.CancellationPending)
						break;
					FractalComplexNumber complexPoint = new FractalComplexNumber (mMinX + (x * xStep) - mCenter.x, mMinY + (y * yStep) - mCenter.y);
					float iterated = Iterate (complexPoint, new FractalComplexNumber (mInitialPoint));
					float value = 1;
					if (iterated >= 0) {
						value = iterated;
					} 

					mData [x, y] = value;
				}
				if (mWorker.CancellationPending)
					break;
			}
			mMutex.WaitOne ();
			mFinishedThreadCount++;
			mMutex.ReleaseMutex ();
		}
	}
}

