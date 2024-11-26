using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    public int da�oAtaque = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto con el que colisiona tiene el script EnemyMovement
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement enemigo = collision.GetComponent<EnemyMovement>();
            if (enemigo != null)
            {
                // Aplica el da�o al enemigo
                Vector2 direccionEmpuje = (collision.transform.position - transform.position).normalized;
                enemigo.RecibirDa�o(da�oAtaque, direccionEmpuje, 10f); // Aplica el da�o y el empuje
                Debug.Log($"Enemigo golpeado por el arma. Da�o infligido: {da�oAtaque}");
            }
        }
    }
}
