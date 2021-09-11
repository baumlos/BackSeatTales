using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Taxi : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _screenWrap;
    
    [Header("On Getting Hit Settings")]
    [SerializeField] private float _blinkTimeSingle = 0.2f;
    [SerializeField] private float _blinkTimeTotal = 2f;

    public int CurrentLives { get; private set; }
    public bool IsInvincible { get; private set; }
    public State VehicleState { get; private set; }
    
    // references
    private Transform _transform;
    private Renderer _renderer;
    
    private WaitForSeconds _waitForBlink;

    private void Awake()
    {
        CurrentLives = _maxLives;
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _waitForBlink = new WaitForSeconds(_blinkTimeSingle);
    }
    
    void Update()
    {
        // Get input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (GameData.Instance.IsPaused)
            return;

        var amount = (inputH * Vector3.right + inputV * Vector3.up) * _speed * Time.deltaTime;
        _transform.Translate(amount, Space.Self);
        
        // Screen border
        if (_transform.localPosition.x > _screenWrap.x)
            _transform.localPosition = new Vector3(_screenWrap.x, _transform.localPosition.y, _transform.localPosition.z);
        if (_transform.localPosition.x < -_screenWrap.x)
            _transform.localPosition = new Vector3(-_screenWrap.x, _transform.localPosition.y, _transform.localPosition.z);
        if (_transform.localPosition.y > _screenWrap.y)
            _transform.localPosition = new Vector3(_transform.localPosition.x, _screenWrap.y, _transform.localPosition.z);
        if (_transform.localPosition.y < -_screenWrap.y)
            _transform.localPosition = new Vector3(_transform.localPosition.x, -_screenWrap.y, _transform.localPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInvincible)
            return;
        
        if (CurrentLives > 1)
            StartCoroutine(GetHit());
        else
            Destroy();
    }

    private IEnumerator GetHit()
    {
        // start
        CurrentLives--;
        IsInvincible = true;
        
        // animation: blink
        var blinkAmount = _blinkTimeTotal / _blinkTimeSingle;
        for (int i = 0; i < blinkAmount; i++)
        {
            _renderer.enabled = i % 2 != 0;
            yield return _waitForBlink;
        }
        _renderer.enabled = true;

        // finish
        IsInvincible = false;
    }

    private void Destroy()
    {
        Debug.Log("Death");  
    }
}
