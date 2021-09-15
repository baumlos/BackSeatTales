using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Dialogue>().PlayClip();
    }
}
