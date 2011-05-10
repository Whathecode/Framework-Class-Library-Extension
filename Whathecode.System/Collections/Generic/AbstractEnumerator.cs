using System;
using System.Collections;
using System.Collections.Generic;


namespace Whathecode.System.Collections.Generic
{
    /// <summary>
    ///   A base abstract implementation which simplifies creating an enumerator for a type.
    /// </summary>
    /// <typeparam name = "T">The type returned by the enumerator.</typeparam>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractEnumerator<T> : IEnumerable<T>, IEnumerator<T>
    {
        /// <summary>
        ///   The current element which will be returned by the enumerator when one is available.
        /// </summary>
        T _current;

        /// <summary>
        ///   The amount of elements which were already enumerated.
        /// </summary>
        int _enumeratedAlready;

        /// <summary>
        ///   Indicates whether the enumerator is currently traversing elements or not.
        /// </summary>
        bool _isTraversing;

        /// <summary>
        ///   Inidicates whether there are any elements left to enumerate over.
        /// </summary>
        bool _hasMoreElements;


        protected AbstractEnumerator()
        {
            Reset();
        }


        public T Current
        {
            get
            {
                if ( !_isTraversing || !_hasMoreElements )
                {
                    throw new InvalidOperationException( "No value available." );
                }

                return _current;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }


        public bool MoveNext()
        {
            int previous = _enumeratedAlready;

            _hasMoreElements = _isTraversing
                                   ? HasMoreElements( previous, _current )
                                   : HasElements(); // First element, check whether any available at all.

            if ( _hasMoreElements )
            {
                _current = _isTraversing ? GetNext( previous, _current ) : GetFirst();
            }

            ++_enumeratedAlready;
            _isTraversing = true;

            return _hasMoreElements;
        }

        public void Reset()
        {
            _isTraversing = false;
        }


        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion  // IEnumerable


        #region Abstract Definitions        

        /// <summary>
        ///   Get the first element in the enumeration.
        /// </summary>
        /// <returns>The first element in the enumeration.</returns>
        protected abstract T GetFirst();

        /// <summary>
        ///   Get next element after the given previous element.
        /// </summary>
        /// <param name = "enumeratedAlready">The amount of elements enumerated already.</param>
        /// <param name = "previous">The last element over which was enumerated.</param>
        /// <returns>The next element in the enumeration.</returns>
        protected abstract T GetNext( int enumeratedAlready, T previous );

        /// <summary>
        ///   Checks whether the enumeration has any elements to enumerate over.
        /// </summary>
        /// <returns>True when elements are available, false otherwise.</returns>
        protected abstract bool HasElements();

        /// <summary>
        ///   Check whether any more elements are available.
        /// </summary>
        /// <param name = "enumeratedAlready">The amount of elements enumerated already.</param>
        /// <param name = "previous">The element previously in the enumeration, default value for the type when first.</param>
        /// <returns>True when more elements are available, false otherwise.</returns>
        protected abstract bool HasMoreElements( int enumeratedAlready, T previous );

        public abstract void Dispose();

        #endregion  // Abstract Definitions
    }
}