using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField]
    private Invader[] prefabs;
    [SerializeField]
    private int rows = 5;
    [SerializeField]
    private int cols = 11;

    [SerializeField]
    private float invaderSpacing = 2.0f;
    [SerializeField]
    private AnimationCurve invaderSpeed;

    [SerializeField]
    private Projectile missilePrefab;
    [SerializeField]
    private float missileAttackRate = 1f;

    private Vector3 direction = Vector2.right;

    public int invadersKilled { get; private set; }
    private int invadersAlive => totalInvaders - invadersKilled;
    private int totalInvaders => rows * cols;
    private float percentKilled => (float)invadersKilled / (float)totalInvaders;


    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            float gridWidth = invaderSpacing * (cols - 1);
            float gridHeight = invaderSpacing * (rows - 1);

            Vector2 centering = new Vector2(-gridWidth / 2, -gridHeight / 2);
            Vector3 rowPos = new Vector3(centering.x, centering.y + (row * invaderSpacing), 0.0f);

            for (int col = 0; col < cols; col++)
            {
                Invader invader = Instantiate(prefabs[row], transform);
                invader.killed += InvaderKilled;
                Vector3 pos = rowPos;
                pos.x += col * invaderSpacing;
                invader.transform.localPosition = pos;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
    }

    private void Update()
    {
        transform.position += direction * invaderSpeed.Evaluate(percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1))
            {
                AdvanceRow();
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1;

        Vector3 pos = transform.position;
        pos.y -= 1f;
        transform.position = pos;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0f / invadersAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        invadersKilled++;
    }
}
