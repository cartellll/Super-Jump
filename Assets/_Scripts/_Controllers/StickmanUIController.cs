using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StickmanUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    public TextMeshProUGUI Text => text;
    [SerializeField] private float textPositionY;
    private Vector3 startTextLocalScale;
    private bool isInit = false;
    private void Start()
    {
        Init();
      //  textRectTransform = text.gameObject.GetComponent<RectTransform>();
    }

    public void Init()
    {
        if(isInit)
        {
            return;
        }

        startTextLocalScale = GetComponentInParent<DamageHPManager>().gameObject.transform.localScale;
        isInit = true;


    }

   
    public void UpdateVisibleText(bool ragdolled, float HP)
    {
        if (animator == null || HP<=0)
        {
            return;
        }

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        if (ragdolled || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand_up_from_back") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand_up_from_belly"))
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }
    }
    public void UpdateText(float HP)
    {
        text.text = HP.ToString();
    }
    public void UpdateTextSize(float stickmanScale)
    {
        Init();
        text.gameObject.transform.localScale = startTextLocalScale / stickmanScale;
        
      //  print(startTextLocalScale / stickmanScale);
        
       // textRectTransform.position = new Vector3(textRectTransform.position.x, textPositionY, textRectTransform.position.z);
    }
}
