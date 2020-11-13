using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Header("UI Click")]
    [SerializeField] AudioClip clickSFX;
    [SerializeField] float clickPitch;
    [SerializeField] float clickVolume;

    [Header("Arrow Click")]
    [SerializeField] AudioClip arrowSFX;
    [SerializeField] float arrowPitch;
    [SerializeField] float arrowVolume;

    [Header("Level Select Click")]
    [SerializeField] AudioClip levelSelectSFX;
    [SerializeField] float levelSelectPitch;
    [SerializeField] float levelSelectVolume;

    [Header("Upgrade Click")]
    [SerializeField] AudioClip upgradeSFX;
    [SerializeField] float upgradePitch;
    [SerializeField] float upgradeVolume;

    [Header("No Money Click")]
    [SerializeField] AudioClip noMoneySFX;
    [SerializeField] float noMoneyPitch;
    [SerializeField] float noMoneyVolume;

    [Header("Card Sound")]
    [SerializeField] AudioClip cardSFX;
    [SerializeField] float cardPitch;
    [SerializeField] float cardVolume;

    [Header("Enemy Turn Sound")]
    [SerializeField] AudioClip enemyTurnSFX;
    [SerializeField] float enemyTurnPitch;
    [SerializeField] float enemyTurnVolume;

    [Header("Player Turn Sound")]
    [SerializeField] AudioClip playerTurnSFX;
    [SerializeField] float playerTurnPitch;
    [SerializeField] float playerTurnVolume;

    [Header("Enemy Take Damage Sound")]
    [SerializeField] AudioClip enemyTakeDamageSFX;
    [SerializeField] float enemyTakeDamagePitch;
    [SerializeField] float enemyTakeDamageVolume;

    [Header("Player Take Damage Sound")]
    [SerializeField] AudioClip playerTakeDamageSFX;
    [SerializeField] float playerTakeDamagePitch;
    [SerializeField] float playerTakeDamageVolume;

    [Header("Player Heal Sound")]
    [SerializeField] AudioClip playerHealSFX;
    [SerializeField] float playerHealPitch;
    [SerializeField] float playerHealVolume;

    [Header("Victory Sound")]
    [SerializeField] AudioClip victorySFX;
    [SerializeField] float victoryPitch;
    [SerializeField] float victoryVolume;

    [Header("Defeat Sound")]
    [SerializeField] AudioClip defeatSFX;
    [SerializeField] float defeatPitch;
    [SerializeField] float defeatVolume;

    [Header("Collide Sound")]
    [SerializeField] AudioClip collideSFX;
    [SerializeField] float collidePitch;
    [SerializeField] float collideVolume;

    [Header("Fuse Sound")]
    [SerializeField] AudioClip fuseSFX;
    [SerializeField] float fusePitch;
    [SerializeField] float fuseVolume;

    [Header("Fuse Effect Sound")]
    [SerializeField] AudioClip fuseEffectSFX;
    [SerializeField] float fuseEffectPitch;
    [SerializeField] float fuseEffectVolume;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClickSound()
    {
        audioSource.pitch = clickPitch;
        audioSource.volume = clickVolume;
        audioSource.PlayOneShot(clickSFX);
    }

    public void PlayFullClickSound()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        AudioSource gmAudioSource = gameManager.GetComponent<AudioSource>();

        gmAudioSource.pitch = clickPitch;
        gmAudioSource.volume = clickVolume;
        gmAudioSource.PlayOneShot(clickSFX);
    }

    public void PlayArrowSound()
    {
        audioSource.pitch = arrowPitch;
        audioSource.volume = arrowVolume;
        audioSource.PlayOneShot(arrowSFX);
    }

    public void PlayFullLevelSelectSound()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        AudioSource gmAudioSource = gameManager.GetComponent<AudioSource>();

        gmAudioSource.pitch = levelSelectPitch;
        gmAudioSource.volume = levelSelectVolume;
        gmAudioSource.PlayOneShot(levelSelectSFX);
    }

    public void PlayUpgradeSound()
    {
        audioSource.pitch = upgradePitch;
        audioSource.volume = upgradeVolume;
        audioSource.PlayOneShot(upgradeSFX);
    }

    public void PlayNoMoneySound()
    {
        audioSource.pitch = noMoneyPitch;
        audioSource.volume = noMoneyVolume;
        audioSource.PlayOneShot(noMoneySFX);
    }

    public void PlayCardSound()
    {
        audioSource.pitch = cardPitch;
        audioSource.volume = cardVolume;
        audioSource.PlayOneShot(cardSFX);
    }

    public void PlayPlayerTurnSound()
    {
        audioSource.pitch = playerTurnPitch;
        audioSource.volume = playerTurnVolume;
        audioSource.PlayOneShot(playerTurnSFX);
    }

    public void PlayEnemyTurnSound()
    {
        audioSource.pitch = enemyTurnPitch;
        audioSource.volume = enemyTurnVolume;
        audioSource.PlayOneShot(enemyTurnSFX);
    }

    public void PlayFuseSound()
    {
        audioSource.pitch =fusePitch;
        audioSource.volume = fuseVolume;
        audioSource.PlayOneShot(fuseSFX);
    }

    public void PlayFuseEffectSound()
    {
        audioSource.pitch = fuseEffectPitch;
        audioSource.volume = fuseEffectVolume;
        audioSource.PlayOneShot(fuseEffectSFX);
    }

    public void PlayCollideSound()
    {
        audioSource.pitch = collidePitch;
        audioSource.volume = collideVolume;
        audioSource.PlayOneShot(collideSFX);
    }

    public void PlayEnemyTakeDamageSound()
    {
        audioSource.pitch = Random.Range(enemyTakeDamagePitch - 0.05f, enemyTakeDamagePitch + 0.05f);
        audioSource.volume = enemyTakeDamageVolume;
        audioSource.PlayOneShot(enemyTakeDamageSFX);
    }

    public void PlayPlayerTakeDamageSound()
    {
        audioSource.pitch = Random.Range(playerTakeDamagePitch - 0.05f, playerTakeDamagePitch + 0.05f);
        audioSource.volume = playerTakeDamageVolume;
        audioSource.PlayOneShot(playerTakeDamageSFX);
    }

    public void PlayPlayerHealSound()
    {
        audioSource.pitch = playerHealPitch;
        audioSource.volume = playerHealVolume;
        audioSource.PlayOneShot(playerHealSFX);
    }

    public void PlayVictorySound()
    {
        audioSource.pitch = victoryPitch;
        audioSource.volume = victoryVolume;
        audioSource.PlayOneShot(victorySFX);
    }

    public void PlayDefeatSound()
    {
        audioSource.pitch = defeatPitch;
        audioSource.volume = defeatVolume;
        audioSource.PlayOneShot(defeatSFX);
    }
}
