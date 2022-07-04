using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource enemyDeathSource;
    [SerializeField]
    private AudioClip enemyDeathSound;

    [SerializeField]
    private AudioSource playerShootSource;
    [SerializeField]
    private AudioClip playerShootSound;

    [SerializeField]
    private AudioSource playerDeathSource;
    [SerializeField]
    private AudioClip playerDeathSound;

    [SerializeField]
    private AudioSource mysteryShipHighPitchSource;
    [SerializeField]
    private AudioSource mysteryShipLowPitchSource;

    [SerializeField]
    private AudioSource invaderMoveSource;
    [SerializeField]
    private AudioClip[] invaderMoveSounds;
    private int invaderMoveIndex = 0;

    [SerializeField]
    private AudioSource secretSongSource;
    private bool playSecretSong = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            playSecretSong = !playSecretSong;

            if (playSecretSong)
            {
                secretSongSource.Play();
            }
            else
            {
                secretSongSource.Stop();
            }
        }
    }

    public void PlayEnemyDeathSound()
    {
        enemyDeathSource.PlayOneShot(enemyDeathSound, 0.6f);
    }

    public void PlayPlayerShootSound()
    {
        playerShootSource.PlayOneShot(playerShootSound, 0.6f);
    }

    public void PlayPlayerDeathSound()
    {
        playerDeathSource.PlayOneShot(playerDeathSound, 0.6f);
    }

    public void PlayMysteryShipSound()
    {
        mysteryShipHighPitchSource.Play();
        //mysteryShipLowPitchSource.Play();
    }

    public void StopMysteryShipSound()
    {
        mysteryShipHighPitchSource.Stop();
        //mysteryShipLowPitchSource.Stop();
    }

    public void PlayInvaderMoveSound()
    {
        invaderMoveSource.PlayOneShot(invaderMoveSounds[invaderMoveIndex], 0.6f);
        invaderMoveIndex++;
        if (invaderMoveIndex >= invaderMoveSounds.Length)
        {
            invaderMoveIndex = 0;
        }
    }
}
