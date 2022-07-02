using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Projectile laserPrefab;
    [SerializeField]
    private float speed = 5;
    private bool laserActive = false;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    [SerializeField]
    private float edgeBuffer = 1.5f;

    public System.Action killed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > (leftEdge.position.x + edgeBuffer))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < (rightEdge.position.x - edgeBuffer))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            } 
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!laserActive)
        {
            Projectile projectile = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            laserActive = true;
        }
    }

    private void LaserDestroyed(Projectile laser)
    {
        laserActive = false;
    }

    public void ResetLaser()
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            if (killed != null)
            {
                killed.Invoke();
            }
        }
    }
}
