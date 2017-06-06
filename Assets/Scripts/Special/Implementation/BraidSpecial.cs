using UnityEngine;

[CreateAssetMenu(fileName = "BraidSpecial", menuName = "ScriptableObjects/Specials/Implementation/BraidSpecial")]
public class BraidSpecial : SpecialAction
{
    public float ySpawnOffset;
    public BraidRing braidRingPrefab;

    private BraidRing braidRingInstance;

    public void OnEnable()
    {
        braidRingInstance = Instantiate(braidRingPrefab, null, true) as BraidRing;
        braidRingInstance.gameObject.SetActive(false);
    }

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
}