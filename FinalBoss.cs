using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Boss : MonoBehaviour
{

    public bool PatrolRange = false;
    public bool FacingRight = true;
    private float BringerSpeed = 2f;
    private Rigidbody2D Bringer_rb2d;
    private Animator Bringer_anim;
    private int DirectionFactor = -1;
    private bool IsMoving = false;
    private EnemyPatrolTest enemyPatrolTest;



    
    // Start is called before the first frame update
    void Awake()
    {
        enemyPatrolTest = GameObject.FindObjectOfType<EnemyPatrolTest> ();
        Bringer_rb2d = GetComponent<Rigidbody2D>();
        Bringer_anim = GetComponent<Animator>();
    }

    public void UpdPatrolRange ()
    {
        PatrolRange = !PatrolRange;
    }

    // Update is called once per frame
    void Update()
    {

        // Swap direction of sprite depending on walk direction
        if (FacingRight == false){
            transform.localScale = new Vector3(-5.9f, 5.9f, 5.9f);
            DirectionFactor = 1;
    }
        else if (FacingRight == true){
            transform.localScale = new Vector3(5.9f, 5.9f, 5.9f);
        }

        //Movement or swap directions
        if(PatrolRange == false){
        Bringer_rb2d.velocity = new Vector2(DirectionFactor * BringerSpeed, Bringer_rb2d.velocity.y);
        IsMoving = true;
        }
        else{
            FacingRight = !FacingRight;
            DirectionFactor = -1 * DirectionFactor;
        }


        //animations
        if (IsMoving)
            Bringer_anim.SetInteger("AnimState", 1);
        else
            Bringer_anim.SetInteger("AnimState", 0);


        IsMoving = false;
        PatrolRange = false;
    }
}
