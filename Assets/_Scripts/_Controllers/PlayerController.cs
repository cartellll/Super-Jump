using System.Collections;
using System.Collections.Generic;
using SuperStars.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;
    private PlayerStickmanController controlledStickman;
    [SerializeField] private LineRendererController lineRendererController;
    [SerializeField] private Rope rope;
    public PlayerStickmanController ControlledStickman => controlledStickman;
    [SerializeField] private GameField gameField;
    [SerializeField] private float startOffsetZ = 1;

    private Vector3 prevInputPosition;
    private Vector3 startControlledStickmanPosition;

    private LevelController levelController;

    private bool isInit = false;
    private bool isTouch = false;
    private void Start()
    {
        Init();
    }

  
    private void OnEnable()
    {
        EventManager.StartListening<Vector3>(InputEvents.Input_Start, OnInputStart);
        EventManager.StartListening<Vector3>(InputEvents.Input_Update, OnInputUpdate);
        EventManager.StartListening<Vector3>(InputEvents.Input_End, OnInputEnd);   
    }

    private void OnDisable()
    {
        EventManager.StopListening<Vector3>(InputEvents.Input_Start, OnInputStart);
        EventManager.StopListening<Vector3>(InputEvents.Input_Update, OnInputUpdate);
        EventManager.StopListening<Vector3>(InputEvents.Input_End, OnInputEnd);
    }

    public void Init()
    {
        if(isInit)
        {
            return;
        }

        levelController = GameManager.Instance.LevelManager.CurrentLevel;
        if (controlledStickman == null)
        {
            controlledStickman = Instantiate(config.PlayerStickmanPrefab, gameField.InstantinatePosition.position ,Quaternion.identity, levelController.transform);
            controlledStickman.Init();
            //  controlledStickman.SetSize(Random.Range(0, config.StickmanConfigs.PlayerStickmanConfigs.Length-1));
            controlledStickman.SetSize(0);
        }
        controlledStickman.Init();
        controlledStickman.SetIsControlledStickman(true);

        lineRendererController.SetMaterial(controlledStickman.Config.LineMaterial);

        if (!levelController.PlayerCrowd.Contains(controlledStickman.DamageHpManager))
        {
            levelController.PlayerCrowd.AddStickman(controlledStickman.DamageHpManager);
        }

        lineRendererController?.Init();
        lineRendererController?.UpdateLineRenderer(controlledStickman.transform);

        startControlledStickmanPosition = controlledStickman.transform.position;
        isInit = true;
    }

    private void OnInputStart(Vector3 inputPosition)
    {
        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }
        if (inputPosition.y > 0.8f * Screen.height)
        {
            return;
        }
        isTouch = true;

        if (controlledStickman != null)
        {
            ControlledStickmanStepBack();
        }
        prevInputPosition = GetScreenToGroundPoint(inputPosition);
    }

    private void OnInputUpdate(Vector3 inputPosition)
    {
        if (!isTouch)
        {
            return;
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        if (controlledStickman == null)
        {
            return;
        }

        if (controlledStickman != null && controlledStickman.transform.position.z > startControlledStickmanPosition.z - startOffsetZ/2)
        {
            ControlledStickmanStepBack();
        }

        lineRendererController?.UpdateLineRenderer(controlledStickman.transform);

        
        Vector3 screenToGroundPoint = GetScreenToGroundPoint(inputPosition);
      
        float newStickmanPositionX = controlledStickman.transform.position.x + (screenToGroundPoint.x - prevInputPosition.x);
        float clampX = Mathf.Clamp(newStickmanPositionX, gameField.LeftPlayerLimit.position.x, gameField.RightPlayerLimit.position.x);
        controlledStickman.transform.position = new Vector3(clampX, controlledStickman.transform.position.y, controlledStickman.transform.position.z);
        
        prevInputPosition = screenToGroundPoint;
        
    }

    private void OnInputEnd(Vector3 inputPosition)
    {
        if (!isTouch)
        {
            return;
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        if (controlledStickman == null)
        {
            return;
        }
        isTouch = false;

        controlledStickman.StartControlledSitckman();
        controlledStickman = null;

        StartCoroutine(InstantinateStickman());
       
    }

    private void ControlledStickmanStepBack()
    {
        rope.SetPlayerTransform(controlledStickman.transform);
        controlledStickman.StepBack();
        controlledStickman.transform.position = startControlledStickmanPosition - startOffsetZ * Vector3.forward;
    }
    private Vector3 GetScreenToGroundPoint(Vector3 screenPoint)
    {
        float screnToGroundDistance = 0;
        Camera camera = Camera.main;
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            if (hit.transform.gameObject.layer != LayerMask.GetMask("UI"))
            {
                screnToGroundDistance = hit.distance;
            }
        }
        return camera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, screnToGroundDistance));
    }

    private IEnumerator InstantinateStickman()
    {
        yield return new WaitForSeconds(config.TimeToRestartStickman);

        controlledStickman = Instantiate(config.PlayerStickmanPrefab, gameField.InstantinatePosition.position, Quaternion.identity, levelController.transform);
        controlledStickman.Init();
        controlledStickman.SetIsControlledStickman(true);
        controlledStickman.SetSize(Random.Range(0, config.StickmanConfigs.PlayerStickmanConfigs.Length-1));
        lineRendererController.SetMaterial(controlledStickman.Config.LineMaterial);
        levelController.PlayerCrowd.AddStickman(controlledStickman.DamageHpManager);

        startControlledStickmanPosition = controlledStickman.transform.position;

        lineRendererController?.UpdateLineRenderer(controlledStickman.transform);

    }

    private void OnDestroy()
    {
        if(controlledStickman != null)
        {
            EventManager.TriggerEvent(DeathEvents.Death_stickman, controlledStickman.DamageHpManager);
            Destroy(controlledStickman.gameObject);
        }
    }
}
