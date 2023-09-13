using System;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    private GameObject playerPrefab = Resources.Load("Player") as GameObject;
    private List<GameObject> players = new List<GameObject>();
    public DungeonDataStruct dungeonDataHandler = new DungeonDataStruct(1, 1, DungeonRoomType.FloorType.QUADRANGULAR, new List<TrapType.Type>());

    private MatchState state = MatchState.NONE;
    public MatchState State
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
                case MatchState.START:
                    players.Add(dungeonCreatedEvent.Invoke(playerPrefab));
                    break;
                case MatchState.VICTORY:
                    EndGameText = "YOU WON!";
                    break;
                case MatchState.DEFEAT:
                    EndGameText = "YOU LOST!";
                    break;
                case MatchState.END:
                    players.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            onMachStateChanged?.Invoke(state);
        }
    }
    private Func<GameObject, GameObject> dungeonCreatedEvent;
    public Func<GameObject, GameObject> DungeonCreatedEvent
    {
        set
        {
            dungeonCreatedEvent = value;
        }
    }
    private string endGameText = "";
    public string EndGameText
    {
        get
        {
            return endGameText;
        }
        private set
        {
            endGameText = value;
        }
    }

    public Action<MatchState> onMachStateChanged { private get; set; }

    public enum MatchState
    {
        NONE,
        START,
        VICTORY,
        DEFEAT,
        END
    }
}
