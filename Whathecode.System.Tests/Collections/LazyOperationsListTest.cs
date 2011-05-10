using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections;


namespace Whathecode.System.Tests.Collections
{
    /// <summary>
    ///   Unit tests for <see href="System.Collections.LazyOperationsList{T}">LazyOperationsList</see>.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [TestClass]
    public class LazyOperationsListTest
    {
        #region Common Test Members

        LazyOperationsList<int> _list;
        List<int> _originalData;
        Func<int, int> _operation;
        List<Interval<int>> _ranges;

        [TestInitialize]
        public void InitializeTest()
        {
            // Create list and original data.
            _list = new LazyOperationsList<int>();
            _originalData = new List<int>
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };

            // Add all data to list.
            foreach ( var d in _originalData )
            {
                _list.Add( d );
            }

            // Create ranges on which operations will be applied.
            _ranges = new List<Interval<int>>
            {
                new Interval<int>( 0, 1 ),
                new Interval<int>( 3, 4 ),
                new Interval<int>( 1, 3 ), // Overlaps two.
                new Interval<int>( 7, 8 ),
                new Interval<int>( 8, 9 ), // Overlap right.
                new Interval<int>( 6, 7 ), // Overlap left.
                new Interval<int>( 5, 5 ), // Bordering.
                new Interval<int>( 1, 9 ) // Lots of intersections.
            };

            // Create operation which is done for every range.
            _operation = i => { return i + 1; };
        }

        void PerformAllOperations()
        {
            foreach ( var range in _ranges )
            {
                // Perform on original data.
                for ( int i = range.Start; i <= range.End; ++i )
                {
                    _originalData[ i ] = _operation( _originalData[ i ] );
                }

                // Add to list.
                _list.AddOperation( _operation, range );
            }
        }

        void VerifyData()
        {
            for ( int i = 0; i < _originalData.Count; ++i )
            {
                Assert.AreEqual( _originalData[ i ], _list[ i ] );
            }
        }

        #endregion  // Common Test Members


        [TestMethod]
        public void FlushPendingOperationsTest()
        {
            PerformAllOperations();

            _list.FlushPendingOperations();

            VerifyData();
        }

        /// <summary>
        ///   Test adding operations, and compare values with expected values after retrieving.
        /// </summary>
        [TestMethod]
        public void AddOperationTest()
        {
            PerformAllOperations();

            VerifyData();
            VerifyData(); // Check second time to verify correct multiple access.
        }

        [TestMethod]
        public void RemoveTest()
        {
            PerformAllOperations();

            Random rand = new Random();

            while ( _originalData.Count > 0 )
            {
                // Remove.
                int removeIndex = rand.Next( _originalData.Count );
                _originalData.RemoveAt( removeIndex );
                _list.RemoveAt( removeIndex );

                // Verify.
                for ( int i = 0; i < _originalData.Count; ++i )
                {
                    Assert.AreEqual( _originalData[ i ], _list[ i ] );
                }
            }
        }
    }
}