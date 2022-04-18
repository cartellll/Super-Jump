using UnityEngine;
using SuperStars.Events;

public class LevelController : MonoBehaviour
{
    private GameManager gameManager;
    public enum LevelState
    {
        Start,
        Battle,
        Finish
    }
    private LevelState levelState = LevelState.Start;

    public LevelState GetLevelState => levelState;

    [SerializeField] private PlayerController playerController;

    public PlayerController PlayerController => playerController;

    [SerializeField] private CrowdStickman playerCrowd;
    [SerializeField] private CrowdStickman enemyCrowd;

    public CrowdStickman PlayerCrowd => playerCrowd;
    public CrowdStickman EnemyCrowd => enemyCrowd;

    [SerializeField] private GameField gameField;
    public GameField GameField => gameField;

    private int moneyCount = 0;

    private void OnEnable()
    {
        EventManager.StartListening<int>(MoneyEvents.AddMoney, AddMoney);
    }

    private void OnDisable()
    {
        EventManager.StopListening<int>(MoneyEvents.AddMoney, AddMoney);
    }

    private void Start()
    {
        EventManager.TriggerEvent(MoneyEvents.SetMoney, moneyCount);
    }

    private void AddMoney(int count)
    {
        moneyCount += count;
        EventManager.TriggerEvent(MoneyEvents.SetMoney, moneyCount);
    }
    public void StartBattle()
    {
        if(levelState != LevelState.Start)
        {
            return;
        }
        
        Destroy(playerController.gameObject);
        levelState = LevelState.Battle;
        gameField.WallBetweenEnemyAndPlayer.SetActive(false);
    }

    // public GameObject Finish;

 
   //  [Header("PointManager")]
   //  [SerializeField] private Transform spawnPointsOrder;

    /* private void Start()
     {
        gameManager = GameManager.Instance;
        if (spawnPointsOrder != null)
        {
            InitEnemies();
        }
     }
     public List<PointController> GetSpawnPoints(string Tag)
     {
         var pointManagers = spawnPointsOrder.GetComponentsInChildren<PointManager>();

         List<PointController> points = new List<PointController>();
         foreach (var pointManager in pointManagers)
         {
             if (pointManager.tag == Tag)
             {
                 points = pointManager.GetSpawnPoints();
             }
         }
         return points;
     }

     public GameObject GetEnemyPrefab(EnemyType type)
     {
         GameObject tempEnemyPrefab = null;
         foreach (EnemyConfig enemyConfig in gameManager.EnemyStickmansConfig.EnemyStickmanConfigs)
         {
             if (enemyConfig.Type == type)
             {
                 tempEnemyPrefab = enemyConfig.Prefab;
             }
         }
         return tempEnemyPrefab;
     }


     private void SpawnBot(EnemyType type, Transform pointTransform)
     {
         GameObject enemyPrefab;
         enemyPrefab = GetEnemyPrefab(type);
         Instantiate(enemyPrefab, pointTransform);

     }

     public void InitEnemies()
     {
         foreach (EnemyConfig tempConfig in gameManager.EnemyStickmansConfig.EnemyStickmanConfigs)
         {
             List<PointController> points = GetSpawnPoints(tempConfig.Type.ToString());
             if (points != null)
             {
                 foreach (PointController point in points)
                 {
                     Instantiate(tempConfig.Prefab, point.transform);
                 }
             }
         }
     }*/
}
