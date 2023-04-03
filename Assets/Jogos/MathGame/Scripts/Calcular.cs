using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcular : MonoBehaviour
{
    int primeiroValor, segundoValor, valorTemp, resposta;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown("z"))
        {
            Calcula("somar");
        }

        if(Input.GetKeyDown("x"))
        {
            Calcula("subtrair");
        }

        if(Input.GetKeyDown("c"))
        {
            Calcula("multiplicar");
        }

        if(Input.GetKeyDown("v"))
        {
            Calcula("dividir");
        }
    }

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
        }

        if(operacao == "subtrair") {
            resposta = primeiroValor - segundoValor;
        }

        if(operacao == "multiplicar") {
            resposta = primeiroValor * segundoValor;
        }

        if(operacao == "dividir") {
            resposta = primeiroValor / segundoValor;
        }

        Debug.Log("(1) Valor: " + primeiroValor + " / (2) Valor: " + segundoValor + "\nResposta: " + resposta);

    }

}
