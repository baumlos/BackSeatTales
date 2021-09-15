using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Complaints : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private AudioClip[] _audioClips;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _dialogue.GetPassengerVolume();
    }

    public void PlayComplaint()
    {
        if (_dialogue.IsSilent() && !_audioSource.isPlaying)
        {
            var i = Random.Range(0, _audioClips.Length);
            _audioSource.clip = _audioClips[i];
            _audioSource.Play();
        }
    }
    
}
