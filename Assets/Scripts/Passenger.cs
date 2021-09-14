using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Passenger", menuName = "Passenger")]
public class Passenger : ScriptableObject
{
    public string personName;
    public Color color;
    public TMP_FontAsset font;
}
