using UnityEngine;
using System.Collections;
 
public class Bandit : MonoBehaviour {
 
    [SerializeField] float      m_speed = 4f;
    [SerializeField] float      m_jumpForce = 6f;
    [SerializeField] int      m_HP = 5;
    
 
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private bool                m_isAttacking = false;
    private float                targetTime = 0.366f;
    private bool                TimerDone = false;
    private PlayerCombat        PlayerCombat;
    private HealthUI            HealthDisplay;
    private float               AttackTime = 1f;
    public float                timeBetweenAttacks = 1f;
 
    // Use this for initialization
    void Start () {
        PlayerCombat = GameObject.FindObjectOfType<PlayerCombat> ();
        HealthDisplay = GameObject.FindObjectOfType<HealthUI> ();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }
    

    public void playerHurt(int damage){
            m_animator.SetTrigger("Hurt");
            m_HP = m_HP - damage;
            FindObjectOfType<AudioManager>().Play("PlayerHit");
            HealthDisplay.UpdateHealthUI(m_HP);
    }

    public void AggroIdle(){ //Changes the idle animation of the player when an enemy is aggroed omn them
        m_combatIdle = true;
    }

    public void NormalIdle() { //Changes the idle animation back to the normal one when an eney is no loger aggroed
        m_combatIdle = false;
    }
 
    // Update is called once per frame
    void Update () {
        if (AttackTime >= 0.5f){
            m_isAttacking = false;
        }

        if (!m_isDead){
            //timer for movement sfx
            targetTime -= Time.deltaTime;
            if (targetTime <= 0.0f){
                timerEnded();
            }
 
            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State()) {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }
 
            //Check if character just started falling
            if(m_grounded && !m_groundSensor.State()) {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }
 
            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");
 
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-1.7f, 1.7f, 1.7f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
 
            // Move
            if (!m_isAttacking){
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            }
 
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
 
            // -- Handle Animations --
            //Death
            if (m_HP <= 0) {
                m_isDead = true;
                m_animator.SetTrigger("Death");
                FindObjectOfType<GameState>().GameOver();
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
            }
 
            //Attack
            else if((Input.GetMouseButtonDown(0)) && (AttackTime >= timeBetweenAttacks)) {
                m_animator.SetTrigger("Attack");
                m_isAttacking = true;
                Debug.Log("Pressed left click");
                PlayerCombat.Attack();
                AttackTime = 0f;
            }
 
            //Jump
            else if (Input.GetKeyDown("space") && m_grounded) {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
 
            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon){
                m_animator.SetInteger("AnimState", 2);
                if((m_grounded == true) && (TimerDone == true)){
                    FindObjectOfType<AudioManager>().Play("PlayerMove");
                    TimerDone = false;
                }
            }
 
            //Combat Idle
            else if (m_combatIdle)
                m_animator.SetInteger("AnimState", 1);
 
            //Idle
            else
                m_animator.SetInteger("AnimState", 0);
 
        }
        AttackTime += Time.deltaTime;
    }
    private void timerEnded(){
        TimerDone = true;
        targetTime = 0.366f;
    }
}

