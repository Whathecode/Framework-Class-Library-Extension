// Copyright (c) 2004, Rüdiger Klaehn
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of lambda computing nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System.Collections.Generic;


namespace Lambda.Generic.Arithmetic
{
    /// <summary>
    /// Provides methods like Sum, Average, Max, Min, Sigma for Lists of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that is used in the lists.</typeparam>
    /// <typeparam name="C">The type providing the arithmetic operations for <typeparamref name="T"></typeparamref>.</typeparam>
    public static class Lists<T, C>
        where C : 
            IAdder<T>,ISubtracter<T>,IDivider<T>,IHasRoot<T>,
            IMinValueProvider<T>,IMaxValueProvider<T>,IZeroProvider<T>,
            IConversionProvider<T>, IComparer<T>, new()
    {
        /// <summary>
        /// An instance of the calculator. We will use this to perform the calculations.
        /// </summary>
        private static C c = new C();
        /// <summary>
        /// The sum of all elements in the <paramref name="list"/>
        /// </summary>
        /// <param name="list">The list containing the elements to be summed up</param>
        /// <returns>The sum</returns>
        public static T Sum(List<T> list)
        {
            T sum = c.Zero;
            for (int i = 0; i < list.Count; i++)
                sum = c.Add(sum, list[i]);
            return sum;
        }
        /// <summary>
        /// The average of all elements in the <paramref name="list"/>
        /// </summary>
        /// <param name="list">The list containing the elements to be averaged</param>
        /// <returns>The average</returns>
        public static T Average(List<T> list)
        {
            return c.Divide(Sum(list), c.ConvertFrom((ulong)list.Count));
        }
        /// <summary>
        /// The maximum of all elements in the <paramref name="list"/>
        /// </summary>
        /// <param name="list">The list containing the elements</param>
        /// <returns>The biggest element</returns>
        public static T Max(List<T> list)
        {
            T max = c.MinValue;
            for (int i = 0; i < list.Count; i++)
                if (c.Compare(max, list[i]) > 0)
                    max = list[i];
            return max;
        }
        /// <summary>
        /// The minimum of all elements in the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list containing the elements</param>
        /// <returns>The smallest element</returns>
        public static T Min(List<T> list)
        {
            T min = c.MaxValue;
            for (int i = 0; i < list.Count; i++)
                if (c.Compare(min, list[i]) < 0)
                    min = list[i];
            return min;
        }
        /// <summary>
        /// The standard deviation of all elements in the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list containing the elements</param>
        /// <param name="avg">The average</param>
        /// <returns>The standard deviation</returns>
        public static T Sigma(List<T> list, out T avg)
        {
            avg = Average(list);
            T rms = c.Zero;
            for (int i = 0; i < list.Count; i++)
                rms = c.Add(rms, c.Sqr(c.Subtract(list[i], avg)));
            rms = c.Sqrt(c.Divide(rms, c.ConvertFrom((ulong)list.Count)));
            return rms;
        }
        /// <summary>
        /// The standard deviation of all elements in the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list containing the elements</param>
        /// <returns>The standard deviation</returns>
        public static T Sigma(List<T> list)
        {
            T dummy;
            return Sigma(list, out dummy);
        }
    }
}