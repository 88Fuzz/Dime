using UnityEngine;

/*
 * A stupid wrapper arounod the stupid Vector3 class used for stupid Dictionaries. It rounds all values in a Vector3 to an integer because
 * of the stupid floating point precision errors.
 */
public class IntVector3
{
    private Vector3 vector3;

    public IntVector3(Vector3 vector3)
    {
        setVector3(vector3);
    }

    public void setVector3(Vector3 vector3)
    {
        this.vector3 = new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (obj is IntVector3)
        {
            IntVector3 other = (IntVector3)obj;
            bool result = vector3.Equals(other.vector3);
            return result;
        }

        return false;
    }

    public override int GetHashCode()
    {
        int result = vector3.GetHashCode();
        return vector3.GetHashCode();
    }
}