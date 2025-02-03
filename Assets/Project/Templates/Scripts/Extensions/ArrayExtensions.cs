public static class ArrayExtensions
{
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
}
