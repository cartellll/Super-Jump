using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rope : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float ropePositionY = 1;
    [SerializeField] private float ropeWidth = 0.1f;
    private Transform targetPlayerTransform;
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private List <Vector3> circePositions;
    private void Start()
    {
        circePositions = new List<Vector3>();
        leftPosition = GameManager.Instance.LevelManager.CurrentLevel.GameField.GetRopeConstruction.LeftCylinder.position;
        rightPosition = GameManager.Instance.LevelManager.CurrentLevel.GameField.GetRopeConstruction.RightCyilinder.position;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        lineRenderer.positionCount = 2;

        lineRenderer.SetPositions(new Vector3[] { new Vector3(leftPosition.x, ropePositionY, leftPosition.z), new Vector3(rightPosition.x, ropePositionY, rightPosition.z) });
    }


    private void Update()
    {
        UpdateRope();
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        targetPlayerTransform = playerTransform;
    }
    private void UpdateRope()
    {
        if(targetPlayerTransform == null)
        {
            return;
        }
        lineRenderer.positionCount = 12;

        circePositions.Add(new Vector3(leftPosition.x, ropePositionY, leftPosition.z));
        for(int i = 0; i < 10; i++)
        {
            Vector3 partCircle = targetPlayerTransform.position + Quaternion.Euler(0, -i * 15 / 10, 0) * Quaternion.Euler(0, -7.5f, 0) * Vector3.back * targetPlayerTransform.localScale.x / 3f;
            circePositions.Add(new Vector3(Mathf.Clamp(partCircle.x,leftPosition.x,rightPosition.x), ropePositionY, Mathf.Clamp(partCircle.z, partCircle.z, rightPosition.z)));
        }
        circePositions.Add(new Vector3(rightPosition.x, ropePositionY, rightPosition.z));

         /* lineRenderer.SetPositions(new Vector3[] { new Vector3(leftPosition.x, 1f, leftPosition.z), 
                                                    new Vector3(targetPlayerTransform.position.x,1f,Mathf.Clamp(targetPlayerTransform.position.z - targetPlayerTransform.localScale.x/2,targetPlayerTransform.position.z - targetPlayerTransform.localScale.x/2, rightPosition.z)),
                                                    new Vector3(rightPosition.x, 1f, rightPosition.z) });*/
        lineRenderer.SetPositions(circePositions.ToArray());
        circePositions.Clear();
    }
}
