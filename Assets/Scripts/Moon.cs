using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public bool Activated { get; set; }

    [SerializeField] private float _speed = 1.5f;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Activated && _transform.position.y > 0 && !GameData.Instance.IsPaused)
        {
            float amount = _speed * Time.deltaTime;
            _transform.Translate(amount * Vector3.down, Space.World);
        }
    }
}
