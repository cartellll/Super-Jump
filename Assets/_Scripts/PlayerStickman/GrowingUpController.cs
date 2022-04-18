using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStickmanController))]
public class GrowingUpController : MonoBehaviour
{
    [Min(0)]
    private int size;
    public int Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            UpdateParametrs();
        }
    }

    [SerializeField] private StickmanConfigs stickmanConfigs;

    private PlayerStickmanController stickmanController;

    private bool isInit = false;
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

        stickmanController = GetComponent<PlayerStickmanController>();
        size = stickmanController.Config.Size;
        UpdateParametrs();
        isInit = true;
    }

    public void UpdateParametrs()
    {
       
        if (size < stickmanConfigs.PlayerStickmanConfigs.Length)
        {
            stickmanController.Config = stickmanConfigs.PlayerStickmanConfigs[size];
            UpdateColorMaterial();
            UpdateSpeed();
            UpdateScale();
            UpdateHP();
            UpdateDamage();

            stickmanController.UpdateUI();
        }
    }

    public void UpdateColorMaterial()
    {
        Material newMaterial = stickmanController.Config.Material;
        Material[] materials = new Material[GetComponentInChildren<SkinnedMeshRenderer>().materials.Length];
        for (int i = 0; i < materials.Length; i++ )
        { 
            materials[i] = newMaterial;
        }

        GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

        stickmanController.Shield.GetComponentInChildren<MeshRenderer>().material = newMaterial;
        stickmanController.Shelmet.GetComponentInChildren<MeshRenderer>().material = newMaterial;
        stickmanController.Sword.GetComponentInChildren<MeshRenderer>().material = newMaterial;

     /*   foreach (Renderer rend in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            rend.material = newMaterial;
        }

        foreach (Renderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            rend.material = newMaterial;
        }*/

    }

    public void UpdateScale()
    {
        transform.localScale = Vector3.one * stickmanController.Config.Scale;
    }

    public void UpdateSpeed()
    {
        stickmanController.Speed = stickmanController.Config.Speed;
    }

    public void UpdateHP()
    {
        stickmanController.DamageHpManager.HP = stickmanController.Config.HP;
    }

    public void UpdateDamage()
    {
        stickmanController.DamageHpManager.Damage = stickmanController.Config.Damage;
    }


    public void GrowingUpStickman(PlayerStickmanController prevStickman)
    {
        if (size < stickmanConfigs.PlayerStickmanConfigs.Length)
        {
            Size++;
        }

        GameObject prevStickmanShield = prevStickman.Shield.gameObject;
        GameObject prevStickmanSword = prevStickman.Sword.gameObject;
        GameObject prevStickmanShelmet = prevStickman.Shelmet.gameObject;

        if (prevStickmanShield.activeSelf)
        {
            stickmanController.Shield.gameObject.SetActive(true);
        }

        if (prevStickmanSword.activeSelf)
        {
            stickmanController.Sword.gameObject.SetActive(true);
            stickmanController.BattleController.ChangeAnimationAttack(global::BattleController.PunchingState.Sword);
        }

        if (prevStickmanShelmet.activeSelf)
        {
            stickmanController.Shelmet.gameObject.SetActive(true);
        }

        stickmanController.DamageHpManager.HP += (prevStickman.DamageHpManager.HP - prevStickman.Config.HP);
        stickmanController.DamageHpManager.Damage += (prevStickman.DamageHpManager.Damage - prevStickman.Config.Damage);
    }

}
