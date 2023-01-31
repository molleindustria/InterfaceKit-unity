using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionExample : MonoBehaviour
{
    public int count = 0;

    //you want to import TMPPro and use the TMP_ components not the standard "Text"
    public TMP_Text countField;

    public TMP_Dropdown dropDown;

    public Button decreaseButton;

    public Image colorElement;

    // Start is called before the first frame update
    void Start()
    {
        countField.text = count.ToString();

        //delegate method: add a listener triggered by the click
        decreaseButton.onClick.AddListener(delegate
        {
            Decrease();
        });


        //delegate method: add a listener triggered by a change in a UI element
        dropDown.onValueChanged.AddListener(delegate
        {
            DropDownChanged(dropDown);
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called by the button on click, the call has been set up in the inspector
    public void Increase()
    {
        count++;
        countField.text = count.ToString();
    }

    //Same effect as the above, it's just set up via code at the start
    public void Decrease()
    {
        count--;
        countField.text = count.ToString();
    }

    //called by a specific toggle which passes its value
    public void ToggleA(bool value)
    {
        print("Toggle A changed to " + value);
    }

    public void ToggleB(bool value)
    {
        print("Toggle B changed to " + value);
    }

    //called by two different toggles which pass a value set up in the inspector
    //note: it will be called also when its unchecked
    public void ToggleStatic(string value)
    {
        print("Value is " + value);
    }

    //this is the "delegate" function called by a listener set up at the Start
    //all code, no inspector, useful for dynamically created UI
    public void DropDownChanged(TMP_Dropdown d)
    {
        print("The value of the drop down menu is " + d.value);
        print("The option text is "+ d.options[d.value].text);
    }

    //The slider can pass a dynamic float value corresponding to the position
    public void OnSliderValueChanged(float value)
    {
        //set the hue 0-1 convert to HSV to RGB 
        colorElement.color = Color.HSVToRGB(value, 1, 1);


    }


    public void FullscreenCheck(bool value)
    {
        if (value == true)
            print("Fullscreen on");
        else
            print("Fullscreen off");
    }

    public void SwordOn(string value)
    {
        
            print("SwordCheck! "+value);
    }
}
