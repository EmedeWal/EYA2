using UnityEngine;

namespace EmeWillem
{
    [CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Data/Attack Data/Base Attack")]
    public class BaseAttackData : ScriptableObject
    {
        [Header("AUDIO")]
        public AudioClip AudioClip;
        public float AudioOffset;
        public float AudioVolume;
    }
}