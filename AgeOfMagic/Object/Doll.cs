using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Object
{
    public class Doll
    {
        public static List<Doll> staticList = new List<Doll>();

        public Doll(Vector2 vector)
        {
            var doll = new GameObject("Doll");
            var gameObject = PlayerControl.LocalPlayer.gameObject;

            var list = gameObject.GetComponents<SpriteRenderer>();
            Debug.Log(list.Count.ToString());

            Debug.Log("step1");
            Vector3 position = new Vector3(vector.x, vector.y, PlayerControl.LocalPlayer.transform.position.z + 1f);
            position += (Vector3)PlayerControl.LocalPlayer.Collider.offset;
            gameObject.transform.position = position;

            var boxRenderer1 = doll.AddComponent<SpriteRenderer>();
            Debug.Log("step2");
            var boxRenderer2 = gameObject.GetComponent<SpriteRenderer>();
            Debug.Log("step3");
            boxRenderer1.sprite = boxRenderer2.sprite;
            doll.SetActive(true);
            Debug.Log("step4");
            staticList.Add(this);
        }
    }
}
