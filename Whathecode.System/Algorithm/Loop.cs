using System;
using System.Collections.Generic;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections;
using Whathecode.System.Collections.Generic;


namespace Whathecode.System.Algorithm
{
    /// <summary>
    ///   Class which helps setting up complex loops.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public partial class Loop : AbstractEnumerator<Loop.LoopIteration>
    {
        /// <summary>
        ///   List containing the actions to perform on iteration intervals.
        /// </summary>
        readonly IntervalCollection<int, Action> _iterations = new IntervalCollection<int, Action>();

        /// <summary>
        ///   List of all the iterations. Filled by using _iterations when enumerating.
        /// </summary>
        readonly List<LoopIteration> _enumerateIterations = new List<LoopIteration>();

        /// <summary>
        ///   Can be used to hook extra operations before specified iterations of the loop.
        /// </summary>
        public OperationHook Before { get; private set; }

        /// <summary>
        ///   Can be used to hook extra operations after specified iterations of the loop.
        /// </summary>
        public OperationHook After { get; private set; }


        Loop()
        {
            Before = new OperationHook( _enumerateIterations );
            After = new OperationHook( _enumerateIterations );
        }


        /// <summary>
        ///   Initialize an operation which is executed a number of times.
        /// </summary>
        /// <param name = "times">The amount of times to run the given operation.</param>
        /// <param name = "action">The operation to run.</param>
        /// <returns>A loop object which can control the created loop.</returns>
        public static Loop NumberOfTimes( int times, Action action )
        {
            Loop loop = new Loop();

            loop._iterations.Add( new Interval<int>( 0, times - 1 ), action );

            return loop;
        }


        /// <summary>
        ///   Runs all iterations of the loop.
        /// </summary>
        public void Run()
        {
            foreach ( var i in this )
            {
                Before.DoActions( i );

                i.Operation();

                After.DoActions( i );
            }
        }

        protected override LoopIteration GetFirst()
        {
            return _enumerateIterations[ 0 ];
        }

        protected override LoopIteration GetNext( int enumeratedAlready, LoopIteration previous )
        {
            return _enumerateIterations[ enumeratedAlready ];
        }

        protected override bool HasElements()
        {
            InitializeEnumeration();

            return _enumerateIterations.Count > 0;
        }

        protected override bool HasMoreElements( int enumeratedAlready, LoopIteration previous )
        {
            return enumeratedAlready < _enumerateIterations.Count;
        }

        void InitializeEnumeration()
        {
            _enumerateIterations.Clear();

            foreach ( var objectRange in _iterations )
            {
                // Create action for the objectRange.
                Action iteration;
                if ( objectRange.Values.Count == 1 )
                {
                    // This iteration has to perform just one action.
                    iteration = objectRange.Values[ 0 ];
                }
                else if ( objectRange.Values.Count > 1 )
                {
                    // Perform multiple actions.
                    IList<Action> actions = objectRange.Values;
                    iteration = () =>
                    {
                        foreach ( var a in actions )
                        {
                            a();
                        }
                    };
                }
                else
                {
                    // Nothing to do.
                    iteration = () => { };
                }

                // Add action for every step in the interval.
                objectRange.Interval.EveryStepOf( 1, i => _enumerateIterations.Add(
                    new LoopIteration
                    {
                        Index = i,
                        Operation = iteration
                    } )
                    );
            }
        }

        public override void Dispose()
        {
            // TODO: Nothing to do?
        }
    }
}