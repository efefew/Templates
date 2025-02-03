using System.Collections.Generic;

using UnityEngine;

public static class RandomExtensions
{
    public static bool Chance(float probability) => Random.Range(0, 100f) < probability;

    public static int Probability(params float[] probabilities)
    {
        float max = 0;
        for (int id = 0; id < probabilities.Length; id++)
        {
            max += probabilities[id];
        }

        float random = Random.Range(0, max);
        int result = 0;

        for (int id = 0; id < probabilities.Length; id++)
        {
            random -= probabilities[id];
            result = id;
            if (random <= 0)
            {
                break;
            }
        }

        return result;
    }

    public static int Probability(List<float> probabilities)
    {
        float max = 0;
        for (int id = 0; id < probabilities.Count; id++)
        {
            max += probabilities[id];
        }

        float random = UnityEngine.Random.Range(0, max);
        int result = 0;

        for (int id = 0; id < probabilities.Count; id++)
        {
            random -= probabilities[id];
            result = id;
            if (random <= 0)
            {
                break;
            }
        }

        return result;
    }
}
