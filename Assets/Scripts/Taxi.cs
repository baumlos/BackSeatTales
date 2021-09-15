using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Taxi : MonoBehaviour
{
    private static readonly int ANIM_DIRECTION = Animator.StringToHash("directionX");
    private const float SCREENWRAP_Y_ADJUST = 0.5f;
    
    [Header("General Settings")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _screenWrap;
    
    [Header("On Getting Hit Settings")]
    [SerializeField] private float _blinkTimeSingle = 0.2f;
    [SerializeField] private float _blinkTimeTotal = 2f;

    [Header("Sound Effects")] 
    [SerializeField] private AudioSource _audioSourceEffects;
    [SerializeField] private AudioClip[] _crashEffects;
    [SerializeField] private bool _pickRandomEffect;
    
    [Header("References")]
    [SerializeField] private UiManager _uiManager;

    public bool IsInvincible { get; private set; }

    // references
    private Transform _transform;
    private Renderer _renderer;
    private Animator _animator;
    private AudioSource _audioSourceEngine;
    
    private WaitForSeconds _waitForBlink;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
        _audioSourceEngine = GetComponent<AudioSource>();
        
        _waitForBlink = new WaitForSeconds(_blinkTimeSingle);

        _screenWrap = new Vector2(_screenWrap.x, _screenWrap.y - SCREENWRAP_Y_ADJUST);
    }
    
    void Update()
    {
        // Get input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (GameData.Instance.IsPaused)
        {
            _animator.speed = 0;
            _audioSourceEngine.Pause();
            return;
        }
        
        _animator.speed = 1;
        
        if(!_audioSourceEngine.isPlaying)
            _audioSourceEngine.UnPause();

        // move
        var amount = (inputH * Vector3.right + inputV * Vector3.up) * _speed * Time.deltaTime;
        _transform.Translate(amount, Space.Self);
        
        // TODO add animation
        _animator.SetFloat(ANIM_DIRECTION, inputH);
        
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
        
        if (_pickRandomEffect)
        {
            var i = Random.Range(0, _crashEffects.Length - 1);
            _audioSourceEffects.clip = _crashEffects[i];
        }
        _audioSourceEffects.Play();
        
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
