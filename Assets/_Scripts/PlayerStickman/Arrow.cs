using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Arrow : MonoBehaviour
{
    private Rigidbody rigidBody;

    private string parentTag;
    
    public float damage { get; set; }

    [Header("Effects")]
    public GameObject CollisionEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Init()
    {
       // CollisionEffect.SetActive(false);
        //CollisionEffect.GetComponent<ParticleSystem>().Stop();
        //CollisionEffect.transform.SetParent(gameObject.transform);    
    }
    private void Update()
    {
    
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rigidBody.velocity, Vector3.forward);
        transform.localRotation = Quaternion.LookRotation(rigidBody.velocity,Vector3.forward);
    }

    public void SetTarget(Transform target, string parentTag, float shootAngle)
    {
        Vector3 dir = BallisticVel(target, shootAngle);
        if (!float.IsNaN(dir.x) && !float.IsNaN(dir.y) && !float.IsNaN(dir.z))
        {
            GetComponent<Rigidbody>().velocity = dir;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = 10 * (target.transform.position - transform.position);
        }
       
        this.parentTag = parentTag;
    }

    public Vector3 BallisticVel(Transform target, float angle)
    {
     
        Vector3 dir = target.position - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude


        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        var enemy = target.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            float Ve = enemy.Config.StartSpeed;
           // vel = (-Ve + Mathf.Sqrt(Ve + 4 * dist * Physics.gravity.magnitude / Mathf.Sin(2 * a))) / 2;
        }

        return vel * dir.normalized;
    }

  

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.DamageHpManager.HP = 0;
            GameObject effect = Instantiate(CollisionEffectPrefab, transform.Find("ExplosionFirePosition").transform.position, Quaternion.identity, null);
            Destroy(effect, 3);
            Destroy(gameObject);
        }
    }

}
