
using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Lean.Pool;
using MelonLoader;
using Unity.VisualScripting;
using UnityEngine;

namespace AutoLoadGoods
{

    [HarmonyPatch(typeof(BoxInteraction))]
    public static class BoxInteractionPatch
    {
        // 选择一个合适的方法来补丁，这个方法在逻辑上会在m_Box被赋值后执行
        // 例如，如果m_Box在Awake方法中被赋值，你可以补丁Awake方法
        [HarmonyPostfix, HarmonyPatch("Update")]
        public static void UpdatePostfix(BoxInteraction __instance)
        {
            // 使用反射获取私有字段的值
            var m_BoxField = AccessTools.Field(typeof(BoxInteraction), "m_Box");
            var m_BoxValue = m_BoxField.GetValue(__instance);

            // 现在你可以访问m_BoxValue，它就是m_Box的值
            // 你可以将其转换为正确的类型，例如：
            Box box = m_BoxValue as Box;
            if(box==null)return;
            if(box.Data.ProductCount<=0||box.Data.ProductID==-1)
            {
                // 使用反射获取私有方法的信息
                MethodInfo throwIntoTrashBinMethod = AccessTools.Method(typeof(BoxInteraction), "ThrowIntoTrashBin");
                // 检查方法是否存在
                if (throwIntoTrashBinMethod != null)
                {
                    // 调用私有方法
                    throwIntoTrashBinMethod.Invoke(__instance, null);
                }
            }
            int id = box.Product.ID;
            // 获取最大的Cheap价格-0.03f的值并赋予
            PriceManager priceManager = MyBox.Singleton<PriceManager>.Instance;
            ProductSO productSO = MyBox.Singleton<IDManager>.Instance.ProductSO(id);
            float num2 = priceManager.CurrentCost(id);
            float num3 = (float)Math.Round((double)(num2 + num2 * productSO.OptimumProfitRate / 100f), 2);
            // float num4 = (float)Math.Round((double)(num2 + num2 * productSO.MaxProfitRate / 100f), 2);
            priceManager.PriceSet(new Pricing(id,num3-0.03f));
            
            DisplayManager displayManager = MyBox.Singleton<DisplayManager>.Instance;
            
            var m_DisplayField = AccessTools.Field(typeof(DisplayManager), "m_Displays");
            var m_DisplayValue = m_DisplayField.GetValue(displayManager);
            List<Display> display = m_DisplayValue as List<Display>;
            if(display==null)return;
            for (int i = 0; i < display.Count; i++)
            {
                if(display[i].DisplayType!=box.Product.ProductDisplayType)continue;
                var m_DisplaySlotsField = AccessTools.Field(typeof(Display), "m_DisplaySlots");
                var m_DisplaySlotsValue = m_DisplaySlotsField.GetValue(display[i]);
                DisplaySlot[] displaySlots = m_DisplaySlotsValue as DisplaySlot[];
                if(displaySlots==null)continue;
                for (int j = 0; j < displaySlots.Length && box.IsOpen && box.Data.ProductCount >= 0; j++)
                {
                    if(displaySlots[j].HasProduct&&displaySlots[j].ProductID!=id)continue;
                    while (box.Data.ProductCount >= 0&&!displaySlots[j].Full)
                    {
                        
                        Product productFromBox = box.GetProductFromBox();
                        if (productFromBox == null)
                        {
                            break;
                        }
                        displaySlots[j].AddProduct(id, productFromBox);
                        MyBox.Singleton<InventoryManager>.Instance.AddProductToDisplay(new ItemQuantity
                        {
                            Products = new Dictionary<int, int>
                            {
                                {
                                    id,
                                    1
                                }
                            }
                        });
                        if (MyBox.Singleton<OnboardingManager>.Instance != null && MyBox.Singleton<OnboardingManager>.Instance.Step == 2)
                        {
                            MyBox.Singleton<OnboardingManager>.Instance.NextStep(2f, false);
                        }
                    
                        MyBox.Singleton<SFXManager>.Instance.PlayPlacingProductSFX();
                    }
                }
            }
            
            
            
            // var displaySlots =  displayManager.GetDisplaySlots(box.Data.ProductID);
            
            
        }

    }

}