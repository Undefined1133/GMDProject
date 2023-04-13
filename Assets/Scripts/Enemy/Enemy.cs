using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{
    private PlayerManager playerManager;
    private readonly List<CharacterStats> myStats = new();

    private void Start()
    {
        playerManager = PlayerManager.instance;
        myStats.Add(GetComponent<CharacterStats>());
    }

    public override void Interact()
    {
        base.Interact();
        var playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        if (playerCombat != null) playerCombat.Attack(myStats);
    }
}