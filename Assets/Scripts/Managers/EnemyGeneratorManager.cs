using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorManager : MonoBehaviour {
    [SerializeField] private List<GameObject> enemyObjects;
    [SerializeField] private Transform enemyInstantiatePoint;
    [SerializeField] private float startTime = 0f;
    [SerializeField] private float repeatRate = 3f;

    private int secondReferencePoint = 1000;
    private int thirdReferencePoint = 2000;

    private void Start() {
        InvokeRepeating(nameof(SpawnEnemies), startTime, repeatRate);
    }

    private void SpawnEnemies() {
        if (Player.Instance.GetScore() >= 0 &&
            Player.Instance.GetScore() < secondReferencePoint) {
            // only one
            Instantiate(enemyObjects[0], enemyInstantiatePoint.position, Quaternion.identity);
        } else if (Player.Instance.GetScore() >= secondReferencePoint &&
            Player.Instance.GetScore() < thirdReferencePoint) {
            //only two
            Instantiate(enemyObjects[1], enemyInstantiatePoint.position, Quaternion.identity);
        } else if (Player.Instance.GetScore() >= thirdReferencePoint) {
            //only three
            Instantiate(enemyObjects[2], enemyInstantiatePoint.position, Quaternion.identity);
        }
    }
}