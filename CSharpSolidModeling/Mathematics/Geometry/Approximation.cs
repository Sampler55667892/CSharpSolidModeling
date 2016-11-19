using System.Linq;

namespace Mathematics.Geometry
{
    public static class Approximation
    {
        #region Fields

        /// <summary>
        /// 小数部の有効桁数
        /// </summary>
        public static readonly int ValidLengthOfFractionPart = 5;

        #endregion  // Fields

        #region Methods

        public static string ToApproximatedString( double value ) =>
            ToApproximatedString( value, ValidLengthOfFractionPart );

        public static string ToApproximatedString( double value, int validLengthOfFractionPart )
        {
            var valueString = ((decimal)value).ToString();
            var indexOfFractionPoint = valueString.IndexOf( '.' );

            // 小数点なし
            if (indexOfFractionPoint == -1)
                return valueString;

            // 整数部と小数部に分ける
            var integerPart = valueString.Substring( 0, indexOfFractionPoint );
            // 小数部の有効桁数が0の場合
            if (validLengthOfFractionPart == 0)
                return integerPart;
            var fractionPart = valueString.Substring( indexOfFractionPoint + 1 );
            // 小数部が全て0の場合 -> 整数部だけ返す
            if (IsFractionPartAllZero( fractionPart ))
                return integerPart;
            // 小数部の桁数が有効桁数を超えている場合 -> 有効桁数分だけ返す
            if (fractionPart.Length > validLengthOfFractionPart)
                return valueString.Substring( 0, indexOfFractionPoint + 1 + validLengthOfFractionPart );
            if (fractionPart.Length == validLengthOfFractionPart)
                return valueString;
            return valueString.PadRight( indexOfFractionPoint + 1 + validLengthOfFractionPart, '0' );
        }

        static bool IsFractionPartAllZero( string fractionPart ) =>
            fractionPart.ToArray().All(c => c == '0');

        public static string ToApproximatedString( Vector3d point ) =>
            ToApproximatedString( point, ValidLengthOfFractionPart );

        public static string ToApproximatedString( Vector3d point, int validLengthOfFractionPart )
        {
            return
                ToApproximatedString( point.X, validLengthOfFractionPart ) + "/" +
                ToApproximatedString( point.Y, validLengthOfFractionPart ) + "/" +
                ToApproximatedString( point.Z, validLengthOfFractionPart );
        }

        #endregion  // Methods
    }
}
