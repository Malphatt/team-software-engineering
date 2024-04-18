using UnityEngine;

public class Colliders
{
    public GameObject[] HeadColliders { get; }
    public GameObject[] BodyColliders { get; }

    public Colliders(GameObject[] headColliders, GameObject[] bodyColliders)
    {
        HeadColliders = headColliders;
        BodyColliders = bodyColliders;
    }
}

public class MeleeHitbox : MonoBehaviour
{
    public GameObject HeadHitbox;
    public GameObject BodyHitbox;

    public Colliders GetColliders(float range)
    {
        HeadHitbox.transform.localScale = new Vector3(HeadHitbox.transform.localScale.x, range / 2, HeadHitbox.transform.localScale.z);
        HeadHitbox.transform.localPosition = new Vector3(HeadHitbox.transform.localPosition.x, HeadHitbox.transform.localPosition.y, (3 * range / 8) + 0.5f);
        BodyHitbox.transform.localScale = new Vector3(BodyHitbox.transform.localScale.x, range / 2, BodyHitbox.transform.localScale.z);
        BodyHitbox.transform.localPosition = new Vector3(BodyHitbox.transform.localPosition.x, BodyHitbox.transform.localPosition.y, (3 * range / 8) + 0.5f);
        return new Colliders(HeadHitbox.GetComponent<Hitbox>().GameObjectColliders, BodyHitbox.GetComponent<Hitbox>().GameObjectColliders);
    }
}
