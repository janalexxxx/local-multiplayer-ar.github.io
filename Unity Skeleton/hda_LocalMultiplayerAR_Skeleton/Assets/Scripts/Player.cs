using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player 
{
    public int id;
    public float positionX;
    public float positionY;
    public float positionZ;

    public Vector3 position {
        get {
            return new Vector3(positionX, positionY, positionZ);
        }
        set {
            positionX = value.x;
            positionY = value.y;
            positionZ = value.z;
        }
    }

    public Player(int id, float positionX, float positionY, float positionZ) {
      this.id = id;
      this.positionX = positionX;
      this.positionY = positionY;
      this.positionZ = positionZ;
    }
}
