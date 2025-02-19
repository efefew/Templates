// https://kvodo.ru/sortirovka-shella.html
/// <summary>
/// сортировки
/// </summary>
internal abstract class Sorting<T>
{
    /// <summary>
    /// Сортировка
    /// </summary>
    /// <param name="array">массив, который необходимо отсортировать</param>
    /// <returns>отсортированный массив</returns>
    public abstract T[] SortingMethod(T[] array);
    /// <summary>
    /// функция обмена
    /// </summary>
    /// <param name="array">массив, в котором необходимо поменять местами элементы</param>
    /// <param name="id">индекс следующего элемента, который поменяется местами с предыдущим</param>
    protected void Swap(ref float[] array, int id)// функция обмена
    {
        float temp;
        temp = array[id];
        array[id] = array[id - 1];
        array[id - 1] = temp;
    }
    /// <summary>
    /// Сортировка методом вставки
    /// </summary>
    public class SortingByInsertionMethod/*Сортировка методом вставки*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            //int CountComparison = 0/*Количество сравнений*/, CountTransfer = 0/*Количество пересылок*/;
            float temp;
            int j;
            for (int i = 1; i < size; i++) //проходит по всем элементам
            {
                temp = array[i];
                j = i - 1;
                //CountComparison++;
                while (j >= 0 && array[j] > temp)//пересылает
                {
                    array[j + 1] = array[j];
                    array[j] = temp;
                    j--;
                    //CountTransfer++;
                }
            }

