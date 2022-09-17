using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace Lumiere
{
    public class LumiereItemBase : MonoBehaviour
    {
        public Item itemLumiere { get; protected set; }
        public Light light;
        protected bool disableMesh = false;
        public LumiereController lumiereController;
        protected float massOri = 0f;
        protected float dragOri = 0f;
        protected float angularDragOri = 0f;

        public virtual void Awake()
        {
            itemLumiere = GetComponent<Item>();
            lumiereController = GameManager.local.gameObject.GetComponent<LumiereController>();
            light = itemLumiere.gameObject.GetComponentInChildren<Light>();
            massOri = itemLumiere.rb.mass;
            dragOri = itemLumiere.rb.drag;
            angularDragOri = itemLumiere.rb.angularDrag;
        }

        public virtual void Init(bool useBook = true, LumiereSaveData saveData = null)
        {
            if(useBook)
            {
                light.color = new Color(lumiereController.data.ColorRValueGetSet, lumiereController.data.ColorGValueGetSet, lumiereController.data.ColorBValueGetSet) / 255f;
                light.intensity = lumiereController.data.LightIntensityGetSet;
                light.range = lumiereController.data.LightRangeGetSet;
            }
            else
            {
                light.color = saveData.color;
                light.intensity = saveData.intensity;
                light.range = saveData.range;
            }
        }

        public virtual void Update()
        {
            if (disableMesh != lumiereController.data.DisableMeshRendererGetSet)
            {
                if (lumiereController.data.DisableMeshRendererGetSet)
                {
                    foreach (Renderer renderer in itemLumiere.renderers)
                    {
                        renderer.enabled = false;
                    }
                }
                else
                {
                    foreach (Renderer renderer in itemLumiere.renderers)
                    {
                        renderer.enabled = true;
                    }
                }
                disableMesh = lumiereController.data.DisableMeshRendererGetSet;
            }
        }

        public void DisableCollision()
        {
            foreach (ColliderGroup cg in itemLumiere.colliderGroups)
            {
                foreach (Collider collider in cg.colliders)
                {
                    collider.enabled = false;
                }
            }
        }
        public void EnableCollision()
        {
            foreach (ColliderGroup cg in itemLumiere.colliderGroups)
            {
                foreach (Collider collider in cg.colliders)
                {
                    collider.enabled = true;
                }
            }
        }
    }
}
