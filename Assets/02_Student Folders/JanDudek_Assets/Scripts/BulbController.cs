using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class BulbController : MonoBehaviour
{
    [System.Serializable]
    public struct RendererIndexData
    {
        public Renderer renderer;
        public int materialIndex;

        public RendererIndexData(Renderer renderer, int index)
        {
            this.renderer = renderer;
            this.materialIndex = index;
        }
    }

    [Header("Parameters")]
    [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
    public float deathDuration = 0f;

    [Header("Sounds")]
    [Tooltip("Sound played when recieving damages")]
    public AudioClip damageTick;

    [Header("VFX")]
    [Tooltip("The VFX prefab spawned when the enemy dies")]
    public GameObject deathVFX;

    [Tooltip("The point at which the death VFX is spawned")]
    public Transform deathVFXSpawnPoint;

    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public UnityAction onDamaged;


    float m_LastTimeDamaged = float.NegativeInfinity;


    Health m_Health;
    Actor m_Actor;
    Collider[] m_SelfColliders;
    GameFlowManager m_GameFlowManager;
    bool m_WasDamagedThisFrame;

    void Start()
    {

        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, BulbController>(m_Health, this, gameObject);


        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, BulbController>(m_GameFlowManager, this);

        // Subscribe to damage & death actions
        m_Health.onDie += OnDie;
        m_Health.onDamaged += OnDamaged;

    }

    void Update()
    {
        m_WasDamagedThisFrame = false;
    }


    void OnDamaged(float damage, GameObject damageSource)
    {
        // test if the damage source is the player
        if (damageSource && damageSource.GetComponent<PlayerCharacterController>())
        {

            if (onDamaged != null)
            {
                onDamaged.Invoke();
            }
            m_LastTimeDamaged = Time.time;

            // play the damage tick sound
            if (damageTick && !m_WasDamagedThisFrame)
                AudioUtility.CreateSFX(damageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f);

            m_WasDamagedThisFrame = true;
        }
    }

    void OnDie()
    {
        // spawn a particle system when dying
        var vfx = Instantiate(deathVFX, deathVFXSpawnPoint.position, Quaternion.identity);
        Destroy(vfx, 5f);
        Destroy(gameObject, deathDuration);
    }

   
}
