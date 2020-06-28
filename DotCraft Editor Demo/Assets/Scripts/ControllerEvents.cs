using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerEvents : MonoBehaviour
{
    public static ControllerEvents Instance;
    public Color[] colorList;
    private int colorIndex;
    private void Awake()
    {
        Instance = this;
    }
    public event Action PressedUpButton;
    public event Action PressedDownButton;
    public event Action PressedRightButton;
    public event Action PressedLeftButton;
    public event Action onSave;
    public event Action<Color> onColorChanged;
    public void Up()
    {
        if (PressedUpButton != null)
        {
            PressedUpButton();
        }
    }
    public void Down()
    {
        if (PressedDownButton != null)
        {
            PressedDownButton();
        }
    }
    public void Right()
    {
        if (PressedRightButton != null)
        {
            PressedRightButton();
        }
    }
    public void Left()
    {
        if (PressedLeftButton != null)
        {
            PressedLeftButton();
        }
    }
    public void colorChange()
    {
        Image colorButton = transform.GetChild(transform.childCount - 1).GetComponent<Image>();
        colorIndex++;
        if (colorIndex >= colorList.Length) colorIndex = 0;
        Color targetColor = colorList[colorIndex];
        colorButton.color = targetColor;

        if (onColorChanged != null)
        {
            onColorChanged(targetColor);
        }
    }
    public void SaveGrid()
    {
        if (onSave != null)
        {
            onSave();
        }
    }
}
