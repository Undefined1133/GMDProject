using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    public PlayerManager playerManager;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;
        player = playerManager.player.transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
