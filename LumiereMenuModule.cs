using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        private Button btnPointToLights;
        private Text txtPointToLights;
        public LumiereController lumiereController;
        public LumiereHook lumiereHook;

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
            lumiereController.data.PreviewLight = false;

            lumiereHook = menu.gameObject.AddComponent<LumiereHook>();
            lumiereHook.menu = this;

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
            lumiereController.data.PreviewLight ^= true;
            UpdateDataPageLeft1();
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
                    lumiereController.data.ColorValueRButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Green Value
                if (lumiereController.data.ColorValueGButtonPressedGetSet == true)
                {
                    lumiereController.data.ColorGValueGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    lumiereController.data.ColorValueGButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Blue Value
                if (lumiereController.data.ColorValueBButtonPressedGetSet == true)
                {
                    lumiereController.data.ColorBValueGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    lumiereController.data.ColorValueBButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Blue Value
                if (lumiereController.data.LightIntensityButtonPressedGetSet == true)
                {
                    lumiereController.data.LightIntensityGetSet = lumiereController.data.ValueToAssignedFloatGetSet;
                    lumiereController.data.LightIntensityButtonPressedGetSet = false;
                    lumiereController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                }
                // Assign the Color Blue Value
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
            txtDisableMeshRenderer.text = lumiereController.data.DisableMeshRendererGetSet ? "Enabled" : "Disabled";
            txtPointToLights.text = lumiereController.data.PointToLightsGetSet ? "Enabled" : "Disabled";
            txtPreviewLight.text = lumiereController.data.PreviewLight ? "Enabled" : "Disabled";
        }

        /*public void UpdateDataPageRight1()
        {
            
        }*/

    }

    // Refresh the menu each frame (need optimization)
    public class LumiereHook : MonoBehaviour
    {
        public LumiereMenuModule menu;

        void Update()
        {
            menu.UpdateDataPageLeft1();
            //menu.UpdateDataPageRight1();
        }
    }
}
