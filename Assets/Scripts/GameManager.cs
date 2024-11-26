using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool jugadorMuerto = false;

    [Header("Sprite de Victoria")]
    public GameObject spriteWin;

    [Header("Lista de enemigos")]
    private List<GameObject> enemigosRestantes = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject enemigo in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemigosRestantes.Add(enemigo);
        }

        if (spriteWin != null)
        {
            spriteWin.SetActive(false); // Asegura que el sprite esté desactivado al inicio
        }
    }

    public void SetJugadorMuerto()
    {
        jugadorMuerto = true;
    }

    public void EnemigoMuerto(GameObject enemigo) 
    {
        if (enemigosRestantes.Contains(enemigo))
        {
            enemigosRestantes.Remove(enemigo);
            Debug.Log($"Enemigos restantes: {enemigosRestantes.Count}");

            // Si no quedan enemigos, activa el sprite de victoria
            if (enemigosRestantes.Count == 0 && spriteWin != null)
            {
                spriteWin.SetActive(true);
            }
        }
    }


    void Update()
    {
        if (jugadorMuerto && Input.GetKeyDown(KeyCode.Return)) // Detecta la tecla Enter
        {
            ReiniciarEscena();
        }
    }

    private void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
