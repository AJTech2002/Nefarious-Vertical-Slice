using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Item", menuName ="Items")]
public class Item : ScriptableObject
{
    public new string name;
    public Vector2 size;
    public Texture2D icon; 
}
