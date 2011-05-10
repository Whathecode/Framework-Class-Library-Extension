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
namespace Lambda.Generic.Arithmetic
{
    public struct Size<T, C>
        where C : 
            IAdder<T>,ISubtracter<T>,IComparer<T>,
            new()
    {
        private readonly T w, h;
        private static C c = new C();
        public static Size<T, C> Empty
        {
            get { return new Size<T, C>(); }
        }
        public bool IsEmpty
        {
            get { return c.Equals(Width, Empty.Width) && c.Equals(Height, Empty.Height); }
        }
        public T Width
        {
            get { return w; }
        }
        public T Height
        {
            get { return h; }
        }
        public Size(T width, T height)
        {
            this.w = width;
            this.h = height;
        }
        public static bool operator ==(Size<T, C> a, Size<T, C> b)
        {
            return c.Equals(a.Width, b.Width) && c.Equals(a.Height, b.Height);
        }
        public static bool operator !=(Size<T, C> a, Size<T, C> b)
        {
            return !c.Equals(a.Width, b.Width) || !c.Equals(a.Height, b.Height);
        }
        public override int GetHashCode()
        {
            return c.GetHashCode(Width) ^ c.GetHashCode(Height);
        }
        public override bool Equals(object obj)
        {
            if (obj is Size<T, C>)
                return this == (Size<T, C>)obj;
            else
                return false;
        }
    }

    public struct Point<T, C>
        where C:
            IAdder<T>,ISubtracter<T>,IComparer<T>,
            new()
    {
        private static C c = new C();
        private readonly T x, y;
        public static Point<T, C> Empty
        {
            get { return new Point<T, C>(); }
        }
        public bool IsEmpty
        {
            get { return c.Equals(X, Empty.X) && c.Equals(Y, Empty.Y); }
        }
        public T X
        {
            get { return x; }
        }
        public T Y
        {
            get { return y; }
        }
        public Point(T x, T y)
        {
            this.x = x;
            this.y = y;
        }
        public static Point<T, C> operator +(Point<T, C> a, Size<T, C> b)
        {
            return new Point<T, C>(c.Add(a.X, b.Width), c.Add(a.Y, b.Height));
        }
        public static Point<T, C> operator -(Point<T, C> a, Size<T, C> b)
        {
            return new Point<T, C>(c.Subtract(a.X, b.Width), c.Subtract(a.Y, b.Height));
        }
        public static bool operator ==(Point<T, C> a, Point<T, C> b)
        {
            return c.Equals(a.X, b.X) && c.Equals(a.Y, b.Y);
        }
        public static bool operator !=(Point<T, C> a, Point<T, C> b)
        {
            return !c.Equals(a.X, b.X) || !c.Equals(a.Y, b.Y);
        }
        public override int GetHashCode()
        {
            return c.GetHashCode(X) ^ c.GetHashCode(Y);
        }
        public override bool Equals(object obj)
        {
            if (obj is Point<T, C>)
                return this == (Point<T, C>)obj;
            else
                return false;
        }
    }
}