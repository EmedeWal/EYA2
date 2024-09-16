using UnityEngine;

public class PlayerSkip : MonoBehaviour
{
    //private PlayerInputManager _inputManager;

    public delegate void PlayerSkip_Skip();
    public static event PlayerSkip_Skip Skip;

    //private float _skipCooldown = 0.2f;
    //private bool _canSkip = true;

    //private void Awake()
    //{
    //    _inputManager = GetComponent<PlayerInputManager>();
    //}

    //private void OnEnable()
    //{
    //    _inputManager.SkipInput_Performed += PlayerSkip_SkipInput_Performed;
    //}

    //private void OnDisable()
    //{
    //    _inputManager.SkipInput_Performed -= PlayerSkip_SkipInput_Performed;
    //}

    //private void PlayerSkip_SkipInput_Performed()
    //{
    //    if (_canSkip)
    //    {
    //        _canSkip = false;
    //        Invoke(nameof(ResetCanSkip), _skipCooldown);

    //        OnSkip();
    //    }
    //}

    private void OnSkip()
    {
        Skip?.Invoke();
    }

    //private void ResetCanSkip()
    //{
    //    _canSkip = true;    
    //}
}
