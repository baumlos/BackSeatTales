using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    /*
    1. panel 1 appears
    2. enigne sound plays
    3. a voice line plays
    4. text appears
    5. wait a bit
    6. panel 2 appears
    */
    [SerializeField] private GameObject panelImageBoxLeft;
    [SerializeField] private GameObject panelImageBoxRightTop;
    [SerializeField] private GameObject panelImageBoxRightBottom;

    [Header("SFX")]
    [SerializeField] private AudioClip engineSound;
    [SerializeField] [Range(0, 1)] float engineSoundVolume = 0.25f;

    [SerializeField] private List<AudioClip> voiceClips;
    [SerializeField] [Range(0, 10)] float voiceClipVolume = 5.0f;

    [SerializeField] private TMP_Text dialogueBox;
    [SerializeField] private List<string> voiceString;
    [SerializeField] private float waitTime = 0.5f;

    private const string GAME_SCENE = "Game";

    void Start()
    {
        panelImageBoxRightTop.SetActive(false);
        panelImageBoxRightBottom.SetActive(false);
        StartCoroutine(StartIntro());    
    }
    

    private IEnumerator StartIntro()
    {
        // Play sounds
        PlayEngineSound();

        yield return new WaitForSeconds(waitTime);

        // Show panels
        for(int i = 0; i < voiceClips.Count; i++)
        {
            yield return StartCoroutine(PlayAndShowDialog(i));
        }

        //load game scene
        StartCoroutine(LoadGameScene());
    }

    private void PlayEngineSound()
    {
        if (engineSound != null)
        {
            AudioSource.PlayClipAtPoint(engineSound, Camera.main.transform.position, engineSoundVolume);
        }
    }

    private IEnumerator PlayAndShowDialog(int index)
    {
        if (index == 1)
        {
            panelImageBoxRightTop.SetActive(true);
        }
        if (index == 2)
        {
            panelImageBoxRightBottom.SetActive(true);
        }


        dialogueBox.text = voiceString[index];

        if (voiceClips[index] != null)
        {
            AudioSource.PlayClipAtPoint(voiceClips[index], Camera.main.transform.position, voiceClipVolume);
        }

        yield return new WaitForSeconds(waitTime);
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(GAME_SCENE);
    }
}