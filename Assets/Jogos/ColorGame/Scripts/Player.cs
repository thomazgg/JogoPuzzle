using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float pulo = 10f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public string corAtual;
    public Color corAzul;
    public Color corAmarelo;
    public Color corRoxo;
    public Color corRosa;

    public GameObject btPlay;

    public string nome;

    private void Start()
    {
        SetRandomColor();
        Time.timeScale = 0;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            rb.velocity = Vector2.up * pulo;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "trocaCor") {
            SetRandomColor();
            Destroy(collision.gameObject);
            return;
        }

        if (collision.tag != corAtual) { 
            Debug.Log("GAME OVER!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.tag == "Fase")
        {
            SceneManager.LoadScene(nome);
            Debug.Log("eu passei");
        }
    }

    public void SetRandomColor() {
        int index = Random.Range(1, 5);

        switch (index) {
            case 1:
                corAtual = "azul";
                sr.color = corAzul;
                break;
            case 2:
                corAtual = "amarelo";
                sr.color = corAmarelo;
                break;
            case 3:
                corAtual = "roxo";
                sr.color = corRoxo;
                break;
            case 4:
                corAtual = "rosa";
                sr.color = corRosa;
                break;
        }
    }

    public void Jogar()
    {
        Time.timeScale = 1;
        btPlay.SetActive(false);
    }
}
