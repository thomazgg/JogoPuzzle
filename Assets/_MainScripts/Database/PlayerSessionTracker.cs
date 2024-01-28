using System;
using UnityEngine;

public class PlayerSessionTracker : MonoBehaviour
{
    public string playerName = ""; // Nome do jogador
    public string gameTitle = ""; // Título do jogo
    private DateTime entryTime;
    private PlayerSessionManager playerSessionManager;

    private void Start()
    {
        playerSessionManager = FindObjectOfType<PlayerSessionManager>();
    }

    private void OnEnable()
    {
        // Registrar a entrada do jogador quando o objeto ficar ativo
        entryTime = DateTime.Now;
    }

    private void OnDisable()
    {
        // Registrar a saída do jogador quando o objeto for desativado
        DateTime exitTime = DateTime.Now;

        // Inserir a sessão do jogador no banco de dados
        playerSessionManager.InsertPlayerSession(playerName, gameTitle, entryTime, exitTime);
    }
}
