using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FaceData", menuName = "ScriptableObjects/Face", order = 1)]
public class Face : ScriptableObject
{
    public Texture[] NormalFace;
    public Texture attackFace, damageFace;
}
