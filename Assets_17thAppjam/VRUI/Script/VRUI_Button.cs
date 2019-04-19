using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum ButtonMode { Click, Filled }

public class VRUI_Button : MonoBehaviour
{
    [SerializeField] private ButtonMode mode = ButtonMode.Click;
    [SerializeField][Range(0.1f, 10f)] private float FullTime = 1f;
    [SerializeField] private Image LoadBar;
    [SerializeField] private Image Panel;
    [SerializeField] private Color Normal;
    [SerializeField] private Color Highlight;
    [SerializeField] private UnityEvent EventMethods;
    
    private bool isCasted = false;

    private void Start()
    {
        if (LoadBar.fillAmount > 0)
            LoadBar.fillAmount = 0;
    }

    private void Update()
    {
        if (isCasted && mode == ButtonMode.Filled) 
        {
            LoadBar.fillAmount += 1 / FullTime * Time.deltaTime;
            if (LoadBar.fillAmount >= 1)
            {
                EventMethods.Invoke();
                StopLoading();
            }
        }
    }

    public void StartLoading()
    {
        isCasted = true;
    }

    public void StopLoading()
    {
        isCasted = false;
        LoadBar.fillAmount = 0;
    }

    public void HighlightOn()
    {
        Panel.color = Highlight;
    }

    public void HighlightOff()
    {
        Panel.color = Normal;
    }

    public void OnClick()
    {
        EventMethods.Invoke();
    }
}
