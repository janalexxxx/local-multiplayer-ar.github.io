using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerLeftPackage
{
    public static string packageType = "PlayerLeftPackage";

    public Player player;

    public PlayerLeftPackage(Player player) {
        this.player = player;
    }
}
