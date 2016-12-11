using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class SoundsSlider : MonoBehaviour, IPointerDownHandler// required interface when using the OnPointerDown method.
{
    public Slider slider;
    public Text textOff;
    public Text textOn;

    void Start()
    {
        if (slider.value == slider.maxValue)
        {
            slider.value = slider.minValue;
            textOff.gameObject.SetActive(true);
            textOn.gameObject.SetActive(false);
        }
        else if (slider.value == slider.minValue)
        {
            slider.value = slider.maxValue;
            textOff.gameObject.SetActive(false);
            textOn.gameObject.SetActive(true);
        }
    }

    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (slider.value == slider.maxValue)
        {
            slider.value = slider.minValue;
            textOff.gameObject.SetActive(true);
            textOn.gameObject.SetActive(false);
        }
        else if (slider.value == slider.minValue)
        {
            slider.value = slider.maxValue;
            textOff.gameObject.SetActive(false);
            textOn.gameObject.SetActive(true);
        }
    }
}