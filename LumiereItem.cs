using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using SnippetCode;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Lumiere
{
    public class LumiereItem : MonoBehaviour
    {
        private Item itemLumiere;
        private Vector3 vec3zero = Vector3.zero;
        public Light light;

        /// <summary>
        /// this items Module.
        /// </summary>
        public LumiereItemModule ItemModule { get; internal set; }
        public LumiereController lumiereController;
        private void OnEnable()
        {
            // Each time this is unpooled it will recache and reinitialize.

            // Cache.
            lumiereController = GameManager.local.gameObject.GetComponent<LumiereController>();
            itemLumiere = GetComponent<Item>();
            itemLumiere.OnGrabEvent += ItemLumiere_OnGrabEvent;
            itemLumiere.OnUngrabEvent += ItemLumiere_OnUngrabEvent;
            itemLumiere.OnTelekinesisGrabEvent += ItemLumiere_OnTelekinesisGrabEvent;
            itemLumiere.OnTelekinesisReleaseEvent += ItemLumiere_OnTelekinesisReleaseEvent;
            light = itemLumiere.gameObject.GetComponentInChildren<Light>();
            light.color = new Color(lumiereController.data.ColorRValueGetSet, lumiereController.data.ColorGValueGetSet, lumiereController.data.ColorBValueGetSet) / 255f;
            light.intensity = lumiereController.data.LightIntensityGetSet;
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 5, 0, 0, 1000f, 1000f);
            }
            Player.local.creature.handRight.Grab(itemLumiere.GetMainHandle(Side.Right), true);
        }

        /// <summary>
        /// When the light is dropped, make the item immobile on the air.
        /// </summary>
        private void ItemLumiere_OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
        {
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 5, 0, 0, 1000f, 1000f);
            }
            itemLumiere.rb.velocity = vec3zero;
            itemLumiere.rb.angularVelocity = vec3zero;
        }

        /// <summary>
        /// When the light is grabbed, desactivate the restraints.
        /// </summary>
        private void ItemLumiere_OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
        {
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.RemovePhysicModifier(this);
            }
        }

        /// <summary>
        /// When the light is dropped, make the item immobile on the air.
        /// </summary>
        private void ItemLumiere_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
        {
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 5, 0, 0, 1000f, 1000f);
            }
            itemLumiere.rb.velocity = vec3zero;
            itemLumiere.rb.angularVelocity = vec3zero;
            if (ragdollHand.side == Side.Right)
            {
                lumiereController.data.holdingALightRightHand = false;
            }
            if (ragdollHand.side == Side.Left)
            {
                lumiereController.data.holdingALightLeftHand = false;
            }
        }

        /// <summary>
        /// When the light is grabbed, desactivate the restraints.
        /// </summary>
        private void ItemLumiere_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.RemovePhysicModifier(this);
            }
            if (ragdollHand.side == Side.Right)
            {
                lumiereController.data.holdingALightRightHand = true;
            }
            if (ragdollHand.side == Side.Left)
            {
                lumiereController.data.holdingALightLeftHand = true;
            }
        }

        private void OnDisable()
        {
            // When this gets pooled/destroyed, dispose of anything that may cause issues 
            // when it's next unpooled.
            Dispose();
        }

        /// <summary>
        /// Dispose of this item.
        /// </summary>
        public void Dispose()
        {
            itemLumiere.OnGrabEvent -= ItemLumiere_OnGrabEvent;
            itemLumiere.OnUngrabEvent -= ItemLumiere_OnUngrabEvent;
            itemLumiere.OnTelekinesisGrabEvent -= ItemLumiere_OnTelekinesisGrabEvent;
            itemLumiere.OnTelekinesisReleaseEvent -= ItemLumiere_OnTelekinesisReleaseEvent;
            for(int i = itemLumiere.handlers.Count() - 1; i >= 0; --i)
            {
                itemLumiere.handlers[i].UnGrab(false);
            }
        }
    }
}
