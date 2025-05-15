using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase, ITriggerCheckable, IEnemyMoving
{
    public Rigidbody2D RB { get; private set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public EnemyStateMachine StateMachine { get; private set; }

    [SerializeField]
    private EnemyIdleBaseSO enemyIdleBase;
    public EnemyIdleBaseSO EnemyIdleBaseInstance { get; private set; }

    [SerializeField]
    private EnemyChaseBaseSO enemyChaseBase;
    public EnemyChaseBaseSO EnemyChaseBaseInstance { get; private set; }

    [SerializeField]
    private EnemyAttackBaseSO enemyAttackBase;

    public EnemyAttackBaseSO EnemyAttackBaseInstance { get; private set; }

    private Quaternion desiredRotation;

    [SerializeField]
    private float rotationSpeed = 5f;

    // Tried utilizing the OnAttacked event, but that won't work.
    [SerializeField]
    private EnemyHealthBarHandler enemyHealthBar;

    public event Action OnEnemyDeath;
    public event Action OnEnemySpawned;

    protected override void Awake()
    {
        base.Awake();
        EnemyIdleBaseInstance = Instantiate(enemyIdleBase);
        EnemyIdleBaseInstance.Initialize(this);

        EnemyChaseBaseInstance = Instantiate(enemyChaseBase);
        EnemyChaseBaseInstance.Initialize(this);

        EnemyAttackBaseInstance = Instantiate(enemyAttackBase);
        EnemyAttackBaseInstance.Initialize(this);

        StateMachine = new EnemyStateMachine(EnemyIdleBaseInstance);
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        OnEnemySpawned?.Invoke();
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.DoFrameUpdateLogic();
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.DoPhysicsLogic();
    }

    public void Move(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
        if (velocity == Vector2.zero)
        {
            return;
        }
        desiredRotation = Quaternion.FromToRotation(Vector2.right, velocity);
    }

    public override void TakeDamage(float amount)
    {
        enemyHealthBar.gameObject.SetActive(true);
        base.TakeDamage(amount);
        // TODO: Notify UI and animators that this has taken damage 
        //  (could be in abstract class instead of here)
    }

    public override void Die()
    {
        OnEnemyDeath?.Invoke();
        base.Die();
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.DoAnimationTriggerEventLogic(triggerType);
    }

    // I'm going to be honest, I'm not sure what these are for right now.
    // But considering the tutorial this code is based on had them, I'm keeping them in.
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }

    private void OnDestroy()
    {
        // Make sure the scriptable object instances are destroyed
        Destroy(EnemyIdleBaseInstance);
        Destroy(EnemyChaseBaseInstance);
        Destroy(EnemyAttackBaseInstance);

        // Now that there is a sibling relationship, make sure the whole object is destroyed.
        Destroy(transform.parent.gameObject);
    }
}
