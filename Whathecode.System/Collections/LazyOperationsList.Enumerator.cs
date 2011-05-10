using System.Collections.Generic;
using Whathecode.System.Collections.Generic;


namespace Whathecode.System.Collections
{
    public partial class LazyOperationsList<TObject>
    {
        public class Enumerator : AbstractEnumerator<TObject>
        {
            readonly List<TObject> _list;


            public Enumerator( List<TObject> list )
            {
                _list = list;
            }


            protected override TObject GetFirst()
            {
                return _list[ 0 ];
            }

            protected override TObject GetNext( int enumeratedAlready, TObject previous )
            {
                return _list[ enumeratedAlready ];
            }

            protected override bool HasElements()
            {
                return _list.Count > 0;
            }

            protected override bool HasMoreElements( int enumeratedAlready, TObject previous )
            {
                return enumeratedAlready < _list.Count;
            }

            public override void Dispose()
            {
                // TODO: Nothing to do?
            }
        }
    }
}