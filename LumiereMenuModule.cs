using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

namespace Lumiere
{
    public class LumiereMenuModule : MenuModule
    {
        private Button btnColorValueR;
        private Button btnColorValueG;
        private Button btnColorValueB;
        private Button btnLightIntensity;
        private Button btnLightRange;
        private Text txtColorValueR;
        private Text txtColorValueG;
        private Text txtColorValueB;
        private Text txtLightIntensity;
        private Text txtLightRange;
        private Text txtPreviewLight;
        private Button btnPreviewLight;
        private Button btnSpawnLight;
        private Button btnDisableMeshRenderer;
        private Text txtDisableMeshRenderer;
        private Button btnDisablePreviewAfterSpawn;
        private Text txtDisablePreviewAfterSpawn;
        private Button btnPointToLights;
        private Text txtPointToLights;
        private Text txtDistancePreviewValue;
        private Slider sliderDistancePreview;
        private Button btnStickyLights;
        private Text txtStickyLights;
        private Button btnNegColorValueR;
        private Button btnNegColorValueG;
        private Button btnNegColorValueB;
        private Text txtNegColorValueR;
        private Text txtNegColorValueG;
        private Text txtNegColorValueB;
        private Text txtStickToItemCreature;
        private Button btnStickToItemCreature;
        private Slider sliderRedColor;
        private Slider sliderGreenColor;
        private Slider sliderBlueColor;
        public LumiereController lumiereController;
        public LumiereHook lumiereHook;

        private Button btnSavePlus;
        private Transform ObjSaves;
        private Button btnSave;
        private Button btnLoad;
        private Button btnDespawn;
        private Button btnDelete;
        private Button btnDisableHandles;
        private Text txtDisableHandles;
        private Text txtSaveDescription;
        private List<Button> btnSavesList = new List<Button>();
        private string buttonNameOri = "btn_Save";
        private bool saveRead = false;
        private int nbSaves = 0;

        private Transform ObjLightPositiveColors;
        private Transform ObjLightNegativeColors;

        AsyncOperationHandle<GameObject> handleButtonPrefab = Addressables.LoadAssetAsync<GameObject>("Neeshka.Lumiere.ButtonPrefab");

