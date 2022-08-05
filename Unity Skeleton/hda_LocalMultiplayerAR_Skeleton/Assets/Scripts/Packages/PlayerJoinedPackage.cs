using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerJoinedPackage
{
    public static string packageType = "PlayerJoinedPackage";

    public Player player;

    public PlayerJoinedPackage(Player player) {
        this.player = player;
    }
}
