using System;
using System.Globalization;
using System.Text;

/*
 * The LevelRandomNumberGenerator should only be used when generating the level as it's based on a user input seed. 
 * Level generation should include loot, rooms, and mobs.
 */
public class LevelRandomNumberGenerator
{
    private static readonly int SEED_SIZE = 9;
    private static readonly int DASH_LOCATION = 4;
    public static readonly LevelRandomNumberGenerator levelRNG = new LevelRandomNumberGenerator();

    private Random random;

    public LevelRandomNumberGenerator()
    {
        float randoValue = UnityEngine.Random.value;
        uint seed = BitConverter.ToUInt32(BitConverter.GetBytes(randoValue), 0);
        string seedString = seed.ToString("X");

        StringBuilder builder = new StringBuilder(SEED_SIZE);
        for(int i = 0; i < SEED_SIZE - 1 - seedString.Length; i++)
            builder.Append("0");
        builder.Append(seedString);

        builder.Insert(DASH_LOCATION, "-");

        Seed(builder.ToString());
    }

    /*
     * returns false if the seed entered was invalid and could not be set. True if the seed is valid and entered correctly.
     * The expected format is XXXX-XXXX where X is a hex digit.
     */
    public bool Seed(string seed)
    {
        seed = "3EBB-F671";
        //TODO remove debug log
        UnityEngine.Debug.Log("SEED: " + seed);

        if (seed == null || seed.Length != SEED_SIZE)
            return false;

        string hexString = seed.Remove(DASH_LOCATION, 1);
        int seedValue;
        if (!Int32.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out seedValue))
            return false;

        random = new Random(seedValue);
        return true;
    }

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

        return (float) (random.NextDouble() * (max - min)) + min;
    }

    /*
     * Return a random number in the range [min, max). If min > max, max is returned
     */
    public int GetValueInRange(int min, int max)
    {
        if (min >= max)
            return max;

        return random.Next(min, max);
    }
}