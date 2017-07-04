using UnityEngine;

public class Beam : MyMonoBehaviour
{
    private delegate void FixedUpdateAction(float deltaTime, float timeScale);

    private static readonly float Y_SIZE = .1f;
    private static readonly float MAX_DISTANCE = 100f;

    public LayerMask layerMask;
    public Range width;
    public float widthExpandRate;
    public float widthShrinkRate;
    public float damage;

    private FixedUpdateAction updateAction;
    private float currentWidth;

    //Figure out how to make this work.
        //Also, Create a better hit animation. Where theres an explosion of lines from the hit point. The colors should be a little random, but mainly a specific color
        //Crits are typically more deep red.
        //Standard hits are oranges.
        //Glances are yellowish.
    private LineRenderer lineRenderer;
    private Vector3[] linePositions;

    protected override void MyAwake()
    {
        //TODO object pooling
        linePositions = new Vector3[2];
        lineRenderer = GetComponentInChildren<LineRenderer>();
        updateAction = DoNothingAction;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        updateAction(myDeltaTime, timeScale);
    }

    public bool IsActive()
    {
        return enabled;
    }

    public void StartBeam()
    {
        gameObject.SetActive(true);
        lineRenderer.startWidth = width.min;
        lineRenderer.endWidth = width.min;
    }

    public void EndBeam()
    {
        updateAction = ShrinkBeamAction;
    }

    private void ExpandBeamAction(float deltaTime, float timeScale)
    {
        currentWidth += widthExpandRate * timeScale;
        if(currentWidth >= width.max)
        {
            currentWidth = width.max;
            updateAction = FullAttackBeamAction;
        }
    }

    private void ShrinkBeamAction(float deltaTime, float timeScale)
    {
        currentWidth += widthShrinkRate * timeScale;
        if(currentWidth < width.min)
        {
            updateAction = DoNothingAction;
            gameObject.SetActive(false);
        }
    }

    private void DoNothingAction(float deltaTime, float timeScale) { }

    private void FullAttackBeamAction(float deltaTime, float timeScale)
    {
        ShootBeam();
    }

    private void ShootBeam()
    {
        RaycastHit hitInfo;
        Vector3 size = new Vector3(currentWidth / 2, Y_SIZE, currentWidth / 2);
        Ray ray = new Ray(transform.position, transform.forward);
        if (!Physics.BoxCast(transform.position, size, transform.forward, out hitInfo, transform.rotation, MAX_DISTANCE, layerMask, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        Hittable hittable = hitInfo.collider.GetComponent<Hittable>();
        if(hittable)
            hittable.Hit(damage);

        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
    }
}