using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Menu References")] 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject quitMenu;
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject gameCompleteMenu;
    
    [Header("Game Board References")] 
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text multiplierText;
    
    [Header("Main Menu Buttons")] 
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Game Menu Buttons")] 
    [SerializeField] private Button gameMenuBackButton;
    [SerializeField] private Button gameMenuNextButton;
    
    [Header("Setting Menu Buttons, Toggles And Sliders")] 
    [SerializeField] private Button settingsBackButton;
    [SerializeField]private Toggle musicToggle;
    [SerializeField]private Toggle sfxToggle;
    [SerializeField]private Slider musicVolumeSlider;
    [SerializeField]private Slider sfxVolumeSlider;
    
    [Header("Credits Menu Buttons")] 
    [SerializeField]private Button creditsBackButton;
    
    [Header("Quit Menu Buttons")] 
    [SerializeField]private Button quitGameYesButton;
    [SerializeField]private Button quitGameNoButton;

    private GameManager gameManager;
    
    [Header("Audio Manager References")]
    [SerializeField]private AudioManager audioManager;
 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
       
        
        resumeButton.gameObject.SetActive(SaveManager.Instance.IsSaveAvailable());
       
        SaveManager.Instance.LoadMusicSettings(ref musicVolumeSlider,ref sfxVolumeSlider,ref musicToggle,ref sfxToggle);
    }
    

    void OnEnable()
    {
        resumeButton.onClick.AddListener(ResumeButtonPressed);
        startButton.onClick.AddListener(StartButtonPressed);
        settingButton.onClick.AddListener(SettingButtonPressed);
        creditsButton.onClick.AddListener(CreditsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        gameMenuBackButton.onClick.AddListener(GameMenuBackButtonPressed);
        gameMenuNextButton.onClick.AddListener(GameMenuNextButtonPressed);
        
        settingsBackButton.onClick.AddListener(SettingsBackButtonPressed);
        
        musicToggle.onValueChanged.AddListener(MusicTogglePressed);
        sfxToggle.onValueChanged.AddListener(SfxTogglePressed);
        
        musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChangePressed);
        sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChangePressed);
        
        creditsBackButton.onClick.AddListener(CreditsBackButtonPressed);
        
        quitGameYesButton.onClick.AddListener(QuitGameYesPressed);
        quitGameNoButton.onClick.AddListener(QuitGameNoPressed);
    }
   

    void ResumeButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(gameMenu);
        gameManager.ResumeGame();
    }

    void StartButtonPressed()
    {
        gameBoard.SetActive(true);
        gameCompleteMenu.SetActive(false);
        
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(gameMenu);
       
        gameManager.StartGame();
    }

    private void GameMenuBackButtonPressed()
    {
       
        gameManager.SaveActiveLevel();
        
        HandleMenuItems(gameMenu, false);
        HandleMenuItems(mainMenu);
       
        resumeButton.gameObject.SetActive(SaveManager.Instance.IsSaveAvailable());
    }

    public void LevelCompleteEvent()
    {
        gameBoard.SetActive(false);
        gameCompleteMenu.SetActive(true);
    }
    private void GameMenuNextButtonPressed()
    {
        audioManager.PlayButtonClickAudio();
        gameCompleteMenu.SetActive(false);
        gameBoard.SetActive(true);
        gameManager.ResetBoardCards();
        gameManager.StartGame();
       
    }
    void SettingButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(settingMenu);
    }
    
    #region Settings Sub-Menu Buttons
    void MusicTogglePressed(bool isOn)
    {
        audioManager.MusicTogglePressReceived(isOn);
       
    }
    void SfxTogglePressed(bool isOn)
    {
        audioManager.SfxTogglePressReceived(isOn);
       
    }
    void MusicVolumeChangePressed(float value)
    {
        value = Mathf.Round(value*10f) /10f;
        audioManager.MusicVolumeChangeReceived(value);
       
    }
    void SfxVolumeChangePressed(float value)
    {
        value = Mathf.Round(value*10f) /10f;
        audioManager.SfxVolumeChangeReceived(value);
       
    }
    void SettingsBackButtonPressed()
    {
        HandleMenuItems(settingMenu, false);
        HandleMenuItems(mainMenu);
        SaveManager.Instance.SetMusicSettings(musicVolumeSlider.value, sfxVolumeSlider.value, musicToggle.isOn, sfxToggle.isOn);
    }
    #endregion
    
    #region Credit Menu Buttons
    void CreditsButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(creditsMenu);
    }

    void CreditsBackButtonPressed()
    {
        HandleMenuItems(creditsMenu, false);
        HandleMenuItems(mainMenu);
    }
    #endregion
    
    #region Game Quit Menu Buttons
    void QuitButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(quitMenu);
        SaveManager.Instance.SaveGameData();
    }

    void QuitGameYesPressed()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    void QuitGameNoPressed()
    {
        HandleMenuItems(quitMenu, false);
        HandleMenuItems(mainMenu);
    }
    #endregion
   
    void HandleMenuItems(GameObject menuItem, bool open = true)
    {
        audioManager.PlayButtonClickAudio();
        menuItem.SetActive(open);
    }

    public void UpdateScoreUI(int score, int highScore, int multiplier)
    {
       scoreText.text =score.ToString();
       highScoreText.text = highScore.ToString();
       multiplierText.text = multiplier.ToString();
    }

    void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeButtonPressed);
        startButton.onClick.RemoveListener(StartButtonPressed);
        settingButton.onClick.RemoveListener(SettingButtonPressed);
        creditsButton.onClick.RemoveListener(CreditsButtonPressed);
        quitButton.onClick.RemoveListener(QuitButtonPressed);

        gameMenuBackButton.onClick.RemoveListener(GameMenuBackButtonPressed);
        gameMenuNextButton.onClick.RemoveListener(GameMenuNextButtonPressed);
        
        settingsBackButton.onClick.RemoveListener(SettingsBackButtonPressed);
        
        musicToggle.onValueChanged.RemoveListener(MusicTogglePressed);
        sfxToggle.onValueChanged.RemoveListener(SfxTogglePressed);
        
        musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeChangePressed);
        sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeChangePressed);
        
        creditsBackButton.onClick.RemoveListener(CreditsBackButtonPressed);
        
        quitGameYesButton.onClick.RemoveListener(QuitGameYesPressed);
        quitGameNoButton.onClick.RemoveListener(QuitGameNoPressed);
        
    }
}