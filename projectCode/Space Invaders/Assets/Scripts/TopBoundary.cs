using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBoundary : MonoBehaviour
{
    [SerializeField]
    private DeathAnimation deathEffect;
    [SerializeField]
    private Sprite laserExplosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            DeathAnimation effect = Instantiate(deathEffect, collision.gameObject.transform.position, Quaternion.identity);
            effect.sr.sprite = laserExplosion;
            effect.sr.color = new Color32(0xFF, 0x00, 0x00, 0xFF); // FF0000;
        }
    }
}
