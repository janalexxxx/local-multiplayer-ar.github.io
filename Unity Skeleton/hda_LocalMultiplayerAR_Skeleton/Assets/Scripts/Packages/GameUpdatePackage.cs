using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameUpdatePackage
{
    public static string packageType = "GameUpdatePackage";

    public Game game;

    public GameUpdatePackage(Game game) {
        this.game = game;
    }
}
