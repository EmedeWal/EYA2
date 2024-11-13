using UnityEngine;

namespace EmeWillem
{
    [CreateAssetMenu(fileName = "Animation Clip Data", menuName = "Scriptable Object/Data/Animation Clip")]
    public class AnimationClipData : ScriptableObject
    {
        [Header("SETTINGS")]
        public string Name;
        public float TransitionDuration = 0.1f;
        public bool AllowOverrideDuringEnter = false;
        public bool AllowOverrideDuringExit = false;
        public bool AllowRepeat = false;

        [HideInInspector] public int Hash;

        public void Init()
        {
            Hash = Animator.StringToHash(Name);
        }
    }
}