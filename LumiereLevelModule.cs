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
        private bool previewModeOn = false;
        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onPossess += EventManager_onPossess;
            return base.OnLoadCoroutine();
        }

        private void EventManager_onPossess(Creature creature, EventTime eventTime)
        {
            if (EventTime.OnEnd == eventTime)
            {
                if (lumiereController == null)
                {
                    lumiereController = GameManager.local.gameObject.GetComponent<LumiereController>();
                }
                lumiereItemsList = new List<Item>();
            }
        }

        public override void OnUnload()
        {
            EventManager.onPossess -= EventManager_onPossess;
        }


        public override void Update()
        {
            base.Update();
            if (Player.local?.creature != null)
            {
                if (lumiereController.data.SpawnLight)
                {
                    Catalog.GetData<ItemData>("LumiereItem").SpawnAsync(item =>
                    {
                        item.disallowDespawn = true;
                        item.SetColliderLayer((int)LayerName.Zone);
                        lumiereItem = item;
                        lumiereItem.gameObject.AddComponent<LumiereNormalMode>().Awake();
                        lumiereItemsList.Add(lumiereItem);
                    }, Player.local.creature.handRight.transform.position + Player.local.creature.handRight.transform.forward * 0.1f);
                    lumiereController.data.SpawnLight = false;
                    if (lumiereController.data.DisablePreviewAfterSpawnGetSet && lumiereController.data.PreviewLightGetSet)
                    {
                        lumiereController.data.PreviewLightGetSet = false;
                    }
                }
                if(lumiereController.data.PreviewLightGetSet && previewModeOn == false)
                {
                    Catalog.GetData<ItemData>("LumiereItem").SpawnAsync(item =>
                    {
                        item.disallowDespawn = true;
                        item.SetColliderLayer((int)LayerName.Zone);
                        lumiereItem = item;
                        lumiereItem.gameObject.AddComponent<LumierePreviewMode>().Awake();
                    }, Player.local.head.transform.position + Player.local.head.transform.forward * 0.5f);
                    previewModeOn = true;
                }
                previewModeOn = lumiereController.data.PreviewLightGetSet;
                if (lumiereController.data.holdingALightRightHand && PlayerControl.handRight.alternateUsePressed && !Player.local.handRight.controlHand.castPressed)
                {
                    lumiereItemsList.Remove(Player.local.handRight.ragdollHand.grabbedHandle.item);
                    Player.local.handRight.ragdollHand.grabbedHandle.item.Despawn();
                }
                if (lumiereController.data.holdingALightLeftHand && PlayerControl.handLeft.alternateUsePressed && !Player.local.handLeft.controlHand.castPressed)
                {
                    lumiereItemsList.Remove(Player.local.handLeft.ragdollHand.grabbedHandle.item);
                    Player.local.handLeft.ragdollHand.grabbedHandle.item.Despawn();
                }
            }
        }
    }

}
