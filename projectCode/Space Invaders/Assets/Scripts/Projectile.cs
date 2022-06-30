using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private float speed;

    public System.Action destroyed;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed != null)
        {
            destroyed.Invoke();
        }
        Destroy(gameObject);
    }
}
