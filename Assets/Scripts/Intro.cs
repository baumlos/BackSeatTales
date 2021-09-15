using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private GameObject panelImageBox;
    [SerializeField] private List<Sprite> panels;

    [Header("SFX")]
    [SerializeField] private AudioClip engineSound;
    [SerializeField] [Range(0, 1)] float engineSoundVolume = 0.25f;

    [SerializeField] private List<AudioClip> voiceClips;
    [SerializeField] [Range(0, 5)] float voiceClipVolume = 5.0f;

    [SerializeField] private TMP_Text dialogueBox;
    [SerializeField] private List<string> voiceString;
    [SerializeField] private float waitTime = 5.0f;

    private Image pannelImage;

    // Start is called before the first frame update
    void Start()
    {
        pannelImage = panelImageBox.GetComponent<Image>();
        pannelImage.sprite = panels[0];

        StartCoroutine(StartIntro());    
    }
    

    private IEnumerator StartIntro()
    {
        // Play sounds
        PlayEngineSound();

        yield return new WaitForSeconds(waitTime);

        // Show panels
        for(int i = 0; i < panels.Count; i++)
        {
            yield return StartCoroutine(PlayAndShowDialog(i));
        }

        //Hide itself
        gameObject.SetActive(false);
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
        pannelImage.sprite = panels[index];
        dialogueBox.text = voiceString[index];

        if (voiceClips[index] != null)
        {
            AudioSource.PlayClipAtPoint(voiceClips[index], Camera.main.transform.position, voiceClipVolume);
        }

        yield return new WaitForSeconds(waitTime);
    }
}
