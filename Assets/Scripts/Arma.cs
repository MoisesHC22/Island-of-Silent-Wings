using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    public int dañoAtaque = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto con el que colisiona tiene el script EnemyMovement
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement enemigo = collision.GetComponent<EnemyMovement>();
            if (enemigo != null)
            {
                // Aplica el daño al enemigo
                Vector2 direccionEmpuje = (collision.transform.position - transform.position).normalized;
                enemigo.RecibirDaño(dañoAtaque, direccionEmpuje, 10f); // Aplica el daño y el empuje
                Debug.Log($"Enemigo golpeado por el arma. Daño infligido: {dañoAtaque}");
            }
        }
    }
}
