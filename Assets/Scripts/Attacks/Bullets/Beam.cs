using UnityEngine;
using System.Collections;

public class Beam : MyMonoBehaviour
{
    private static readonly float Y_SIZE = .1f;
    private static readonly float MAX_DISTANCE = 100f;

    public LayerMask layerMask;
    public LayerMask initialLayerMaskCheck;
    public Range width;
    public float widthExpandRate;
    public float widthShrinkRate;
    public float damage;

    private FixedUpdateAction updateAction;
    private LineRenderer lineRenderer;
    private Vector3[] linePositions;

    protected override void MyAwake()
    {
        if (widthShrinkRate > 0)
            widthShrinkRate = -widthShrinkRate;
        //TODO object pooling
        linePositions = new Vector3[2];
        lineRenderer = GetComponentInChildren<LineRenderer>();
        updateAction = DoNothingAction;
        SetWidth(width.min);
        SetInitialPositions();
        MyDisable();
    }

    private void SetLineRendererPositions(Vector3 startPosition, Vector3 endPosition)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void SetInitialPositions()
    {
        RaycastHit hitInfo;
        float currentWidth = GetWidth();
        Vector3 size = new Vector3(currentWidth / 2, Y_SIZE, currentWidth / 2);
        Vector3 startPosition;
        Vector3 endPosition;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, MAX_DISTANCE, initialLayerMaskCheck, QueryTriggerInteraction.Ignore))
        {
            startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            endPosition = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
        }
        else
        {
            startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            endPosition = transform.position + transform.forward * MAX_DISTANCE;
        }

        SetLineRendererPositions(startPosition, endPosition);
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        updateAction(myDeltaTime, timeScale);
    }

    public bool AtMaxBeamWidth()
    {
        return GetWidth() >= width.max;
    }

    public bool AtMinBeamWidth()
    {
        return GetWidth() <= width.min;
    }

    public void StartBeam()
    {
        MyEnable();
        updateAction = ExpandBeamAction;
        SetWidth(width.min);
    }

    public void EndBeam()
    {
        updateAction = ShrinkBeamAction;
    }

    private void ExpandBeamAction(float deltaTime, float timeScale)
    {
        float currentWidth = GetWidth();
        currentWidth += widthExpandRate * timeScale;
        if(currentWidth >= width.max)
        {
            currentWidth = width.max;
            updateAction = FullAttackBeamAction;
        }
        SetWidth(currentWidth);
    }

    private void ShrinkBeamAction(float deltaTime, float timeScale)
    {
        float currentWidth = GetWidth();
        currentWidth += widthShrinkRate * timeScale;
        SetWidth(currentWidth);
        if(currentWidth < width.min)
        {
            updateAction = DoNothingAction;
            MyDisable();
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
        float currentWidth = GetWidth();
        Vector3 size = new Vector3(currentWidth / 2, Y_SIZE, currentWidth / 2);
        Ray ray = new Ray(transform.position, transform.forward);
        if (!Physics.BoxCast(transform.position, size, transform.forward, out hitInfo, transform.rotation, MAX_DISTANCE, layerMask, QueryTriggerInteraction.Ignore))
            return;

        Hittable hittable = hitInfo.collider.GetComponent<Hittable>();
        if(hittable)
            hittable.Hit(damage);

        SetLineRendererPositions(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
    }

    private void SetWidth(float width)
    {
        /*
         * This line renderer is stupid as shit. It will sometimes ignore the requests to change the line width if you use start/endWidth methods.
         * Using the AnimationCurve seems to work. I'm not 100% sure widthMultiplier will always work. But it seems to be. Keeping the AnimationCurve
         * around because fuck line renderers.
         */

        /*AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, width);
        curve.AddKey(1, width);
        lineRenderer.widthCurve = curve;*/

        lineRenderer.widthMultiplier = width;
    }

    private float GetWidth()
    {
        return lineRenderer.startWidth;
    }
}