using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGiftObjectParent
{
    public Transform GetGiftObjectFollowTransform();

    public void SetGiftObject(GiftObject giftObject);

    public GiftObject GetGiftObject();

    public void ClearGiftObject();

    public bool HasGiftObject();
}
