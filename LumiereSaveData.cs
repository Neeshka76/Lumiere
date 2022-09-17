using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace Lumiere
{
    [Serializable]
    public class LumiereSaveData
    {
        public Vector3 position {get;set;}
        public Quaternion rotation {get;set;}
        public Color color {get;set;}
        public float intensity {get;set;}
        public float range { get; set; }
    }
}
