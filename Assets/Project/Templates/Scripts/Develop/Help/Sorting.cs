// https://kvodo.ru/sortirovka-shella.html
/// <summary>
/// ����������
/// </summary>
internal abstract class Sorting<T>
{
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="array">������, ������� ���������� �������������</param>
    /// <returns>��������������� ������</returns>
    public abstract T[] SortingMethod(T[] array);
    /// <summary>
    /// ������� ������
    /// </summary>
    /// <param name="array">������, � ������� ���������� �������� ������� ��������</param>
    /// <param name="id">������ ���������� ��������, ������� ���������� ������� � ����������</param>
    protected void Swap(ref float[] array, int id)// ������� ������
    {
        float temp;
        temp = array[id];
        array[id] = array[id - 1];
        array[id - 1] = temp;
    }
    /// <summary>
    /// ���������� ������� �������
    /// </summary>
    public class SortingByInsertionMethod/*���������� ������� �������*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            //int CountComparison = 0/*���������� ���������*/, CountTransfer = 0/*���������� ���������*/;
            float temp;
            int j;
            for (int i = 1; i < size; i++) //�������� �� ���� ���������
            {
                temp = array[i];
                j = i - 1;
                //CountComparison++;
                while (j >= 0 && array[j] > temp)//����������
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
    /// ���������� ������� �����
    /// </summary>
    public class SortingByTheShellMethod/*���������� ������� �����*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            int h = size / 2;//����� �����
                             //int CountComparison = 0/*���������� ���������*/, CountTransfer = 0/*���������� ���������*/;
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
    /// ���������� �� ��������
    /// </summary>
    public class SortingByCounting/*���������� �� ��������*/ : Sorting<int>
    {
        private int _maxNumber;//������������ �������� � ������� (���� -1, �� ����� �����)
        private bool _find;
        public SortingByCounting(int maxNumber = -1)
        {
            _find = maxNumber < 0;
            this._maxNumber = maxNumber;
        }
        public override int[] SortingMethod(int[] array)
        {
            int size = array.Length;
            if (_find)
            {
                int max = array[0];
                for (int i = 0; i < size; i++)
                {
                    if (array[i] > max)
                    {
                        max = array[i];
                    }
                }

                _maxNumber = max;
            }

            int[] numbers = new int[_maxNumber + 1];
            for (int i = 0; i < size; i++)
            {
                numbers[array[i]] = numbers[array[i]] + 1;
            }

            int j = 0;
            for (int i = 0; i < (_maxNumber + 1); i++)
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
    /// ���������� �� ��������� � ��������
    /// </summary>
    public class SortingByComparisonAndCounting/*���������� �� ��������� � ��������*/ : Sorting<float>
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
    /// ���������� �������
    /// </summary>
    public class SortingByChoice/*���������� �������*/ : Sorting<float>
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
    /// ������ ����������
    /// </summary>
    public class DwarfSorting/*������ ����������*/ : Sorting<float>
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
    /// ������� ���������� (��������������)
    /// </summary>
    public class ShakerSorting/*������� ���������� (��������������)*/ : Sorting<float>
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
    /// ������� ����������
    /// </summary>
    public class QuickSorting/*������� ����������*/ : Sorting<float>
    {
        public override float[] SortingMethod(float[] array)
        {
            int size = array.Length;
            QuickSort(array, 0, size - 1);
            return array;
        }

        private void QuickSort(float[] array, int first, int last)//����������� ������� ����������
        {
            float mid, count;
            int newFirst = first, newLast = last;
            mid = array[(newFirst + newLast) / 2]; //���������� �������� ��������
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

                if (newFirst <= newLast) //������������ ���������
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
    /// ���������� ��������
    /// </summary>
    public class MergeSorting/*���������� ��������*/ : Sorting<float>
    {
        private int _size;
        public override float[] SortingMethod(float[] array)
        {
            _size = array.Length;
            return MergeSort(array, 0, _size - 1);
        }

        private float[] MergeSort(float[] array, int first, int last)//����������� ��������� ����������
        {
            if (first < last)
            {
                array = MergeSort(array, first, (first + last) / 2); //���������� ����� �����
                array = MergeSort(array, ((first + last) / 2) + 1, last); //���������� ������ �����
                array = Merge(array, first, last); //������� ���� ������
            }

            return array;
        }

        private float[] Merge(float[] array, int first, int last)
        {
            int middle, start, final, j;
            float[] tempArray = new float[_size];
            middle = (first + last) / 2; //���������� �������� ��������
            start = first; //������ ����� �����
            final = middle + 1; //������ ������ �����
            for (j = first; j <= last; j++) //��������� �� ������ �� �����
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
            //����������� ���������� � ������
            for (j = first; j <= last; j++)
            {
                array[j] = tempArray[j];
            }

            return array;
        }
    }
    /// <summary>
    /// ���������� ���������
    /// </summary>
    public class BubbleSorting/*���������� ���������*/ : Sorting<float>
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