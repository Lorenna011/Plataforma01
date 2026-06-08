using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    int moedas = 0;

    private void OnEnable()
    {
        PlayerOM.OnIncrementCoin += ColetarMoeda;
    }

    private void Disable()
    {
        PlayerOM.OnIncrementCoin -= ColetarMoeda;
    }

    public void ColetarMoeda()
    {
        moedas++;

        Debug.Log("Moedas coletadas: " + moedas);
        PlayerOM.AddCoin(moedas);
    }
}