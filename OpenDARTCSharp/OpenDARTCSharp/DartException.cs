using System;

namespace OpenDARTCSharp
{
    public sealed class DartException : Exception
    {
        /// <summary> 에러 및 정보 코드 </summary>
        public readonly int StatusCode;
        /// <summary> 에러 및 정보 메시지 </summary>
        public readonly string ErrorMessage;

        internal DartException(int statusCode, string errorMessage)
            : base(errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"{nameof(DartException)} ({StatusCode:D3}): {ErrorMessage}";
        }
    }
}