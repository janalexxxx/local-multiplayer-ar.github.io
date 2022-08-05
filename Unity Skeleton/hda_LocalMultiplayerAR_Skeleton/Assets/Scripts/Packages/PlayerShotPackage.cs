using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerShotPackage
{
    public string packageType = "PlayerShotPackage";

    public Player player;

    public PlayerShotPackage(Player player) {
        this.player = player;
    }
}
