using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    //tags 
    private const string PLATFORM_TAG = "Platform";
    private const string DEATH_TAG = "Death";
    private const string SLIME_TAG = "Slime";
    private const string SLIME_ATTACK_ANIM_PARA = "Damaged01";
    private const string DAMAGED_ANIM_PARA = "Damaged00";
    private const string CHECKPOINT_TAG = "CheckPoint";
    private const string SLIME_GNRT_TAG = "SlimeGenerator";

    //camera
    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_timeWaitingForDeadAnimation = 2f;
    [SerializeField] private Text text;
    [SerializeField] private GameObject m_slimeGenerator;
    //needed components 
    private Animator m_playerAnimator;
    private Rigidbody m_playerRigidBody;

    //
    [SerializeField] private Transform birthPoint; 

    //paras dealing with move
    [SerializeField] private float m_velocity;
    [SerializeField] private float m_velocityZ;
    [SerializeField] private float m_rotateVelocity;
    [SerializeField] private float m_velocityRunning;
    [SerializeField] private float m_velocityRunningZ;
    private float m_inputH = 0;
    private float m_inputV = 0;
    public bool AllowJump = true;

    //paras deal with Idle changing
    private float m_timer = 0;
    private bool m_isTiming = false;
    //private bool m_isWASDToMove;
    //private bool m_isAnyKeyDown = false;

    // Use this for initialization
    void Start () {
        m_playerAnimator = GetComponent<Animator>();
        m_playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        //move
        setInputParameters();
        setRuningParameters();
        runAndRotate();

        //jump
        if (Input.GetKeyDown(KeyCode.Z))
        {
            jump();
        }

        //idleTimer
        if(Input.anyKeyDown == false)
        {
            m_timer += Time.deltaTime;
        }
        else
        {
            m_timer = 0;
            
        }
        if(m_timer>5)
        {
            idleTimeOut();
        }
    }

    private void LateUpdate()
    {
        var a = transform.position;
    }

    private void setInputParameters()
    {
        m_inputH = Input.GetAxis("Horizontal");
        m_inputV = Input.GetAxis("Vertical");
        m_playerAnimator.SetFloat("InputH", m_inputH);
        m_playerAnimator.SetFloat("InputV", m_inputV);
    }

    private void setRuningParameters()
    {
        if (m_inputV <= 0)
        {
            m_inputH = 0;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            m_playerAnimator.SetBool("Running", true);
            m_velocity = m_velocityRunning;
            m_velocityZ = m_velocityRunningZ;
        }
        else
        {
            m_playerAnimator.SetBool("Running", false);
        }
    }

    private void runAndRotate()
    {
        float m_moveH = m_inputH * m_velocityZ * Time.deltaTime;
        float m_moveV = m_inputV * m_velocity * Time.deltaTime;
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * m_rotateVelocity, 0);
        m_playerRigidBody.velocity = new Vector3(0f, m_playerRigidBody.velocity.y, 0f) + transform.rotation * new Vector3(m_moveH, 0, m_moveV);
    }

    private void jump()
    {
        if (AllowJump == true)
        {
            m_playerAnimator.SetTrigger("Jumping");
            AllowJump = false;
        }
    }

    public void ReSetTimer()
    {
        if (m_isTiming == false)
        {
            m_isTiming = true;
            m_timer = 0;
        }
    }

    private void idleTimeOut()
    {
        m_playerAnimator.SetTrigger("IdleTimeOut");
        m_isTiming = false;
        m_timer = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == SLIME_TAG)
        {
            playerDie();
        }

        if (collision.collider.tag == DEATH_TAG)
        {
            playerDie();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == CHECKPOINT_TAG)
        {
            PlayerWin();
        }
    }

    private void playerDie()
    {
        m_slimeGenerator.GetComponent<SlimeGenerator>().enabled = false;

        DestoryItemsWithTag(SLIME_TAG);

        m_playerAnimator.SetTrigger(SLIME_ATTACK_ANIM_PARA);

        IEnumerator coroutine = WaitForDeadAnimation(m_timeWaitingForDeadAnimation);
        StartCoroutine(coroutine);

        //transform.position = birthPoint.position;
        //transform.rotation = birthPoint.rotation;
    }

    private void PlayerWin()
    {
        DestoryItemsWithTag(SLIME_GNRT_TAG);
        DestoryItemsWithTag(SLIME_TAG);
        text.enabled = true;
        m_playerAnimator.SetTrigger("Win");
    }

    private void DestoryItemsWithTag(string itemTag)
    {
        GameObject[] slimes;
        slimes = GameObject.FindGameObjectsWithTag(itemTag);
        foreach (GameObject i  in slimes)
        {
            Destroy(i);
        }
    }

    private IEnumerator WaitForDeadAnimation(float waitTime)
    {
        //Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(waitTime);
        transform.position = birthPoint.position;
        transform.rotation = birthPoint.rotation;
        m_playerRigidBody.velocity = Vector3.zero;
        m_slimeGenerator.GetComponent<SlimeGenerator>().enabled = true;

        //Time.timeScale = 1f;

    }

    //public void OnInteractableClick(Interactable interactable)
    //{
    //    m_currentInteractable = interactable;
    //    m_destinationPosition = m_currentInteractable.interactionLocation.position;
    //    m_navMeshAgent.SetDestination(m_destinationPosition);
    //    m_navMeshAgent.isStopped = false;
    //}
}
