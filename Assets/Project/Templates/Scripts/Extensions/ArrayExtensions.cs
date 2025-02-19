using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ArrayExtensions
{
    private static System.Random random = new((int)DateTime.Now.Ticks & 0x0000FFFF);
    public static int IndexOf<T>(this T[] array, T value)
    {
        for (int id = 0; id < array.Length; id++)
        {
            if (array[id].Equals(value))
            {
                return id;
            }
        }

        return -1;
    }
    /// <summary>
    /// Ќормализовать массив
    /// </summary>
    /// <param name="array">массив</param>
    public static void Normalize(this double[] array, double min = 0, double max = 1)
    {
        double minInArray = array.Min();
        double maxInArray = array.Max();
        if (minInArray == maxInArray)
        {
            double value = maxInArray > 0 ? 1 : 0;
            for (int id = 0; id < array.Length; id++)
            {
                array[id] = value;
            }

            return;
        }

        double relativeValue;
        for (int id = 0; id < array.Length; id++)
        {
            relativeValue = (array[id] - minInArray) / (maxInArray - minInArray);//от 0 до 1
            array[id] = (relativeValue * (max - min)) + min;
        }
    }
    public static T[] ToArray<T>(this T[,] matrix)
    {
        T[] array = new T[matrix.GetLength(0) * matrix.GetLength(1)];
        int id = 0;
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                array[id] = matrix[x, y];
                id++;
            }
        }

        return array;
    }
    public static void SetAll<T>(this T[] array, T value)
    {
        for (int id = 0; id < array.Length; id++)
        {
            array[id] = value;
        }
    }
    public static void SetAll<T>(this T[,] matrix, T value)
    {
        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                matrix[y, x] = value;
            }
        }
    }
    public static void WriteArray(this BinaryWriter writer, double[] arr)
    {
        writer.Write(arr.Length);

        for (int id = 0; id < arr.Length; id++)
        {
            writer.Write(arr[id]);
        }
    }

    public static void WriteArray2D(this BinaryWriter writer, double[,] arr)
    {
        writer.Write(arr.GetLength(1));
        writer.Write(arr.GetLength(0));

        for (int y = 0; y < arr.GetLength(1); y++)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
            {
                writer.Write(arr[x, y]);
            }
        }
    }

    public static double[] ReadArray(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        double[] array = new double[length];

        for (int id = 0; id < length; id++)
        {
            array[id] = reader.ReadDouble();
        }

        return array;
    }

    public static double[,] ReadArray2D(this BinaryReader reader)
    {
        int yCount = reader.ReadInt32();
        int xCount = reader.ReadInt32();
        double[,] array = new double[xCount, yCount];
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                array[x, y] = reader.ReadDouble();
            }
        }

        return array;
    }
    /// <summary>
    /// ѕеремешивание списка
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">список</param>
    public static void Mixing<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary>
    /// ѕеремешивание массива
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">массив</param>
    public static void Mixing<T>(this T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    /// <summary>
    /// ѕеремешивание двух списков одинаково
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list1">список 1</param>
    /// <param name="list2">список 2</param>
    public static void MixingTwoLists<T>(IList<T> list1, IList<T> list2)
    {
        if (list1.Count != list2.Count)
        {
            throw new Exception("списки должны иметь одинаковый размер");
        }

        int n = list1.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list1[n], list1[k]) = (list1[k], list1[n]);
            (list2[n], list2[k]) = (list2[k], list2[n]);
        }
    }
    /// <summary>
    /// метод визуализации массива
    /// </summary> 
    /// <returns>строка визуализации</returns>
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
    /// метод визуализации списка
    /// </summary> 
    /// <returns>строка визуализации</returns>
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
    /// метод визуализации 2D массива
    /// </summary>
    /// <returns>строка визуализации</returns>
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
    /// <summary>
    /// Ќарастить границы матрице заполнив значением value
    /// </summary>
    /// <param name="matrix">массив</param>
    /// <param name="value">значение</param>
    /// <returns>массив с наращенными границами</returns>
    public static bool[,] AddBorders(this bool[,] matrix, bool value = true)
    {
        bool[,] newMatrix = new bool[matrix.GetLength(0) + 2, matrix.GetLength(1) + 2];
        for (int x = 0; x < newMatrix.GetLength(0); x++)
        {
            newMatrix[x, 0] = value;
            newMatrix[x, newMatrix.GetLength(0) - 1] = value;
        }

        for (int y = 0; y < newMatrix.GetLength(1); y++)
        {
            newMatrix[0, y] = value;
            newMatrix[newMatrix.GetLength(1) - 1, y] = value;
        }

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                newMatrix[x + 1, y + 1] = matrix[x, y];
            }
        }

        return newMatrix;
    }
    public static bool IsEmptyRow(this int[,] matrix, int row)
    {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            if (matrix[row, y] != 0)
            {
                return false;
            }
        }

        return true;
    }
    public static bool IsEmptyColumn(this int[,] matrix, int column)
    {
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            if (matrix[x, column] != 0)
            {
                return false;
            }
        }

        return true;
    }
    public static int[,] RowRemove(this int[,] matrix, int rowToRemove)
    {
        int[,] result = new int[matrix.GetLength(0) - 1, matrix.GetLength(1)];

        for (int xOriginal = 0, xResult = 0; xOriginal < matrix.GetLength(0); xOriginal++)
        {
            if (xOriginal == rowToRemove)
            {
                continue;
            }

            for (int yOriginal = 0, yResult = 0; yOriginal < matrix.GetLength(1); yOriginal++)
            {
                result[xResult, yResult] = matrix[xOriginal, yOriginal];
                yResult++;
            }

            xResult++;
        }

        return result;
    }
    public static int[,] ColumnRemove(this int[,] matrix, int columnToRemove)
    {
        int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1) - 1];

        for (int xOriginal = 0, xResult = 0; xOriginal < matrix.GetLength(0); xOriginal++)
        {
            for (int yOriginal = 0, yResult = 0; yOriginal < matrix.GetLength(1); yOriginal++)
            {
                if (yOriginal == columnToRemove)
                {
                    continue;
                }

                result[xResult, yResult] = matrix[xOriginal, yOriginal];
                yResult++;
            }

            xResult++;
        }

        return result;
    }
    /// <summary>
    /// транспонирование матрицы
    /// </summary>
    /// <typeparam name="T">тип €чеек матрицы</typeparam>
    /// <param name="matrix">матрица</param>
    /// <returns>транспонирована€ матрица</returns>
    public static T[,] Transposition<T>(this T[,] matrix)
    {
        T[,] newArr = new T[matrix.GetLength(1), matrix.GetLength(0)];
        for (int x = 0; x < newArr.GetLength(0); x++)
        {
            for (int y = 0; y < newArr.GetLength(1); y++)
            {
                newArr[x, y] = matrix[y, x];
            }
        }

        return newArr;
    }
    /// <summary>
    /// умножение матриц
    /// </summary>
    /// <param name="A">матрица A</param>
    /// <param name="B">матрица B</param>
    /// <returns>матрица — = A * B</returns>
    /// <exception cref="Exception">если число столбцов матрицы A не равно числу строк B</exception>
    public static double[,] Multiplication(this double[,] A, double[,] B)
    {
        if (A.GetLength(1) != B.GetLength(0))
        {
            throw new Exception("число столбцов матрицы A не равно числу строк B");
        }

        double[,] C = new double[A.GetLength(0), B.GetLength(1)];
        for (int x = 0; x < C.GetLength(0); x++)
        {
            for (int y = 0; y < C.GetLength(1); y++)
            {
                for (int index = 0; index < A.GetLength(0); index++)
                {
                    C[x, y] += A[x, index] * B[index, y];
                }
            }
        }

        return C;
    }
    /// <summary>
    /// ”множение матрицы на число
    /// </summary>
    /// <param name="A">матрица</param>
    /// <param name="number">число</param>
    /// <returns>матрица B = number * A</returns>
    public static double[,] Multiplication(this double[,] A, double number)
    {
        double[,] B = new double[A.GetLength(0), A.GetLength(1)];
        for (int x = 0; x < B.GetLength(0); x++)
        {
            for (int y = 0; y < B.GetLength(1); y++)
            {
                B[x, y] = A[x, y] * number;
            }
        }

        return B;
    }
    public static double[] Multiplication(this double[] A, double number)
    {
        double[] B = new double[A.Length];
        for (int id = 0; id < B.Length; id++)
        {
            B[id] = A[id] * number;
        }

        return B;
    }
    /// <summary>
    /// сложение матриц
    /// </summary>
    /// <param name="A">матрица A</param>
    /// <param name="B">матрица B</param>
    /// <returns>матрица — = A + B</returns>
    /// <exception cref="Exception">если матрицы A и B не одинаковых размеров</exception>
    public static double[,] Add(this double[,] A, double[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            throw new Exception("матрицы A и B не одинаковых размеров");
        }

        double[,] C = new double[A.GetLength(0), A.GetLength(1)];
        for (int x = 0; x < C.GetLength(0); x++)
        {
            for (int y = 0; y < C.GetLength(1); y++)
            {
                C[x, y] = A[x, y] + B[x, y];
            }
        }

        return C;
    }
    /// <summary>
    /// вычитание матриц
    /// </summary>
    /// <param name="A">матрица A</param>
    /// <param name="B">матрица B</param>
    /// <returns>матрица — = A - B</returns>
    /// <exception cref="Exception">если матрицы A и B не одинаковых размеров</exception>
    public static double[,] Subtract(this double[,] A, double[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            throw new Exception("матрицы A и B не одинаковых размеров");
        }

        double[,] C = new double[A.GetLength(0), A.GetLength(1)];
        for (int x = 0; x < C.GetLength(0); x++)
        {
            for (int y = 0; y < C.GetLength(1); y++)
            {
                C[x, y] = A[x, y] - B[x, y];
            }
        }

        return C;
    }
    /// <summary>
    /// ѕостроение матрицы комплексов C
    /// </summary>
    /// <param name="A">матрица A</param>
    /// <param name="B">матрица B</param>
    /// <returns>матрица комплексов — = логически перемножа€ элементы матриц A и B</returns>
    /// <exception cref="Exception">если матрицы A и B не одинаковых размеров</exception>
    public static double[,] Complex(this double[,] A, double[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            throw new Exception("матрицы A и B не одинаковых размеров");
        }

        double[,] C = new double[A.GetLength(0), A.GetLength(1)];
        for (int x = 0; x < C.GetLength(0); x++)
        {
            for (int y = 0; y < C.GetLength(1); y++)
            {
                C[x, y] = A[x, y] * B[x, y];
            }
        }

        return C;
    }
    /// <summary>
    /// ѕостроение матрицы комплексов C
    /// </summary>
    /// <param name="A">матрица A</param>
    /// <param name="B">матрица B</param>
    /// <returns>матрица комплексов — = логически перемножа€ элементы матриц A и B</returns>
    /// <exception cref="Exception">если матрицы A и B не одинаковых размеров</exception>
    public static int[,] Complex(this int[,] A, int[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            throw new Exception("матрицы A и B не одинаковых размеров");
        }

        int[,] C = new int[A.GetLength(0), A.GetLength(1)];
        for (int x = 0; x < C.GetLength(0); x++)
        {
            for (int y = 0; y < C.GetLength(1); y++)
            {
                C[x, y] = A[x, y] * B[x, y];
            }
        }

        return C;
    }
    public static bool Identical<T>(this T[,] A, T[,] B)
    {
        if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
        {
            return false;
        }

        T a, b;
        for (int y = 0; y < A.GetLength(0); y++)
        {
            for (int x = 0; x < A.GetLength(1); x++)
            {
                a = A[y, x];
                b = B[y, x];
                if (!a.Equals(b))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
