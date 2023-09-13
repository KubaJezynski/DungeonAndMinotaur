using UnityEngine;
using UnityEngine.UI;

public class AfterGameManager : MonoBehaviour
{
    [SerializeField] private Text endGameText;

    void Awake()
    {
        endGameText.text = GameManager.Instance.match.EndGameText;
    }

    public void OnClickButton_Return()
    {
        GameManager.Instance.State = GameManager.GameState.MAIN_MENU;
    }
}
