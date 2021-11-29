using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private Enemy[] EnemySelection;

    public void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        EnemySelection = GameObject.FindObjectsOfType<Enemy> ();

        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("We hit"+ enemy.name);
            foreach(Enemy SelectedEnemy in EnemySelection){
                if (SelectedEnemy.name == enemy.name){
                    SelectedEnemy.UpdHealth();
                }
            }
        }
        FindObjectOfType<AudioManager>().Play("PlayerAttack");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
