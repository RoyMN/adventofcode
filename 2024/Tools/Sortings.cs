namespace AdventOfCode2024.Tools;

public static class Sortings
{
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
}