        // Note on sliders : Put the listener after the initialization of the value !
        public override void Init(MenuData menuData, Menu menu)
        {
            base.Init(menuData, menu);
            // Grab the value from Unity
            txtColorValueR = menu.GetCustomReference("txt_ColorValueR").GetComponent<Text>();
            txtColorValueG = menu.GetCustomReference("txt_ColorValueG").GetComponent<Text>();
            txtColorValueB = menu.GetCustomReference("txt_ColorValueB").GetComponent<Text>();
            txtLightIntensity = menu.GetCustomReference("txt_LightIntensity").GetComponent<Text>();
            txtLightRange = menu.GetCustomReference("txt_LightRange").GetComponent<Text>();
            txtDisableMeshRenderer = menu.GetCustomReference("txt_DisableMeshRenderer").GetComponent<Text>();
            txtPointToLights = menu.GetCustomReference("txt_PointToLights").GetComponent<Text>();
            txtPreviewLight = menu.GetCustomReference("txt_PreviewLight").GetComponent<Text>();
            btnColorValueR = menu.GetCustomReference("btn_ColorValueR").GetComponent<Button>();
            btnColorValueG = menu.GetCustomReference("btn_ColorValueG").GetComponent<Button>();
            btnColorValueB = menu.GetCustomReference("btn_ColorValueB").GetComponent<Button>();
            btnLightIntensity = menu.GetCustomReference("btn_LightIntensity").GetComponent<Button>();
            btnLightRange = menu.GetCustomReference("btn_LightRange").GetComponent<Button>();
            btnSpawnLight = menu.GetCustomReference("btn_SpawnLight").GetComponent<Button>();
            btnDisableMeshRenderer = menu.GetCustomReference("btn_DisableMeshRenderer").GetComponent<Button>();
            btnPointToLights = menu.GetCustomReference("btn_PointToLights").GetComponent<Button>();
            btnPreviewLight = menu.GetCustomReference("btn_PreviewLight").GetComponent<Button>();
            txtDistancePreviewValue = menu.GetCustomReference("txt_DistancePreviewValue").GetComponent<Text>();
            sliderDistancePreview = menu.GetCustomReference("slider_DistancePreview").GetComponent<Slider>();
            btnStickyLights = menu.GetCustomReference("btn_StickyLights").GetComponent<Button>();
            txtStickyLights = menu.GetCustomReference("txt_StickyLights").GetComponent<Text>();
            sliderRedColor = menu.GetCustomReference("slider_RedColor").GetComponent<Slider>();
            sliderGreenColor = menu.GetCustomReference("slider_GreenColor").GetComponent<Slider>();
            sliderBlueColor = menu.GetCustomReference("slider_BlueColor").GetComponent<Slider>();
            btnNegColorValueR = menu.GetCustomReference("btn_NegColorValueR").GetComponent<Button>();
            txtNegColorValueR = menu.GetCustomReference("txt_NegColorValueR").GetComponent<Text>();
            btnNegColorValueG = menu.GetCustomReference("btn_NegColorValueG").GetComponent<Button>();
            txtNegColorValueG = menu.GetCustomReference("txt_NegColorValueG").GetComponent<Text>();
            btnNegColorValueB = menu.GetCustomReference("btn_NegColorValueB").GetComponent<Button>();
            txtNegColorValueB = menu.GetCustomReference("txt_NegColorValueB").GetComponent<Text>();
            txtDisablePreviewAfterSpawn = menu.GetCustomReference("txt_DisablePreviewAfterSpawn").GetComponent<Text>();
            btnDisablePreviewAfterSpawn = menu.GetCustomReference("btn_DisablePreviewAfterSpawn").GetComponent<Button>();
            txtStickToItemCreature = menu.GetCustomReference("txt_StickToItemCreature").GetComponent<Text>();
            btnStickToItemCreature = menu.GetCustomReference("btn_StickToItemCreature").GetComponent<Button>();
            btnSavePlus = menu.GetCustomReference("btn_SavePlus").GetComponent<Button>();
            ObjSaves = menu.GetCustomReference("ObjSaves");
            btnSave = menu.GetCustomReference("btn_Save").GetComponent<Button>();
            btnLoad = menu.GetCustomReference("btn_Load").GetComponent<Button>();
            btnDespawn = menu.GetCustomReference("btn_Despawn").GetComponent<Button>();
            btnDelete = menu.GetCustomReference("btn_Delete").GetComponent<Button>();
            txtSaveDescription = menu.GetCustomReference("txt_SaveDescription").GetComponent<Text>();
            btnDisableHandles = menu.GetCustomReference("btn_DisableHandles").GetComponent<Button>();
            txtDisableHandles = btnDisableHandles.gameObject.GetComponentInChildren<Text>();

            ObjLightPositiveColors = menu.GetCustomReference("ObjLightPositiveColors");
            ObjLightNegativeColors = menu.GetCustomReference("ObjLightNegativeColors");

        // Add an event listener for buttons
        btnColorValueR.onClick.AddListener(ClickColorRValue);
            btnColorValueG.onClick.AddListener(ClickColorGValue);
            btnColorValueB.onClick.AddListener(ClickColorBValue);
            btnLightIntensity.onClick.AddListener(ClickLightIntensity);
            btnLightRange.onClick.AddListener(ClickLightRange);
            btnSpawnLight.onClick.AddListener(ClickSpawnLight);
            btnDisableMeshRenderer.onClick.AddListener(ClickDisableMeshRenderer);
            btnPointToLights.onClick.AddListener(ClickPointToLights);
            btnPreviewLight.onClick.AddListener(ClickPreviewLight);
            btnStickyLights.onClick.AddListener(ClickStickyLights);
            btnNegColorValueR.onClick.AddListener(ClickNegColorR);
            btnNegColorValueG.onClick.AddListener(ClickNegColorG);
            btnNegColorValueB.onClick.AddListener(ClickNegColorB);
            btnDisablePreviewAfterSpawn.onClick.AddListener(ClickDisablePreviewAfterSpawn);
            btnStickToItemCreature.onClick.AddListener(ClickStickToItemOrCreature);
            btnDisableHandles.onClick.AddListener(ClickDisableHandles);

            btnSavePlus.onClick.AddListener(ClickSavePlus);
            btnSave.onClick.AddListener(ClickSave);
            btnLoad.onClick.AddListener(ClickLoad);
            btnDespawn.onClick.AddListener(ClickDespawn);
            btnDelete.onClick.AddListener(ClickDelete);



            // Initialization of sliders
            sliderDistancePreview.wholeNumbers = false;
            sliderDistancePreview.minValue = 0.5f;
            sliderDistancePreview.maxValue = 5f;
            sliderDistancePreview.value = sliderDistancePreview.minValue;
            sliderRedColor.wholeNumbers = true;
            sliderRedColor.minValue = 0;
            sliderRedColor.maxValue = 255f;
            sliderRedColor.value = sliderRedColor.maxValue;
            sliderGreenColor.wholeNumbers = true;
            sliderGreenColor.minValue = 0;
            sliderGreenColor.maxValue = 255f;
            sliderGreenColor.value = sliderGreenColor.maxValue;
            sliderBlueColor.wholeNumbers = true;
            sliderBlueColor.minValue = 0;
            sliderBlueColor.maxValue = 255f;
            sliderBlueColor.value = sliderBlueColor.maxValue;
            // Add an event listener for sliders
            sliderDistancePreview.onValueChanged.AddListener(delegate { ValueChangedSliderDistancePreview(); });
            sliderRedColor.onValueChanged.AddListener(delegate { ValueChangedSliderRedColor(); });
            sliderGreenColor.onValueChanged.AddListener(delegate { ValueChangedSliderGreenColor(); });
            sliderBlueColor.onValueChanged.AddListener(delegate { ValueChangedSliderBlueColor(); });

            // Initialization of datas


            lumiereController = GameManager.local.gameObject.AddComponent<LumiereController>();
            lumiereController.data.ColorRValueGetSet = 255f;
            lumiereController.data.ColorGValueGetSet = 255f;
            lumiereController.data.ColorBValueGetSet = 255f;
            lumiereController.data.LightIntensityGetSet = 1f;
            lumiereController.data.LightRangeGetSet = 10f;
            lumiereController.data.DisableMeshRendererGetSet = false;
            lumiereController.data.PointToLightsGetSet = false;
            lumiereController.data.SpawnLight = false;
            lumiereController.data.PreviewLightGetSet = false;
            lumiereController.data.SliderDistancePreviewValueGetSet = 0.5f;
            lumiereController.data.IsStickyGetSet = false;
            lumiereController.data.NegColorRGetSet = true;
            lumiereController.data.NegColorGGetSet = true;
            lumiereController.data.NegColorBGetSet = true;
            lumiereController.data.DisablePreviewAfterSpawnGetSet = true;
            lumiereController.data.StickToItemWhenFalseCreatureTrueGetSet = false;
            lumiereController.data.DisableHandlesGetSet = false;

            lumiereHook = menu.gameObject.AddComponent<LumiereHook>();
            lumiereHook.menu = this;

            for(int i = 1; i <= Enum.GetNames(typeof(LumiereColor.LightPositiveColors)).Length ; i++)
            {
                AddColorButton(i);
                AddColorButton(i, false);
            }

            // Update all the Data for left page (text, visibility of buttons etc...)
            UpdateDataPageLeft1();
            // Update all the Data for right page (text, visibility of buttons etc...)
            //UpdateDataPageRight1();

        }

