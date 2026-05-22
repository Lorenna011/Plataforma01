using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
        // Singleton
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

        // primeira cena
        CarregarCena("Splash");
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
            // SPLASH
            case "Splash":

                SceneManager.LoadScene("Splash");

                break;

            // MENU
            case "MenuPrincipal":

                SceneManager.LoadScene("MenuPrincipal");

                MudarEstado(GameState.MenuPrincipal);

                break;

            // GAMEPLAY
            case "SampleScene":

                if (estadoAtual == GameState.MenuPrincipal)
                {
                    // carrega gameplay
                    SceneManager.LoadScene("SampleScene");

                    // carrega GUI junto
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

    // INPUT
    public void AtribuirInput(PlayerInput input)
    {
        playerInput = input;

        Debug.Log("Input atribuído ao jogador");
    }

    
}