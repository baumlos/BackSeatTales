using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePath : MonoBehaviour
{
    private ObstacleWaveConfig waveConfig;
    private List<Transform> wayPoints;
    private int currentWayPointIndex = 0;
    private float speed;
    private bool shouldIncreaseSpeed = false;
    private float accelerate;
    private int moveOption = 1;

    private Vector2 screenWrap = new Vector2(10, 10);

	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GameData.Instance.IsPaused)
            return;

        if (waveConfig == null)
            return;
        
        if (shouldIncreaseSpeed)
        {
            speed += accelerate;
        }

        Move();
    }

    public void SetWaveConfig(ObstacleWaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;

        wayPoints = waveConfig.GetWayPoints();
        transform.position = wayPoints[currentWayPointIndex].transform.position;
        speed = waveConfig.GetMoveSpeed();
        shouldIncreaseSpeed = waveConfig.GetShouldIncreaseSpeed();
        moveOption = waveConfig.GetMoveOption();
        accelerate = waveConfig.GetAcceleration();
    }

    private void Move()
    {
        if (moveOption == 1)
        {
            MoveDown();
        }
        else if (moveOption == 2)
        {
            MoveDiagonally();
        }
        else if (moveOption == 3)
        {
            MoveAlongAPath();
        }
        else
        {
            //Default
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        ReBornIfOutOfScreen();
    }

    private void MoveDiagonally()
    {
        transform.Translate((Vector3.down + Vector3.right).normalized * speed * Time.deltaTime, Space.World);
        ReBornIfOutOfScreen();
    }

    private void ReBornIfOutOfScreen()
    {
        if (transform.localPosition.x > screenWrap.x || 
            transform.localPosition.x < -screenWrap.x || 
            transform.localPosition.y < -screenWrap.y)
        {
            ReInstantiateAndRandomizeMoveOptions();
        }
    }

    private void ReInstantiateAndRandomizeMoveOptions()
    {
        // Re-instantiate the speed and position
            transform.localPosition = wayPoints[currentWayPointIndex].transform.position;
            speed = waveConfig.GetMoveSpeed();

        // Randomize the moveOption
            moveOption = Random.Range(1, 4);
    }

    private void MoveAlongAPath()
    {
        float step = speed * Time.deltaTime;

        // If not yet reached last waypoint
        if (currentWayPointIndex <= wayPoints.Count - 1)
        {
            Vector3 current = transform.position;
            Vector3 targetPosition = wayPoints[currentWayPointIndex].position;

            // Move toward to target waypoint
            transform.position = Vector3.MoveTowards(current, targetPosition, step);

            // If we've reached the target
            if (current == targetPosition)
            {
                // If so, increment target waypoint
                currentWayPointIndex++;
            }
        }
        else
        {
            currentWayPointIndex = 0;
            ReInstantiateAndRandomizeMoveOptions();
        }
    }
}
