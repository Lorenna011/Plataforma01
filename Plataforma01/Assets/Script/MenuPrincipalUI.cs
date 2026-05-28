using UnityEngine;

public class MenuPrincipalUI : MonoBehaviour
{
    public void IniciarJogo()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}