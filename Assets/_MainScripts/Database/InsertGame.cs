using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertGame : MonoBehaviour
{
    // Título do jogo
    public string gameTitle = "";
    // Imagem do jogo
    public string gameImage = "";

    void Start()
    {
        // Localize o componente PlayerSessionManager
        PlayerSessionManager playerSessionManager = FindObjectOfType<PlayerSessionManager>();

        // Verifique se o componente PlayerSessionManager foi encontrado
        if (playerSessionManager != null)
        {
            // Chame o método InsertGame para inserir um novo jogo com os valores definidos no Inspector
            playerSessionManager.InsertGame(gameTitle, gameImage);
        }
        else
        {
            Debug.LogError("PlayerSessionManager não encontrado no cenário.");
        }
    }
}
