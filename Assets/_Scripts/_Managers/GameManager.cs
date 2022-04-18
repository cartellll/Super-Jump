using System.Collections;
using UnityEngine;
using YsoCorp.GameUtils;
using SuperStars.Events;
public class GameManager: MonoBehaviour
{
    public CoreConfig CoreConfig;
    
    public InputManager InputManager;
    public LevelManager LevelManager;
    public SoundManager SoundManager;
  //  public SoundManager SoundManager;
  
    public static GameManager Instance { get; private set; }

    public enum State
    {
        Start,
        Play,
        Finish,
        Pause
    }
    public State state { get; private set; }

    [Header("IronSource")]
    public IronSourceController ironSourceController;

    [Header("SaveLoad")]
    public SaveLoadController mSaveLoad;

    //public List<string> crateNames = new List<string>();

    [Header("UI")]
    public UIBaseScreen windowStart;
    public UIBaseScreen windowFail;
    public UIBaseScreen windowFinish;
    public GamePlayScreen gamePlayScreen;
    //public GameObject windowPlay;
    [SerializeField] private float timeBetweenFinishAndShowWindow = 3;
    //private readonly CompositeDisposable _lifetimeDisposables = new CompositeDisposable();

   /* [Header("Confgs")]
    [SerializeField] private EnemiesConfig enemyStickmansConfig;
    public EnemiesConfig EnemyStickmansConfig => enemyStickmansConfig;
    [SerializeField] private StickmanConfigs playerStickmansConfig;
    public StickmanConfigs PlayerStickmansConfig => playerStickmansConfig;*/

    [SerializeField] private MyCameraManager cameraManger;


   private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mSaveLoad.InitiateClass();
        int levelIndex = mSaveLoad.GetLevelNumber();
        StartNewGame(levelIndex);
      
      //  EventManager.StartListening<bool>(CoreLevelEvents.Finish, Finish);
      //  EventManager.StartListening<bool>(CoreLevelEvents.Fail, GameOver);
    }
    private void Start()
    {
        
        YCManager.instance.OnGameStarted(LevelManager.CurrentLevelIndex);
        
        windowStart.Init();

        windowStart.Show();
        state = State.Start;

        //Time.timeScale = 0;
    }

    private void Update()
    {
        if(state == State.Play)
        {
            if (LevelManager.CurrentLevel.GetLevelState == LevelController.LevelState.Battle)
            {
                if (LevelManager.CurrentLevel.PlayerCrowd.GetCount() == 0)
                {
                    GameOver();
                }

                else if (LevelManager.CurrentLevel.EnemyCrowd.GetCount() == 0)
                {
                    Finish();
                }
            }
        }
    }

 
    private IEnumerator HideWindowHandler()
    {
        yield return new WaitForSeconds(0.05f);
        state = State.Play;
    }

    private void Finish()
    {
        YCManager.instance.OnGameFinished(true);
        state = State.Finish;
        mSaveLoad.SetLevelNumber(mSaveLoad.GetLevelNumber() + 1);
        mSaveLoad.SaveParametrs();
        gamePlayScreen.Hide();
        StartCoroutine(OpenFinishWindow(windowFinish));
       
        if (mSaveLoad.GetLevelNumber() == CoreConfig.Levels.Length)
        {
            mSaveLoad.SetLevelNumber(0);
            mSaveLoad.SaveParametrs();
        }

        GAManager.Instance.OnFinishLevel(mSaveLoad.GetLevelNumber() + 1);
        
    }

    private void GameOver()
    {
        YCManager.instance.OnGameFinished(false);
        GAManager.Instance.OnFailedLevel(mSaveLoad.GetLevelNumber() + 1);
        state = State.Finish;
        gamePlayScreen.Hide();
        StartCoroutine(OpenFinishWindow(windowFail));
    }

    
    IEnumerator OpenFinishWindow(UIBaseScreen screen)
    {
        yield return new WaitForSeconds(timeBetweenFinishAndShowWindow);
        screen.Show();
       // Time.timeScale = 0;
    }

    public void StartButton()
    {
        YCManager.instance.adsManager.ShowInterstitial(() => ClickStart());
    }

    private void ClickStart()
    {
        SoundManager.AudioClickButton.Play();
        windowStart.Hide();
        gamePlayScreen.Show();
        StartCoroutine(HideWindowHandler());
    
        // Time.timeScale = 1;
    }

    public void RestartButton()
    {
        YCManager.instance.adsManager.ShowInterstitial(() => Restart());
    }

    private void Restart()
    {
        SoundManager.AudioClickButton.Play();
        int levelIndex = mSaveLoad.GetLevelNumber();
        StartNewGame(levelIndex);
    }

    public void NextButton()
    {
        YCManager.instance.adsManager.ShowInterstitial(() => NextLevel());
    }

    private void NextLevel()
    {
        SoundManager.AudioClickButton.Play();
        int levelIndex = mSaveLoad.GetLevelNumber();
        StartNewGame(levelIndex);
    }

    public void StartNewGame(int levelIndex)
    {
        if(LevelManager.CurrentLevel != null)
        {
            Destroy(LevelManager.CurrentLevel.gameObject);
        }
       
        LevelManager.LoadLevel(CoreConfig.Levels[levelIndex], levelIndex);

        state = State.Start;

        windowStart.Init();
        windowFinish.Init();
        windowFail.Init();
        gamePlayScreen.Init();

        windowStart.Show();
        windowFinish.Hide();
        windowFail.Hide();
        gamePlayScreen.Hide();

        EventManager.TriggerEvent(MoneyEvents.SetSaveMoney, mSaveLoad.GetMoneyCount());
        EventManager.TriggerEvent(CoreLevelEvents.NewGame);
    }


    /*  private void OnApplicationPause(bool pause)
      {
          IronSource.Agent.onApplicationPause(pause);
      }*/


    /*  private void OnDisable()
      {
          EventManager.StopListening<bool>(CoreLevelEvents.Finish, Finish);
          EventManager.StopListening<bool>(CoreLevelEvents.Fail, GameOver);
          _lifetimeDisposables.Clear();
      }*/
}

