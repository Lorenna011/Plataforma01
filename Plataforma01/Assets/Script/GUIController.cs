using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI textoMoedas;

    private void OnEnable()
    {
        PlayerOM.OnCoinAdded += AtualizarTexto;
    }

    private void OnDisable()
    {
        PlayerOM.OnCoinAdded -= AtualizarTexto;
    }

    void AtualizarTexto(int quantidade)
    {
        Debug.Log("GUI atualizou");

        textoMoedas.text = "Moedas: " + quantidade;
    }
}