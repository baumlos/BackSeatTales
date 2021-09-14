using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleWaveConfig", menuName = "Im-getting-out-of-here/ObstacleWaveConfig", order = 0)]
public class ObstacleWaveConfig : ScriptableObject
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float spawnRandomFactor = 0.3f;
    [SerializeField] private int numberOfObstacles = 5;
    [SerializeField] private bool shouldIncreaseSpeed = false;
    [SerializeField] private float accelerate = 0.05f;
    
    [Tooltip("Move Option between 1 and 3. 1: Move Down; 2: Move Diagonally; 3: Move Along A Path")]
    [SerializeField] private int moveOption = 1;

    public GameObject GetObstaclePrefab() { return obstaclePrefab; }
    public List<Transform> GetWayPoints()
    {
        var pathPoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            pathPoints.Add(child);
        }
        return pathPoints;
    }

    public float GetMoveSpeed() { return moveSpeed; }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return spawnRandomFactor; }
    public int GetNumberOfObstacles() { return numberOfObstacles; }
    public bool GetShouldIncreaseSpeed() { return shouldIncreaseSpeed; }
    public int GetMoveOption() { return moveOption; }
    public float GetAcceleration() { return accelerate; }
}
