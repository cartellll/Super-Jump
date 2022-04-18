using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;

public class GamePlayScreen : UIBaseScreen
{
    [SerializeField] private GameObject battleTutorial;
    public GameObject BattleTutorial => battleTutorial;
    [SerializeField] private GameObject startTutorial;
    private LevelController currentLevel;
    void Start()
    {
        Reset();
    }

    private void OnEnable()
    {
        EventManager.StartListening(CoreLevelEvents.NewGame, Reset);
    }

    private void OnDisable()
    {
        EventManager.StopListening(CoreLevelEvents.NewGame, Reset);

        if (GameManager.Instance.LevelManager.CurrentLevelIndex == 0)
        {
            EventManager.StopListening<Vector3>(InputEvents.Input_Start, OnInputStart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLevel.GetLevelState == LevelController.LevelState.Battle)
        {
            battleTutorial.SetActive(true);
        }
    }

    private void OnInputStart(Vector3 position)
    {
        if(GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        startTutorial.SetActive(false);
    }

    private void Reset()
    {
        currentLevel = GameManager.Instance.LevelManager.CurrentLevel;

        if (GameManager.Instance.LevelManager.CurrentLevelIndex == 0)
        {
            EventManager.StartListening<Vector3>(InputEvents.Input_Start, OnInputStart);
            startTutorial.SetActive(true);
        }
        else
        {
            startTutorial.SetActive(false);
        }

        battleTutorial.SetActive(false);

    }
}