        public void ClickColorRValue()
        {
            lumiereController.data.ColorValueRButtonPressedGetSet = true;
            lumiereController.data.ValueToAssignIsFloat = true;
            UpdateDataPageLeft1();
        }
        public void ClickColorGValue()
        {
            lumiereController.data.ColorValueGButtonPressedGetSet = true;
            lumiereController.data.ValueToAssignIsFloat = true;
            UpdateDataPageLeft1();
        }
        public void ClickColorBValue()
        {
            lumiereController.data.ColorValueBButtonPressedGetSet = true;
            lumiereController.data.ValueToAssignIsFloat = true;
            UpdateDataPageLeft1();
        }
        public void ClickLightIntensity()
        {
            lumiereController.data.LightIntensityButtonPressedGetSet = true;
            lumiereController.data.ValueToAssignIsFloat = true;
            UpdateDataPageLeft1();
        }
        public void ClickLightRange()
        {
            lumiereController.data.LightRangeButtonPressedGetSet = true;
            lumiereController.data.ValueToAssignIsFloat = true;
            UpdateDataPageLeft1();
        }
        public void ClickSpawnLight()
        {
            lumiereController.data.SpawnLight = true;
            UpdateDataPageLeft1();
        }
        public void ClickDisableMeshRenderer()
        {
            lumiereController.data.DisableMeshRendererGetSet ^= true;
            UpdateDataPageLeft1();
        }

