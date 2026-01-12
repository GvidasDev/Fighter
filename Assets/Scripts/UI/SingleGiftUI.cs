using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleGiftUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer giftIcon;

    public void SetGiftSO(GiftObjectSO giftObjectSO) {
        giftIcon.GetComponent<SpriteRenderer>().color = giftObjectSO.color;
    }
}
