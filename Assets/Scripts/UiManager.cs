using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    private const string MENU_SCENE = "MainMenu";
    
    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private Toggle _pauseToggle;
    [SerializeField] private Button _menuButton;
    
    // TODO dialogue system
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Button _confirmButton;

    private Image[] _hearts;
    
    private void Start()
    {
        // button events
        _pauseToggle.onValueChanged.AddListener(PauseGame);
        _menuButton.onClick.AddListener(MainMenu);
        _confirmButton.onClick.AddListener(Confirm);
        
        // on data change events
        GameData.Instance.Health.OnChange += SetHealth;
        GameData.Instance.LevelName.OnChange += SetLevelName;
        
        // get hearts
        _hearts = _healthBar.GetComponentsInChildren<Image>();

        // setup
        SetHealth(GameData.Instance.Health.Current);
        SetLevelName(GameData.Instance.LevelName.Current);
        ShowDialogue("");
    }

    private void SetHealth(int health)
    {
        // spawn more hearts if necessary
        while (health > _hearts.Length)
        {
            Instantiate(_hearts[0], _healthBar);
            _hearts = _healthBar.GetComponentsInChildren<Image>();
        }
        
        // set hearts
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = (i < health);
        }
    }

    private void SetLevelName(string name)
    {
        _levelName.text = name;
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

    private void ShowDialogue(string dialogue)
    {
        _dialogueText.text = dialogue;
    }

    private void Confirm()
    {
        Debug.Log("Confirm");
    }
} 
