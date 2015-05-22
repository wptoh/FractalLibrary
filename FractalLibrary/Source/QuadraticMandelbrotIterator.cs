using System;

namespace FractalLibrary
{
	public class QuadraticMandelbrotIterator : MandelbrotIterator
	{
		private const int LIMITRADIUS = 2;

		public override void Iterate (params object[] arguments)
		{
			FractalComplexNumber z = new FractalComplexNumber ();
			FractalComplexNumber[] complexNos = (FractalComplexNumber[])arguments [1];
			int iterations = (int)arguments [0];
			if (complexNos.Length > 1)
			{
				z = complexNos [1];
			}
			FractalComplexNumber c = complexNos [0];
			for (int i = 1; i < iterations + 1; ++i)
			{
				z = z * z + c;
				if (z.Absolute > LIMITRADIUS)
				{
					ReturnValue = i;
					break;
				}
			}
			//throw new NotImplementedException ();
		}
	}
}

