using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public SpriteRenderer sr;
    [SerializeField]
    private float effectTime = 0.25f;

    private void Start()
    {
        Destroy(this.gameObject, effectTime);
    }
}
