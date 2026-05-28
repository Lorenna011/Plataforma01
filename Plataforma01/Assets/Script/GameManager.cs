using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Iniciando,
        MenuPrincipal,
        Gameplay
    }

    public GameState estadoAtual;

    [Header("Input")]
    public PlayerInput playerInput;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MudarEstado(GameState.Iniciando);

        CarregarCena("Splash");
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuRoutine());
    }

    private IEnumerator LoadMainMenuRoutine()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");

        yield return SceneManager.LoadSceneAsync("GUI", LoadSceneMode.Additive);

        estadoAtual = GameState.Gameplay;
    }

    public void MudarEstado(GameState novoEstado)
    {
        estadoAtual = novoEstado;

        Debug.Log("Estado atual: " + estadoAtual);
    }

    public void CarregarCena(string nomeCena)
    {
        switch (nomeCena)
        {
            case "Splash":

                SceneManager.LoadScene("Splash");

                break;

            case "MenuPrincipal":

                SceneManager.LoadScene("MenuPrincipal");

                MudarEstado(GameState.MenuPrincipal);

                break;

            case "SampleScene":

                if (estadoAtual == GameState.MenuPrincipal)
                { 
                    SceneManager.LoadScene("SampleScene");

                    SceneManager.LoadScene("GUI", LoadSceneMode.Additive);

                    MudarEstado(GameState.Gameplay);
                }
                else
                {
                    Debug.Log("Não pode ir para Gameplay agora!");
                }

                break;

            default:

                Debug.Log("Cena não reconhecida");

                break;
        }
    }

    public void AtribuirInput(PlayerInput input)
    {
        playerInput = input;

        Debug.Log("Input atribuído ao jogador");
    }

    
}