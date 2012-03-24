using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;


namespace Whathecode.System.Experimental.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   TODO: This doesn't seem to be working for InitializeEventHandlersAttribute (Whathecode.System.Aspects)
		///         where it was intended to be used.
		///   Determines whether this constructor calls another constructor which is specified in this class.
		///   Base constructors aren't considered.
		/// </summary>
		/// <param name = "constructor">The constructor to verify whether it calls another constructor.</param>
		/// <returns>True when this constructor calls another constructor within the class; false otherwise.</returns>
		public static bool CallsOtherConstructor( this ConstructorInfo constructor )
		{
			MethodBody body = constructor.GetMethodBody();
			if ( body == null )
			{
				throw new ArgumentException( "Constructors are expected to always contain byte code." );
			}

			// Constructors at the end of the invocation chain start with 'call' immediately.
			var untilCall = body.GetILAsByteArray().TakeWhile( b => b != OpCodes.Call.Value );
			return untilCall.Count() != 0 && !untilCall.All( b =>
				b == OpCodes.Nop.Value ||     // Never encountered, but my intuition tells me a no-op would be valid.
				b == OpCodes.Ldarg_0.Value || // Seems to always precede Call immediately.
				b == OpCodes.Ldarg_1.Value    // Seems to be added when calling base constructor.
				);
		}
	}
}
