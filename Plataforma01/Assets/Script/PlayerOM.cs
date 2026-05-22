using UnityEngine;
using System;

public static class PlayerOM
{
    public static Action<int> OnMoedaMudou;

    public static void NotificarMoeda(int quantidade)
    {
        Debug.Log("Observer notificou: " + quantidade);

        OnMoedaMudou?.Invoke(quantidade);
    }
}