using System;

namespace Gsemac.Forms.Styles {

    public static class TypeUtilities {

        public static bool IsNumericType(Type type) {

            // https://stackoverflow.com/a/1750024/5383169 (Philip Wallace)

            switch (Type.GetTypeCode(type)) {

                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;

            }

        }

    }

}