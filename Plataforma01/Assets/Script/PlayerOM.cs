using UnityEngine;
using System;

public static class PlayerOM
{
    public static Action OnIncrementCoin;
    public static Action<int> OnCoinAdded;

    public static void NotificarMoeda()
    {
        // Debug.Log("Observer notificou: " + quantidade);

        OnIncrementCoin?.Invoke();
    }

    public static void AddCoin(int qt)
    {
        OnCoinAdded?.Invoke(qt);
    }
}
