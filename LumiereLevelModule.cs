using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using SnippetCode;
using UnityEngine.Events;

namespace Lumiere
{
    public class LumiereLevelModule : LevelModule
    {
        private LumiereController lumiereController;
        private Item lumiereItem;
        private List<Item> lumiereItemsList;

        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onLevelLoad += EventManager_onLevelLoad;
            return base.OnLoadCoroutine();
        }

        private void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
        {
            if (lumiereController == null)
            {
                lumiereController = GameManager.local.gameObject.GetComponent<LumiereController>();
            }
            lumiereItemsList = new List<Item>();
        }

        public override void Update()
        {
            base.Update();
            if(lumiereController.data.SpawnLight)
            {
                Catalog.GetData<ItemData>("LumiereItem").SpawnAsync(item =>
                {
                    item.disallowDespawn = true;
                    lumiereItem = item;
                    lumiereItemsList.Add(lumiereItem);
                }, Player.local.creature.handRight.transform.position + Player.local.creature.handRight.transform.forward * 0.1f);
                lumiereController.data.SpawnLight = false;
            }
            if(lumiereController.data.holdingALightRightHand && PlayerControl.handRight.alternateUsePressed)
            {
                lumiereItemsList.Remove(Player.local.handRight.ragdollHand.grabbedHandle.item);
                Player.local.handRight.ragdollHand.grabbedHandle.item.Despawn();
            }
            if(lumiereController.data.holdingALightLeftHand && PlayerControl.handLeft.alternateUsePressed)
            {
                lumiereItemsList.Remove(Player.local.handLeft.ragdollHand.grabbedHandle.item);
                Player.local.handLeft.ragdollHand.grabbedHandle.item.Despawn();
            }
        }
    }

}
