using Whathecode.System.Collections;
using Whathecode.System.Collections.Generic;


namespace Whathecode.System.Arithmetic.Interpolation.KeyPoint
{
	public partial class CumulativeKeyPointCollection<TValue, TMath>
	{
		public class Enumerator : AbstractEnumerator<TValue>
		{
			readonly LazyOperationsList<KeyPoint> _data;


			public Enumerator( LazyOperationsList<KeyPoint> data )
			{
				_data = data;
			}


			protected override TValue GetFirst()
			{
				return _data[ 0 ].Value;
			}

			protected override TValue GetNext( int enumeratedAlready, TValue previous )
			{
				return _data[ enumeratedAlready ].Value;
			}

			protected override bool HasElements()
			{
				return _data.Count > 0;
			}

			protected override bool HasMoreElements( int enumeratedAlready, TValue previous )
			{
				return enumeratedAlready < _data.Count;
			}

			public override void Dispose()
			{
				// TODO: Nothing to do?
			}
		}
	}
}