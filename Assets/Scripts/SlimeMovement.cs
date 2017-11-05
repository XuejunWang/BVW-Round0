using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SlimeMovement : MonoBehaviour
{
    public float SlimeSpeed;
    public float SlimeAttackRange;
    [SerializeField] public GameObject Target;
    [SerializeField] private float m_stopRange = 0.8F;
    [SerializeField] private float m_destoryRange = 5f;

    private Animator m_slimeAnimator;
    private Rigidbody m_slimeRigidbody;
    private NavMeshAgent m_navMeshAgent;

    private const float m_locomationAnimationSmoothTime = .1f;
    private const float m_deadWaitingTime = 1f;
    private const string m_slimeDeathTag = "SlimeDeath";
    private const string m_deathTag = "Death";
    private const string m_playerTag = "Player";

     void Start()
     {
        // Set up references.
        m_slimeAnimator = GetComponent<Animator>();
        m_slimeRigidbody = GetComponent<Rigidbody>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.speed = SlimeSpeed;
     }

     void Update()
     {
        if(m_navMeshAgent.isOnNavMesh == true)
        {
            m_navMeshAgent.destination = Target.transform.position;
            float speedPercent = m_navMeshAgent.velocity.magnitude / m_navMeshAgent.speed;
            m_slimeAnimator.SetFloat("speedPercent", speedPercent, m_locomationAnimationSmoothTime, Time.deltaTime);

            if (m_navMeshAgent.remainingDistance > m_destoryRange)
            {
                Destroy(gameObject);
            }
            else if (m_navMeshAgent.remainingDistance > m_stopRange)
            {
                m_navMeshAgent.isStopped = false;
            }

            else if (m_navMeshAgent.remainingDistance <= m_stopRange) //距离palyer足够近
            {
                m_navMeshAgent.isStopped = true;
                Attack();
            }
        }
     }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == m_slimeDeathTag)
        {
            YeildDead(m_deadWaitingTime);
        }

        if (collision.collider.tag == m_deathTag)
        {
            YeildDead(m_deadWaitingTime);
        }
    }

    void Attack()
    {
        if (true)
        {
            m_slimeAnimator.SetTrigger("Attack_1");
        }
    }

    private IEnumerator WaitForDeadAnimation(float waitTime)
    {
        m_slimeAnimator.SetTrigger("Dead");
        yield return new WaitForSeconds(waitTime);
        Dead();
    }

    void YeildDead(float yeildTime)
    {
        IEnumerator coroutine = WaitForDeadAnimation(yeildTime);
        StartCoroutine(coroutine);
        SoundManager.PlayHitSound();
    }

    void Dead()
    {
        m_slimeAnimator.SetTrigger("Dead");
        Destroy(gameObject);
        
    }

    //private void OnAnimatorMove()
    //{
    //    m_navMeshAgent.velocity = m_slimeAnimator.deltaPosition / Time.deltaTime;
    //}
}

