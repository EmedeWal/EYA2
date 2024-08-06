
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action<float> EnemyDamageTaken;

    public delegate void EnemyHealth_EnemyDied();
    public static event EnemyHealth_EnemyDied EnemyDied;
}
