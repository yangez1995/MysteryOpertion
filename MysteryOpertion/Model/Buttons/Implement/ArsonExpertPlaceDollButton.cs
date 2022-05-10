using MysteryOpertion.Object;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class ArsonExpertPlaceDollButton : ButtonBase
    {
        public ArsonExpertPlaceDollButton(Player player) : base(player)
        {
            this.cooldownTime = 10f;
            this.sanityCost = 15;
            this.text = ButtonTextDictionary.ArsonExpertPlaceDollButton;
            this.positionOffset = new Vector3(-0.9f, 1f, 0f);
        }

        public override bool IsAvailable()
        {
            return player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            var pos = PlayerControl.LocalPlayer.transform.position;
            byte[] buff = new byte[sizeof(float) * 2];
            Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
            
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Doll(position);
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
