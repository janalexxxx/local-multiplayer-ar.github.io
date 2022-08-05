using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerMovedPackage
{
    public static string packageType = "PlayerMovedPackage";

    public Player player;

    public PlayerMovedPackage(Player player) {
        this.player = player;
    }
}