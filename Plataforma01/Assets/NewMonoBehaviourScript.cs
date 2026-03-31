using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Movimentação")]
    [Tooltip("Força aplicada com AddForce para movimentar a bola")]
    public float speed = 10f;
    [Tooltip("Velocidade máxima no plano XZ")]
    public float maxSpeed = 8f;

    public enum MovementMode { Force, Acceleration, VelocityChange, DirectVelocity, Torque }
    [Tooltip("Modo de aplicação da força/velocidade para controle da bola")]
    public MovementMode movementMode = MovementMode.VelocityChange;
    [Tooltip("Aplicar torque para fazer a bola rolar (útil para jogos roll-a-ball)")]
    public float torqueForce = 5f;

    [Tooltip("Se verdadeiro, o script tentará remover automaticamente travamentos de posição X/Z no Rigidbody (útil se o jogador não se mover)")]
    public bool autoUnfreezeConstraints = true;

    [Tooltip("Mostrar informações de debug na tela")]
    public bool debugUI;

    [Header("Pulo (opcional)")]
    public bool allowJump;
    public float jumpForce = 5f;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no objeto. O script requer um Rigidbody.");
            return;
        }

        // Se estiver configurado para descongelar travamentos de posição X/Z, faça isso automaticamente
        if (autoUnfreezeConstraints)
        {
            // Remover flags de travamento de posição X e Z (mantemos rotação/travas existentes)
            // Fazer a operação em inteiros para evitar warnings com bitwise em enum
            int toClear = (int)RigidbodyConstraints.FreezePositionX | (int)RigidbodyConstraints.FreezePositionZ;
            int cur = (int)_rb.constraints;
            if ((cur & toClear) != 0)
            {
                int updated = cur & ~toClear;
                _rb.constraints = (RigidbodyConstraints)updated;
                Debug.Log("NewMonoBehaviourScript: Removidos travamentos de posição X/Z do Rigidbody para permitir movimento.");
            }
        }
        // Avisos úteis de configuração
        if (_rb.isKinematic)
        {
            Debug.LogWarning("NewMonoBehaviourScript: Rigidbody está marcado como Kinematic — isso previne movimento por forças. Desmarque 'Is Kinematic'.");
        }
        if (!_rb.useGravity)
        {
            Debug.LogWarning("NewMonoBehaviourScript: Rigidbody não possui gravidade habilitada (useGravity = false).");
        }
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("NewMonoBehaviourScript: Nenhum Collider encontrado no objeto. Sem collider, o objeto pode atravessar o chão.");
        }
    }

    // Use FixedUpdate para física
    void FixedUpdate()
    {
        // Eixos padrão "Horizontal" e "Vertical" (teclas WASD / setas)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Fallback: se não houver leitura dos eixos (por ex. novo Input System ativo), tentar capturar teclas WASD/Setas
        if (Mathf.Approximately(h, 0f))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h = 1f;
        }
        if (Mathf.Approximately(v, 0f))
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v = -1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v = 1f;
        }

        Vector3 input = new Vector3(h, 0f, v);

        if (input.sqrMagnitude > 0f)
        {
            Debug.Log($"NewMonoBehaviourScript: Input detectado H={h:F2} V={v:F2}");
            // Fazer o movimento relativo à rotação da câmera, se existir
            Vector3 move = input;
            if (Camera.main != null)
            {
                // Mantemos apenas a orientação Y da câmera
                Quaternion camRot = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
                move = camRot * input;
            }

            // Aplicação do movimento: escolha o modo que melhor funcione no seu projeto
            switch (movementMode)
            {
                case MovementMode.Force:
                    _rb.AddForce(move.normalized * speed, ForceMode.Force);
                    Debug.Log("NewMonoBehaviourScript: Applied Force (Force)");
                    break;
                case MovementMode.Acceleration:
                    _rb.AddForce(move.normalized * speed, ForceMode.Acceleration);
                    Debug.Log("NewMonoBehaviourScript: Applied Force (Acceleration)");
                    break;
                case MovementMode.VelocityChange:
                    _rb.AddForce(move.normalized * speed, ForceMode.VelocityChange);
                    Debug.Log("NewMonoBehaviourScript: Applied Force (VelocityChange)");
                    break;
                case MovementMode.DirectVelocity:
                    // Define diretamente a componente XZ da velocidade (usar maxSpeed como referência)
                    Vector3 target = move.normalized * maxSpeed;
                    _rb.linearVelocity = new Vector3(target.x, _rb.linearVelocity.y, target.z);
                    Debug.Log("NewMonoBehaviourScript: Set Direct Velocity");
                    break;
                case MovementMode.Torque:
                    // Aplicar torque para fazer a bola rolar
                    // Torque é aplicado perpendicular ao vetor de movimento
                    Vector3 localMove = move.normalized;
                    Vector3 torque = new Vector3(-localMove.z, 0f, localMove.x) * torqueForce;
                    _rb.AddTorque(torque, ForceMode.Force);
                    Debug.Log("NewMonoBehaviourScript: Applied Torque");
                    break;
            }
        }

        // Limitar a velocidade horizontal (plano XZ)
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limited = flatVel.normalized * maxSpeed;
            _rb.linearVelocity = new Vector3(limited.x, _rb.linearVelocity.y, limited.z);
        }
    }

    void Update()
    {
        if (allowJump && Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnGUI()
    {
        if (!debugUI) return;

        GUIStyle s = new GUIStyle(GUI.skin.label);
        s.fontSize = 14;
        s.normal.textColor = Color.white;

        Vector3 lv = _rb != null ? _rb.linearVelocity : Vector3.zero;
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label($"Input H: {Input.GetAxis("Horizontal"):F2} V: {Input.GetAxis("Vertical"):F2}", s);
        GUILayout.Label($"LinearVel XZ: {new Vector2(lv.x, lv.z).magnitude:F2}", s);
        GUILayout.Label($"MovementMode: {movementMode}", s);
        if (_rb != null)
        {
            GUILayout.Label($"isKinematic: {_rb.isKinematic}", s);
            GUILayout.Label($"useGravity: {_rb.useGravity}", s);
            GUILayout.Label($"mass: {_rb.mass:F2} linearDamping: {_rb.linearDamping:F2} angularDamping: {_rb.angularDamping:F2}", s);
            GUILayout.Label($"constraints: {_rb.constraints}", s);
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                GUILayout.Label($"Collider: {col.GetType().Name} isTrigger: {col.isTrigger}", s);
            }
        }
        GUILayout.EndArea();
    }

    // Verifica se está próximo ao chão (ajuste o comprimento do raycast conforme o tamanho da bola)
    bool IsGrounded()
    {
        float distance = 0.51f; // supondo bola com raio ~0.5
        return Physics.Raycast(transform.position, Vector3.down, distance);
    }

    // Desenhar gizmo para o raycast de checagem de chão (apenas no editor)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.51f);
    }
}
