using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    [SerializeField]
    private GameObject gameOverUI;

    public int score { get; private set; }
    public int lives { get; private set; }

    [SerializeField]
    private Image livesIcon;
    [SerializeField]
    private Image extraLifeIcon1;
    [SerializeField]
    private Image extraLifeIcon2;

    [SerializeField]
    private Image scoreOnesPlace;
    [SerializeField]
    private Image scoreTensPlace;
    [SerializeField]
    private Image scoreHundredsPlace;
    [SerializeField]
    private Image scoreThousandsPlace;

    [SerializeField]
    private Image highScoreOnesPlace;
    [SerializeField]
    private Image highScoreTensPlace;
    [SerializeField]
    private Image highScoreHundredsPlace;
    [SerializeField]
    private Image highScoreThousandsPlace;

    [SerializeField]
    private Sprite zeroSprite;
    [SerializeField]
    private Sprite oneSprite;
    [SerializeField]
    private Sprite twoSprite;
    [SerializeField]
    private Sprite threeSprite;
    [SerializeField]
    private Sprite fourSprite;
    [SerializeField]
    private Sprite fiveSprite;
    [SerializeField]
    private Sprite sixSprite;
    [SerializeField]
    private Sprite sevenSprite;
    [SerializeField]
    private Sprite eightSprite;
    [SerializeField]
    private Sprite nineSprite;

    [HideInInspector]
    public int wave = 1;

    [SerializeField]
    private GameObject playerDeath;
    [SerializeField]
    private Sprite[] playerDeathSprites;

    private AudioManager am;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        am = FindObjectOfType<AudioManager>();

        if (!PlayerPrefs.HasKey("SpaceInvadersHighScore")) // if high score doesnt exist, initialize to zero
        {
            PlayerPrefs.SetInt("SpaceInvadersHighScore", 0);
        }

        UpdateHighScoreUI();
    }

    private void Start()
    {
        player.killed += OnPlayerKilled;
        mysteryShip.killed += OnMysteryShipKilled;
        invaders.killed += OnInvaderKilled;

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);

        wave = 1;

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        mysteryShip.Despawn();

        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        player.ResetLaser();

        invaders.gameObject.SetActive(true);
        invaders.ResetInvaders();

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        UpdateScoreUI();

        if (PlayerPrefs.GetInt("SpaceInvadersHighScore") < score) // if score greater than high score, high score becomes score
        {
            PlayerPrefs.SetInt("SpaceInvadersHighScore", score);
            UpdateHighScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        int scoreAmount = score;

        scoreThousandsPlace.sprite = GetNumberSprite(scoreAmount / 1000);
        scoreAmount = scoreAmount % 1000;

        scoreHundredsPlace.sprite = GetNumberSprite(scoreAmount / 100);
        scoreAmount = scoreAmount % 100;

        scoreTensPlace.sprite = GetNumberSprite(scoreAmount / 10);
        scoreAmount = scoreAmount % 10;

        scoreOnesPlace.sprite = GetNumberSprite(scoreAmount);
    }

    private void UpdateHighScoreUI()
    {
        int scoreAmount = PlayerPrefs.GetInt("SpaceInvadersHighScore");

        highScoreThousandsPlace.sprite = GetNumberSprite(scoreAmount / 1000);
        scoreAmount = scoreAmount % 1000;

        highScoreHundredsPlace.sprite = GetNumberSprite(scoreAmount / 100);
        scoreAmount = scoreAmount % 100;

        highScoreTensPlace.sprite = GetNumberSprite(scoreAmount / 10);
        scoreAmount = scoreAmount % 10;

        highScoreOnesPlace.sprite = GetNumberSprite(scoreAmount);
    }

    private Sprite GetNumberSprite(int digit)
    {
        switch (digit)
        {
            case 0:
                return zeroSprite;
            case 1:
                return oneSprite;
            case 2:
                return twoSprite;
            case 3:
                return threeSprite;
            case 4:
                return fourSprite;
            case 5:
                return fiveSprite;
            case 6:
                return sixSprite;
            case 7:
                return sevenSprite;
            case 8:
                return eightSprite;
            case 9:
                return nineSprite;
        }

        return null;
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        
        switch (this.lives)
        {
            case 0:
                livesIcon.sprite = zeroSprite;
                break;
            case 1:
                livesIcon.sprite = oneSprite;
                extraLifeIcon1.enabled = false;
                extraLifeIcon2.enabled = false;
                break;
            case 2:
                livesIcon.sprite = twoSprite;
                extraLifeIcon1.enabled = true;
                extraLifeIcon2.enabled = false;
                break;
            case 3:
                livesIcon.sprite = threeSprite;
                extraLifeIcon1.enabled = true;
                extraLifeIcon2.enabled = true;
                break;
            default:
                Debug.LogError("Invalid Case in SetLives switch!");
                break;
        }
    }

    private void OnPlayerKilled()
    {
        // SetLives(lives - 1);

        Transform playerPos = player.gameObject.transform;
        player.gameObject.SetActive(false);

        am.PlayPlayerDeathSound();

        StartCoroutine(PlayerDeathSequence(2f, playerPos));

        //if (lives > 0)
        //{
        //    Invoke(nameof(NewRound), 1f);
        //}
        //else
        //{
        //    GameOver();
        //}
    }

    private IEnumerator PlayerDeathSequence(float delay, Transform playerPos)
    {
        float frameTime = delay / 12f;

        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        Time.timeScale = 0;

        player.ResetLaser();

        playerDeath.SetActive(true);
        playerDeath.transform.position = playerPos.position;

        int frame = 0;

        SpriteRenderer sr = playerDeath.GetComponent<SpriteRenderer>();
        sr.sprite = playerDeathSprites[frame];

        yield return null;
        DeathAnimation[] deathEffects = FindObjectsOfType<DeathAnimation>();
        foreach (DeathAnimation deathEffect in deathEffects)
        {
            Destroy(deathEffect.gameObject);
        }

        float pauseEndTime = Time.realtimeSinceStartup + delay;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return frameTime;

            frame++;
            if (frame >= playerDeathSprites.Length)
            {
                frame = 0;
            }

            sr.sprite = playerDeathSprites[frame];
        }

        playerDeath.SetActive(false);
        SetLives(lives - 1);

        pauseEndTime = Time.realtimeSinceStartup + (delay / 2);
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        Time.timeScale = 1;

        if (lives > 0)
        {
            Respawn();

            // Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.score);

        if (invaders.invadersKilled == invaders.totalInvaders || !invaders.InvadersLeft())
        {
            wave++;
            if (wave > 7)
            {
                wave = 1;
            }
            Invoke(nameof(NewRound), 1f);
            // NewRound();
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }
}
