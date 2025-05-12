using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EyesUI : MonoBehaviour {

    public Toggle heteroChromiaToggle;
    public GameObject rightEyeTitle;
    public GameObject leftEyeTitle;

    public GameObject rightEyeContainer;
    public GameObject leftEyeContainer;

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
        mainEyeMaterial.SetTexture("_SectorTex", blueEye.texture);
        mainEyeMaterial.SetInt("_SectorialEnabled", 0);

        leftEyeMaterial.SetFloat("_Angle", 1f);
        leftEyeMaterial.SetFloat("_Offset", 0f);
        leftEyeMaterial.SetTexture("_MainTex", blueEye.texture);
        leftEyeMaterial.SetTexture("_SectorTex", blueEye.texture);
        leftEyeMaterial.SetInt("_SectorialEnabled", 0);

    }

    void SliderValueChanged(Slider slider) {

        switch(slider.gameObject.name) {

            case "MainSectoralSlider":
                mainEyeMaterial.SetFloat("_Angle", slider.value);
                break;

            case "LeftSectoralSlider":
                leftEyeMaterial.SetFloat("_Angle", slider.value);
                break;

            case "MainOffsetSlider":
                mainEyeMaterial.SetFloat("_Offset", slider.value);
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
                case "MainEyeColourDropdown":
                    mainEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
                    break;
                case "LeftEyeColourDropdown":
                    leftEyeMaterial.SetTexture("_MainTex", ColourChange(dropdown));
                    break;
                case "MainSectoralColourDropDown":
                    mainEyeMaterial.SetTexture("_SectorTex", ColourChange(dropdown));
                    break;
                case "LeftSectoralColourDropDown":
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
            case 0:
                return blueEye.texture;
            case 1:
                return brownEye.texture;
            case 2:
                return greenEye.texture;
            default:
                Debug.Log("ERROR @ line 157");
                return null;
        }
    }

    void ToggleValueChanged(Toggle toggle) {

        switch(toggle.gameObject.name) {
            case "HeterochromiaToggle":
                //Right Eye
                rightEyeTitle.SetActive(!rightEyeTitle.activeSelf);
                mainEyeSectoralToggle.SetActive(!mainEyeSectoralToggle.activeSelf);

                //LeftEye
                leftEye.SetActive(!leftEye.activeSelf);
                leftEyeTitle.SetActive(!leftEyeTitle.activeSelf);
                leftEyeSectoralToggle.SetActive(!leftEyeSectoralToggle.activeSelf);

                if(heteroChromiaToggle.isOn) {
                    if (mainEyeSectoralToggle.GetComponent<Toggle>().isOn) {
                        mainEyeMaterial.SetInt("_SectorialEnabled", 1);
                    }
                    if (leftEyeSectoralToggle.GetComponent<Toggle>().isOn) {
                        leftEyeMaterial.SetInt("_SectorialEnabled", 1);
                    }

                    leftEyeMaterial.SetTexture("_MainTex", ColourChange(leftEyeColourDropDown.GetComponent<Dropdown>()));

                } else {
                    leftEyeMaterial.SetTexture("_MainTex" ,mainEyeMaterial.GetTexture("_MainTex"));
                    mainEyeMaterial.SetInt("_SectorialEnabled", 0);
                    leftEyeMaterial.SetInt("_SectorialEnabled", 0);
                }

                    break;
            
            case "MainEyeSectoralToggle":
                mainEyeSectoralSettings.SetActive(!mainEyeSectoralSettings.activeSelf);

                if(mainEyeSectoralToggle.GetComponent<Toggle>().isOn) {
                    leftEyeContainer.transform.localPosition = new Vector3(-443.2466f, -63.6f, 0);
                    mainEyeMaterial.SetInt("_SectorialEnabled", 1);
                } 
                else {
                    leftEyeContainer.transform.localPosition = new Vector3(-443.2466f, 36.26899f, 0);
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
