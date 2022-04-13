using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
[UnityEngine.AddComponentMenu("Input/Custom On-Screen Button")]

[UnityEngine.RequireComponent(typeof(Image))]
public class CustomOnScreenButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        SendValueToControl(0.0f);
        this.image.sprite = this.buttonSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SendValueToControl(1.0f);
        this.image.sprite = this.buttonSpritePressed;

    }

    public void Awake()
    {
        this.image = GetComponent<Image>();
    }



    ////TODO: pressure support
    /*
    /// <summary>
    /// If true, the button's value is driven from the pressure value of touch or pen input.
    /// </summary>
    /// <remarks>
    /// This essentially allows having trigger-like buttons as on-screen controls.
    /// </remarks>
    [SerializeField] private bool m_UsePressure;
    */
    public UnityEngine.Sprite buttonSprite;
    public UnityEngine.Sprite buttonSpritePressed;
    private Image image;

    [InputControl(layout = "Button")]
    [UnityEngine.SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}