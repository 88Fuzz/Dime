using UnityEngine;

[CreateAssetMenu(fileName = "BraidSpecial", menuName = "ScriptableObjects/Specials/Implementation/BraidSpecial")]
public class BraidSpecial : SpecialAction
{
    public float ySpawnOffset;
    public BraidRing braidRingPrefab;
    public BraidTimeScaleModifier distanceSlowModifier;

    private BraidRing braidRingInstance;

    public override void DoAction(SpecialManager specialManager)
    {
        Vector3 spawnOffset = Vector3.zero;
        PlayerHittable playerHittable = specialManager.player.GetComponent<PlayerHittable>();
        if (playerHittable)
            spawnOffset.y =  ySpawnOffset - playerHittable.ySpawnOffset;

        braidRingInstance.gameObject.SetActive(false);
        braidRingInstance.transform.position = specialManager.player.transform.position - spawnOffset;
        braidRingInstance.gameObject.SetActive(true);
    }

    public override void SpecialRemoved(SpecialManager specialManager)
    {
        MyMonoBehaviourManager manager = Singleton<MyMonoBehaviourManager>.Instance;
        manager.DeregisterMyMonoBehaviourTimeScaleModifier(distanceSlowModifier);
    }

    public override void SpecialEquiped(SpecialManager specialManager)
    {
        if (braidRingInstance == null)
            CreateRing();
        MyMonoBehaviourManager manager = Singleton<MyMonoBehaviourManager>.Instance;
        distanceSlowModifier.braidRing = braidRingInstance;
        manager.RegisterMyMonoBehaviourTimeScaleModifier(distanceSlowModifier);
    }

    private void CreateRing()
    {
        braidRingInstance = Instantiate(braidRingPrefab, null, true) as BraidRing;
        braidRingInstance.gameObject.SetActive(false);
    }
}