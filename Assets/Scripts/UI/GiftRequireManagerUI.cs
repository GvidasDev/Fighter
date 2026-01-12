using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftRequireManagerUI : MonoBehaviour
{
    [SerializeField] private ParentTree parentTree;
    [SerializeField] private Transform container;
    [SerializeField] private Transform singleGiftTemplate;

    private void Awake() {
        singleGiftTemplate.gameObject.SetActive(false);
    }

    private void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == singleGiftTemplate) continue;
            Destroy(child.gameObject);
        }

        TreeGiftListSO treeGiftListSO = parentTree.GetGiftsList();
        foreach (GiftObjectSO giftObjectSO in treeGiftListSO.giftObjectsSOList) {
            Transform recipeTransform = Instantiate(singleGiftTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<SingleGiftUI>().SetGiftSO(giftObjectSO);
        }
    }
}
