using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region !SETUP!

    #region REFERENCES

    [Header("REFERENCES")]

    #region GameObjects

    [Header("References: GameObjects")]
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private Transform lightAttackPoint;
    [SerializeField] private Transform heavyAttackPoint;
    [SerializeField] private Animator animator;

    private Transform attackPoint;
    private Rigidbody rb;
    private Health health;
    //private Souls souls;
    #endregion

    #region Audio
    [Header("AUDIO")]
    [SerializeField] private AudioSource[] audioSources;
    #endregion

    #endregion

    // End of References

    #region VARIABLES
    private int soulGain;

    private float audioOffset = 0;
    private float audioVolume = 0;
    #endregion 

    // End of Variables

    #region EVENTS

    [Header("EVENTS")]
    [SerializeField] private FloatEvent onUltimateStart;
    [SerializeField] private FloatEvent onDashStart;

    [SerializeField] private FloatEvent onOrcUltimate;
    [SerializeField] private UnityEvent onGhostUltimateStart;

    public delegate void OnSwapIcons(string currentStance, bool ultimateActive);
    public static event OnSwapIcons onSwapIcons;

    public delegate void OnUltimateEnd();
    public static event OnUltimateEnd onUltimateEnd;

    public delegate void OnDashAttack();
    public static event OnDashAttack onDashAttack;
    #endregion

    // End of Events

    #region STANCES

    [Header("STANCES")]

    #region General

    [Header("General")]
    [SerializeField] private List<string> stances;

    private int stancePosition = 0;
    private string currentStance;
    #endregion

    #region Visuals

    [Header("Visuals")]
    [SerializeField] private Color[] stanceColors;
    [SerializeField] private Renderer swordRenderer;
    #endregion

    #region Vampire Stance

    [Header("Vampire Stance: Audio")]
    [SerializeField] private AudioClip vampireClip;
    [SerializeField] private float vampireAudioOffset;
    [SerializeField] private float vampireAudioVolume;

    [Header("Vampire Stance: Variables")]
    [SerializeField] private float bleedDamage;
    [SerializeField] private float bleedTicks;
    [SerializeField] private float bleedIntervals;

    [SerializeField] private Image vampireUltGFX;
    [SerializeField] private float vampireUltTicks;
    [SerializeField] private float vampireUltRange;
    #endregion

    #region Orc Stance

    [Header("Orc Stance: Audio")]
    [SerializeField] private AudioClip orcClip;
    [SerializeField] private float orcAudioOffset;
    [SerializeField] private float orcAudioVolume;

    [Header("Orc Stance: Variables")]
    [SerializeField] private GameObject shockwaveCanvas;
    [SerializeField] private float orcDamageMultiplier;
    [SerializeField] private float orcDamageReduction;
    [SerializeField] private float shockwaveRange;
    [SerializeField] private float shockwaveModifier;
    private bool orcUltActive = false;
    #endregion

    #region Ghost Stance

    [Header("Ghost Stance: Audio")]
    [SerializeField] private AudioClip ghostClip;
    [SerializeField] private float ghostAudioOffset;
    [SerializeField] private float ghostAudioVolume;

    [Header("Ghost Stance: Variables")]
    [SerializeField] private float ghostDamageMultiplier;
    [SerializeField] private float ghostDashCooldown;
    #endregion

    #endregion

    // End of Stances

    #region MOVEMENT

    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float deadzoneValue = 0.4f;

    private Coroutine footstepCoroutine;
    private Vector2 move;
    private bool canMove = true;
    private bool canRotate = true;
    #endregion

    // End of Movement

    #region ATTACKING

    [Header("ATTACKING")]
    [SerializeField] private AudioClip lightAttackClip;
    [SerializeField] private AudioClip heavyAttackClip;
    [SerializeField] private int lightAttackSoulGain;
    [SerializeField] private int heavyAttackSoulGain;

    private List<GameObject> damagedEnemies = new List<GameObject>();
    private Coroutine attackReset;
    private Coroutine comboReset;
    private int comboCounter = 0;
    private Vector3 attackSize;
    private float attackDamage;
    private float attackSpeed;
    private float attackChargeTime;
    private float movementDelay;
    private bool canAttack = true;
    private bool isAttacking = false;
    #endregion

    // End of Attacking

    #region ULTIMATE

    [Header("ULTIMATE")]
    [SerializeField] private float ultimateDuration;
    [HideInInspector] public bool ultimateActive;

    #endregion

    // End of Ultimate

    #region DASHING

    [Header("DASHING")]

    [SerializeField] private AudioClip dashClip;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    public float dashCooldown = 6f;

    private Coroutine dashCoroutine;
    private bool isDashing = false;
    private bool canDash = true;
    #endregion

    // End of Dodging

    #endregion

    // END OF SETUP

    #region !EXECUTION!

    #region DEFAULT

    //private void OnEnable()
    //{
    //    StancePurchaseMenu.onStanceGranted += AddStance;
    //    StancePurchaseMenu.onFirstStanceGranted += DetermineStance;
    //}

    //private void OnDisable()
    //{
    //    StancePurchaseMenu.onStanceGranted -= AddStance;
    //    StancePurchaseMenu.onFirstStanceGranted -= DetermineStance;
    //}

    // Retrieve components, set objects inactive, make the cursor invisible
    private void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        playerCanvas.SetActive(false);
        Cursor.visible = false;
    }

    // Swap to the first stance
    private void Start()
    {
        if (stances.Count > 0) SwapStance();
    }

    // Check conditions of player states and execute code accordingly
    private void FixedUpdate()
    {
        if (!isAttacking && !isDashing) Move();
        if (isAttacking && isDashing) DealDamage();
    }
    #endregion

    // End of Default

    #region INPUT

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnSwapStance(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) DetermineStance();
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (canAttack && !isDashing)
            {
                SetAttackVariables("Light");
                Attack();
            }
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (canAttack && !isDashing)
            {
                SetAttackVariables("Heavy");
                Attack();
            }
        }
    }

    public void OnUltimate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!ultimateActive) Ultimate();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!isAttacking && canDash) Dash();
        }
    }

    #endregion

    // End of Input

    #region STANCES

    // Handle the input of the player and update _stancePosition accordingly
    public void DetermineStance()
    {
        if (stances.Count == 0) return;

        int lastPosition = stances.Count - 1;
        stancePosition += 1;
        if (stancePosition < 0) stancePosition = lastPosition;
        else if (stancePosition > lastPosition) stancePosition = 0;

        SwapStance();
    }

    // Based on the current _stancePosition, swap to the correct stance and handle additional functionality
    private void SwapStance()
    {
        currentStance = stances[stancePosition];
        onSwapIcons?.Invoke(currentStance, ultimateActive);

        if (currentStance == "Vampire") swordRenderer.material.color = stanceColors[0];
        else if (currentStance == "Orc") swordRenderer.material.color = stanceColors[1];
        else if (currentStance == "Ghost") swordRenderer.material.color = stanceColors[2];
    }

    public void AddStance(string stance)
    {
        stances.Add(stance);
    }

    #endregion

    // End of Stances

    #region MOVEMENT

    // Calculate movement and call rotate to rotate the player in the movement direction
    // If the player cannot move, make sure no "ghost" moving happens
    private void Move()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (canRotate) Rotate(movement);

        if (canMove && movement.magnitude >= deadzoneValue)
        {
            movement = 100 * moveSpeed * Time.fixedDeltaTime * movement.normalized;
            rb.velocity = movement;
            animator.SetFloat("Speed", movement.magnitude);

            if (movement.magnitude == 0) rb.velocity = Vector3.zero;
            footstepCoroutine ??= StartCoroutine(FootStepManager());
        }
        else
        {
            CancelFootsteps();
            animator.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
        }    
    }

    // Rotate the player in his movement direction
    private void Rotate(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
        }
    }

    private IEnumerator FootStepManager()
    {
        while(true)
        {
            audioSources[3].Play();
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void CancelFootsteps()
    {
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
            audioSources[3].Stop();
        }
    }

    #endregion

    // End of Movement

    #region ATTACKING

    /// <summary>
    /// Reset relevant coroutines and clear the damagedEnemies list
    /// In this stage, the player can still rotate, but not move or attack
    /// Start coroutines for attackCD reset and for entering the next stage of attacking
    /// </summary>
    private void Attack()
    {
        if (attackReset != null) StopCoroutine(attackReset);

        damagedEnemies.Clear();
        audioSources[0].Play();
        canMove = false;
        canRotate = true;
        canAttack = false;

        Invoke(nameof(AttackStart), attackChargeTime);

        attackReset = StartCoroutine(AttackReset());
    }

    // The player casts his attack and can no longer rotate. SpentMana is dealt
    private void AttackStart()
    {
        canRotate = false;
        isAttacking = true;

        DealDamage();
    }

    // The player can attack and dash again, but movement and rotation can only happen after a small delay
    private IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(attackSpeed - movementDelay);

        canAttack = true;
        isAttacking = false;

        yield return new WaitForSeconds(movementDelay);

        canMove = true;
        canRotate = true;
    }

    /// <summary>
    /// SpentMana is dealt an additional logic is being checked to handle stance logic
    /// Vampire stance: apply bleed
    /// Orc stance: double hits
    /// Ghost stance: double damage on lunges
    /// </summary>
    private void DealDamage()
    {
        bool attackLanded = false;
        //bool isLunge = false;

        //if (isDashing) isLunge = true;

        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackSize);

        foreach (Collider hit in hits)
        {
            GameObject hitObject = hit.gameObject;
            Health hitHhealth = hitObject.GetComponent<Health>();

            if (hitHhealth != null && !damagedEnemies.Contains(hitObject) && !hitObject.CompareTag("Player"))
            {
                if (!attackLanded) AttackCombo();

                float damage = attackDamage;
                damagedEnemies.Add(hitObject);

                //#region Stance Related Logic

                //if (currentStance == "Vampire") hitObject.GetComponent<StatusEffect>().Bleed(bleedDamage, bleedTicks, bleedIntervals);

                //if (currentStance == "Orc" && (comboCounter % 3 == 0)) damage *= orcDamageMultiplier;

                //if (currentStance == "Ghost" && isLunge) damage *= ghostDamageMultiplier;
                //#endregion

                if (!attackLanded)
                {
                    attackLanded = true;
    

                    if (orcUltActive) OrcUltimateShockwave(hitObject, damage);
                }

                hitHhealth.TakeDamage(damage);
            }
        }
    }

    // Increment the comboCounter to keep track of orc attack combo's
    private void AttackCombo()
    {
        if (comboReset != null) StopCoroutine(comboReset);

        comboReset = StartCoroutine(ComboReset());
        comboCounter++;
    }

    // Reset the combo if the player does not attack for a little while
    private IEnumerator ComboReset()
    {
        yield return new WaitForSeconds(attackSpeed + movementDelay);

        comboCounter = 0;
    }

    // Set all relevant attack variables based on which attack is used
    private void SetAttackVariables(string attackType)
    {
        if (attackType == "Light")
        {
            animator.SetTrigger("Attack - Slash");
            audioSources[0].clip = lightAttackClip;
            attackPoint = lightAttackPoint;
            attackSize = new Vector3(2f, 1f, 2f);
            soulGain = lightAttackSoulGain;
            attackDamage = 10f;
            attackChargeTime = 0.3f;
            attackSpeed = 1f;
            movementDelay = 0.3f;
        }

        if (attackType == "Heavy")
        {
            animator.SetTrigger("Attack - Pierce");
            audioSources[0].clip = heavyAttackClip;
            attackPoint = heavyAttackPoint;
            attackSize = new Vector3(1.5f, 1f, 3f);
            soulGain = heavyAttackSoulGain;
            attackDamage = 15f;
            attackChargeTime = 0.5f;
            attackSpeed = 1.5f;
            movementDelay = 0.3f;
        }
    }
    #endregion

    // End of Attacking

    #region ULTIMATE

    /// <summary>
    /// Cast the correct ultimate and spend all player souls
    /// Determine which stance the player is in and which ultimate to cast
    /// Then cast the vampire, orc, or ghost ultimate
    /// </summary>
    private void Ultimate()
    {
        AudioSource audioSource = audioSources[1];

        if (true)
        {

            ultimateActive = true;

            if (currentStance == "Vampire")
            {
                audioOffset = vampireAudioOffset;
                audioVolume = vampireAudioVolume;
                audioSource.clip = vampireClip;
                VampireUltimate();
            }

            if (currentStance == "Orc")
            {
                audioOffset = orcAudioOffset;
                audioVolume = orcAudioVolume;
                audioSource.clip = orcClip;
                OrcUltimate();
            }

            if (currentStance == "Ghost")
            {
                audioOffset = ghostAudioOffset;
                audioVolume = ghostAudioVolume;
                audioSource.clip = ghostClip;
                GhostUltimate();
            }

            audioSource.volume = audioVolume;
            audioSource.time = audioOffset;
            audioSource.Play();

            ultimateActive = true;
            onUltimateStart.Invoke(ultimateDuration);
            Invoke(nameof(UltimateEnd), ultimateDuration);
        }
    }

    /// <summary>
    /// Cast a big circle around the player, which inflicts a more potent heal on all enemies hit
    /// This bleed deals about half the damage of a regular bleed, but heals twice as much
    /// The GFX are the circle component which expands until it has reached the range of the special
    /// </summary>
    private void VampireUltimate()
    {
        StartCoroutine(VampireUltimateGFX());

        Collider[] hits = Physics.OverlapSphere(transform.position, vampireUltRange);

        foreach (Collider hit in hits)
        {
            //StatusEffect statusEffectManager = hit.GetComponent<StatusEffect>();
            //if (statusEffectManager != null) statusEffectManager.Curse(ultimateDuration);
        }
    }

    private IEnumerator VampireUltimateGFX()
    {
        playerCanvas.SetActive(true);

        RectTransform size = vampireUltGFX.rectTransform;
        Vector2 nativeSize = size.sizeDelta;
        Vector2 maxSize = new Vector2(vampireUltRange, vampireUltRange);
        float increment = 0.3f;
        float pause = 0.01f;

        while (size.sizeDelta.magnitude < maxSize.magnitude)
        {
            size.sizeDelta += new Vector2(increment, increment);

            yield return new WaitForSeconds(pause);
        }

        yield return new WaitForSeconds(pause * 10);

        size.sizeDelta = nativeSize;
        playerCanvas.SetActive(false);
    }

    /// <summary>
    /// The player becomes invulnerable for the entire duration
    /// All attacks now cause a small shockwave around the _target
    /// This shockwave deals half the damage of the original attack in a small aeo
    /// The shockwave is instantiated at the _target and destroyed a second later
    /// </summary>
    private void OrcUltimate()
    {
        onOrcUltimate.Invoke(orcDamageReduction);
        orcUltActive = true;
    }

    private void OrcUltimateShockwave(GameObject target, float damage)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int layerMask = 1 << enemyLayer;
        Vector3 origin = new Vector3(target.transform.position.x, attackPoint.position.y, target.transform.position.z);

        StartCoroutine(OrcUltimateGFX(origin));

        Collider[] hits = Physics.OverlapSphere(target.transform.position, shockwaveRange, layerMask);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == target) continue;

            Health eHealth = hit.GetComponent<Health>();
            if (eHealth != null && !damagedEnemies.Contains(hit.gameObject)) eHealth.TakeDamage(damage / shockwaveModifier);
        }
    }

    private IEnumerator OrcUltimateGFX(Vector3 origin)
    {
        GameObject VFX = Instantiate(shockwaveCanvas, origin, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        Destroy(VFX);
    }

    /// <summary>
    /// The player resets his dash
    /// Gains reduced dash _timer
    /// And no longer collides with enemies
    /// </summary>
    private void GhostUltimate()
    {
        StartCoroutine(GhostUltimateEffect());
    }

    private IEnumerator GhostUltimateEffect()
    {
        onGhostUltimateStart?.Invoke();
        canDash = true;

        ModifyDashCD(-ghostDashCooldown);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(ultimateDuration);

        ModifyDashCD(ghostDashCooldown);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        UltimateEnd();
    }

    /// <summary>
    /// Set special _active for the duration and initialise the _currentCD system based on the duration
    /// Also set the color of the _currentCD text correct, to match the stance colors
    /// Set the _currentCD text _active via an event
    /// </summary>
    public void UltimateEnd()
    {
        ultimateActive = false;

        if (orcUltActive)
        {
            orcUltActive = false;
            onOrcUltimate.Invoke(0);
        }

        onSwapIcons?.Invoke(currentStance, ultimateActive);
        onUltimateEnd?.Invoke();
        StartCoroutine(UltimateAudioFadeOut());
    }

    private IEnumerator UltimateAudioFadeOut()
    {
        AudioSource audioSource = audioSources[1];

        float audioModification = audioSource.volume / 100;
        float audioDelay = 0.01f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= audioModification;

            yield return new WaitForSeconds(audioDelay);
        }

        audioSource.Stop();
    }

    #endregion

    // End of Ultimate

    #region DASHING

    /// <summary>
    /// Add a force to dash forward
    /// The dash is stopped after dashduration
    /// The dash is reset after dash CD
    /// </summary>
    private void Dash()
    {
        if (!canAttack && canRotate) onDashAttack?.Invoke(); 

        onDashStart.Invoke(dashCooldown);

        AudioSource audioSource = audioSources[2];
        audioSource.clip = dashClip;
        audioSource.time = 0.05f;
        audioSource.Play();

        // Shenanigans for dashing out of an attack
        if (canAttack && !isAttacking && !canMove && !canRotate)
        {
            animator.SetTrigger("Dash");

            canMove = true;
            canRotate = true;

            Vector3 movement = new Vector3(move.x, 0f, move.y);
            transform.rotation = Quaternion.LookRotation(movement);
        }

        canDash = false;
        isDashing = true;

        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        StartCoroutine(Invincible(dashDuration));

        Invoke(nameof(DashEnd), dashDuration);
        Invoke(nameof(DashReset), dashCooldown);
    }

    // Reset rigidBody states, player velocity, and booleans
    public void DashEnd()
    {
        rb.velocity = Vector3.zero;

        isDashing = false;
    }

    private void DashReset()
    {
        canDash = true;
    }

    public void ModifyDashCD(float amount)
    {
        dashCooldown += amount;
    }

    #endregion

    // End of Dashing

    #region Other

    private IEnumerator Invincible(float duration)
    {
        //health.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        //health.SetInvincible(false);
    }
    #endregion

    #endregion

    // END OF EXECUTION
}
