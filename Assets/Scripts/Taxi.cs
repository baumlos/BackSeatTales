using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Taxi : MonoBehaviour
{
    private static readonly int ANIM_DIRECTION = Animator.StringToHash("directionX");
    
    [Header("General Settings")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _screenWrap;
    
    [Header("On Getting Hit Settings")]
    [SerializeField] private float _blinkTimeSingle = 0.2f;
    [SerializeField] private float _blinkTimeTotal = 2f;

    [Header("References")]
    [SerializeField] private UiManager _uiManager;
    
    public float debug;
    public float lastPosX;

    public bool IsInvincible { get; private set; }

    // references
    private Transform _transform;
    private Renderer _renderer;
    private Animator _animator;
    
    private WaitForSeconds _waitForBlink;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
        
        _waitForBlink = new WaitForSeconds(_blinkTimeSingle);
    }
    
    void Update()
    {
        // Get input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (GameData.Instance.IsPaused)
        {
            _animator.speed = 0;
            return;
        }

        _animator.speed = 1;

        // move
        var amount = (inputH * Vector3.right + inputV * Vector3.up) * _speed * Time.deltaTime;
        _transform.Translate(amount, Space.Self);
        
        // TODO add animation
        _animator.SetFloat(ANIM_DIRECTION, inputH);

        debug = inputH;
        
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
        if (other.gameObject.tag.Equals("Dialogue"))
            other.GetComponent<Dialogue>().Respawn(true);
        
        else if (!IsInvincible)
            StartCoroutine(GetHit());
    }

    private IEnumerator GetHit()
    {
        // start
        IsInvincible = true;
        _uiManager.TakePenalty();
        
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
}
