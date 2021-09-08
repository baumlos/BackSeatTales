using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxi : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private float _speed = 1f;

    private Transform _transform;
    private int _currentLives;

    private void Awake()
    {
        _currentLives = _maxLives;
        _transform = GetComponent<Transform>();
    }
    
    void Update()
    {
        // Get input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        _transform.Translate((inputH * Vector3.right + inputV * Vector3.up) * _speed * Time.deltaTime, Space.World);
    }
}
