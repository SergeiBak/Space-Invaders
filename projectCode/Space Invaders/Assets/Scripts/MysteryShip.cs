using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public float cycleTime = 30f;
    public int score = 300;
    public System.Action<MysteryShip> killed;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    public Vector3 leftDestination { get; private set; }
    public Vector3 rightDestination { get; private set; }
    public int direction { get; private set; } = -1;
    public bool spawned { get; private set; }

    [SerializeField]
    private DeathAnimation deathEffect;
    [SerializeField]
    private Sprite deathSprite;

    private AudioManager am;

    private void Awake()
    {
        am = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        // Offset the destination by a unit so the ship is fully out of sight
        Vector3 left = transform.position;
        left.x = leftEdge.position.x - 1f;
        leftDestination = left;

        Vector3 right = transform.position;
        right.x = rightEdge.position.x + 1f;
        rightDestination = right;

        transform.position = leftDestination;
        Despawn();
    }

    private void Update()
    {
        if (!spawned) {
            return;
        }

        if (direction == 1) {
            MoveRight();
        } else {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= rightDestination.x) {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= leftDestination.x) {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;

        if (direction == 1) {
            transform.position = leftDestination;
        } else {
            transform.position = rightDestination;
        }

        spawned = true;

        am.PlayMysteryShipSound();
    }

    public void Despawn()
    {
        spawned = false;

        am.StopMysteryShipSound();

        if (direction == 1) {
            transform.position = rightDestination;
        } else {
            transform.position = leftDestination;
        }

        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            DeathAnimation effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.sr.sprite = deathSprite;
            effect.sr.color = new Color32(0xFF, 0x00, 0x00, 0xFF); // FF0000

            Despawn();
            am.PlayEnemyDeathSound();

            if (killed != null) {
                killed.Invoke(this);
            }
        }
    }

}
