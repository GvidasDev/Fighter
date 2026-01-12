using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGiftParentObject : MonoBehaviour, IGiftObjectParent
{
    [SerializeField] private Transform parentTopPoint;

    private GiftObject giftObject;
    public virtual void Interact(Player player)
    {

    }

    public Transform GetGiftObjectFollowTransform()
    {
        return parentTopPoint;
    }

    public void SetGiftObject(GiftObject giftObject)
    {
        this.giftObject = giftObject;
    }

    public GiftObject GetGiftObject()
    {
        return giftObject;
    }

    public void ClearGiftObject()
    {
        giftObject = null;
    }

    public bool HasGiftObject()
    {
        return giftObject != null;
    }
}