        public void ClickPointToLights()
        {
            lumiereController.data.PointToLightsGetSet ^= true;
            UpdateDataPageLeft1();
        }
        public void ClickPreviewLight()
        {
            lumiereController.data.PreviewLightGetSet ^= true;
            UpdateDataPageLeft1();
        }

        public void ValueChangedSliderDistancePreview()
        {
            lumiereController.data.SliderDistancePreviewValueGetSet = sliderDistancePreview.value;
            UpdateDataPageLeft1();
        }
        public void ValueChangedSliderRedColor()
        {
            lumiereController.data.ColorRValueGetSet = sliderRedColor.value;
            UpdateDataPageLeft1();
        }
        public void ValueChangedSliderGreenColor()
        {
            lumiereController.data.ColorGValueGetSet = sliderGreenColor.value;
            UpdateDataPageLeft1();
        }
        public void ValueChangedSliderBlueColor()
        {
            lumiereController.data.ColorBValueGetSet = sliderBlueColor.value;
            UpdateDataPageLeft1();
        }

        public void ClickStickyLights()
        {
            lumiereController.data.IsStickyGetSet ^= true;
            UpdateDataPageLeft1();
        }

        public void ClickNegColorR()
        {
            lumiereController.data.NegColorRGetSet ^= true;
            if (lumiereController.data.NegColorRGetSet)
            {
                sliderRedColor.minValue = 0f;
                sliderRedColor.maxValue = 255f;
                sliderRedColor.value = 0 - sliderRedColor.value;
            }
            else
            {
                sliderRedColor.minValue = -255f;
                sliderRedColor.maxValue = 0f;
                sliderRedColor.value = 0 - sliderRedColor.value;
            }
            UpdateDataPageLeft1();
        }
        public void ClickNegColorG()
        {
            lumiereController.data.NegColorGGetSet ^= true;
            if (lumiereController.data.NegColorGGetSet)
            {
                sliderGreenColor.minValue = 0f;
                sliderGreenColor.maxValue = 255f;
                sliderGreenColor.value = 0 - sliderGreenColor.value;
            }
            else
            {
                sliderGreenColor.minValue = -255f;
                sliderGreenColor.maxValue = 0f;
                sliderGreenColor.value = 0 - sliderGreenColor.value;
            }
            UpdateDataPageLeft1();
        }
        public void ClickNegColorB()
        {
            lumiereController.data.NegColorBGetSet ^= true;
            if (lumiereController.data.NegColorBGetSet)
            {
                sliderBlueColor.minValue = 0f;
                sliderBlueColor.maxValue = 255f;
                sliderBlueColor.value = 0 - sliderBlueColor.value;
            }
            else
            {
                sliderBlueColor.minValue = -255f;
                sliderBlueColor.maxValue = 0f;
                sliderBlueColor.value = 0 - sliderBlueColor.value;
            }
            UpdateDataPageLeft1();
        }

        public void ClickDisablePreviewAfterSpawn()
        {
            lumiereController.data.DisablePreviewAfterSpawnGetSet ^= true;
            UpdateDataPageLeft1();
        }
        public void ClickStickToItemOrCreature()
        {
            //lumiereController.data.StickToItemFalseCreatureTrueGetSet ^= true;
            UpdateDataPageLeft1();
        }

