using System;
using System.Collections.Generic;
using System.Linq;


namespace Whathecode.System.Algorithm
{
	public partial class Loop
	{
		/// <summary>
		///   Class which allows to hook operations into certain iterations of loop.
		///   TODO: Implement abstractly (generalized for other classes?) when there might be a need to.
		/// </summary>
		public class OperationHook
		{
			/// <summary>
			///   A wrapper to verify whether a hook should be executed or not.
			/// </summary>
			struct ConditionalHook
			{
				/// <summary>
				///   The condition which needs to validate to true in order for the hook to be executed.
				/// </summary>
				public Func<List<LoopIteration>, LoopIteration, bool> Condition;

				/// <summary>
				///   The hooked operation.
				/// </summary>
				public Action HookOperation;
			}


			readonly List<LoopIteration> _iterationList;
			readonly List<ConditionalHook> _hookedOperation = new List<ConditionalHook>();


			/// <summary>
			///   Create a new operation hook for a given loop.
			/// </summary>
			/// <param name = "iterationList">The list of iterations in which to hook.</param>
			public OperationHook( List<LoopIteration> iterationList )
			{
				_iterationList = iterationList;
			}


			/// <summary>
			///   Hook an operation the first iteration of the loop.
			/// </summary>
			/// <param name = "operation">The operation to be executed.</param>
			public void First( Action operation )
			{
				_hookedOperation.Add( new ConditionalHook
				{
					Condition = ( list, i ) => i.Index == 0,
					HookOperation = operation
				} );
			}

			/// <summary>
			///   Hook an operation at every iteration of the loop except the last.
			/// </summary>
			/// <param name = "operation">The operation to be executed.</param>
			public void AllButLast( Action operation )
			{
				_hookedOperation.Add( new ConditionalHook
				{
					Condition = ( list, i ) => i.Index != list.Count - 1,
					HookOperation = operation
				} );
			}

			/// <summary>
			///   Performs the actions for a given iteration, when available.
			/// </summary>
			/// <param name = "currentIteration">The current iteration to do actions for.</param>
			public void DoActions( LoopIteration currentIteration )
			{
				foreach ( var hookedOperation in _hookedOperation.Where( o => o.Condition( _iterationList, currentIteration ) ) )
				{
					hookedOperation.HookOperation();
				}
			}
		}
	}
}