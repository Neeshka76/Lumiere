using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace Lumiere
{
    public class LumierePreviewMode : LumiereItemBase
    {
        public override void Awake()
        {
            base.Awake();
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 5, 0, 0, 0, 0);
            }
            for (int i = itemLumiere.handles.Count() - 1; i >= 0; --i)
            {
                itemLumiere.handles[i].SetTelekinesis(false);
                itemLumiere.handles[i].data.allowTelekinesis = false;
                itemLumiere.handles[i].SetTouch(false);
            }
            DisableCollision();
        }

        public override void Update()
        {
            base.Update();
            itemLumiere.transform.position = Player.local.head.transform.position + Player.local.head.transform.forward * lumiereController.data.SliderDistancePreviewValueGetSet;
            light.color = new Color(lumiereController.data.ColorRValueGetSet, lumiereController.data.ColorGValueGetSet, lumiereController.data.ColorBValueGetSet) / 255f;
            light.intensity = lumiereController.data.LightIntensityGetSet;
            light.range = lumiereController.data.LightRangeGetSet;

            if (!lumiereController.data.PreviewLightGetSet)
            {
                OnDisable();
            }
        }

        private void OnDisable()
        {
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.RemovePhysicModifier(this);
            }
            itemLumiere.Despawn();
        }
    }
}
