﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MyUtility
{
    /// <summary>
    /// 3D integer class - operates similarly to Unity's Vector3D
    /// </summary>
    [System.Serializable]
    [StructLayout( LayoutKind.Sequential, Pack = 4 )]
    public struct Int2 : IEquatable<Int2>, IFormattable
    {
//---------------------------------------------------------------------------FIELDS:

        public static readonly Int2 zero = new Int2( 0, 0 );
        public static readonly Int2 one = new Int2( 1, 1 );
        
        public static readonly Int2 up = new Int2( 0, 1);
        public static readonly Int2 down = new Int2( 0, -1);
        public static readonly Int2 left = new Int2( -1, 0);
        public static readonly Int2 right = new Int2( 1, 0);

        public int x;
        public int y;

//---------------------------------------------------------------------CONSTRUCTORS:

        public Int2( int x, int y )
        {
            this.x = x;
            this.y = y;
        }

        public Int2( float x, float y )
        {
            this.x = (int)x;
            this.y = (int)y;
        }
        
        public Int2( Vector2 vec )
        {
            this.x = (int)vec.x;
            this.y = (int)vec.y;
        }

//--------------------------------------------------------------------------METHODS:

        public override int GetHashCode()
        {
            var hashCode = x.GetHashCode();
            return ( hashCode * 397 ) ^ y.GetHashCode();
        }

        public int this[int index]
        {
            get
            {
                if( index == 0 ) return x;
                if( index == 1 ) return y;
                throw new ArgumentOutOfRangeException( "Invalid Int2 index!" );
            }
            set
            {
                if( index == 0 ) x = value;
                if( index == 1 ) y = value;
                throw new ArgumentOutOfRangeException( "Invalid Int2 index!" );
            }
        }

        public static explicit operator Vector2( Int2 v )
        {
            return new Vector2( v.x, v.y );
        }        

        #region math operations
        public float magnitude
        {
            get { return Magnitude( this ); }
        }

        public int sqrMagnitude
        {
            get { return SqrMagnitude( this ); }
        }

        public int dotOne
        {
            get { return DotOne( this ); }
        }

        public float Magnitude( Int2 v )
        {
            return Mathf.Sqrt( SqrMagnitude( v ) );
        }

        public int SqrMagnitude( Int2 v )
        {
            return v.x * v.x + v.y * v.y;
        }

        public static Int2 Scale( Int2 l, Int2 r )
        {
            return new Int2( l.x * r.x, l.y * r.y);
        }

        public static int Dot( Int2 l, Int2 r )
        {
            return l.x * r.x + l.y * r.y;
        }

        public static int DotOne( Int2 v )
        {
            return v.x * v.y;
        }
        
        public static Int2 Min( Int2 l, Int2 r )
        {
            return new Int2( Mathf.Min( l.x, r.x ), Mathf.Min( l.y, r.y ) );
        }

        public static Int2 Max( Int2 l, Int2 r )
        {
            return new Int2( Mathf.Max( l.x, r.x ), Mathf.Max( l.y, r.y ) );
        }

        public static Int2 Clamp( Int2 v, Int2 min, Int2 max )
        {
            return new Int2( Mathf.Clamp( v.x, min.x, max.x ),
                            Mathf.Clamp( v.y, min.y, max.y ) );
        }

        public static Int2 ClosestPowerOfTwo( Int2 v )
        {
            return new Int2( Mathf.ClosestPowerOfTwo( v.x ), Mathf.ClosestPowerOfTwo( v.y ) );
        }

        public static int CubicToLinearIndex( Int2 v, Int2 size )
        {
            return ( v.x ) +
                   ( v.y * size.x );
        }

        public static Int2 LinearToCubicIndex( int v, Int2 size )
        {
            return new Int2( v % size.x,
                           ( v / size.x ) % size.y );
        }
        #endregion math operations

        #region math operators
        public static Int2 operator +( Int2 l, Int2 r )
        {
            return new Int2( l.x + r.x, l.y + r.y);
        }

        public static Int2 operator -( Int2 l, Int2 r )
        {
            return new Int2( l.x - r.x, l.y - r.y );
        }

        public static Int2 operator -( Int2 v )
        {
            return new Int2( -v.x, -v.y );
        }

        public static Int2 operator *( Int2 v, int d )
        {
            return new Int2( v.x * d, v.y * d );
        }

        public static Int2 operator *( int d, Int2 v )
        {
            return new Int2( v.x * d, v.y * d );
        }

        public static Int2 operator /( Int2 v, int d )
        {
            return new Int2( v.x / d, v.y / d );
        }
        #endregion math operators

        #region comparison
        public bool Equals( Int2 other )
        {
            return x.Equals( other.x ) && y.Equals( other.y );
        }

        public static bool operator ==( Int2 l, Int2 r )
        {
            return l.Equals( r );
        }

        public static bool operator !=( Int2 l, Int2 r )
        {
            return !l.Equals( r );
        }

        public override bool Equals( object value )
        {
            return ( value is Int2 ) ? Equals( (Int2)value ) : false;
        }
        #endregion

        #region IFormattable
        public override string ToString()
        {
            return ToString( CultureInfo.CurrentCulture );
        }

        public string ToString( IFormatProvider formatProvider )
        {
            return string.Format( formatProvider, "X:{0} Y:{1}", x, y );
        }

        public string ToString( string format, IFormatProvider formatProvider )
        {
            if( format == null )
            {
                return ToString( formatProvider );
            }

            return string.Format( formatProvider,
                                 "X:{0} Y:{1}",
                                 x.ToString( format, formatProvider ),
                                 y.ToString( format, formatProvider ) );
        }
        #endregion
    }
}