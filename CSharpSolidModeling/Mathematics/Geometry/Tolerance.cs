﻿using System;

namespace Mathematics.Geometry
{
    public static class Tolerance
    {
        #region Fields

        /// <summary>
        /// 実数での許容誤差 (10^{-6})
        /// </summary>
        public static double Real = 0.000001;

        #endregion  // Fields

        #region Methods

        public static bool IsIgnorable( double realValue )
        {
            return Math.Abs( realValue ) < Real;
        }

        #endregion  // Methods
    }
}
