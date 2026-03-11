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
    
    [Header("Main Menu Buttons")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject settingButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject quitButton;
    
   
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (PlayerPrefs.GetInt("SaveGameAvailable", 0) == 0)
        {
            //Save not available
            resumeButton.SetActive(false);
        }
        else
        {
            //Save available
            resumeButton.SetActive(true);
        }
    }

    void OnEnable()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(ResumeButtonPressed);
        startButton.GetComponent<Button>().onClick.AddListener(StartButtonPressed);
        settingButton.GetComponent<Button>().onClick.AddListener(SettingButtonPressed);
        creditsButton.GetComponent<Button>().onClick.AddListener(CreditsButtonPressed);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitButtonPressed);
    }

    void ResumeButtonPressed()
    {
        
    }
    void StartButtonPressed()
    {
        gameManager.StartGame();
    }

    void SettingButtonPressed()
    {
        
    }
    void CreditsButtonPressed()
    {
        
    }
    void QuitButtonPressed()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    void OnDisable()
    {
        resumeButton.GetComponent<Button>().onClick.RemoveListener(ResumeButtonPressed);
        startButton.GetComponent<Button>().onClick.RemoveListener(StartButtonPressed);
        settingButton.GetComponent<Button>().onClick.RemoveListener(SettingButtonPressed);
        creditsButton.GetComponent<Button>().onClick.RemoveListener(CreditsButtonPressed);
        quitButton.GetComponent<Button>().onClick.RemoveListener(QuitButtonPressed);
    }
}
