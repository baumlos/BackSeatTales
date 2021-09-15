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

    [Header("Sound Effect")] 
    [SerializeField] private AudioSource _audioSourceEffect;
    [SerializeField] private AudioClip[] _soundEffects;
    [SerializeField] private bool _pickRandomEffect;

    private Transform _transform;
    private AudioSource _audioSourceVoice;

    private int _currentIndex;
    private bool _soundTriggered;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _audioSourceVoice = GetComponent<AudioSource>();
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
        _soundTriggered = false;
        var spawnPos = new Vector2(Random.Range(_resetWidthBetween.x, _resetWidthBetween.y), _resetHeight);
        _transform.position = spawnPos;

        if (next)
        {
            if (_pickRandomEffect)
            {
                var i = Random.Range(0, _soundEffects.Length);
                _audioSourceEffect.clip = _soundEffects[i];
            }
            _audioSourceEffect.Play();
            
            if (_currentIndex >= _script.Length-1)
            {
                _currentIndex++;
                _transform.position = new Vector3(0, -100, 0);
                EndGame();
            }
            else
            {
                _currentIndex++;
                UpdateTextbox();
            }
        }
    }

    public void PlayClip()
    {
        // play audio clip
        _soundTriggered = true;
        if (_script[_currentIndex].audio != null)
        {
            _audioSourceVoice.clip = _script[_currentIndex].audio;
            _audioSourceVoice.volume = _script[_currentIndex].speaker.volume;
            _audioSourceVoice.Play();
        }
    }

    private void UpdateTextbox()
    {
        if (_currentIndex == 0)
        {
            var passenger = _script[_currentIndex].speaker;
            _name.text = passenger.personName;
            _nameBackground.color = passenger.color;
            _text.font = passenger.font;
        }
        else
        {
            if (!_script[_currentIndex - 1].speaker.personName.Equals(_script[_currentIndex].speaker.personName))
            {
                var passenger = _script[_currentIndex].speaker;
                _name.text = passenger.personName;
                _nameBackground.color = passenger.color;
                _text.font = passenger.font;
            }
        }
        _text.text = _script[_currentIndex].line;
    }

    private void EndGame()
    {
        StartCoroutine(WaitForLastClipToFinish());
        // GameData.Instance.IsPaused = true;
    }

    private IEnumerator WaitForLastClipToFinish()
    {
        while (_audioSourceVoice.isPlaying)
        {
            yield return null;
        }

        GameData.Instance.IsPaused = true;
    }

    public bool IsSilent()
    {
        return _soundTriggered && !_audioSourceVoice.isPlaying;
    }

    public float GetPassengerVolume()
    {
        return _passenger.volume;
    }
}
