[System.Serializable]
public struct Range
{
    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GetDifference()
    {
        return max - min;
    }

    public override string ToString()
    {
        return "[" + min + ", " + max + "]";
    }
}