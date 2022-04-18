using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // public PoolObject projectilePrefab;

    public float speedReloaded { get; set; }
    
    public float minShootAngle;
    public float maxShootAngle;
    private float tempTime;

    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform arrowTransform;
    // Update is called once per frame
    private void Start()
    {
        var playerStickman = GetComponentInParent<PlayerStickmanController>();
        if(playerStickman != null)
        {
            playerStickman.InitBow(this);
        }
        tempTime = 0;
    }

    private void Update()
    {
        if(tempTime>0)
        {
            tempTime -= Time.deltaTime;
        }
    }

    public void Attack(GameObject target, string parentTag, float damage)
    {
      
        if (tempTime <= 0)
        {
            var targetTransform = target.transform;
            var arrow = Instantiate(arrowPrefab, transform.position + 3 * Vector3.up, Quaternion.identity, null);
            arrow.gameObject.transform.position = arrowTransform.position;
           // arrow.transform.SetParent(null);
           // arrow.gameObject.transform.rotation = arrowTransform.rotation;
            arrow.Init();
         
            if (target.transform.Find("ArrowTarget") != null)
            {
                targetTransform = target.transform.Find("ArrowTarget").transform;
            }
            arrow.SetTarget(targetTransform, parentTag,Random.Range(minShootAngle,maxShootAngle));
            arrow.damage = damage;
            tempTime = speedReloaded;
            arrow.gameObject.SetActive(true);
        }
    }
}
