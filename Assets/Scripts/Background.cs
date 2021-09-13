using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float halfLength = 20;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(GameData.Instance.IsPaused)
            return;
        
        float amount = _speed * Time.deltaTime;
        transform.Translate(amount * Vector3.down, Space.World);

        if (transform.position.y < -halfLength)
        {
            transform.position = new Vector3(_transform.position.x, halfLength, _transform.position.z);
        }
    }
}
