using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolTest : MonoBehaviour
{

    private Enemy[] Enemies;
    public string SpecificEnemy;

    // Start is called before the first frame update
    //void start(){
   //     final_Boss = GameObject.FindObjectOfType<Final_Boss> ();
   // }
    void OnTriggerEnter2D(Collider2D other)
    {
        Enemies = GameObject.FindObjectsOfType<Enemy> ();
        foreach (Enemy Enemy in Enemies){
            if ((other.gameObject.tag == "Enemy") && (Enemy.EnemyType == SpecificEnemy)){
                Enemy.UpdPatrolRange();
            }
        }
    }

//    void OnTriggerExit2D(Collider2D other)
 //   {
//        final_Boss = GameObject.FindObjectOfType<Final_Boss> ();
 //       if (other.gameObject.tag == "Enemy"){
 //       final_Boss.UpdPatrolRange();
 //       }
   // }
}
