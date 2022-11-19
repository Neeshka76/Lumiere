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
        private Transform transformToStuck;
        private Creature creatureStuck;


        public override void Awake()
        {
            base.Awake();
            itemLumiere.OnGrabEvent += ItemLumiere_OnGrabEvent;
            itemLumiere.OnUngrabEvent += ItemLumiere_OnUngrabEvent;
            itemLumiere.OnTelekinesisGrabEvent += ItemLumiere_OnTelekinesisGrabEvent;
            itemLumiere.OnTelekinesisReleaseEvent += ItemLumiere_OnTelekinesisReleaseEvent;
            itemLumiere.data.flyFromThrow = false;
        }

        public override void Init(bool useBook = true, LumiereSaveData saveData = null)
        {
            base.Init(useBook, saveData);

            if (useBook)
            {
                if (Player.local.creature.handRight.grabbedHandle != null)
                    Player.local.creature.handRight.UnGrab(false);
                Player.local.creature.handRight.Grab(itemLumiere.GetMainHandle(Side.Right), true);
            }
            else
            {
                itemLumiere.transform.position = saveData.position;
                itemLumiere.transform.rotation = saveData.rotation;
                FreezeLight();
                DisableCollision();
            }
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
                    if (!lumiereController.data.StickToItemWhenFalseCreatureTrueGetSet)
                    {
                        itemStuck = Snippet.ClosestItemAroundItemOverlapSphere(itemLumiere, 0.2f);
                        if (itemStuck != itemLumiere)
                        {
                            transformToStuck = itemStuck.transform;
                            itemStuck.OnDespawnEvent += ItemStuck_OnDespawnEvent;
                        }
                    }
                    else
                    {
                        partStuck = Snippet.ClosestRagdollPartAroundItemOverlapSphere(itemLumiere, 0.2f);
                        if (partStuck != null)
                        {
                            transformToStuck = partStuck.transform;
                            creatureStuck = partStuck.ragdoll.creature;
                            creatureStuck.ragdoll.AddPhysicToggleModifier(this);
                            creatureStuck.OnDespawnEvent += CreatureStuck_OnDespawnEvent;
                        }
                    }
                    if (transformToStuck != null)
                    {
                        SetTKHandle(false);
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
                if (isStuck && itemLumiere.handles.FirstOrDefault(handle => handle.data.disableTouch == true) && !lumiereController.data.DisableHandlesGetSet)
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
            if (lumiereController.data.DisableHandlesGetSet)
            {
                if (itemLumiere.handles.FirstOrDefault(handle => handle.data.disableTouch == false))
                    SetTouchHandle(false);
                if (itemLumiere.handles.FirstOrDefault(handle => handle.data.allowTelekinesis == true))
                    SetTKHandle(false);
            }
            else
            {
                if (!lumiereController.data.IsStickyGetSet && !isStuck)
                {
                    if (itemLumiere.handles.FirstOrDefault(handle => handle.data.disableTouch == true))
                        SetTouchHandle(true);
                    if (itemLumiere.handles.FirstOrDefault(handle => handle.data.allowTelekinesis == false))
                        SetTKHandle(true);
                }
            }
        }

        private void CreatureStuck_OnDespawnEvent(EventTime eventTime)
        {
            if (EventTime.OnStart == eventTime)
            {
                if (isStuck && partStuck != null)
                {
                    AttachLight(false);
                    SetTKHandle(true);
                    creatureStuck.ragdoll.RemovePhysicToggleModifier(this);
                    creatureStuck.OnDespawnEvent -= CreatureStuck_OnDespawnEvent;
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
                    SetTKHandle(true);
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
            FreezeLight();
            DisableCollision();
            telegrabLeft = false;
            telegrabRight = false;
        }

        /// <summary>
        /// When the light is grabbed, desactivate the restraints.
        /// </summary>
        private void ItemLumiere_OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
        {
            FreezeLight(false);
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
                FreezeLight();
            }
            if (ragdollHand.side == Side.Right)
            {
                lumiereController.data.holdingALightRightHand = false;
            }
            if (ragdollHand.side == Side.Left)
            {
                lumiereController.data.holdingALightLeftHand = false;
            }
            if(transformToStuck != null)
            {
                AttachLight(true, transformToStuck);
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
                SetTKHandle(true);
                AttachLight(false);
                if(itemStuck != null)
                {
                    itemStuck.OnDespawnEvent -= ItemStuck_OnDespawnEvent;
                    itemStuck = null;
                }
                if(creatureStuck != null)
                {
                    creatureStuck.ragdoll.RemovePhysicToggleModifier(this);
                    creatureStuck.OnDespawnEvent -= CreatureStuck_OnDespawnEvent;
                    creatureStuck = null;
                }
                transformToStuck = null;
            }
            FreezeLight(false);
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
            itemLumiere.transform.parent = null;
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

        private void FreezeLight(bool enable = true)
        {
            if (enable)
            {
                foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                {
                    handler.SetPhysicModifier(this, 0f, 0f, 1000f, 1000f);
                }
                itemLumiere.rb.velocity = vec3zero;
                itemLumiere.rb.angularVelocity = vec3zero;
            }
            else
            {
                itemLumiere.rb.mass = massOri;
                itemLumiere.rb.drag = dragOri;
                itemLumiere.rb.angularDrag = angularDragOri;
                foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                {
                    handler.RemovePhysicModifier(this);
                }
                EnableCollision();
            }
        }

        private void AttachLight(bool active = true, Transform transform = null)
        {
            if (active)
            {
                itemLumiere.transform.SetParent(transform);
                foreach (CollisionHandler handler in itemLumiere.collisionHandlers)
                {
                    handler.SetPhysicModifier(this, 0f, 0f, dragOri, angularDragOri);
                }
            }
            else
            {
                itemLumiere.transform.parent = transform;
            }
            itemLumiere.rb.isKinematic = active;
            isStuck = active;
        }
    }
}
