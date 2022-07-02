using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite[] missile1Sprites;
    [SerializeField]
    private Sprite[] missile2Sprites;
    [SerializeField]
    private Sprite[] missile3Sprites;

    private Sprite[] animationSprites;
    [SerializeField]
    private float animationTime = 0.25f;

    private SpriteRenderer sr;
    private int animationFrame;

    private bool colorSwapped = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        int missileType = Random.Range(1, 4);

        switch (missileType)
        {
            case 1:
                animationSprites = missile1Sprites;
                break;
            case 2:
                animationSprites = missile2Sprites;
                break;
            case 3:
                animationSprites = missile3Sprites;
                break;
            default:
                Debug.LogError("Invalid missileType in MissileAnimation!");
                break;
        }

        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        sr.sprite = animationSprites[animationFrame];
    }

    private void Update()
    {
        if (!colorSwapped && transform.position.y < -6.5)
        {
            colorSwapped = true;
            sr.color = new Color32(0x0E, 0xFF, 0x00, 0xFF); // 0EFF00
        }
    }
}
