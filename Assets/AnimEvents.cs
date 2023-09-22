using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public event Action Take;

    void TakeEvent()
    {
        Take?.Invoke();
    }
}
