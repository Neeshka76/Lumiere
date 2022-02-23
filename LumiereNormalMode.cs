using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace Lumiere
{
    public class LumiereNormalMode : LumiereItemBase
    {
        private Vector3 vec3zero = Vector3.zero;
        private bool pointsToLight = false;
        private bool telegrabRight = false;
        private bool telegrabLeft = false;
        private LineRenderer lineRenderer;

        public override void Awake()
        {
            base.Awake();
            itemLumiere.OnGrabEvent += ItemLumiere_OnGrabEvent;
            itemLumiere.OnUngrabEvent += ItemLumiere_OnUngrabEvent;
            itemLumiere.OnTelekinesisGrabEvent += ItemLumiere_OnTelekinesisGrabEvent;
            itemLumiere.OnTelekinesisReleaseEvent += ItemLumiere_OnTelekinesisReleaseEvent;
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 5, 0, 0, 1000f, 1000f);
            }
            Player.local.creature.handRight.Grab(itemLumiere.GetMainHandle(Side.Right), true);
        }

        public override void Update()
        {
            base.Update();
            if (pointsToLight != lumiereController.data.PointToLightsGetSet && lumiereController.data.PointToLightsGetSet == false)
            {
                if (lineRenderer != null)
                {
                    GameObject.Destroy(lineRenderer);
                }
                pointsToLight = lumiereController.data.PointToLightsGetSet;
            }
            if (lumiereController.data.PointToLightsGetSet)
            {
                if (itemLumiere.gameObject.GetComponent<LineRenderer>() == null)
                {
                    lineRenderer = itemLumiere.gameObject.AddComponent<LineRenderer>();
                    lineRenderer.startWidth = 0.003f;
                    lineRenderer.endWidth = 0.003f;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startColor = new Color(1f - light.color.r, 1f - light.color.g, 1f - light.color.b);
                    lineRenderer.endColor = new Color(light.color.r, light.color.g, light.color.b);
                }
                else
                {
                    lineRenderer?.SetPosition(0, itemLumiere.transform.position);
                    lineRenderer?.SetPosition(1, Player.local.handRight.ragdollHand.fingerIndex.tip.position);
                }
                pointsToLight = lumiereController.data.PointToLightsGetSet;
            }
            
            if (itemLumiere.isTelekinesisGrabbed && telegrabLeft && Player.local.handLeft.controlHand.castPressed && Player.local.handLeft.controlHand.alternateUsePressed)
            {
                Player.local.creature.handLeft.Grab(itemLumiere.GetMainHandle(Side.Left), true);
            }
            if (itemLumiere.isTelekinesisGrabbed && telegrabRight && Player.local.handRight.controlHand.castPressed && Player.local.handRight.controlHand.alternateUsePressed)
            {
                Player.local.creature.handRight.Grab(itemLumiere.GetMainHandle(Side.Right), true);
            }
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
            DisableCollision();
            telegrabLeft = false;
            telegrabRight = false;
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
            EnableCollision();
            if (teleGrabber.spellCaster.ragdollHand.side == Side.Left)
                telegrabLeft = true;
            if (teleGrabber.spellCaster.ragdollHand.side == Side.Right)
                telegrabRight = true;
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
            DisableCollision();
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
            EnableCollision();
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
            for (int i = itemLumiere.handlers.Count() - 1; i >= 0; --i)
            {
                itemLumiere.handlers[i].UnGrab(false);
            }
            GameObject.Destroy(lineRenderer);
        }
    }
}
