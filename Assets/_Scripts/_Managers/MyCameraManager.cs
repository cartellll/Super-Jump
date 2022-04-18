using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStars.Events;

public class MyCameraManager : MonoBehaviour
{
    private LevelController currentLevel;
    private bool isMove = false;
    [SerializeField] private Camera camera;
    [SerializeField] private float offsetZregardingPlayer;
    private float cameraBattleZ;
    [SerializeField] private float moveTime;
    private GameObject controlledStickman;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EventManager.StartListening(CoreLevelEvents.NewGame, Init);
    }

    private void OnDisable()
    {
        EventManager.StopListening(CoreLevelEvents.NewGame, Init);
    }

    public void Init()
    {
        currentLevel = GameManager.Instance.LevelManager.CurrentLevel;
        currentLevel.PlayerController.Init();
        controlledStickman = currentLevel.PlayerController.ControlledStickman.gameObject;
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, controlledStickman.transform.position.z + offsetZregardingPlayer);
        cameraBattleZ = currentLevel.GameField.CameraBattleTransform.position.z;
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel.GetLevelState == LevelController.LevelState.Battle && !isMove)
        {
            isMove = true;
            StartCoroutine(MoveCamera());
        }

    }

    private IEnumerator MoveCamera()
    {
        float tempTime = 0;
        Vector3 startPosition = camera.transform.position;
        Vector3 endPosition = new Vector3(camera.transform.position.x, camera.transform.position.y,cameraBattleZ);
        while (tempTime <= moveTime)
        {
            tempTime += Time.deltaTime;
            camera.transform.position = Vector3.Lerp(startPosition, endPosition, tempTime / moveTime);
            yield return null;
        }
        camera.transform.position = endPosition;
    }
}
