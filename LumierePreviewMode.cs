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
            itemLumiere.rb.useGravity = false;
            DisableCollision();
        }

        public override void Update()
        {
            base.Update();
            itemLumiere.transform.position = Player.local.head.transform.position + Player.local.head.transform.forward * 0.85f;
            light.color = new Color(lumiereController.data.ColorRValueGetSet, lumiereController.data.ColorGValueGetSet, lumiereController.data.ColorBValueGetSet) / 255f;
            light.intensity = lumiereController.data.LightIntensityGetSet;
            light.range = lumiereController.data.LightRangeGetSet;

            if (!lumiereController.data.PreviewLight)
            {
                OnDisable();
            }
        }

        private void OnDisable()
        {
            itemLumiere.Despawn();
        }

    }
}
