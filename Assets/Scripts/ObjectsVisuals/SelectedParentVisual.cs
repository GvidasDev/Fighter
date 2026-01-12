using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedParentVisual : MonoBehaviour
{
    [SerializeField] private BaseGiftParentObject baseParent;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedGiftParentChanged += Instance_OnSelectedGiftParentChanged;
    }

    private void Instance_OnSelectedGiftParentChanged(object sender, Player.OnSelectedGiftParentChangedEventArgs e)
    {
        if (e.selectedParent == baseParent)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
