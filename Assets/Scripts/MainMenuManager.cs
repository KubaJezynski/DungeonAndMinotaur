using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject trapCanvasPrefab;
    [SerializeField] private GameObject trapsContainer;
    [SerializeField] private GameObject roomTypeCanvasPrefab;
    [SerializeField] private GameObject roomTypesContainer;
    [SerializeField] private Slider safePathLengthSettingSlider;
    [SerializeField] private Text safePathLengthSettingText;
    [SerializeField] private Slider dungeonSizeSettingSlider;
    [SerializeField] private Text dungeonSizeSettingText;
    [SerializeField] private Text roomTypeSettingButtonText;

    public int safePathLength = 1;
    public int dungeonSize = 1;
    public DungeonRoomType.FloorType floorType = DungeonRoomType.FloorType.QUADRANGULAR;
    public List<TrapType.Type> traps = new List<TrapType.Type>();
    private DungeonDataStruct dungeonData;

    void Awake()
    {
        safePathLengthSettingSlider.onValueChanged.AddListener((value) => { safePathLengthSettingText.text = value.ToString(); safePathLength = (int)value; });
        dungeonSizeSettingSlider.onValueChanged.AddListener((value) => { dungeonSizeSettingText.text = value.ToString(); dungeonSize = (int)value; });
        roomTypeSettingButtonText.text = DungeonRoomType.FloorType.QUADRANGULAR.ToString();
        InitializeRoomTypeList();
        InitializeTrapsList();
    }

    private void InitializeRoomTypeList()
    {
        foreach (Enum e in Enum.GetValues(typeof(DungeonRoomType.FloorType)))
        {
            GameObject roomTypeCanvas = Instantiate(this.roomTypeCanvasPrefab);
            Button roomTypeButton = roomTypeCanvas.GetComponentInChildren<Button>();
            Text roomTypeText = roomTypeButton.GetComponentInChildren<Text>();
            roomTypeText.text = Enum.GetName(typeof(DungeonRoomType.FloorType), e);
            roomTypeButton.onClick.AddListener(() => { roomTypeSettingButtonText.text = roomTypeText.text; this.floorType = (DungeonRoomType.FloorType)e; });
            roomTypeCanvas.transform.parent = roomTypesContainer.transform;
        }
    }

    private void InitializeTrapsList()
    {
        foreach (TrapType.Type t in Enum.GetValues(typeof(TrapType.Type)))
        {
            GameObject trapCanvas = Instantiate(this.trapCanvasPrefab);
            Toggle trapToggle = trapCanvas.GetComponentInChildren<Toggle>();
            trapToggle.onValueChanged.AddListener((isOn) => { if (isOn) traps.Add(t); else traps.Remove(t); });
            traps.Add(t);
            Text trapText = trapToggle.GetComponentInChildren<Text>();
            trapText.text = Enum.GetName(typeof(TrapType.Type), t);
            trapCanvas.transform.parent = trapsContainer.transform;
        }
    }

    public void OnClickButton_Start()
    {
        dungeonData = new DungeonDataStruct(this.safePathLength, this.dungeonSize, this.floorType, this.traps);
        GameManager.Instance.onGameStateChanged = (state) =>
        {
            GameManager.Instance.dungeonDataHandler = dungeonData;
            SceneManager.LoadScene("InGame");
            GameManager.Instance.onGameStateChanged = (state) => { };
        };
        GameManager.Instance.State = GameManager.GameState.IN_GAME;
    }

    public void OnClickButton_Exit()
    {
        Application.Quit();
    }

    public void OnClickButton_DisableEnable(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
