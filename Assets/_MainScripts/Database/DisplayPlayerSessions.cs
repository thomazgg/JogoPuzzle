using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerSessions : MonoBehaviour
{
    public GameObject playerSessionCardPrefab; // Prefab do card
    public Transform content; // ScrollView

    private List<DailyUsageRecord> dailyUsageRecords;

    private void Start()
    {
        // Localize o componente PlayerSessionManager
        PlayerSessionManager playerSessionManager = FindObjectOfType<PlayerSessionManager>();

        // Consulta de tempo de uso diário
        dailyUsageRecords = playerSessionManager.RetrieveDailyUsage();

        // Crie os cards dinamicamente
        CreatePlayerSessionCards();
    }

    private void CreatePlayerSessionCards()
    {
        foreach (DailyUsageRecord record in dailyUsageRecords)
        {
            // Crie uma instância do card a partir do Prefab
            GameObject card = Instantiate(playerSessionCardPrefab, content);

            // Acesse os elementos de UI do card para atribuir as informações
            Text gameTitleText = card.transform.Find("GameTitleText").GetComponent<Text>();
            Text tempoDeUsoText = card.transform.Find("TempoDeUsoText").GetComponent<Text>();
            Image gameImage = card.transform.Find("GameImage").GetComponent<Image>();

            // Atribua o tempo de uso ao elemento de texto
            tempoDeUsoText.text = record.UsageTime.ToString() + " minutos";

            // Carregue e atribua a imagem de fundo do jogo (se disponível)
            if (!string.IsNullOrEmpty(record.GameImage))
            {
                Sprite sprite = Resources.Load<Sprite>(record.GameImage);
                if (sprite != null)
                {
                    gameImage.sprite = sprite;
                }
            }
        }
    }
}
