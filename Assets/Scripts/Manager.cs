using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float stepDeltaTime = float.MaxValue;
    [SerializeField] private Cell emptyCellPrefab;
    [SerializeField] private SpriteRenderer marker;


    [Header("UI")]
    [SerializeField] private Button resetButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider sizeXSlider;
    [SerializeField] private Slider sizeYSlider;
    [SerializeField] private Toggle useMarkerToggle;


    public float StepDeltaTime => stepDeltaTime;
    public Vector3Int Size => new Vector3Int((int)sizeXSlider.value, (int)sizeYSlider.value, 0);
    public Cell EmptyCellPrefab => emptyCellPrefab;
    public SpriteRenderer Marker => marker;
    public bool UseMarker => useMarkerToggle.isOn;


    public static Manager Instance { get; private set; }
    public Manager()
    {
        if (Instance == null) Instance = this;
        else throw new System.Exception("There is already a Manager object");
    }


    public void OnResetClicked()
    {
        Grid.Instance.ResetAll();
        Grid.Instance.Initiate();
        stepDeltaTime = speedSlider.value;
        resetButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";
    }

    public void OnPauseClicked()
    {
        if (stepDeltaTime == float.MaxValue)
        {
            stepDeltaTime = speedSlider.value;
            pauseButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
        }
        else
        {
            stepDeltaTime = float.MaxValue;
            pauseButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
        }
    }

    public void OnQuitClicked() => Application.Quit();

    public void OnSliderValueChanged()
    {
        if (stepDeltaTime == float.MaxValue) return;
        stepDeltaTime = speedSlider.value;
    }
}
