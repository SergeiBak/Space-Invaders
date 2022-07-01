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

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

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

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

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
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.score);

        if (invaders.invadersKilled == invaders.totalInvaders)
        {
            NewRound();
        }
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }
}