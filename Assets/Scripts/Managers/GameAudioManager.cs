using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    #region Singleton

    public static GameAudioManager instance;
    public PlayerManager playerManager;
    public EquipmentManager equipmentManager;
    private PlayerController playerController;

    public AudioSource menuSource;
    public AudioSource runningSource;
    public AudioSource mainSource;

    public AudioClip acceptClickClip;
    public AudioClip cancelButtonClip;
    public AudioClip buttonClickedClip;
    public AudioClip questCompletedClip;
    public AudioClip deathClip;
    public AudioClip levelUpClip;
    public AudioClip attackClip1;
    public AudioClip attackClip2;
    public AudioClip equipClip;
    public AudioClip dealDamageClip;
    public AudioClip getDamagedClip1;
    public AudioClip getDamagedClip2;
    public AudioClip runningClip1;
    public AudioClip runningClip2;
    public AudioClip runningClip3;


    private readonly List<AudioClip> attackClips = new();
    private readonly List<AudioClip> getDamagedClips = new();


    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        playerManager = PlayerManager.instance;
        equipmentManager = EquipmentManager.instance;
        playerController = playerManager.player.GetComponent<PlayerController>();
        equipmentManager.equip += OnEquip;
        playerManager.playerStats.takenDamage += OnTakenDamagePlay;
        playerManager.playerStats.died += OnDeathPlay;
        playerController.dealtDamage += OnDealDamagePlay;


        SetAttackClips();
        SetGetDamagedClips();
    }

    private void SetAttackClips()
    {
        if (attackClip1 != null && attackClip2 != null)
        {
            attackClips.Add(attackClip1);
            attackClips.Add(attackClip2);
        }
    }

    private void SetGetDamagedClips()
    {
        if (getDamagedClip1 != null && getDamagedClip2 != null)
        {
            getDamagedClips.Add(getDamagedClip1);
            getDamagedClips.Add(getDamagedClip2);
        }
    }


    public void OnQuestCompletedPlay()
    {
        if (mainSource != null && questCompletedClip != null) mainSource.PlayOneShot(questCompletedClip);
    }

    private void OnDeathPlay()
    {
        if (deathClip != null && mainSource != null) mainSource.PlayOneShot(deathClip);
    }

    public void OnLevelUpPlay()
    {
        if (levelUpClip != null && mainSource != null) mainSource.PlayOneShot(levelUpClip);
    }

    public void OnAttackPlay()
    {
        if (mainSource != null && attackClip1 != null && attackClip2 != null && attackClips.Count > 0)
        {
            var randomAttack = Random.Range(0, 2);

            mainSource.PlayOneShot(attackClips[randomAttack]);
        }
    }

    private void OnEquip()
    {
        if (equipClip != null && mainSource != null) mainSource.PlayOneShot(equipClip);
    }

    public void OnTakenDamagePlay()
    {
        if (mainSource != null && getDamagedClip1 != null && getDamagedClip2 != null && getDamagedClips.Count > 0)
        {
            var damageClipNumber = Random.Range(0, 2); // 0 or 1

            mainSource.PlayOneShot(getDamagedClips[damageClipNumber]);
        }
    }

    public void OnDealDamagePlay()
    {
        if (dealDamageClip != null && mainSource != null) mainSource.PlayOneShot(dealDamageClip);
    }

    public void OnButtonClicked()
    {
        menuSource.PlayOneShot(buttonClickedClip);
    }

    public void OnAcceptButtonClicked()
    {
        menuSource.PlayOneShot(acceptClickClip);
    }

    public void OnCancelButtonClicked()
    {
        menuSource.PlayOneShot(cancelButtonClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        mainSource.PlayOneShot(audioClip);
    }
}