using Random = System.Random;
using UnityEngine;

public static class RandomHelper {
    public static int Range(int min, int max)
    {
        Random rnd = new Random();
        return rnd.Next(min, max);
    }
}