using UnityEngine;

/*
 * Wrapper around the UnityEngine random number generator used for anything other than level generation.
 */
public class RandomNumberGeneratorUtils
{
    public static readonly RandomNumberGeneratorUtils unityRNG = new RandomNumberGeneratorUtils();

    /*
     * Return a random number in the range [min, max). If min > max, max is returned
     */
    public float GetValueInRange(Range range)
    {
        return GetValueInRange(range.min, range.max);
    }

    /*
     * Return a random number in the range [min, max). If min > max, max is returned
     */
    public float GetValueInRange(float min, float max)
    {
        if (min >= max)
            return max;

        return (Random.value * (max - min)) + min;
    }

    /*
     * Return a random number in the range [min, max). If min > max, max is returned
     */
    public int GetValueInRange(int min, int max)
    {
        if (min >= max)
            return max;

        return (int)(Random.value * (max - min)) + min;
    }
}