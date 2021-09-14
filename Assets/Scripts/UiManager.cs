using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    private const string MENU_SCENE = "MainMenu";
    
    [SerializeField] private Toggle _pauseToggle;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Slider _speedSlider;
    

    private void Start()
    {
        // button events
        _pauseToggle.onValueChanged.AddListener(PauseGame);
        _menuButton.onClick.AddListener(MainMenu);
        _speedSlider.onValueChanged.AddListener(SpeedChange);
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
    
} 
