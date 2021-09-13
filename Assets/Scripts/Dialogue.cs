using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Serializable]
    public struct DialogueLine
    {
        public Passenger speaker;
        public string line;
        public AudioClip audio;
    }
    
    [Header("General Settings")]
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _resetHeight = 10;
    [SerializeField] private Vector2 _resetWidthBetween = new Vector2(-5, 5);

    [Header("Text References")] 
    [SerializeField] private TMP_Text _name;
    [SerializeField] private SpriteRenderer _nameBackground;
    [SerializeField] private TMP_Text _text;

    [Header("Passenger")] 
    [SerializeField] private Passenger _passenger;
    [SerializeField] private DialogueLine[] _script;

    private Transform _transform;
    private AudioSource _audioSource;

    private int _currentIndex;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        for (int i = 0; i < _script.Length; i++)
        {
            var line = _script[i];
            if (line.speaker == null)
                line.speaker = _passenger;
            _script[i] = line;
        }

        UpdateTextbox();
    }

    private void Update()
    {
        if(GameData.Instance.IsPaused)
            return;

        var amount = GameData.Instance.SpeedConstant * _speed * Time.deltaTime;
        _transform.Translate(Vector3.down * amount, Space.World);
    }

    public void Respawn(bool next)
    {
        // play audio clip
        if (_script[_currentIndex].audio != null)
        {
            _audioSource.clip = _script[_currentIndex].audio;
            _audioSource.Play();
        }
        
        var spawnPos = new Vector2(Random.Range(_resetWidthBetween.x, _resetWidthBetween.y), _resetHeight);
        _transform.position = spawnPos;

        if (next)
        {
            if (_currentIndex >= _script.Length-1)
            {
                EndGame();
            }
            else
            {
                _currentIndex++;
                UpdateTextbox();
            }
        }
    }

    private void UpdateTextbox()
    {
        _name.text = _script[_currentIndex].speaker.personName;
        _nameBackground.color = _script[_currentIndex].speaker.color;
        _text.text = _script[_currentIndex].line;
        _text.font = _script[_currentIndex].speaker.font;
    }

    private void EndGame()
    {
        Debug.Log("End Game");
        GameData.Instance.IsPaused = true;
    }
}
