using System;

namespace FractalLibrary
{
	public static class FractalUtility
	{
		public class ThreadData
		{
			public int start;
			public int end;
			public ThreadData (int s, int e)
			{
				start = s;
				end = e;
			}
		}
	}
}

