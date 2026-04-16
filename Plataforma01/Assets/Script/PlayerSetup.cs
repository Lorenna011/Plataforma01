using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.AtribuirInput(GetComponent<PlayerInput>());
    }
}