        public void ClickDisableHandles()
        {
            lumiereController.data.DisableHandlesGetSet ^= true;
        }

        public void ClickSavePlus()
        {
            if (!lumiereController.data.SavePlusPressedGetSet)
            {
                nbSaves = CheckSaves();
                AddSaveButton(nbSaves + 1, true);
                btnSavePlus.gameObject.SetActive(false);
                lumiereController.data.SavePlusPressedGetSet = true;
                lumiereController.data.ValueButtonSaveSelectedGetSet = 0;
            }
        }
        public void ClickSave()
        {
            //lumiereController.data.SavePressedGetSet ^= true;
            if (lumiereController.data.ValueButtonSaveSelectedGetSet != 0)
            {
                LumiereLevelModule.SaveDatas(lumiereController.data.ValueButtonSaveSelectedGetSet);
                lumiereController.data.nbLightInLevel = LumiereLevelModule.returnNbLightInLevel();
                if (lumiereController.data.nbLightInLevel != 0)
                {
                    btnSavePlus.gameObject.SetActive(true);
                    lumiereController.data.SavePlusPressedGetSet = false;
                    btnSavesList[lumiereController.data.ValueButtonSaveSelectedGetSet - 1].gameObject.GetComponentInChildren<Outline>().effectColor = Color.white;
                    lumiereController.data.ValueButtonSaveSelectedGetSet = 0;
                }
            }
        }
        public void ClickLoad()
        {
            lumiereController.data.LoadPressedGetSet ^= true;
            if (lumiereController.data.ValueButtonSaveSelectedGetSet != 0)
            {
                LumiereLevelModule.LoadDatas(lumiereController.data.ValueButtonSaveSelectedGetSet);
                lumiereController.data.ValueButtonSaveSelectedGetSet = 0;
            }
        }
        public void ClickDespawn()
        {
            LumiereLevelModule.DespawnAllLights();
        }
        public void ClickDelete()
        {
            if (lumiereController.data.ValueButtonSaveSelectedGetSet != 0)
            {
                RemoveSaveButton(lumiereController.data.ValueButtonSaveSelectedGetSet);
                if (CheckSaves() >= lumiereController.data.ValueButtonSaveSelectedGetSet)
                {
                    LumiereLevelModule.DeleteDatas(lumiereController.data.ValueButtonSaveSelectedGetSet);
                    LumiereLevelModule.SortDatas(lumiereController.data.ValueButtonSaveSelectedGetSet);
                }
                else
                {
                    btnSavePlus.gameObject.SetActive(true);
                    lumiereController.data.SavePlusPressedGetSet = false;
                }
                lumiereController.data.ValueButtonSaveSelectedGetSet = 0;
            }
        }

