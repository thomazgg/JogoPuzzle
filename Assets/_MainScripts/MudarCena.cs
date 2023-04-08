using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MudarCena : MonoBehaviour
{
    public string nomeDaCena; // Nome da cena a ser carregada

    public void MudarDeCena()
    {
        SceneManager.LoadScene(nomeDaCena); // Carrega a cena com o nome fornecido
    }
}
