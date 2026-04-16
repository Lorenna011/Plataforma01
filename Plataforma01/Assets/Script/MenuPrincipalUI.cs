using UnityEngine;

public class MenuPrincipalUI : MonoBehaviour
{
    public void IniciarJogo()
    {
        GameManager.Instance.CarregarCena("SampleScene");
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}