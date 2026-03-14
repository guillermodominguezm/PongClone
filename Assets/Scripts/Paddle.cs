using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    GameManager gm;

    Vector3 startPosition;
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float yBound;

    private void Awake()
    {
        gm = GameManager.Instance;  
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (gm == null || gm.state != GameManager.GameState.Playing) return;

        float input = moveAction.action.ReadValue<float>();
        float movement = input * moveSpeed * Time.deltaTime;
        
        float newY = transform.position.y + movement;
        newY = Mathf.Clamp(newY, -yBound, yBound);

        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }

    public void ResetPos()
    {
        transform.position = startPosition;
    }
}
