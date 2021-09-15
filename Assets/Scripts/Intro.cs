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
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject panelImageBoxLeft;
    [SerializeField] private GameObject panelImageBoxRightTop;
    [SerializeField] private GameObject panelImageBoxRightBottom;

    [Header("SFX")]
    [SerializeField] private AudioClip engineSound;
    [SerializeField] [Range(0, 1)] float engineSoundVolume = 0.25f;

    [SerializeField] private List<AudioClip> voiceClips;
    [SerializeField] [Range(0, 30)] float voiceClipVolume = 5.0f;

    [SerializeField] private TMP_Text dialogueBox;
    [SerializeField] private List<string> voiceString;
    [SerializeField] private float waitTime = 1.0f;

    private const string GAME_SCENE = "Game";

    private Image backgroundImage;
    private Color32 GREYOUT = new Color32(255, 255, 255, 50);
    private Color32 WHITE = new Color32(255, 255, 255, 255);
    private Color32 BG_LEFT = new Color32(170, 209, 213, 255);
    private Color32 BG_RIGHT_TOP = new Color32(173, 227, 242, 255);
    private Color32 BG_RIGHT_BOTTOM = new Color32(39, 32, 52, 255);

    void Start()
    {
        backgroundImage = image.GetComponent<Image>();
        backgroundImage.color = BG_LEFT;
        panelImageBoxRightTop.GetComponent<Image>().color = GREYOUT;
        panelImageBoxRightBottom.GetComponent<Image>().color = GREYOUT;
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
            backgroundImage.color = BG_RIGHT_TOP;
            panelImageBoxLeft.GetComponent<Image>().color = GREYOUT;
            panelImageBoxRightTop.GetComponent<Image>().color = WHITE;
            panelImageBoxRightBottom.GetComponent<Image>().color = GREYOUT;
        }

        if (index == 2)
        {
            backgroundImage.color = BG_RIGHT_BOTTOM;
            panelImageBoxRightTop.GetComponent<Image>().color = GREYOUT;
            panelImageBoxRightBottom.GetComponent<Image>().color = WHITE;
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