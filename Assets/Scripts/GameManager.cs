using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private const int maxScore = 10;
    WaitForSeconds wait = new WaitForSeconds(1.0f);

    public enum GameState 
    { Playing, 
      GoalScored,
      Victory 
    }
    public GameState state = GameState.Playing;

    public static GameManager Instance;

    [Header("GameComponent Refs")]
    [SerializeField] Ball ball;
    [SerializeField] Paddle PaddleLeft;
    [SerializeField] Paddle PaddleRight;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI score1Txt;
    [SerializeField] TextMeshProUGUI score2Txt;

    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap gameplayMap;

    public int scoreP1 = 0;
    public int scoreP2 = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameplayMap = inputActions.FindActionMap("Gameplay");
    }

    private void OnEnable()
    {
        gameplayMap.Enable();
    }

    private void OnDisable()
    {
        gameplayMap.Disable();
    }

    private void Update()
    {
        if(state == GameState.Victory && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void OnGoalScored(int player)
    {
        if (state != GameState.Playing) return;

        state = GameState.GoalScored;

        ball.OnBallPoint();
        UpdateScore(player);

        if(state != GameState.Victory)
            StartCoroutine(OnPoint(player));
    }

    private void UpdateScore(int player)
    {

        if (player == 1) scoreP1 += 2;
        else scoreP2 += 2;

        UpdateScoreUI();

        if(scoreP1 >= maxScore) { OnVictory(1); return; }
        if(scoreP2 >= maxScore) { OnVictory(2); return; }
        
    }

    private void UpdateScoreUI()
    {
        score1Txt.text = scoreP1.ToString();
        score2Txt.text = scoreP2.ToString();
    }

    private void OnVictory(int player)
    {
        state = GameState.Victory;

        if (player == 1) score1Txt.text = "Win";
        else score2Txt.text = "Win";

    }

    IEnumerator OnPoint(int player)
    {
        
        yield return wait;

        ball.Reset();
        ball.Service(player);

        state = GameState.Playing;
    }

    void ResetGame()
    {
        scoreP1 = 0;
        scoreP2 = 0;

        UpdateScoreUI();

        ball.Reset();
        ball.FirstService();

        PaddleLeft.ResetPos();
        PaddleRight.ResetPos();

        state = GameState.Playing;
    }
}
