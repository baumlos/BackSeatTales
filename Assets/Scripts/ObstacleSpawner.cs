using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private List<ObstacleWaveConfig> configs;

    void Start()
    {
        if (configs != null)
        {
            StartCoroutine(SpawnAllWaves());
        }
    }
    
    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = 0; waveIndex < configs.Count; waveIndex++)
        {
            var currentWave = configs[waveIndex];
            yield return StartCoroutine(SpawnAllObstaclesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllObstaclesInWave(ObstacleWaveConfig waveConfig)
    {
        for (int obstacleCount = 0; obstacleCount < waveConfig.GetNumberOfObstacles(); obstacleCount++)
        {
            var newObstacle = Instantiate(
                waveConfig.GetObstaclePrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);
            newObstacle.GetComponent<ObstaclePath>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
