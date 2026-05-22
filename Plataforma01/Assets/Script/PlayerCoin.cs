using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    int moedas = 0;

    public void ColetarMoeda()
    {
        moedas++;

        Debug.Log("Moedas coletadas: " + moedas);

        
        PlayerOM.NotificarMoeda(moedas);
    }
}