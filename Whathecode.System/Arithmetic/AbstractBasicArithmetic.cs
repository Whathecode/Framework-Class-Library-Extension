using System;
using System.Diagnostics.Contracts;
using Lambda.Generic.Arithmetic;


namespace Whathecode.System.Arithmetic
{
    /// <summary>
    ///   An abstract base class which can be used by extending classes to allow for basic generic math arithmetic.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public abstract class AbstractBasicArithmetic<TMath>
    {
        /// <summary>
        ///   Calculator which can be used for arithmetic operations on type TMath.
        /// </summary>
        protected IMath<TMath> Calculator { get; private set; }


        /// <summary>
        ///   Create a new arithmetic base class, using a factory to initialize the calculator with default supported types.
        /// </summary>
        /// <exception cref = "NotSupportedException">Thrown when the factory can't create a calculator for the requested type.</exception>
        protected AbstractBasicArithmetic()
            : this( CalculatorFactory.CreateBasicCalculator<TMath>( CalculatorFactory.CheckedOption.Unchecked ) ) {}

        /// <summary>
        ///   Create a new arithmetic base class, using a specified custom calculator to do operations on the specified type.
        /// </summary>
        /// <param name = "calculator">The custom calculator used to do operations on the specified type.</param>
        protected AbstractBasicArithmetic( IMath<TMath> calculator )
        {
            Contract.Requires( calculator != null );

            Calculator = calculator;
        }
    }
}