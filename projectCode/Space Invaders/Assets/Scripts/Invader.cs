using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] animationSprites;
    [SerializeField]
    private float animationTime = 1.0f;
    public int score = 10;

    private SpriteRenderer sr;
    private int animationFrame;

    public System.Action<Invader> killed;

    [SerializeField]
    private DeathAnimation deathEffect;
    [SerializeField]
    private Sprite deathSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    public void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        sr.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            DeathAnimation effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.sr.sprite = deathSprite;

            FindObjectOfType<AudioManager>().PlayEnemyDeathSound();
            killed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
