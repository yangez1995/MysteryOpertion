using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace MysteryOpertion.Object
{
    public static class Tips
    {
        private static List<TMP_Text> tipList = new List<TMP_Text>();

        public static void ShowTip(string message)
        {
            var roomTracker = DestroyableSingleton<HudManager>.Instance.roomTracker;
            var obj = UnityEngine.Object.Instantiate(roomTracker.gameObject, DestroyableSingleton<HudManager>.Instance.transform);
            UnityEngine.Object.DestroyImmediate(obj.GetComponent<RoomTracker>());
            obj.transform.localScale *= 1.5f;
            
            var tip = obj.GetComponent<TMP_Text>();
            tip.text = message;
            tipList.Insert(0, tip);
            for (int i = 0; i < tipList.Count; i++)
            {
                tipList[i].gameObject.transform.localPosition = new Vector3(0f, -1.8f + (0.3f * i), obj.transform.localPosition.z);
            }

            Action<float> action = (float p) =>
            {
                if (p == 1f)
                {
                    var tip = tipList[tipList.Count - 1];
                    tipList.Remove(tip);
                    UnityEngine.Object.Destroy(tip.gameObject);
                }
            };
            DestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(3, action));
        }


    }
}
