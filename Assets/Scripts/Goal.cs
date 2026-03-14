using UnityEngine;

public class Goal : MonoBehaviour
{
    public int playerScored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.state != GameManager.GameState.Playing) return;

        if (collision.gameObject.name == "Ball")
        {
            GameManager.Instance.OnGoalScored(playerScored);
        }
        
    }
}
