using System.Collections.Generic;
using MelonLoader;
using MyBox;
using UnityEngine;
using Input = UnityEngine.Input;

namespace AutoLoadGoods
{
    public class AutoLoadGoods:MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            var harmony = new HarmonyLib.Harmony("com.harry.AutoLoadGoods");
            harmony.PatchAll();
            MelonLogger.Msg("AutoLoadGoods mod loaded!");

            
        }

        public override void OnUpdate()
        {
            // base.OnFixedUpdate();
            if (Input.GetKeyDown(KeyCode.T))
            {
                // if(Singleton<SaveManager>.Instance==null)return;
                // SaveManager saveManager = Singleton<SaveManager>.Instance;
                // DisplayManager displayManager = Singleton<DisplayManager>.Instance;
                // List<BoxData> box = saveManager.Progression.BoxDatas;
                // List<DisplayData> displayData = saveManager.Progression.DisplayDatas;
                // BoxInteraction[] boxInteractions = Object.FindObjectsOfType<BoxInteraction>();
                // MelonLogger.Msg(boxInteractions.Length);
                // boxInteractions.ForEach(boxInteraction =>
                // {
                //     MelonLogger.Msg(boxInteraction.GetType().GetMember("m_Box"));
                // });
                // for(int l= 0; l < box.Count; l++)
                // {
                //     List<DisplaySlot> displaySlots = displayManager.GetDisplaySlots(box[l].ProductID);
                //     foreach (var displaySlot in displaySlots)
                //     {
                //         if (box[l].ProductCount <= 0)
                //         {
                //             break;
                //         }
                //         if (!displaySlot.Full&&displaySlot.HasProduct&&displaySlot.ProductID==box[l].ProductID)
                //         {
                //             
                //             
                //             displaySlot.Data.FirstItemCount++;
                //             box[l].ProductCount--;
                //         }
                //     }
                // }
                // for(int i = 0; i < displayData.Count; i++)
                // {
                //     List<ItemQuantity> itemQuantity = displayData[i].DisplaySlots;
                //     for (int j = 0; j < itemQuantity.Count; j++)
                //     {
                //         ItemQuantity item = itemQuantity[j];
                //         if(item.FirstItemCount>4)continue;
                //
                //
                //     }
                // }
            }

            
        }
    }
}