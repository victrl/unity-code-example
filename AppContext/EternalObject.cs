using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
