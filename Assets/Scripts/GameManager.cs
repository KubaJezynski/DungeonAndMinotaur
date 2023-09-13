using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static readonly GameManager instance = new GameManager();
    public static GameManager Instance { get { return instance; } }

    private GameObject playerPrefab = Resources.Load("Player") as GameObject;

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
                    break;
                case GameState.AFTER_GAME:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            onGameStateChanged?.Invoke(state);
        }
    }

    private Action<GameObject> dungeonCreatedEvent;
    public Action<GameObject> DungeonCreatedEvent
    {
        set
        {
            Debug.Log("playerPrefab = " + playerPrefab);
            dungeonCreatedEvent = value;
            dungeonCreatedEvent.Invoke(playerPrefab);
        }
    }
    public Action<GameState> onGameStateChanged { private get; set; }

    public DungeonDataStruct dungeonDataHandler = new DungeonDataStruct(1, 1, DungeonRoomType.FloorType.QUADRANGULAR, new List<TrapType.Type>());

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
