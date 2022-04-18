using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBounce : MonoBehaviour
{
    public enum TypeArmor
    {
        Shield,
        Sword,
        Shelmet
    }
    [SerializeField] private TypeArmor typeArmor;
    public TypeArmor GetTypeArmor => typeArmor;


    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float hpBounce;
    public float HpBounce => hpBounce;
    [SerializeField] private float damageBounce;
    public float DamageBounce => damageBounce;

    private void Update()
    {
       transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

       if( GameManager.Instance.LevelManager.CurrentLevel.GetLevelState == LevelController.LevelState.Battle)
       {
            gameObject.SetActive(false);
       }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerStickmanController stickman = other.gameObject.GetComponentInParent<PlayerStickmanController>();
        if (stickman == null)
        {
            return;
        }

        stickman.AddBounce(this);
        Destroy(gameObject);
    }
}
