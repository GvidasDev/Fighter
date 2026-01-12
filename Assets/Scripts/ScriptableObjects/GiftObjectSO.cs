using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GiftObjectSO : ScriptableObject
{
    public Transform prefab;
    public string objectName;
    public Color color;
    public Sprite sprite;
}
