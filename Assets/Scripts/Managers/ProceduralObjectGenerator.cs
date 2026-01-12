using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectGenerator : MonoBehaviour {
    [Header("Specific Objects")]
    public GameObject[] specificObjects;
    public Vector3[] specificPositions;

    [Header("Random Objects")]
    public GameObject randomObjectPrefab;
    public int numberOfRandomObjects = 15;
    public float randomObjectMinDistance = 2f;
    public float randomObjectSpacing = 1.5f;

    [Header("Additional Objects")]
    public GameObject additionalObjectPrefab;
    public int numberOfAdditionalObjects = 4;
    public float additionalObjectMinDistance = 4f;
    public float additionalObjectSpacing = 2f;

    [Header("Spawn Area")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);

    private List<Vector3> occupiedPositions = new List<Vector3>();

    void Start() {
        for (int i = 0; i < specificObjects.Length; i++) {
            if (i < specificPositions.Length) {
                Instantiate(specificObjects[i], specificPositions[i], Quaternion.identity);
                occupiedPositions.Add(specificPositions[i]);
            }
        }

        bool additionalObjectsGenerated = GenerateAdditionalObjects();

        int adjustedRandomObjectCount = additionalObjectsGenerated ? numberOfRandomObjects : Mathf.Max(numberOfRandomObjects - numberOfAdditionalObjects, 0);

        GenerateObjects(randomObjectPrefab, adjustedRandomObjectCount, randomObjectMinDistance, randomObjectSpacing);
    }

    void GenerateObjects(GameObject prefab, int count, float minDistanceFromCenter, float minSpacingBetweenObjects) {
        int spawnedCount = 0;
        int maxAttempts = count * 10;
        int attempts = 0;

        while (spawnedCount < count && attempts < maxAttempts) {
            Vector3 randomPosition = GetRandomPositionWithinArea();
            if (IsValidPosition(randomPosition, minDistanceFromCenter, minSpacingBetweenObjects)) {
                Instantiate(prefab, randomPosition, Quaternion.identity);
                occupiedPositions.Add(randomPosition);
                spawnedCount++;
            }
            attempts++;
        }

        if (spawnedCount < count) {
            Debug.LogWarning($"Only {spawnedCount} out of {count} objects were spawned. Consider adjusting the spawn area or minimum distance.");
        }
    }

    bool GenerateAdditionalObjects() {
        Vector3[] quadrantCenters = GetQuadrantCenters();
        int objectsPerQuadrant = numberOfAdditionalObjects / quadrantCenters.Length;
        int extraObjects = numberOfAdditionalObjects % quadrantCenters.Length;

        int additionalObjectsPlaced = 0;

        for (int i = 0; i < quadrantCenters.Length; i++) {
            int targetObjectsInQuadrant = objectsPerQuadrant + (i < extraObjects ? 1 : 0);
            int spawnedInQuadrant = 0;
            int maxAttempts = 10 * targetObjectsInQuadrant;

            while (spawnedInQuadrant < targetObjectsInQuadrant && maxAttempts > 0) {
                Vector3 randomPosition = GetRandomPositionWithinQuadrant(quadrantCenters[i], spawnAreaSize.x / 4, spawnAreaSize.z / 4);
                if (IsValidPosition(randomPosition, additionalObjectMinDistance, additionalObjectSpacing)) {
                    Instantiate(additionalObjectPrefab, randomPosition, Quaternion.identity);
                    occupiedPositions.Add(randomPosition);
                    spawnedInQuadrant++;
                    additionalObjectsPlaced++;
                }
                maxAttempts--;
            }
        }

        if (additionalObjectsPlaced < numberOfAdditionalObjects) {
            Debug.LogWarning($"Could not place all additional objects. Placed {additionalObjectsPlaced} out of {numberOfAdditionalObjects}.");
            return false;
        }
        return true;
    }

    Vector3[] GetQuadrantCenters() {
        float halfWidth = spawnAreaSize.x / 2;
        float halfDepth = spawnAreaSize.z / 2;

        return new Vector3[]
        {
            spawnAreaCenter + new Vector3(-halfWidth / 2, 0, halfDepth / 2),
            spawnAreaCenter + new Vector3(halfWidth / 2, 0, halfDepth / 2),
            spawnAreaCenter + new Vector3(-halfWidth / 2, 0, -halfDepth / 2),
            spawnAreaCenter + new Vector3(halfWidth / 2, 0, -halfDepth / 2)
        };
    }

    bool IsValidPosition(Vector3 position, float minDistanceFromCenter, float minSpacingBetweenObjects) {
        if (Vector3.Distance(spawnAreaCenter, position) < minDistanceFromCenter) {
            return false;
        }

        foreach (Vector3 occupiedPos in occupiedPositions) {
            if (Vector3.Distance(occupiedPos, position) < minSpacingBetweenObjects) {
                return false;
            }
        }

        return true;
    }

    Vector3 GetRandomPositionWithinArea() {
        float x = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float z = Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);
        return new Vector3(x, spawnAreaCenter.y, z);
    }

    Vector3 GetRandomPositionWithinQuadrant(Vector3 quadrantCenter, float quadrantWidth, float quadrantDepth) {
        float x = Random.Range(quadrantCenter.x - quadrantWidth, quadrantCenter.x + quadrantWidth);
        float z = Random.Range(quadrantCenter.z - quadrantDepth, quadrantCenter.z + quadrantDepth);
        return new Vector3(x, spawnAreaCenter.y, z);
    }
}
