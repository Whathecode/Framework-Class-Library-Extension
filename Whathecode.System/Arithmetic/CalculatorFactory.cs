using System;
using System.Collections.Generic;
using Lambda.Generic.Arithmetic;
using Whathecode.System.Reflection.Emit;


namespace Whathecode.System.Arithmetic
{
    /// <summary>
    ///   A factory to create calculators for supported math types.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public static class CalculatorFactory
    {
        /// <summary>
        ///   Delegate used to create instances of the calculators.
        /// </summary>
        /// <returns></returns>
        delegate object CreateCalculatorDelegate();


        /// <summary>
        ///   Whether to use checked math or not for integral types.
        /// </summary>
        public enum CheckedOption
        {
            Checked,
            Unchecked
        }


        static readonly Dictionary<Type, CreateCalculatorDelegate> IntegerCalculators = new Dictionary<Type, CreateCalculatorDelegate>
        {
            // Unsigned math.
            { typeof( Byte ), () => new ByteMath() },
            { typeof( UInt16 ), () => new UShortMath() },
            { typeof( UInt32 ), () => new UIntMath() },
            { typeof( UInt64 ), () => new ULongMath() },
            // Signed math.
            { typeof( SByte ), () => new SByteMath() },
            { typeof( Int16 ), () => new ShortMath() },
            { typeof( Int32 ), () => new IntMath() },
            { typeof( Int64 ), () => new LongMath() },
        };

        static readonly Dictionary<Type, CreateCalculatorDelegate> IntegerCheckedCalculators = new Dictionary<Type, CreateCalculatorDelegate>
        {
            // Unsigned math.
            { typeof( Byte ), () => new CheckedByteMath() },
            { typeof( UInt16 ), () => new CheckedUShortMath() },
            { typeof( UInt32 ), () => new CheckedUIntMath() },
            { typeof( UInt64 ), () => new CheckedULongMath() },
            // Signed math.
            { typeof( SByte ), () => new CheckedSByteMath() },
            { typeof( Int16 ), () => new CheckedShortMath() },
            { typeof( Int32 ), () => new CheckedIntMath() },
            { typeof( Int64 ), () => new CheckedLongMath() },
        };

        static readonly Dictionary<Type, CreateCalculatorDelegate> RationalCalculators = new Dictionary<Type, CreateCalculatorDelegate>
        {
            { typeof( Single ), () => new FloatMath() }, // Doesn't support 'checked' operations.
            { typeof( Double ), () => new DoubleMath() }, // Doesn't support 'checked' operations.
            { typeof( Decimal ), () => new DecimalMath() } // Is always 'checked' operation by default anyhow.
        };


        /// <summary>
        ///   Create a basic calculator for a given type.
        /// </summary>
        /// <typeparam name = "TMath">The type for which to create a calculator.</typeparam>
        /// <param name = "checkedOption">Use checked math or not for integral types.</param>
        /// <returns>A calculator for the given type.</returns>
        public static IMath<TMath> CreateBasicCalculator<TMath>( CheckedOption checkedOption )
        {
            // TODO: Support fuzzy logic calculators?

            Dictionary<Type, CreateCalculatorDelegate> integerCalculators = GetIntegerCalculators( checkedOption );

            // Based on the type, get a correct calculator.
            Type mathType = GetMathType<TMath>();
            if ( integerCalculators.ContainsKey( mathType ) )
            {
                return integerCalculators[ mathType ]() as IMath<TMath>;
            }
            if ( RationalCalculators.ContainsKey( mathType ) )
            {
                return RationalCalculators[ mathType ]() as IMath<TMath>;
            }
            throw new NotSupportedException(
                "The factory can't create a calculator for type \"" + mathType + " (" + checkedOption + ")\"."
                );
        }

        /// <summary>
        ///   Create an integral calculator for a given type.
        /// </summary>
        /// <typeparam name = "TMath">The type for which to create a calculator.</typeparam>
        /// <param name = "checkedOption">Use checked math or not for integral types.</param>
        /// <returns>A calculator for the given type.</returns>
        public static IIntegerMath<TMath> CreateIntegerCalculator<TMath>( CheckedOption checkedOption )
        {
            Dictionary<Type, CreateCalculatorDelegate> integerCalculators = GetIntegerCalculators( checkedOption );

            // Based on the type, get a correct calculator.
            Type mathType = GetMathType<TMath>();
            if ( integerCalculators.ContainsKey( mathType ) )
            {
                object calculator = integerCalculators[ mathType ]();
                return EmitHelper.CreateCompatibleGenericWrapper<IIntegerMath<TMath>>( calculator );
            }
            throw new NotSupportedException(
                "The factory can't create a calculator for type \"" + mathType + " (" + checkedOption + ")\"."
                );
        }

        /// <summary>
        ///   Depending on whether to use checked math or not, return correct list of integer calculators.  
        /// </summary>
        /// <param name = "checkedOption">Use checked math or not for integral types.</param>
        /// <returns>The correct calculator initializer dictionary based on the specified option.</returns>
        static Dictionary<Type, CreateCalculatorDelegate> GetIntegerCalculators( CheckedOption checkedOption )
        {
            switch ( checkedOption )
            {
                case CheckedOption.Checked:
                    return IntegerCalculators;
                case CheckedOption.Unchecked:
                    return IntegerCheckedCalculators;
                default:
                    throw new NotSupportedException();
            }            
        }

        /// <summary>
        ///   Get the math type for the specified type. Underlying type is used for enums.
        /// </summary>
        /// <typeparam name="T">The type to get the math type for.</typeparam>
        static Type GetMathType<T>()
        {
            Type type = typeof( T );

            return type.IsEnum ? type.GetEnumUnderlyingType() : type;
        }
    }
}