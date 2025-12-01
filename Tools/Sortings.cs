namespace AdventOfCodeTools;

public static class Sortings
{
    /**
     * Returns a new array with the element inserted in the sorted (ascending) position.
     */
    public static int[] InsertSort(this int[] arr, int elem)
    {
        int[] newArr = new int[arr.Length + 1];
        int i = 0;
        for (; i < arr.Length; i++)
        {
            if (arr[i] > elem)
            {
                break;
            }
            newArr[i] = arr[i];
        }
        newArr[i] = elem;
        for (; i < arr.Length; i++)
        {
            newArr[i + 1] = arr[i];
        }
        return newArr;
    }

    /**
     * Returns a new array with the element inserted last.
     */
    public static int[] Insert(this int[] arr, int elem)
    {
        int[] newArr = new int[arr.Length + 1];
        for (int i = 0; i < arr.Length; i++)
        {
            newArr[i] = arr[i];
        }
        newArr[arr.Length] = elem;
        return newArr;
    }
}