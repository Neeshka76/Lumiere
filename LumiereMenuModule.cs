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
        private Text txtColorValueR;
        private Text txtColorValueG;
        private Text txtColorValueB;
        private Text txtLightIntensity;
        private Button btnSpawnLight;
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
            btnColorValueR = menu.GetCustomReference("btn_ColorValueR").GetComponent<Button>();
            btnColorValueG = menu.GetCustomReference("btn_ColorValueG").GetComponent<Button>();
            btnColorValueB = menu.GetCustomReference("btn_ColorValueB").GetComponent<Button>();
            btnLightIntensity = menu.GetCustomReference("btn_LightIntensity").GetComponent<Button>();
            btnSpawnLight = menu.GetCustomReference("btn_SpawnLight").GetComponent<Button>();

            // Add an event listener for buttons
            btnColorValueR.onClick.AddListener(ClickColorRValue);
            btnColorValueG.onClick.AddListener(ClickColorGValue);
            btnColorValueB.onClick.AddListener(ClickColorBValue);
            btnLightIntensity.onClick.AddListener(ClickLightIntensity);
            btnSpawnLight.onClick.AddListener(ClickSpawnLight);

            // Initialization of datas

            lumiereController = GameManager.local.gameObject.AddComponent<LumiereController>();
            lumiereController.data.ColorRValueGetSet = 255f;
            lumiereController.data.ColorGValueGetSet = 255f;
            lumiereController.data.ColorBValueGetSet = 255f;
            lumiereController.data.LightIntensityGetSet = 1f;

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
        public void ClickSpawnLight()
        {
            lumiereController.data.SpawnLight = true;
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

            }
            txtColorValueR.text = lumiereController.data.ColorRValueGetSet.ToString();
            txtColorValueG.text = lumiereController.data.ColorGValueGetSet.ToString();
            txtColorValueB.text = lumiereController.data.ColorBValueGetSet.ToString();
            txtLightIntensity.text = lumiereController.data.LightIntensityGetSet.ToString();
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
