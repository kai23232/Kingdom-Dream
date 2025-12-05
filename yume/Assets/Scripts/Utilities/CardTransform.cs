using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTransform
{
    public SerializeVector3 pos;
    public Quaternion rotation;
    
    public CardTransform(Vector3 pos, Quaternion rotation)
    {
        this.pos = new SerializeVector3(pos);
        this.rotation = rotation;
    }
}
