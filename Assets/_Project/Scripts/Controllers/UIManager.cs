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
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private GameObject gameCompleteMenu;
    
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
  
    
    [Header("Credits Menu Buttons")] 
    [SerializeField]private Button creditsBackButton;
    
    [Header("Quit Menu Buttons")] 
    [SerializeField]private Button quitGameYesButton;
    [SerializeField]private Button quitGameNoButton;

    GameManager gameManager;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
       // gameManager.UIMnaager = this;
        
        
        if (PlayerPrefs.GetInt("SaveGameAvailable", 0) == 0)
        {
            //Save not available
            resumeButton.gameObject.SetActive(false);
        }
        else
        {
            //Save available
            resumeButton.gameObject.SetActive(true);
        }
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
        
        creditsBackButton.onClick.AddListener(CreditsBackButtonPressed);
        
        quitGameYesButton.onClick.AddListener(QuitGameYesPressed);
        quitGameNoButton.onClick.AddListener(QuitGameNoPressed);
    }

    void ResumeButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
    }

    void StartButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(gameMenu);
        gameManager.StartGame();
    }

    public void GameMenuBackButtonPressed()
    {
        HandleMenuItems(gameMenu, false);
        HandleMenuItems(mainMenu);
        
    } 
    public void GameMenuNextButtonPressed()
    {
       //add next button functionality here later.
    }
    void SettingButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(settingMenu);
    }

    void SettingsBackButtonPressed()
    {
        HandleMenuItems(settingMenu, false);
        HandleMenuItems(mainMenu);
    }
    
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
    void QuitButtonPressed()
    {
        HandleMenuItems(mainMenu, false);
        HandleMenuItems(quitMenu);

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
    void HandleMenuItems(GameObject menuItem, bool open = true)
    {
        menuItem.SetActive(open);
    }

    void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeButtonPressed);
        startButton.onClick.RemoveListener(StartButtonPressed);
        settingButton.onClick.RemoveListener(SettingButtonPressed);
        creditsButton.onClick.RemoveListener(CreditsButtonPressed);
        quitButton.onClick.RemoveListener(QuitButtonPressed);
        
        creditsBackButton.onClick.RemoveListener(CreditsBackButtonPressed);
        
        quitGameYesButton.onClick.RemoveListener(QuitGameYesPressed);
        quitGameNoButton.onClick.RemoveListener(QuitGameNoPressed);
        
    }
}