using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRagdollHelper : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private Animator animator;
	private Component[] components;
	private bool isInit = false;

	public bool ragdolled
	{
		get
		{
			return ragdolled;
		}
		set
		{
			if (value == true)
			{
				animator.enabled = false;
				SetKinematic(false);
			}
			else
			{
				animator.enabled = true;
				SetKinematic(true);
			}
		}
	}
	private void SetKinematic(bool newValue)
	{
		//For each of the components in the array, treat the component as a Rigidbody and set its isKinematic property
		foreach (Component c in components)
		{
			if (c.gameObject != gameObject)
			{
				(c as Rigidbody).isKinematic = newValue;
			}
		}
	}

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
		components = GetComponentsInChildren(typeof(Rigidbody));
		SetKinematic(true);
		isInit = true;
	}
}
