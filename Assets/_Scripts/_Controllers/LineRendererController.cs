using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private bool isInit = false;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineRendererOffsetZ;
    [SerializeField] private float lineWidth;
    [SerializeField] private float lineLength;


    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if(isInit)
        {
            return;
        }

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
    }

    public void UpdateLineRenderer(Transform targetTransform)
    {
        Vector3 startLinePosition = new Vector3(targetTransform.position.x, 0.5f, lineRendererOffsetZ + targetTransform.position.z);
        Vector3 endLinePosition = new Vector3(targetTransform.position.x, 0.5f, targetTransform.position.z + lineRendererOffsetZ + lineLength);
        RaycastHit hit;
        int mask = LayerMask.GetMask("PlayerStickman", "WallBetweenEnemyPlayer");
         
        if(Physics.BoxCast(startLinePosition,new Vector3(lineWidth/2f, 0.1f, 0.1f),Vector3.forward, out hit, Quaternion.identity, lineLength , mask))
        {
            endLinePosition = new Vector3(targetTransform.position.x, 0.5f, hit.transform.position.z);
        }

        lineRenderer.SetPositions(new Vector3[] { startLinePosition, endLinePosition });
    }

    public void SetMaterial(Material material)
    {
        lineRenderer.material = material;
    }
}
