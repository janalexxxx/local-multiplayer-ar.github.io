using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLeftPackage
{
    public string packageType = "PlayerLeftPackage";

    public Player player;

    public PlayerLeftPackage(Player player) {
        this.player = player;
    }
}
