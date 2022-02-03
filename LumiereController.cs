﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumiere
{

    public class LumiereData
    {
        public float ColorRValueGetSet { get; set; }
        public float ColorGValueGetSet { get; set; }
        public float ColorBValueGetSet { get; set; }
        public float LightIntensityGetSet { get; set; }

        public bool holdingALightRightHand { get; set; }
        public bool holdingALightLeftHand { get; set; }


        // Set if Player has pressed the button Color Red
        public bool ColorValueRButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Color Green
        public bool ColorValueGButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Color Blue
        public bool ColorValueBButtonPressedGetSet { get; set; }
        // Set if Player has pressed the button Intensity Light
        public bool LightIntensityButtonPressedGetSet { get; set; }

        public bool SpawnLight { get; set; }

        // Set if Keyboard has pressed enter button finish
        public bool KeyboardFinishEnterButtonPressedGetSet { get; set; }
        // Set if the value to assign is a int
        public bool ValueToAssignIsInt { get; set; }
        // Set if the value to assign is a float
        public bool ValueToAssignIsFloat { get; set; }
        // Set if the value to assign is uint
        public bool ValueToAssignIsUint { get; set; }
        // Value to pass from the Keyboard in Uint
        public uint ValueToAssignedUintGetSet { get; set; }
        // Value to pass from the Keyboard in Int
        public int ValueToAssignedIntGetSet { get; set; }
        // Value to pass from the Keyboard in float
        public float ValueToAssignedFloatGetSet { get; set; }
    }
    public class LumiereController : MonoBehaviour
    {
        public LumiereData data = new LumiereData();
    }
}
