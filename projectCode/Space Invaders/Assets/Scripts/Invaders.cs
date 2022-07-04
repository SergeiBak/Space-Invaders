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
    public Vector3 initialPosition { get; private set; }
    public System.Action<Invader> killed;

    public int invadersKilled { get; private set; }
    private int invadersAlive => totalInvaders - invadersKilled;
    public int totalInvaders => rows * cols;
    private float percentKilled => (float)invadersKilled / (float)totalInvaders;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    [SerializeField]
    private float shiftAmount = 0.25f;

    private Invader[,] invaders;

    private IEnumerator movementCoroutine;

    private GameManager gm;
    private AudioManager am;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        am = FindObjectOfType<AudioManager>();

        invaders = new Invader[rows, cols];

        initialPosition = transform.position;

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

                invaders[row, col] = invader;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
        movementCoroutine = InvaderShifting();
        StartCoroutine(movementCoroutine);
    }

    private void Update()
    {
        //transform.position += direction * invaderSpeed.Evaluate(percentKilled) * Time.deltaTime;

        ////Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        ////Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        //foreach (Transform invader in transform)
        //{
        //    if (!invader.gameObject.activeInHierarchy)
        //    {
        //        continue;
        //    }

        //    if (direction == Vector3.right && invader.position.x >= (rightEdge.position.x - 1.5))
        //    {
        //        AdvanceRow();
        //    }
        //    else if (direction == Vector3.left && invader.position.x <= (leftEdge.position.x + 1.5))
        //    {
        //        AdvanceRow();
        //    }
        //}
    }

    private IEnumerator InvaderShifting()
    {
        while (true)
        {
            if (Time.timeScale == 0) // make sure Invaders do not move if time is paused
            {
                while (Time.timeScale == 0)
                {
                    yield return null;
                }
            }

            foreach (Transform currInvader in transform)
            {
                if (!currInvader.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (direction == Vector3.right && currInvader.position.x >= 14 ||
            direction == Vector3.left && currInvader.position.x <= -14)
                {
                    direction.x *= -1;

                    for (int row = 0; row < rows; row++)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            Invader invader = invaders[row, col];

                            Vector3 pos = invader.transform.position;
                            pos.y -= 1f;
                            invader.transform.position = pos;
                            invader.AnimateSprite();

                            yield return new WaitForSeconds(invaderSpeed.Evaluate(percentKilled));
                        }
                    }
                }
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Invader invader = invaders[row, col];

                    Vector3 pos = invader.transform.position;
                    pos.x += (direction.x * shiftAmount);
                    invader.transform.position = pos;
                    invader.AnimateSprite();

                    yield return new WaitForSeconds(invaderSpeed.Evaluate(percentKilled));
                }
            }

            am.PlayInvaderMoveSound();
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

    private void InvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        invadersKilled++;
        killed(invader);

        //if (invadersKilled >= totalInvaders)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }

    public void ResetInvaders()
    {
        invadersKilled = 0;
        direction = Vector3.right;
        Vector3 startingPos = initialPosition;
        startingPos.y -= WaveOffset();
        transform.position = startingPos;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }

        StopCoroutine(movementCoroutine);

        for (int row = 0; row < rows; row++)
        {
            float gridWidth = invaderSpacing * (cols - 1);
            float gridHeight = invaderSpacing * (rows - 1);

            Vector2 centering = new Vector2(-gridWidth / 2, -gridHeight / 2);
            Vector3 rowPos = new Vector3(centering.x, centering.y + (row * invaderSpacing), 0.0f);

            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = rowPos;
                pos.x += col * invaderSpacing;
                invaders[row,col].transform.localPosition = pos;
            }
        }

        StartCoroutine(movementCoroutine);
    }

    private int WaveOffset()
    {
        switch (gm.wave)
        {
            case 1:
                return 0;
            case 2:
                return 1;
            case 3:
                return 2;
            case 4:
                return 3;
            case 5:
                return 4;
            case 6:
                return 5;
            case 7:
                return 6;
            default:
                Debug.LogError("Invalid case in wave switch!");
                break;
        }

        return 0;
    }

    public bool InvadersLeft()
    {
        foreach (Transform currInvader in transform)
        {
            if (currInvader.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
}
