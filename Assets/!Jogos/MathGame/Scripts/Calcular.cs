using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calcular : MonoBehaviour
{
    int primeiroValor, segundoValor, valorTemp, resposta;
    public Text PrimeiroDigito, SegundoDigito, Sinal, RespostaFinal;

    int alt1, alt2;
    public Text Alt1, Alt2, Alt3;
    private string TempoOp, varSinal;

    public Sprite sSim, sNao;

    public GameObject buttons;
    public GameObject conta;
    public GameObject SimOuNao1, SimOuNao2, SimOuNao3;

    private void Start() {
        buttons.SetActive(true);
        conta.SetActive(false); 
    }

    // BOTOES

    public void BotaoSomar()
    {
        buttons.SetActive(false);
        conta.SetActive(true); 
        Calcula("somar");
    }

    public void BotaoSubtrair()
    {
        buttons.SetActive(false);
        conta.SetActive(true); 
        Calcula("subtrair");
    }

    public void BotaoMultiplicar()
    {
        buttons.SetActive(false);
        conta.SetActive(true); 
        Calcula("multiplicar");
    }

    public void BotaoDividir()
    {
        buttons.SetActive(false);
        conta.SetActive(true); 
        Calcula("dividir");
    }

    public void BotaoVoltar()
    {
        buttons.SetActive(true);
        conta.SetActive(false); 
        ResetarValores();
    }

    // CALCULAR - SOMA / SUBTRACAO / MULTIPLICACAO / DIVISAO

    public void Calcula(string operacao)
    {
        primeiroValor = Random.Range(1,10);
        segundoValor = Random.Range(1,10);

        if(primeiroValor - segundoValor < 0) {
            valorTemp = segundoValor;
            segundoValor = primeiroValor;
            primeiroValor = valorTemp;
        }

        if(operacao == "somar") {
            resposta = primeiroValor + segundoValor;
            varSinal = "somar";
        }

        if(operacao == "subtrair") {
            resposta = primeiroValor - segundoValor;
            varSinal = "subtrair";
        }

        if(operacao == "multiplicar") {
            resposta = primeiroValor * segundoValor;
            varSinal = "multiplicar";
        }

        if(operacao == "dividir") {
            resposta = primeiroValor / segundoValor;
            varSinal = "dividir";
        }

        Debug.Log("(1) Valor: " + primeiroValor + " / (2) Valor: " + segundoValor + "\nResposta: " + resposta);

        PrimeiroDigito.text = primeiroValor.ToString();
        SegundoDigito.text = segundoValor.ToString();

        // SINAL

        if(varSinal == "somar")
        {
            Sinal.text = "+";
        }
        if(varSinal == "subtrair")
        {
            Sinal.text = "-";
        }
        if(varSinal == "multiplicar")
        {
            Sinal.text = "x";
        }
        if(varSinal == "dividir")
        {
            Sinal.text = "÷";
        }

        RespostaFinal.text = "?";

        // PRIMEIRA ALTERNATIVA
        valorTemp = Random.Range(2,20);
        while (valorTemp == resposta)
        {
            valorTemp = Random.Range(2,20);
        }
        alt1 = valorTemp;

        // SEGUNDA ALTERNATIVA
        valorTemp = Random.Range(2,20);
        while ((valorTemp == resposta || valorTemp == alt1))
        {
            valorTemp = Random.Range(2,20);
        }
        alt2 = valorTemp;

        Debug.Log("Alternativa (1): " + alt1 + " / Alternativa (2): " + alt2 + " / Alternativa (3): " + resposta);

        // ORDENANDO AS ALTERNATIVAS
        valorTemp = Random.Range(1,4);

        switch (valorTemp)
        {
            case 1:
                Alt1.text = resposta.ToString();
                Alt2.text = alt1.ToString();
                Alt3.text = alt2.ToString();
                break;
            case 2:
                Alt1.text = alt1.ToString();
                Alt2.text = resposta.ToString();
                Alt3.text = alt2.ToString();
                break;
            case 3:
                Alt1.text = alt1.ToString();
                Alt2.text = alt2.ToString();
                Alt3.text = resposta.ToString();
                break;
            default:
                Debug.Log("Valor inválido para ordenar alternativas.");
                break;
        }

    }

    private bool acertou = false;

    public void Alt1_Click()
    {
        if (!acertou) // Verifica se ainda não acertou
        {
            SimOuNao1.SetActive(true); 
            if (Alt1.text == resposta.ToString())
            {
                SimOuNao1.GetComponent<Image>().sprite = sSim;
                RespostaCerta();
                acertou = true; // Define o estado de acerto como true
            }
            else
            {
                SimOuNao1.GetComponent<Image>().sprite = sNao;
                RespostaErrada();
            }
        }
    }

    public void Alt2_Click()
    {
        if (!acertou) // Verifica se ainda não acertou
        {
            SimOuNao2.SetActive(true); 
            if (Alt2.text == resposta.ToString())
            {
                SimOuNao2.GetComponent<Image>().sprite = sSim;
                RespostaCerta();
                acertou = true; // Define o estado de acerto como true
            }
            else
            {
                SimOuNao2.GetComponent<Image>().sprite = sNao;
                RespostaErrada();
            }
        }
    }

    public void Alt3_Click()
    {
        if (!acertou) // Verifica se ainda não acertou
        {
            SimOuNao3.SetActive(true); 
            if (Alt3.text == resposta.ToString())
            {
                SimOuNao3.GetComponent<Image>().sprite = sSim;
                RespostaCerta();
                acertou = true; // Define o estado de acerto como true
            }
            else
            {
                SimOuNao3.GetComponent<Image>().sprite = sNao;
                RespostaErrada();
            }
        }
    }

    public void ResetarValores()
    {
        SimOuNao1.SetActive(false); 
        SimOuNao2.SetActive(false); 
        SimOuNao3.SetActive(false);

        acertou = false;
    }

    public void RespostaErrada()
    {
        //
    }

    public void RespostaCerta()
    {
        RespostaFinal.text = resposta.ToString();
        StartCoroutine(ProximaPergunta());
    }

    IEnumerator ProximaPergunta()
    {
        yield return new WaitForSeconds(2);
        ResetarValores();
        Calcula(varSinal);
    }
}
