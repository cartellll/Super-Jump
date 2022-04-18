using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 moveSpeed;
    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;


    private void Update()
    {

        if (GameManager.Instance.state != GameManager.State.Play)
        {
            return;
        }

        Move();
        Roataion();
    }

    private void Move()
    {
        var newPosition = transform.position + moveSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, leftBorder.position.x, rightBorder.position.x), newPosition.y, newPosition.z);

        if(Mathf.Abs(transform.position.x - leftBorder.position.x) <=0.001f || Mathf.Abs(transform.position.x - rightBorder.position.x) <= 0.001f)
        {
            moveSpeed *= -1;
            rotationSpeed *= -1;
        }
    }

    private void Roataion()
    {
        transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerStickmanController playerStickman = collision.transform.GetComponent<PlayerStickmanController>();

        if(playerStickman != null)
        {
            //playerStickman.
            playerStickman.SetLayer("InActiveStickman");
            playerStickman.DamageHpManager.HP = 0;
        }
    }
}
