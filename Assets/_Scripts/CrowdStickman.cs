using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SuperStars.Events;

public class CrowdStickman : MonoBehaviour
{
    private List<DamageHPManager> stickmanArray = new List<DamageHPManager>();
    void Awake()
    {
        stickmanArray = GetComponentsInChildren<DamageHPManager>().ToList();
    }

    private void Update()
    {
        
    }
    private void OnEnable()
    {
        EventManager.StartListening<DamageHPManager>(DeathEvents.Death_stickman,RemoveStickman);
    }

    private void OnDisable()
    {
        EventManager.StopListening<DamageHPManager>(DeathEvents.Death_stickman, RemoveStickman);
    }

    private void RemoveStickman(DamageHPManager stickman)
    {
        if (stickmanArray.Contains(stickman))
        {
            stickmanArray.Remove(stickman);
          //  Destroy(stickman.gameObject);
        }
    }

    public void AddStickman(DamageHPManager stickman)
    {
        stickmanArray.Add(stickman);
    }

    public bool Contains(DamageHPManager stickman)
    {
        return stickmanArray.Contains(stickman);
    }

    public int GetCount()
    {
        return stickmanArray.Count;
    }

    public DamageHPManager GetStickmanByIndex(int index)
    {
        return stickmanArray[index];
    }
    


}
