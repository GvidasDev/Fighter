using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftObject : MonoBehaviour
{
    [SerializeField] private GiftObjectSO giftObjectSO;
    private IGiftObjectParent giftObjectParent;

    public GiftObjectSO GetObjectSO()
    {
        return giftObjectSO;
    }

    public void SetGiftObjectParent(IGiftObjectParent giftObjectParent)
    {
        if (this.giftObjectParent != null)
        {
            this.giftObjectParent.ClearGiftObject();
        }

        this.giftObjectParent = giftObjectParent;

        if (giftObjectParent.HasGiftObject())
        {
            Debug.LogError("has kitchen object");
        }

        giftObjectParent.SetGiftObject(this);

        transform.parent = giftObjectParent.GetGiftObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public IGiftObjectParent GetGiftObjectParent()
    {
        return giftObjectParent;
    }

    public void DestroySelf()
    {
        giftObjectParent.ClearGiftObject();
        Destroy(gameObject);
    }

    public static GiftObject SpawnGiftObject(GiftObjectSO giftObjectSO, IGiftObjectParent giftObjectParent)
    {
        Transform giftObjectTransform = Instantiate(giftObjectSO.prefab);

        GiftObject giftObject = giftObjectTransform.GetComponent<GiftObject>();

        giftObject.SetGiftObjectParent(giftObjectParent);

        return giftObject;
    }
}
