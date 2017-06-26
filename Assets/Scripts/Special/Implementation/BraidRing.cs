using UnityEngine;

/*
 * Once the BraidRing is placed, as entities get closer to it the slower time will move for them.
 */
public class BraidRing : MonoBehaviour
{
    public float maxEnabledTime;

    private float timer;

    public void Awake()
    {
        Enabled();
    }

    public void OnEnable()
    {
        Enabled();
    }

    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer > maxEnabledTime)
            gameObject.SetActive(false);
    }

    private void Enabled()
    {
        timer = 0;
    }
}