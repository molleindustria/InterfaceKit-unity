using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowsExample : MonoBehaviour
{
    public Toggle checkBox1;
    public Toggle checkBox2;
    public Image bar;
    public ToggleGroup group;

    void Start()
    {

        //Action using delegate function: nothing is set up on the component itself
        //Add listener for when the state of the toggle changes, function is called to check the states
        checkBox1.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged();
        });

        checkBox2.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged();
        });


        checkBox1.isOn = true;
        checkBox1.isOn = false;

    }


    void Update()
    {
        //animating the bar
        bar.fillAmount += Time.deltaTime;
        //making sure the bar amount stays between 0 and 1
        bar.fillAmount = bar.fillAmount % 1;
    }

    //called by the ok button without parameters
    public void Okay()
    {
        print("Okay has been pressed");
    }

    //called by the radio buttons upon click with a "static" parameter inserted in the inspector
    //the parameter is the number of the radio button
    public void Radio(int value)
    {
        print("Radio button number " + value + " value changed");

        //get a list of toggles from the group (each toggle has a "group" field on the inspector)
        IEnumerable<Toggle> toggles = group.ActiveToggles();

        //go through the active ones (for radio buttons it will be always one)
        foreach (Toggle t in toggles)
        {
            print(t.name + ": " + t.isOn);
        }
    }


    //This method is simpler, every time a checkbox changes I read the value
    //I can do this directly because I have the references to both checkboxes
    public void ToggleValueChanged()
    {
        print("Toggle changed");

        if (checkBox1.isOn)
            print("Checkbox 1 is on!");

        if (checkBox2.isOn)
            print("Checkbox 2 is on!");

    }

    //The slider can pass a dynamic float value corresponding to the position
    public void OnSliderValueChanged(float value)
    {
        print("Slider value changed " + value.ToString("0.00"));
    }

    //The input can pass a dynamic string at every input update corresponding to the current text
    public void InputFieldChanged(string s)
    {
        print("Input changed " + s);
    }


}
