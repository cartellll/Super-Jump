using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBaseScreen : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    private bool isInit = false;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        if(isInit)
        {
            return;
        }
        canvasGroup = GetComponent<CanvasGroup>();
        isInit = true;
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

}
