using UnityEngine;

public class SplashController : MonoBehaviour
{
    void Start()
    {
        Invoke("IrParaMenu", 2f);
    }

    void IrParaMenu()
    {
        GameManager.Instance.CarregarCena("MenuPrincipal");
    }
}