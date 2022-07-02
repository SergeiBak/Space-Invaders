using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 direction = Vector3.up;
    public System.Action<Projectile> destroyed;
    public new BoxCollider2D collider { get; private set; }

    [SerializeField]
    private DeathAnimation deathEffect;
    [SerializeField]
    private Sprite deathEffectSprite;

    private bool isQuitting = false;

    private SpriteRenderer sr;

    private bool whiteColorChangeActivated = false;
    private bool redColorChangeActivated = false;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (destroyed != null)
        {
            destroyed.Invoke(this);
        }
        else if (!isQuitting)
        {
            DeathAnimation effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.sr.sprite = deathEffectSprite;
            effect.sr.color = sr.color;
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!redColorChangeActivated && transform.position.y > 9)
        {
            redColorChangeActivated = true;
            sr.color = new Color32(0xFF, 0x00, 0x00, 0xFF); // FF0000
        }
        else if (!whiteColorChangeActivated && transform.position.y > -6.5)
        {
            whiteColorChangeActivated = true;
            sr.color = Color.white;
        }
    }

    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(collider, transform.position))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }
}
