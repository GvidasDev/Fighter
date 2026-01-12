using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBag : BaseGiftParentObject
{
    [SerializeField] private GiftObjectSO giftObjectSO;
    public override void Interact(Player player) {
        if (!HasGiftObject()) {
            if (!player.HasGiftObject()) {
                GiftObject.SpawnGiftObject(giftObjectSO, player);
            }
        }
    }
}
