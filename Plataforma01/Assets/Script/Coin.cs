using UnityEngine;

public class Moeda : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            other.GetComponent<PlayerCoins>().ColetarMoeda();

            Destroy(gameObject);
        }
    }
}