            return array;
        }
    }
    /// <summary>
    /// Сортировка методом Шелла
    /// </summary>
    public class SortingByTheShellMethod/*Сортировка методом Шелла*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            int h = size / 2;//длина шагов
                             //int CountComparison = 0/*Количество сравнений*/, CountTransfer = 0/*Количество пересылок*/;
            int j;
            float temp;
            while (h > 0)
            {
                for (int i = 0; i < size - h; i++)
                {
                    j = i;
                    //CountComparison++;
                    while (j >= 0 && array[j] > array[j + h])
                    {
                        temp = array[j];
                        array[j] = array[j + h];
                        array[j + h] = temp;
                        //CountTransfer++;
                        j--;
                    }
                }

                h /= 2;
            }

            return array;
        }
    }
    /// <summary>
    /// Сортировка по подсчёту
    /// </summary>
    public class SortingByCounting/*Сортировка по подсчёту*/ : Sorting<int>
    {
        private int maxNumber;//максимальное значение в массиве (если -1, то нужно найти)
        private bool find;
        public SortingByCounting(int maxNumber = -1)
        {
            find = maxNumber < 0;
            this.maxNumber = maxNumber;
        }
        public override int[] SortingMethod(int[] array)
        {
            int size = array.Length;
            if (find)
            {
                int max = array[0];
                for (int i = 0; i < size; i++)
                {
                    if (array[i] > max)
                    {
                        max = array[i];
                    }
                }

                maxNumber = max;
            }

            int[] numbers = new int[maxNumber + 1];
            for (int i = 0; i < size; i++)
            {
                numbers[array[i]] = numbers[array[i]] + 1;
            }

            int j = 0;
            for (int i = 0; i < (maxNumber + 1); i++)
            {
                while (numbers[i] > 0)
                {
                    array[j] = i;
                    numbers[i]--;
                    j++;
                }
            }

            return array;
        }
    }
    /// <summary>
    /// Сортировка по сравнению и подсчёту
    /// </summary>
    public class SortingByComparisonAndCounting/*Сортировка по сравнению и подсчёту*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            int[] numbers = new int[size];
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (array[i] < array[j])
                    {
                        numbers[j]++;
                    }
                    else
                    {
                        numbers[i]++;
                    }
                }
            }

            float[] newArray = new float[size];
            for (int i = 0; i < size; i++)
            {
                newArray[numbers[i]] = array[i];
            }

            return newArray;
        }
    }
    /// <summary>
    /// Сортировка выбором
    /// </summary>
    public class SortingByChoice/*Сортировка выбором*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            int key;
            float temp;
            for (int i = 0; i < size - 1; i++)
            {
                temp = array[i];
                key = i;
                for (int j = i + 1; j < size; j++)
                {
                    if (array[j] < array[key])
                    {
                        key = j;
                    }
                }

                if (key != i)
                {
                    array[i] = array[key];
                    array[key] = temp;
                }
            }

            return array;
        }
    }
    /// <summary>
    /// Гномья сортировка
    /// </summary>
    public class DwarfSorting/*Гномья сортировка*/ : Sorting<float>
    {

        public override float[] SortingMethod(float[] array)
        {
            int i = 1;
            int size = array.Length;
            float temp;
            while (i < size)
            {
                if (i == 0 || array[i - 1] <= array[i])
                {
                    i++;
                }
                else
                {
                    temp = array[i];
                    array[i] = array[i - 1];
                    array[--i] = temp;
                }
            }

            return array;
        }
    }
    /// <summary>
    /// Шейкера сортировка (перемешиванием)
    /// </summary>
    public class ShakerSorting/*Шейкера сортировка (перемешиванием)*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            int left = 1, right = size - 1, i;
            while (left <= right)
            {
                for (i = right; i >= left; i--)
                {
                    if (array[i - 1] > array[i])
                    {
                        Swap(ref array, i);
                    }
                }

                left++;
                for (i = left; i <= right; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        Swap(ref array, i);
                    }
                }

                right--;
            }

            return array;
        }
    }
    /// <summary>
    /// Быстрая сортировка
    /// </summary>
    public class QuickSorting/*Быстрая сортировка*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            QuickSort(array, 0, size - 1);
            return array;
        }

        private void QuickSort(float[] array, int first, int last)//рекурсивная функция сортировки
        {
            float mid, count;
            int newFirst = first, newLast = last;
            mid = array[(newFirst + newLast) / 2]; //вычисление опорного элемента
            do
            {
                while (array[newFirst] < mid)
                {
                    newFirst++;
                }

                while (array[newLast] > mid)
                {
                    newLast--;
                }

                if (newFirst <= newLast) //перестановка элементов
                {
                    count = array[newFirst];
                    array[newFirst] = array[newLast];
                    array[newLast] = count;
                    newFirst++;
                    newLast--;
                }
            } while (newFirst < newLast);
            if (first < newLast)
            {
                QuickSort(array, first, newLast);
            }

            if (newFirst < last)
            {
                QuickSort(array, newFirst, last);
            }
        }
    }
    /// <summary>
    /// Сортировка слиянием
    /// </summary>
    public class MergeSorting/*Сортировка слиянием*/ : Sorting<float>
    {
        private int size;
        public override float[] SortingMethod(float[] array)
        {
            size = array.Length;
            return MergeSort(array, 0, size - 1);
        }

        private float[] MergeSort(float[] array, int first, int last)//рекурсивная процедура сортировки
        {
            if (first < last)
            {
                array = MergeSort(array, first, (first + last) / 2); //сортировка левой части
                array = MergeSort(array, ((first + last) / 2) + 1, last); //сортировка правой части
                array = Merge(array, first, last); //слияние двух частей
            }

            return array;
        }

        private float[] Merge(float[] array, int first, int last)
        {
            int middle, start, final, j;
            float[] tempArray = new float[size];
            middle = (first + last) / 2; //вычисление среднего элемента
            start = first; //начало левой части
            final = middle + 1; //начало правой части
            for (j = first; j <= last; j++) //выполнять от начала до конца
            {
                if ((start <= middle) && ((final > last) || (array[start] < array[final])))
                {
                    tempArray[j] = array[start];
                    start++;
                }
                else
                {
                    tempArray[j] = array[final];
                    final++;
                }
            }
            //возвращение результата в список
            for (j = first; j <= last; j++)
            {
                array[j] = tempArray[j];
            }

            return array;
        }
    }
    /// <summary>
    /// Сортировка пузырьком
    /// </summary>
    public class BubbleSorting/*Сортировка пузырьком*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - (i + 1); j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        Swap(ref array, j + 1);
                    }
                }
            }

            return array;
        }
    }
}