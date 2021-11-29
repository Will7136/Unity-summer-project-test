using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool PatrolRange = false;
    public bool FacingLeft = true;
    public float EnemySpeed = 2f;
    public float GeneralScale = 1;
    public float XScale = 1;
    public string EnemyType;
    public int hp = 1;
    public Vector3 Home;
    public int DirectionFactor = -1;
    public int EnemyDamage = 1;
    public float timeBetweenAttacks = 1.5f;
    private Rigidbody2D Enemy_rb2d;
    private Animator Enemy_anim;
    private Collider2D Enemy_Collider;
    private bool IsMoving = false;
    private EnemyPatrolTest enemyPatrolTest;
    private Bandit player;
    private Vector3 playerPosition;
    private Vector3 CurrentPosition;
    private bool ActiveAggro = false;
    private bool ReturnHome = false;
    private bool playerInRange = false;
    private float AttackTime = 1.5f;
    private bool IsAttacking = false;
    private bool IsDead = false;

    public void UpdPatrolRange () //turns enemy around when it hits either of the patrol boundaries
    {
        PatrolRange = !PatrolRange;
    }


    public void UpdHealth(){ //takes health away from enemy when hit
        hp = hp -1;
        Enemy_anim.SetTrigger("Hurt");
        if (EnemyType == "Eye"){
            FindObjectOfType<AudioManager>().Play("EyeHit");
        }
        else if (EnemyType == "Skeleton"){
            FindObjectOfType<AudioManager>().Play("SkelHit");
        }
        else if (EnemyType == "Wizard"){
            FindObjectOfType<AudioManager>().Play("WizHit");
        }
        else if (EnemyType == "Bringer"){
            FindObjectOfType<AudioManager>().Play("BringerHit");
        }
        if (hp <= 0)
            Death();

    }

    private void Death (){ //animates enemy death and 
        Debug.Log(name + " is dead");
        Enemy_anim.SetBool("IsDead", true);
        //Plays the death sound effect
        if (EnemyType == "Eye"){
            FindObjectOfType<AudioManager>().Play("EyeDeath");
        }
        else if (EnemyType == "Skeleton"){
            FindObjectOfType<AudioManager>().Play("SkelDeath");
        }
        else if (EnemyType == "Wizard"){
            FindObjectOfType<AudioManager>().Play("WizDeath");
        }
        else if (EnemyType == "Bringer"){
            FindObjectOfType<AudioManager>().Play("BringerDeath");
        }

        if (EnemyType == "Bringer")
        FindObjectOfType<GameState>().GameWon();
        
        Enemy_rb2d.velocity = new Vector2(0, Enemy_rb2d.velocity.y);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        IsDead = true;
        player.NormalIdle();
    }

    public void AggroPlayer(){
        if (!IsDead){
            Debug.Log("AggroPlayer running");
            ActiveAggro = true;
            player.AggroIdle();
        }
    }

    public void ReturnToArea(){
        if (!IsDead){
            Debug.Log("ReturnToArea running");
            ActiveAggro = false;
            ReturnHome = true;
            player.NormalIdle();
        }
    }


    public void AttackInRange(){ //tells whether the player is in attack range
        playerInRange = true;
    }
    public void AttackOutOfRange(){
        playerInRange = false;
    }


     void Awake()
        {
            enemyPatrolTest = GameObject.FindObjectOfType<EnemyPatrolTest>();
            Enemy_rb2d = GetComponent<Rigidbody2D>();
            Enemy_anim = GetComponent<Animator>();
            player = GameObject.FindObjectOfType<Bandit> ();
        }

    void attack(){
        if (AttackTime >= timeBetweenAttacks){
            Enemy_anim.SetTrigger("Attacking");
            //Plays Attack sound
            if (EnemyType == "Eye"){
                FindObjectOfType<AudioManager>().Play("EyeAttack");
            }
            else if (EnemyType == "Wizard"){
                FindObjectOfType<AudioManager>().Play("WizAttack");
            }
            else if (EnemyType == "Bringer"){
                FindObjectOfType<AudioManager>().Play("BringerAttack");
            }
            AttackTime = 0;
        }
    }

    void midAttack(){
        player.playerHurt(EnemyDamage);
        if (EnemyType == "Skeleton"){
            FindObjectOfType<AudioManager>().Play("SkelAttack");
        }
    }

    void attackOver(){
        IsAttacking = false;
    }

    void Update(){



             // Swap direction of sprite depending on walk direction
            if (FacingLeft == false){
                transform.localScale = new Vector3(-XScale, GeneralScale, GeneralScale);
                DirectionFactor = 1;
            }
            else if (FacingLeft == true){
                transform.localScale = new Vector3(XScale, GeneralScale, GeneralScale);
            }




            //Aggro on player, move towards them
            if (ActiveAggro){
                if (playerPosition.x < CurrentPosition.x){ //determines the direction of travel to get to the player
                    FacingLeft = true;
                    DirectionFactor = -1;
                }
                else{
                    FacingLeft = false;
                    DirectionFactor = 1;
                }


                if ((!PatrolRange) && (!playerInRange)){
                    Enemy_rb2d.velocity = new Vector2(DirectionFactor * EnemySpeed , Enemy_rb2d.velocity.y);
                    IsMoving = true;
                }
                else if ((!PatrolRange) && (playerInRange)){
                    Enemy_rb2d.velocity = new Vector2(0 , Enemy_rb2d.velocity.y);
                    attack();
                }
                else if ((PatrolRange) && (playerInRange)){
                    Enemy_rb2d.velocity = new Vector2(0 , Enemy_rb2d.velocity.y);
                    attack();
                }
                else{
                    Enemy_rb2d.velocity = new Vector2(0 , Enemy_rb2d.velocity.y);
                }

            }
            //Return to Home point for patrol restart
            else if (ReturnHome){
                if (Home.x < CurrentPosition.x){ //determines the direction of travel to get home
                    FacingLeft = true;
                    DirectionFactor = -1;
                }
                else{
                    FacingLeft = false;
                    DirectionFactor = 1;
                }

                Enemy_rb2d.velocity = new Vector2(DirectionFactor * EnemySpeed , Enemy_rb2d.velocity.y);
                IsMoving = true;

            }
            //PATROL
            else{
                //Movement or swap directions
                if(PatrolRange == false){
                    Enemy_rb2d.velocity = new Vector2(DirectionFactor * EnemySpeed, Enemy_rb2d.velocity.y);
                    IsMoving = true;
                }
                else{
                    FacingLeft = !FacingLeft;
                    DirectionFactor = -1 * DirectionFactor;
                    PatrolRange = false;
                }
            }




            // animations
            if ((IsMoving) && (!IsAttacking))
                Enemy_anim.SetInteger("AnimState", 1);
            else if (!IsAttacking)
                Enemy_anim.SetInteger("AnimState", 0);
        
        
            IsMoving = false;
            AttackTime += Time.deltaTime;



            //finding player position
            playerPosition = player.transform.position;
            CurrentPosition = transform.position;

            if (CurrentPosition == Home){
                ReturnHome = false;
            }
        
    }
}
