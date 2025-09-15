using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public static class DebugExtensions
{
    /// <summary>
    /// Метод визуализации массива
    /// </summary> 
    /// <returns>Строка визуализации</returns>
    public static string Show<T>(this T[] arr, string separator = "\t")
    {
        if (arr.Length == 0)
        {
            return null;
        }

        string str = "";
        str += arr[0];
        if (arr.Length > 1)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                str += separator + arr[i];
            }
        }

        return str;
    }
    /// <summary>
    /// Метод визуализации списка
    /// </summary> 
    /// <returns>Строка визуализации</returns>
    public static string Show<T>(this List<T> list, string separator = "\t")
    {
        if (list.Count == 0)
        {
            return null;
        }

        string str = "";
        str += list[0];
        if (list.Count > 1)
        {
            for (int i = 1; i < list.Count; i++)
            {
                str += separator + list[i];
            }
        }

        return str;
    }
    /// <summary>
    /// Метод визуализации 2D массива
    /// </summary>
    /// <returns>Строка визуализации</returns>
    public static string Show<T>(this T[,] arr)
    {
        string str = "";
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                str += arr[x, y] + "\t";
            }

            str += "\n";
        }

        return str;
    }
    public static string GetCode<T>(this T value, bool copy = false)
    {
        string typeName = GetCSharpTypeName(typeof(T));
        if (value == null) return $"{typeName} values = null;";

        string code = $"{typeName} value = {ToLiteral(value, typeof(T), typeName)};";
        if (copy)
            GUIUtility.systemCopyBuffer = code;
        return code;
    }
    public static string GetCode<T>(this T[] values, bool copy = false)
    {
        string typeName = GetCSharpTypeName(typeof(T));
        if (values == null) return $"{typeName}[] values = null;";

        int len = values.Length;
        var items = new List<string>(len);

        for (int i = 0; i < len; i++)
        {
            items.Add(ToLiteral(values[i], typeof(T), typeName));
        }

        string body = string.Join(", ", items);
        string code = $"{typeName}[] value = new[] {{ {body} }};";
        if (copy)
            GUIUtility.systemCopyBuffer = code;
        return code;
    }
    public static string GetCode<T>(this T[,] values, bool copy = false)
    {
        string typeName = GetCSharpTypeName(typeof(T));
        if (values == null) return $"{typeName}[,] values = null;";

        int rows = values.GetLength(0);
        int cols = values.GetLength(1);
        var rowStrings = new List<string>(rows);

        for (int i = 0; i < rows; i++)
        {
            var cellLits = new List<string>(cols);
            for (int j = 0; j < cols; j++)
            {
                cellLits.Add(ToLiteral(values[i, j], typeof(T), typeName));
            }
            rowStrings.Add("{ " + string.Join(", ", cellLits) + " }");
        }

        string body = string.Join(", ", rowStrings);
        string code = $"{typeName}[,] value = new[,] {{ {body} }};";
        if (copy)
            GUIUtility.systemCopyBuffer = code;
        return code;
    }

    private static string ToLiteral(object value, Type valueType, string typeName)
    {
        if (value is null) return "null";

        if (valueType.IsEnum)
        {
            // Enum representation: EnumType.Value
            return $"{valueType.FullName ?? valueType.Name}.{value}";
        }

        switch (Type.GetTypeCode(valueType))
        {
            case TypeCode.String:
                return $"\"{EscapeString((string)value)}\"";
            case TypeCode.Char:
                return $"'{EscapeChar((char)value)}'";
            case TypeCode.Boolean:
                return ((bool)value) ? "true" : "false";
            case TypeCode.Single: // float
                return ((float)value).ToString(CultureInfo.InvariantCulture) + "f";
            case TypeCode.Double:
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            case TypeCode.Decimal:
                return ((decimal)value).ToString(CultureInfo.InvariantCulture) + "m";
            case TypeCode.Int64:
                return ((long)value).ToString(CultureInfo.InvariantCulture) + "L";
            case TypeCode.UInt64:
                return ((ulong)value).ToString(CultureInfo.InvariantCulture) + "UL";
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Byte:
            case TypeCode.SByte:
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            default:
                // Для неопознанных типов — попытаться вывести ToString в комментарии или использовать default(T)
                string repr = value.ToString();
                if (!string.IsNullOrWhiteSpace(repr))
                    return $"/*{EscapeString(repr)}*/ default({typeName})";
                return $"default({typeName})";
        }
    }

    private static string GetCSharpTypeName(Type t)
    {
        if (t == typeof(int)) return "int";
        if (t == typeof(long)) return "long";
        if (t == typeof(short)) return "short";
        if (t == typeof(byte)) return "byte";
        if (t == typeof(sbyte)) return "sbyte";
        if (t == typeof(ushort)) return "ushort";
        if (t == typeof(uint)) return "uint";
        if (t == typeof(ulong)) return "ulong";
        if (t == typeof(float)) return "float";
        if (t == typeof(double)) return "double";
        if (t == typeof(decimal)) return "decimal";
        if (t == typeof(bool)) return "bool";
        if (t == typeof(char)) return "char";
        if (t == typeof(string)) return "string";
        if (t.IsEnum) return t.FullName ?? t.Name;
        return t.Name;
    }

    private static string EscapeString(string s)
    {
        if (s == null) return "";
        StringBuilder sb = new();
        foreach (char c in s)
        {
            switch (c)
            {
                case '\\': sb.Append(@"\\"); break;
                case '\"': sb.Append("\\\""); break;
                case '\n': sb.Append(@"\n"); break;
                case '\r': sb.Append(@"\r"); break;
                case '\t': sb.Append(@"\t"); break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    private static string EscapeChar(char c)
    {
        return c switch
        {
            '\\' => @"\\",
            '\'' => @"\'",
            '\n' => @"\n",
            '\r' => @"\r",
            '\t' => @"\t",
            _ => c.ToString()
        };
    }
    
}