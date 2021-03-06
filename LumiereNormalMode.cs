using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using SnippetCode;

namespace Lumiere
{
    public class LumiereNormalMode : LumiereItemBase
    {
        private Vector3 vec3zero = Vector3.zero;
        private bool pointsToLight = false;
        private bool telegrabRight = false;
        private bool telegrabLeft = false;
        private LineRenderer lineRenderer;
        public bool isStuck = false;
        public Item itemStuck;
        public RagdollPart partStuck;
        private FixedJoint stickyJoint;
        private Creature creatureStuck;


        public override void Awake()
        {
            base.Awake();
            itemLumiere.OnGrabEvent += ItemLumiere_OnGrabEvent;
            itemLumiere.OnUngrabEvent += ItemLumiere_OnUngrabEvent;
            itemLumiere.OnTelekinesisGrabEvent += ItemLumiere_OnTelekinesisGrabEvent;
            itemLumiere.OnTelekinesisReleaseEvent += ItemLumiere_OnTelekinesisReleaseEvent;
            itemLumiere.data.flyFromThrow = false;
            Player.local.creature.handRight.UnGrab(false);
            Player.local.creature.handRight.Grab(itemLumiere.GetMainHandle(Side.Right), true);
        }

        public override void Update()
        {
            base.Update();
            if (pointsToLight != lumiereController.data.PointToLightsGetSet && lumiereController.data.PointToLightsGetSet == false)
            {
                if (lineRenderer != null)
                {
                    Destroy(lineRenderer);
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

            if (!isStuck && (itemLumiere.isFlying || itemLumiere.isThrowed))
            {
                foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                {
                    handler.SetPhysicModifier(this, 0f, 0f, 1000f, 1000f);
                }
                itemLumiere.rb.velocity = vec3zero;
                itemLumiere.rb.angularVelocity = vec3zero;
                DisableCollision();
            }


            if (lumiereController.data.IsStickyGetSet)
            {
                if (!isStuck && (lumiereController.data.holdingALightRightHand && Player.local.handRight.controlHand.castPressed && Player.local.handRight.controlHand.gripPressed ||
                                  lumiereController.data.holdingALightLeftHand && Player.local.handLeft.controlHand.castPressed && Player.local.handLeft.controlHand.gripPressed))
                {
                    if (!lumiereController.data.StickToItemFalseCreatureTrueGetSet)
                    {
                        itemStuck = Snippet.ClosestItemAroundItemOverlapSphere(itemLumiere, 0.2f);
                        if (itemStuck != itemLumiere)
                        {
                            stickyJoint = new FixedJoint();
                            stickyJoint = Snippet.CreateStickyJointBetweenTwoRigidBodies(itemLumiere.rb, itemStuck.rb, stickyJoint);
                            itemStuck.OnDespawnEvent += ItemStuck_OnDespawnEvent;
                            isStuck = true;
                        }
                    }
                    else
                    {
                        partStuck = Snippet.ClosestRagdollPartAroundItemOverlapSphere(itemLumiere, 0.2f);
                        if (partStuck != null)
                        {
                            stickyJoint = new FixedJoint();
                            stickyJoint = Snippet.CreateStickyJointBetweenTwoRigidBodies(itemLumiere.rb, partStuck.rb, stickyJoint);
                            creatureStuck = partStuck.ragdoll.creature;
                            creatureStuck.OnDespawnEvent += CreatureStuck_OnDespawnEvent;
                            isStuck = true;
                        }
                    }
                    if (isStuck)
                    {
                        SetTKHandle(false);
                        itemLumiere.rb.mass = 0f;
                        itemLumiere.rb.drag = 0f;
                        itemLumiere.rb.angularDrag = 0f;
                        foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                        {
                            handler.SetPhysicModifier(this, 0f, 0f, 0f, 0f);
                        }
                        DisableCollision();
                        if (lumiereController.data.holdingALightRightHand)
                        {
                            Player.local.creature.handRight.UnGrab(false);
                        }
                        if (lumiereController.data.holdingALightLeftHand)
                        {
                            Player.local.creature.handLeft.UnGrab(false);
                        }
                    }
                }
                if (isStuck && itemLumiere.handles.FirstOrDefault(handle => handle.data.disableTouch == true))
                {
                    SetTouchHandle(true);
                }
            }
            else
            {
                if (isStuck && itemLumiere.handles.FirstOrDefault(handle => handle.data.disableTouch == false))
                {
                    SetTouchHandle(false);
                }
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

        private void CreatureStuck_OnDespawnEvent(EventTime eventTime)
        {
            if (EventTime.OnStart == eventTime)
            {
                if (isStuck && partStuck != null)
                {
                    if (stickyJoint != null)
                    {
                        Destroy(stickyJoint);
                    }
                    SetTKHandle(true);
                    isStuck = false;
                    creatureStuck.OnDespawnEvent -= ItemStuck_OnDespawnEvent;
                    creatureStuck = null;
                }
                itemLumiere.Despawn();
            }
        }

        private void ItemStuck_OnDespawnEvent(EventTime eventTime)
        {
            if (EventTime.OnStart == eventTime)
            {
                if (isStuck && itemStuck != null)
                {
                    if (stickyJoint != null)
                    {
                        Destroy(stickyJoint);
                    }
                    SetTKHandle(true);
                    isStuck = false;
                    itemStuck.OnDespawnEvent -= ItemStuck_OnDespawnEvent;
                    itemStuck = null;
                }
                itemLumiere.Despawn();
            }
        }

        /// <summary>
        /// When the light is dropped, make the item immobile on the air.
        /// </summary>
        private void ItemLumiere_OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
        {
            itemLumiere.rb.mass = massOri;
            itemLumiere.rb.drag = dragOri;
            itemLumiere.rb.angularDrag = angularDragOri;
            foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
            {
                handler.SetPhysicModifier(this, 0f, 0f, 1000f, 1000f);
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
            if (!isStuck)
            {
                itemLumiere.rb.mass = massOri;
                itemLumiere.rb.drag = dragOri;
                itemLumiere.rb.angularDrag = angularDragOri;
                foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                {
                    handler.SetPhysicModifier(this, 0f, 0f, 1000f, 1000f);
                }
                itemLumiere.rb.velocity = vec3zero;
                itemLumiere.rb.angularVelocity = vec3zero;
            }
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
            if (isStuck)
            {
                if (stickyJoint != null)
                {
                    Destroy(stickyJoint);
                }
                SetTKHandle(true);
                isStuck = false;
                itemStuck = null;
            }
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
            Destroy(lineRenderer);
        }

        private void SetTKHandle(bool active)
        {
            for (int i = itemLumiere.handles.Count() - 1; i >= 0; --i)
            {
                itemLumiere.handles[i].SetTelekinesis(active);
                itemLumiere.handles[i].data.allowTelekinesis = active;
            }
        }
        private void SetTouchHandle(bool active)
        {
            for (int i = itemLumiere.handles.Count() - 1; i >= 0; --i)
            {
                itemLumiere.handles[i].SetTouch(active);
                itemLumiere.handles[i].data.disableTouch = !active;
            }
        }
    }
}
