using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GameManager.Instance.match.State = Match.MatchState.VICTORY;
            GameManager.Instance.State = GameManager.GameState.AFTER_GAME;
        }
    }
}
