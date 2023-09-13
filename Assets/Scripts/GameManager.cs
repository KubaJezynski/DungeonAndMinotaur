using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static readonly GameManager instance = new GameManager();
    public static GameManager Instance { get { return instance; } }

    public Match match = new Match();

    private GameState state = GameState.MAIN_MENU;
    public GameState State
    {
        set
        {
            if (state == value)
            {
                return;
            }

            state = value;

            switch (state)
            {
                case GameState.MAIN_MENU:
                    break;
                case GameState.IN_GAME:
                    SceneManager.LoadScene("InGame");
                    break;
                case GameState.AFTER_GAME:
                    SceneManager.LoadScene("AfterGame");
                    match.State = Match.MatchState.END;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            onGameStateChanged?.Invoke(state);
        }
    }

    public Action<GameState> onGameStateChanged { private get; set; }

    private GameManager()
    {

    }

    public enum GameState
    {
        MAIN_MENU,
        IN_GAME,
        AFTER_GAME
    }
}