        public void UpdateDataPageLeft1()
        {
            ///
            ///Example
            ///
            // Assign
            // Color Red Value
            // Color Green Value
            // Color Blue Value
            // Light Intensity Value
            // Light Range Value
            // value when the enter button is pressed
            if (lumiereController.data.KeyboardFinishEnterButtonPressedGetSet == true)
            {
                // Assign the Color Red Value
                if (lumiereController.data.ColorValueRButtonPressedGetSet == true)
                {
                    lumiereController.data.ColorRValueGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    sliderRedColor.value = Mathf.CeilToInt(lumiereController.data.ValueToAssignedFloatGetSet);
                    lumiereController.data.ColorValueRButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Green Value
                if (lumiereController.data.ColorValueGButtonPressedGetSet == true)
                {
                    lumiereController.data.ColorGValueGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    sliderGreenColor.value = Mathf.CeilToInt(lumiereController.data.ValueToAssignedFloatGetSet);
                    lumiereController.data.ColorValueGButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Blue Value
                if (lumiereController.data.ColorValueBButtonPressedGetSet == true)
                {
                    lumiereController.data.ColorBValueGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    sliderBlueColor.value = Mathf.CeilToInt(lumiereController.data.ValueToAssignedFloatGetSet);
                    lumiereController.data.ColorValueBButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Light Intensity Value
                if (lumiereController.data.LightIntensityButtonPressedGetSet == true)
                {
                    lumiereController.data.LightIntensityGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    lumiereController.data.LightIntensityButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Light Range Value
                if (lumiereController.data.LightRangeButtonPressedGetSet == true)
                {
                    lumiereController.data.LightRangeGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    lumiereController.data.LightRangeButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }

            }
            txtColorValueR.text = lumiereController.data.ColorRValueGetSet.ToString();
            txtColorValueG.text = lumiereController.data.ColorGValueGetSet.ToString();
            txtColorValueB.text = lumiereController.data.ColorBValueGetSet.ToString();
            txtLightIntensity.text = lumiereController.data.LightIntensityGetSet.ToString();
            txtLightRange.text = lumiereController.data.LightRangeGetSet.ToString();
            txtDistancePreviewValue.text = lumiereController.data.SliderDistancePreviewValueGetSet.ToString("0.00");
            txtDisableMeshRenderer.text = lumiereController.data.DisableMeshRendererGetSet ? "Enabled" : "Disabled";
            txtPointToLights.text = lumiereController.data.PointToLightsGetSet ? "Enabled" : "Disabled";
            txtPreviewLight.text = lumiereController.data.PreviewLightGetSet ? "Enabled" : "Disabled";
            txtDisablePreviewAfterSpawn.text = lumiereController.data.DisablePreviewAfterSpawnGetSet ? "Enabled" : "Disabled";
            txtStickToItemCreature.text = lumiereController.data.StickToItemWhenFalseCreatureTrueGetSet ? "Creature" : "Item";
            txtStickyLights.text = lumiereController.data.IsStickyGetSet ? "Enabled" : "Disabled";
            txtNegColorValueR.text = lumiereController.data.NegColorRGetSet ? "+" : "-";
            txtNegColorValueG.text = lumiereController.data.NegColorGGetSet ? "+" : "-";
            txtNegColorValueB.text = lumiereController.data.NegColorBGetSet ? "+" : "-";
        }

        public void UpdateDataPageRight1()
        {
            if (lumiereController != null)
            {
                if (lumiereController.data.levelLoadedGetSet && !saveRead)
                {
                    nbSaves = CheckSaves();
                    for (int i = 1; i <= nbSaves; i++)
                    {
                        AddSaveButton(i);
                    }
                    saveRead = true;
                }
                if (!lumiereController.data.levelLoadedGetSet && saveRead)
                {
                    for (int i = btnSavesList.Count - 1; i >= 0; i--)
                    {
                        UnityEngine.Object.Destroy(btnSavesList[i].gameObject);
                        btnSavesList.RemoveAt(i);
                    }
                    lumiereController.data.ValueButtonSaveSelectedGetSet = 0;
                    saveRead = false;
                }
            }
            if (lumiereController.data.ValueButtonSaveSelectedGetSet == 0)
                txtSaveDescription.text = "Save Description";
            txtDisableHandles.text = lumiereController.data.DisableHandlesGetSet ? "Disabled" : "Enabled";
        }

        private int CheckSaves()
        {
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = lumiereController.data.levelNameGetSet;
            HashSet<string> paths = Directory.EnumerateFiles(pathToFolder, title + "*.json").ToHashSet();
            if (!Directory.Exists(pathToFolder))
            {
                if (!Directory.Exists(pathToFolder))
                    Debug.Log($"Lumiere : No folder at : {pathToFolder}");
                return 0;
            }
            return paths.Count();
        }

        public void AddSaveButton(int nbButton, bool newSaveTrue = false)
        {
            GameObject button = handleButtonPrefab.WaitForCompletion();
            button = UnityEngine.Object.Instantiate(button);
            button.transform.SetParent(ObjSaves, false);
            button.name = buttonNameOri + nbButton;
            button.gameObject.GetComponentInChildren<Text>().text = "Save" + nbButton;
            if (newSaveTrue)
                button.gameObject.GetComponentInChildren<Outline>().effectColor = Color.red;
            Button newbuttonSave = button.GetComponent<Button>();
            newbuttonSave.onClick.AddListener(() =>
            {
                lumiereController.data.ValueButtonSaveSelectedGetSet = int.Parse(newbuttonSave.gameObject.name.Substring(buttonNameOri.Length));
                txtSaveDescription.text = "Save " + lumiereController.data.ValueButtonSaveSelectedGetSet;
            });
            btnSavesList.Add(newbuttonSave);
            btnSavePlus.transform.SetAsLastSibling();
        }

        public void RemoveSaveButton(int nbButton)
        {
            Debug.Log($"Lumiere : Remove Save : {nbButton}");
            Debug.Log($"Lumiere : Remove Save : {btnSavesList.Count}");
            for (int i = btnSavesList.Count - 1; i >= 0; i--)
            {
                if (i == nbButton - 1)
                {
                    UnityEngine.Object.Destroy(btnSavesList[i].gameObject);
                    btnSavesList.RemoveAt(i);
                }
                if (nbButton - 1 < i)
                {
                    btnSavesList[i].gameObject.GetComponentInChildren<Text>().text = "Save" + (i);
                    btnSavesList[i].gameObject.name = buttonNameOri + i;
                }
            }
        }

        public void AddColorButton(int nbButton, bool positiveColor = true)
        {
            GameObject button = handleButtonPrefab.WaitForCompletion();
            button = UnityEngine.Object.Instantiate(button);
            if (positiveColor)
            {
                button.transform.SetParent(ObjLightPositiveColors, false);
            }
            else
            {
                button.transform.SetParent(ObjLightNegativeColors, false);
            }
            button.name = buttonNameOri + nbButton;
            string text = Enum.GetName(typeof(LumiereColor.LightPositiveColors), nbButton);
            button.gameObject.GetComponentInChildren<Text>().text = text;
            Button newbuttonColor = button.GetComponent<Button>();
            newbuttonColor.onClick.AddListener(() =>
            {
                LumiereColorButton colorButton = new LumiereColorButton((LumiereColor.LightPositiveColors)Enum.Parse(typeof(LumiereColor.LightPositiveColors), text), positiveColor);
                if(positiveColor)
                {
                    if (!lumiereController.data.NegColorRGetSet || !lumiereController.data.NegColorGGetSet || !lumiereController.data.NegColorBGetSet)
                    {
                        sliderRedColor.minValue = 0f;
                        sliderRedColor.maxValue = 255f;
                        sliderGreenColor.minValue = 0f;
                        sliderGreenColor.maxValue = 255f;
                        sliderBlueColor.minValue = 0f;
                        sliderBlueColor.maxValue = 255f;
                        lumiereController.data.NegColorRGetSet = true;
                        lumiereController.data.NegColorGGetSet = true;
                        lumiereController.data.NegColorBGetSet = true;
                    }
                }
                else
                {
                    if (lumiereController.data.NegColorRGetSet || lumiereController.data.NegColorGGetSet || lumiereController.data.NegColorBGetSet)
                    {
                        sliderRedColor.minValue = -255f;
                        sliderRedColor.maxValue = 0f;
                        sliderGreenColor.minValue = -255f;
                        sliderGreenColor.maxValue = 0f;
                        sliderBlueColor.minValue = -255f;
                        sliderBlueColor.maxValue = 0f;
                        lumiereController.data.NegColorRGetSet = false;
                        lumiereController.data.NegColorGGetSet = false;
                        lumiereController.data.NegColorBGetSet = false;
                    }
                }
                sliderRedColor.value = colorButton.RValue();
                sliderGreenColor.value = colorButton.GValue();
                sliderBlueColor.value = colorButton.BValue();
                lumiereController.data.ColorRValueGetSet = colorButton.RValue();
                lumiereController.data.ColorGValueGetSet = colorButton.GValue();
                lumiereController.data.ColorBValueGetSet = colorButton.BValue();

            });
        }

    }

    // Refresh the menu each frame (need optimization)
    public class LumiereHook : MonoBehaviour
    {
        public LumiereMenuModule menu;

        void Update()
        {
            menu.UpdateDataPageLeft1();
            menu.UpdateDataPageRight1();
        }
    }
}
