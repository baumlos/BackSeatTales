using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    private const string MENU_SCENE = "MainMenu";
    private const int PENALTY_VALUE = 1;
    private readonly string PENALTY_STRING = $"+{PENALTY_VALUE.ToString()}";
    
    [SerializeField] private Toggle _pauseToggle;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _penaltyText;
    [SerializeField] private float _penaltyPopupTime = 1;

    private float _timer;
    private float _penalty;
    private WaitForSeconds _penaltyPopupWaitForSeconds;

    private void Start()
    {
        // button events
        _pauseToggle.onValueChanged.AddListener(PauseGame);
        _menuButton.onClick.AddListener(MainMenu);
        _speedSlider.onValueChanged.AddListener(SpeedChange);

        _penaltyText.text = "";
        _penaltyPopupWaitForSeconds = new WaitForSeconds(_penaltyPopupTime);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        float time = _timer + GameData.Instance.Penalty;
        
        int m = (int) time / 60;
        int s = (int) time % 60;
        int ms = (int) (time % 1 * 10f);

        string min = m < 10 ? $"0{m}" : m.ToString();
        string sec = s < 10 ? $"0{s}" : s.ToString();
        string msec = ms < 10 ? $"0{ms}" : ms.ToString();
        
        _timerText.text = $"{min}:{sec}:{msec}";
    }

    private void PauseGame(bool isPaused)
    {
        _pauseToggle.isOn = isPaused;
        GameData.Instance.IsPaused = isPaused;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    private void SpeedChange(float speed)
    {
        GameData.Instance.SpeedConstant = speed;
    }

    public void TakePenalty()
    {
        GameData.Instance.Penalty += PENALTY_VALUE;
        StartCoroutine(PopUpPenalty());
    }

    private IEnumerator PopUpPenalty()
    {
        _penaltyText.text = PENALTY_STRING;
        yield return _penaltyPopupWaitForSeconds;
        _penaltyText.text = "";
    }
} 
