﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EyesUI : MonoBehaviour {

    public Toggle heteroChromiaToggle;
    public GameObject rightEyeTitle;

    public GameObject mainEye;
    public GameObject mainEyeSectoralToggle;
    public GameObject mainEyeSectoralSettings;
    public GameObject mainEyeColourDropDown;
    public GameObject mainEyeSectoralColourDropDown;
    public GameObject mainEyeSprite;
    public GameObject mainEyeSectoralSlider;
    public Material mainEyeMaterial;
    public GameObject mainEyeSectoralOffsetSlider;
    
    public GameObject leftEye;
    public GameObject leftEyeSectoralToggle;
    public GameObject leftEyeSectoralSettings;
    public GameObject leftEyeColourDropDown;
    public GameObject leftEyeSectoralColourDropDown;
    public GameObject leftEyeSprite;
    public GameObject leftEyeSectoralSlider;
    public Material leftEyeMaterial;
    public GameObject leftEyeSectoralOffsetSlider;

    public Sprite blueEye;
    public Sprite greenEye;
    public Sprite brownEye;

    // Start is called before the first frame update
    void Start() {

        heteroChromiaToggle.onValueChanged.AddListener(delegate {
             ToggleValueChanged(heteroChromiaToggle);
        });

        mainEyeSectoralToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
             ToggleValueChanged(mainEyeSectoralToggle.GetComponent<Toggle>());
        });

        leftEyeSectoralToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
             ToggleValueChanged(leftEyeSectoralToggle.GetComponent<Toggle>());
        });

        mainEyeColourDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(mainEyeColourDropDown.GetComponent<Dropdown>());
        });

        leftEyeColourDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(leftEyeColourDropDown.GetComponent<Dropdown>());
        });

        mainEyeSectoralColourDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(mainEyeSectoralColourDropDown.GetComponent<Dropdown>());
        });

        leftEyeSectoralColourDropDown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(leftEyeSectoralColourDropDown.GetComponent<Dropdown>());
        });
        mainEyeSectoralSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            SliderValueChanged(mainEyeSectoralSlider.GetComponent<Slider>());
        });

        leftEyeSectoralSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            SliderValueChanged(leftEyeSectoralSlider.GetComponent<Slider>());
        });

        mainEyeSectoralOffsetSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            SliderValueChanged(mainEyeSectoralOffsetSlider.GetComponent<Slider>());
        });

        leftEyeSectoralOffsetSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            SliderValueChanged(leftEyeSectoralOffsetSlider.GetComponent<Slider>());
        });

        mainEyeMaterial.SetFloat("_Angle", 1f);
        mainEyeMaterial.SetFloat("_Offset", 0f);
        mainEyeMaterial.SetTexture("_MainTex", blueEye.texture);
        mainEyeMaterial.SetInt("_SectorialEnabled", 0);

        leftEyeMaterial.SetFloat("_Angle", 1f);
        leftEyeMaterial.SetFloat("_Offset", 0f);
        leftEyeMaterial.SetTexture("_MainTex", blueEye.texture);
        leftEyeMaterial.SetInt("_SectorialEnabled", 0);

    }

    void SliderValueChanged(Slider slider) {

        switch(slider.gameObject.name) {

            case "MainSectoralSlider":
                mainEyeMaterial.SetFloat("_Angle", slider.value);

                Debug.Log(
                    "Angle: " + mainEyeMaterial.GetFloat("_Angle") + "\n" +
                    "OffSet: " + mainEyeMaterial.GetFloat("_Offset") + "\n" +
                    "SectorStart: " + mainEyeMaterial.GetFloat("_SectorStart") + "\n" +
                    "SectorEnd: " + mainEyeMaterial.GetFloat("_SectorEnd") + "\n"
                    );

                break;

            case "LeftSectoralSlider":
                leftEyeMaterial.SetFloat("_Angle", slider.value);
                break;

            case "MainOffsetSlider":
                mainEyeMaterial.SetFloat("_Offset", slider.value);

                Debug.Log(
                    "Angle: " + mainEyeMaterial.GetFloat("_Angle") + "\n" +
                    "OffSet: " + mainEyeMaterial.GetFloat("_Offset") + "\n" +
                    "SectorStart: " + mainEyeMaterial.GetFloat("_SectorStart") + "\n" +
                    "SectorEnd: " + mainEyeMaterial.GetFloat("_SectorEnd") + "\n"
                    );

                break;

            case "LeftOffsetSlider":
                leftEyeMaterial.SetFloat("_Offset", slider.value);
                break;
        }
    }

    void DropdownValueChanged(Dropdown dropdown) {

        Debug.Log(dropdown.gameObject.name);

        if(heteroChromiaToggle.isOn) {
            switch(dropdown.gameObject.name) {
                case "MainEye":
                    mainEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
                    break;
                case "LeftEye":
                    leftEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
                    break;
                case "MainSectoralColourDropDown":
                    Debug.Log("Main Here");
                    mainEyeMaterial.SetTexture("_SectorTex", ColourChange(dropdown));
                    break;
                case "LeftSectoralColourDropDown":
                    Debug.Log("Left Here");
                    leftEyeMaterial.SetTexture("_SectorTex", ColourChange(dropdown));
                    break;
            }
        }
        else {
            mainEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
            leftEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
        }
    }

    Texture ColourChange(Dropdown dropdown) {

        switch(dropdown.value) {
            case 1:
                return blueEye.texture;
            case 2:
                return brownEye.texture;
            case 3:
                return greenEye.texture;
            default:
                Debug.Log("ERROR @ line 172");
                return null;
        }
    }

    void ToggleValueChanged(Toggle toggle) {

        switch(toggle.gameObject.name) {
            case "HeterochromiaToggle":
                rightEyeTitle.SetActive(!rightEyeTitle.activeSelf);
                leftEye.SetActive(!leftEye.activeSelf);
                mainEyeSectoralToggle.SetActive(!mainEyeSectoralToggle.activeSelf);
                leftEyeSectoralToggle.SetActive(!leftEyeSectoralToggle.activeSelf);

                if (mainEyeSectoralSettings.activeSelf && !heteroChromiaToggle.isOn) {
                    mainEyeSectoralSettings.SetActive(false);
                } else if (mainEyeSectoralToggle.GetComponent<Toggle>().isOn && heteroChromiaToggle.isOn) {
                    mainEyeSectoralSettings.SetActive(true);
                }
                break;
            
            case "MainEyeSectoralToggle":
                mainEyeSectoralSettings.SetActive(!mainEyeSectoralSettings.activeSelf);

                if(mainEyeSectoralToggle.GetComponent<Toggle>().isOn) {
                    leftEye.transform.localPosition = new Vector3(0, -63.6f, 0);
                    mainEyeMaterial.SetInt("_SectorialEnabled", 1);
                } 
                else {
                    leftEye.transform.localPosition = new Vector3(0, 55, 0);
                    mainEyeMaterial.SetInt("_SectorialEnabled", 0);
                }
                break;

            case "LeftEyeSectoralToggle":
                leftEyeSectoralSettings.SetActive(!leftEyeSectoralSettings.activeSelf);

                if (leftEyeSectoralToggle.GetComponent<Toggle>().isOn) {
                    leftEyeMaterial.SetInt("_SectorialEnabled", 1);
                } else {
                    leftEyeMaterial.SetInt("_SectorialEnabled", 0);
                }
                break;
        }

    }
}
