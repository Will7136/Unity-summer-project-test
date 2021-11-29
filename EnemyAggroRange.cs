using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    private Bandit player;
    private Enemy[] Enemies;
    public string SpecificEnemy;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        player = GameObject.FindObjectOfType<Bandit> ();
        Enemies = GameObject.FindObjectsOfType<Enemy> ();
        foreach (Enemy Enemy in Enemies){
            if ((other.gameObject.tag == "Player") && (Enemy.EnemyType == SpecificEnemy)){
                Enemy.AggroPlayer();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        player = GameObject.FindObjectOfType<Bandit> ();
        Enemies = GameObject.FindObjectsOfType<Enemy> ();
        foreach (Enemy Enemy in Enemies){
            if ((other.gameObject.tag == "Player") && (Enemy.EnemyType == SpecificEnemy)){
                Enemy.ReturnToArea();
            }
        }
    }


}
