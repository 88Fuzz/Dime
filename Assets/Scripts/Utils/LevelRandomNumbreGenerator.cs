using System;
using System.Globalization;
using System.Text;

public class LevelRandomNumbreGenerator
{
    private static readonly int SEED_SIZE = 9;
    private static readonly int DASH_LOCATION = 4;
    public static readonly LevelRandomNumbreGenerator levelRNG;

    private Random random;

    public LevelRandomNumbreGenerator()
    {
        float randoValue = UnityEngine.Random.value;
        uint seed = BitConverter.ToUInt32(BitConverter.GetBytes(randoValue), 0);
        string seedString = seed.ToString("X");
        //TODO will this shit add zeros to the front?
        //string seedString = seed.ToString("X8");

        StringBuilder builder = new StringBuilder(SEED_SIZE);
        for(int i = 0; i < SEED_SIZE - 1 - seedString.Length; i++)
            builder.Append("0");
        builder.Append(seedString);

        builder.Insert(DASH_LOCATION, "-");

        this shit really needs to be testing mother fucker!
        Seed(builder.ToString());
    }

    /*
     * returns false if the seed entered was invalid and could not be set. True if the seed is valid and entered correctly.
     * The expected format is XXXX-XXXX where X is a hex digit.
     */
    public bool Seed(string seed)
    {
        if (seed == null || seed.Length != SEED_SIZE)
            return false;

        string hexString = seed.Remove(DASH_LOCATION, 1);
        int seedValue;
        if (!Int32.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out seedValue))
            return false;

        random = new Random(seedValue);
        return true;
    }
}