using UnityEngine;

/*
 * Once the BraidRing is placed, as entities get closer to it the slower time will move for them.
 */
public class BraidRing : MonoBehaviour
{
    public BraidTimeScaleModifier braidTimeScaleModifier;
    public float maxEnabledTime;

    private float timer;
    this BraidRing does not get destroyed properly :(

    public void Awake()
    {
        braidTimeScaleModifier.braidRing = this;
        Enabled();
    }

    public void OnDestroy()
    {
        braidTimeScaleModifier.braidRing = null;
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