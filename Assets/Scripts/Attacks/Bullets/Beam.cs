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
        //TODO object pooling
        linePositions = new Vector3[2];
        lineRenderer = GetComponentInChildren<LineRenderer>();
        updateAction = DoNothingAction;
        SetWidth(width.max);
        //SetInitialPositions();
        //MyDisable();
        //StartCoroutine(BringTheWidthDown());
    }

    /*private void SetInitialPositions()
    {
        RaycastHit hitInfo;
        float currentWidth = GetWidth();
        Vector3 size = new Vector3(currentWidth / 2, Y_SIZE, currentWidth / 2);
        Debug.Log("End position: " + (transform.position + transform.forward * MAX_DISTANCE));
        Now the raycasts arent working :(
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, MAX_DISTANCE, initialLayerMaskCheck, QueryTriggerInteraction.Ignore))
        {
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            //TODO move this to common logic method
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
            Debug.Log("In here mother fucker");
        }
        else
        {
            Vector3 fuck = transform.position + transform.forward * MAX_DISTANCE;
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
            lineRenderer.SetPosition(1, fuck);
            Debug.Log("Well shit");
        }
    }*/

    private IEnumerator BringTheWidthDown()
    {
        do
        {
            SetWidth(GetWidth() - 0.1f);
            yield return null;
        } while (GetWidth()> width.min);

        SetWidth(width.min);
        //StartBeam();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        updateAction(myDeltaTime, timeScale);
    }

    public bool AtMaxBeamWidth()
    {
        return false;
        //TODO this needs to be changed homie
        //return GetWidth() >= width.max;
    }

    public bool AtMinBeamWidth()
    {
        return GetWidth() <= width.min;
    }

    public void StartBeam()
    {
        Debug.Log("Starting the beammmmmmm");
        MyEnable();
        updateAction = ExpandBeamAction;
        //SetWidth(width.min);
        SetWidth(width.max);
    }

    public void EndBeam()
    {
        updateAction = ShrinkBeamAction;
    }

    private void ExpandBeamAction(float deltaTime, float timeScale)
    {
        float currentWidth = GetWidth();
        Debug.Log("CURRENT WIDTH " + currentWidth);
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
        Debug.Log("Full on attacking!");
        ShootBeam();
    }

    private void ShootBeam()
    {
        /*RaycastHit hitInfo;
        float currentWidth = GetWidth();
        Vector3 size = new Vector3(currentWidth / 2, Y_SIZE, currentWidth / 2);
        Ray ray = new Ray(transform.position, transform.forward);
        if (!Physics.BoxCast(transform.position, size, transform.forward, out hitInfo, transform.rotation, MAX_DISTANCE, layerMask, QueryTriggerInteraction.Ignore))
            return;

        Hittable hittable = hitInfo.collider.GetComponent<Hittable>();
        if(hittable)
            hittable.Hit(damage);

        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));*/
    }

    private void SetWidth(float width)
    {
        //TODO AnimationCurve is a class and using new may not be good. Figure out a better way to handle this
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        //TODO you don't have to use new here. There's other ways of setting the curve but I don't remember how at the moment
        Debug.Log("SET WIDTH " + width);
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, width);
        curve.AddKey(1, width);
        lineRenderer.widthCurve = curve;

        //lineRenderer.widthMultiplier = width;
    }

    private float GetWidth()
    {
        return lineRenderer.startWidth;
    }
}
