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
using IngameDebugConsole;
using System.IO;
using Newtonsoft.Json;

namespace Lumiere
{
    public class LumiereLevelModule : LevelModule
    {
        private LumiereController lumiereController;
        private Item lumiereItem;
        private static List<Item> lumiereItemsList;
        private bool previewModeOn = false;
        private static List<LumiereSaveData> lumiereSaveData;
        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onPossess += EventManager_onPossess;
            EventManager.onUnpossess += EventManager_onUnpossess;
            return base.OnLoadCoroutine();
        }

        private void EventManager_onUnpossess(Creature creature, EventTime eventTime)
        {
            if (EventTime.OnStart == eventTime)
            {
                lumiereController.data.levelLoadedGetSet = false;
                lumiereController.data.levelNameGetSet = "";
            }
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
                lumiereSaveData = new List<LumiereSaveData>();
                lumiereController.data.levelLoadedGetSet = true;
                lumiereController.data.levelNameGetSet = Level.current.data.id;
                DebugLogConsole.AddCommand<int>("Lumiere.Save", "Save lights datas", SaveDatas);
                DebugLogConsole.AddCommand<int, bool>("Lumiere.Save", "Save lights datas", SaveDatas);
                DebugLogConsole.AddCommand<int>("Lumiere.Load", "Load lights datas", LoadDatas);
                DebugLogConsole.AddCommand("Lumiere.Despawn", "Despawn all lights", DespawnAllLights);
                DebugLogConsole.AddCommand<bool>("Lumiere.DisableHandle", "Disable all handles", DisableHandles);
            }
        }

        public override void OnUnload()
        {
            EventManager.onPossess -= EventManager_onPossess;
            EventManager.onUnpossess -= EventManager_onUnpossess;
            DebugLogConsole.RemoveCommand("Lumiere.Save");
            DebugLogConsole.RemoveCommand("Lumiere.Load");
            DebugLogConsole.RemoveCommand("Lumiere.Despawn");
            DebugLogConsole.RemoveCommand("Lumiere.DisableHandles");
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
                        lumiereItem.gameObject.AddComponent<LumiereNormalMode>().Init();
                        lumiereItemsList.Add(lumiereItem);
                    }, Player.local.creature.handRight.transform.position + Player.local.creature.handRight.transform.forward * 0.1f);
                    lumiereController.data.SpawnLight = false;
                    if (lumiereController.data.DisablePreviewAfterSpawnGetSet && lumiereController.data.PreviewLightGetSet)
                    {
                        lumiereController.data.PreviewLightGetSet = false;
                    }
                    lumiereController.data.nbLightInLevel = lumiereItemsList.Count;
                }
                if (lumiereController.data.PreviewLightGetSet && previewModeOn == false)
                {
                    Catalog.GetData<ItemData>("LumiereItem").SpawnAsync(item =>
                    {
                        item.disallowDespawn = true;
                        item.SetColliderLayer((int)LayerName.Zone);
                        lumiereItem = item;
                        lumiereItem.gameObject.AddComponent<LumierePreviewMode>().Init();
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
        //[ConsoleMethod("Lumiere.Save", "Save lights datas")]
        public static void SaveDatas(int nbSave)
        {
            if (lumiereItemsList.Count == 0)
            {
                Debug.Log("Lumiere : No lights to save");
                return;
            }
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = Level.current.data.id + nbSave;
            string pathToFile = pathToFolder + "\\" + title + ".json";
            if (!Directory.Exists(pathToFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathToFolder);
            }
            if (lumiereSaveData.Count > 0)
            {
                lumiereSaveData.Clear();
            }
            foreach (Item item in lumiereItemsList)
            {
                if (item.GetComponent<LumiereNormalMode>().isStuck)
                    continue;
                LumiereSaveData data = new LumiereSaveData
                {
                    position = item.transform.position,
                    rotation = item.transform.rotation,
                    color = item.GetComponent<LumiereItemBase>().light.color,
                    intensity = item.GetComponent<LumiereItemBase>().light.intensity,
                    range = item.GetComponent<LumiereItemBase>().light.range
                };
                lumiereSaveData.Add(data);
                string jsonWrite = JsonConvert.SerializeObject(lumiereSaveData, Formatting.Indented);
                File.WriteAllText(pathToFile, jsonWrite);
            }
            Debug.Log($"Lumiere : Saved {lumiereSaveData.Count} lights at : {title}");
        }
        public static void SaveDatas(int nbSave, bool saveStuck = false)
        {
            if (lumiereItemsList.Count == 0)
            {
                Debug.Log("Lumiere : No lights to save");
                return;
            }
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = Level.current.data.id + nbSave;
            string pathToFile = pathToFolder + "\\" + title + ".json";
            if (!Directory.Exists(pathToFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathToFolder);
            }
            if (lumiereSaveData.Count > 0)
            {
                lumiereSaveData.Clear();
            }
            if (saveStuck)
                saveStuck = false;
            foreach (Item item in lumiereItemsList)
            {
                if (item.GetComponent<LumiereNormalMode>().isStuck && !saveStuck)
                    continue;
                LumiereSaveData data = new LumiereSaveData
                {
                    position = item.transform.position,
                    rotation = item.transform.rotation,
                    color = item.GetComponent<LumiereItemBase>().light.color,
                    intensity = item.GetComponent<LumiereItemBase>().light.intensity,
                    range = item.GetComponent<LumiereItemBase>().light.range
                };
                lumiereSaveData.Add(data);
                string jsonWrite = JsonConvert.SerializeObject(lumiereSaveData, Formatting.Indented);
                File.WriteAllText(pathToFile, jsonWrite);
            }
            Debug.Log($"Lumiere : Saved {lumiereSaveData.Count} lights at : {title}");
        }
        public static void LoadDatas(int nbSave)
        {
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = Level.current.data.id + nbSave;
            string pathToFile = pathToFolder + "\\" + title + ".json";
            if (!File.Exists(pathToFile) || !Directory.Exists(pathToFolder))
            {
                if (!Directory.Exists(pathToFolder))
                    Debug.Log($"Lumiere : No folder at : {pathToFolder}");
                if (!File.Exists(pathToFile))
                    Debug.Log($"Lumiere : No saves for : {title}");
                return;
            }
            if (lumiereItemsList.Count != 0)
            {
                foreach (Item item in lumiereItemsList)
                {
                    item.Despawn();
                }
                lumiereItemsList.Clear();
            }
            String textRead = File.ReadAllText(pathToFile);
            lumiereSaveData = JsonConvert.DeserializeObject<List<LumiereSaveData>>(textRead);
            foreach (LumiereSaveData data in lumiereSaveData)
            {
                Catalog.GetData<ItemData>("LumiereItem").SpawnAsync(item =>
                {
                    item.disallowDespawn = true;
                    item.SetColliderLayer((int)LayerName.Zone);
                    item.gameObject.AddComponent<LumiereNormalMode>().Init(false, data);
                    lumiereItemsList.Add(item);
                }, Player.local.creature.handRight.transform.position + Player.local.creature.handRight.transform.forward * 0.1f);
            }
            Debug.Log($"Lumiere : Loaded {lumiereSaveData.Count} lights from : {title}");
        }

        public static void DeleteDatas(int nbSave)
        {
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = Level.current.data.id + nbSave;
            string pathToFile = pathToFolder + "\\" + title + ".json";
            if (!File.Exists(pathToFile) || !Directory.Exists(pathToFolder))
            {
                if (!Directory.Exists(pathToFolder))
                    Debug.Log($"Lumiere : No folder at : {pathToFolder}");
                if (!File.Exists(pathToFile))
                    Debug.Log($"Lumiere : No saves for : {title}");
                return;
            }
            File.Delete(pathToFile);
            Debug.Log($"Lumiere : Deleted save : {title}");
        }

        public static void DespawnAllLights()
        {
            float nbLights = lumiereItemsList.Count;
            if (lumiereItemsList.Count == 0)
            {
                Debug.Log("Lumiere : No lights to despawn");
                return;
            }
            else
            {
                foreach (Item item in lumiereItemsList)
                {
                    item.Despawn();
                }
                lumiereItemsList.Clear();
            }
            Debug.Log($"Lumiere : Despawn {nbLights} lights from the scene");
        }

        public static void SortDatas(int nbSave)
        {
            string pathToFolder = Environment.CurrentDirectory + "\\BladeAndSorcery_Data\\StreamingAssets\\Mods\\Lumiere\\Saves";
            string title = Level.current.data.id;
            List<string> paths = Directory.EnumerateFiles(pathToFolder, title + "*.json").ToList();
            paths.Sort();
            Debug.Log($"Lumiere : Nb saves : {paths.Count}");
            if (nbSave > paths.Count)
                return;
            for (int i = nbSave - 1; i < paths.Count; i++)
            {
                string localPath = pathToFolder + "\\" + title + (i + 1) + ".json";
                File.Move(paths[i], localPath);
            }
        }

        public static int returnNbLightInLevel()
        {
            return lumiereItemsList.Count;
        }

        public static void DisableHandles(bool disable)
        {
            LumiereController lumiereController = GameManager.local.gameObject.GetComponent<LumiereController>();
            lumiereController.data.DisableHandlesGetSet = disable;
        }
    }
}
