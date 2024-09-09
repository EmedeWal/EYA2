using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Health>().Initialize();

        Mana mana = GetComponent<Mana>();

        if (mana != null)
        {
            mana.Initialize();
        }
    }
}
