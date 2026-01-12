using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentTree : BaseGiftParentObject {
    private GiftObjectSO giftObjectSO;

    [SerializeField] private TreeGiftListsContainerSO allGiftsListSO;

    [SerializeField] Image image1;
    [SerializeField] Image image2;

    [SerializeField] Sprite defaultSprite;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fullSoundClip;

    private TreeGiftListSO giftsListSO;
    private List<GiftObjectSO> validGiftObjectSOList;
    private List<GiftObjectSO> giftObjectSOList;

    private bool isFull = false;

    private void Awake() {
        validGiftObjectSOList = new List<GiftObjectSO>();
        giftObjectSOList = new List<GiftObjectSO>();
    }

    private void Start() {
        giftsListSO = allGiftsListSO.treeGiftLists[Random.Range(0, allGiftsListSO.treeGiftLists.Count)];
        InstantiateGiftsList(giftsListSO);
        image1.sprite = validGiftObjectSOList[0].sprite;
        image2.sprite = validGiftObjectSOList[1].sprite;
    }

    public override void Interact(Player player) {
        if (!IsTreeFull()) {
            if (!HasGiftObject()) {
                if (player.HasGiftObject()) {
                    GiftObjectSO giftObjectSO = player.GetGiftObject().GetObjectSO();
                    if (validGift(giftObjectSO)) {
                        player.GetGiftObject().SetGiftObjectParent(this);
                        ClearGiftObject();
                        Debug.Log("Added: " + giftObjectSO.name);
                        if (IsTreeFull()) {
                            StartCoroutine(HandleTreeFull(player));
                        }
                    }
                }
            }
        }
    }

    private bool validGift(GiftObjectSO giftObjectSO) {
        if (!validGiftObjectSOList.Contains(giftObjectSO)) {
            return false;
        }
        if (giftObjectSOList.Contains(giftObjectSO)) {
            return false;
        } else {
            giftObjectSOList.Add(giftObjectSO);
            return true;
        }
    }

    private void InstantiateGiftsList(TreeGiftListSO giftsListSO) {
        foreach (GiftObjectSO giftObjectSO in giftsListSO.giftObjectsSOList) {
            validGiftObjectSOList.Add(giftObjectSO);
        }
    }

    public bool IsTreeFull() {
        if (giftObjectSOList.Count == validGiftObjectSOList.Count) {
            if (!isFull) {
                isFull = true;
                image1.sprite = defaultSprite;
                image2.sprite = defaultSprite;
                if (audioSource != null && fullSoundClip != null) {
                    audioSource.PlayOneShot(fullSoundClip, 0.3f);
                }
                Debug.Log("TREE IS FULL");
            }
            return true;
        }
        return false;
    }

    private IEnumerator HandleTreeFull(Player player) {
        Debug.Log("Tree will reset in 30 seconds...");
        ClearChildObjects();
        yield return new WaitForSeconds(30f);

        giftsListSO = null;
        giftsListSO = allGiftsListSO.treeGiftLists[Random.Range(0, allGiftsListSO.treeGiftLists.Count)];
        validGiftObjectSOList.Clear();
        giftObjectSOList.Clear();
        InstantiateGiftsList(giftsListSO);
        image1.sprite = validGiftObjectSOList[0].sprite;
        image2.sprite = validGiftObjectSOList[1].sprite;
        isFull = false;

        player.AddScore(100);
        Debug.Log("Tree reset. Player awarded 100 points.");
    }

    private void ClearChildObjects() {
        Transform parentTransform = transform;
        Transform childObject = parentTransform.Find("Point");

        if (childObject != null) {
            foreach (Transform grandChild in childObject) {
                Destroy(grandChild.gameObject);
            }
        }
    }

    public TreeGiftListSO GetGiftsList() {
        return giftsListSO;
    }
}
