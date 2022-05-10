using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons
{
    public interface Button
    {
        void OnClick();
        bool IsShow();
        bool IsAvailable();
        void Update();
        void OnMeetingEnd();
    }

    public abstract class ButtonBase : Button
    {
        protected ActionButton actionButton;
        protected Player player;

        protected Sprite sprite;
        protected Vector3 positionOffset;
        protected string text;

        public float timer;
        public float cooldownTime;
        public int sanityCost;

        public ButtonBase(Player player)
        {
            this.actionButton = UnityEngine.Object.Instantiate(HudManager.Instance.KillButton, HudManager.Instance.KillButton.transform.parent);
            this.player = player;
            this.timer = 5f;
            this.cooldownTime = 30f;
            this.sanityCost = 0;
            this.sprite = ToolBox.loadSpriteFromResources("MysteryOpertion.Resources.EmptyButton.png", 115f);
            this.positionOffset = Vector3.zero;
            this.text = string.Empty;

            PassiveButton button = actionButton.GetComponent<PassiveButton>();
            button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)OnClickPatch);

            SetActive(true);
        }

        public abstract void OnClick();
        public abstract bool IsShow();
        public abstract bool IsAvailable();
        public abstract void OnMeetingEnd();

        protected void OnClickPatch()
        {
            if (timer > 0f || !IsShow() || !IsAvailable()) return;

            actionButton.graphic.color = new Color(1f, 1f, 1f, 0.3f);
            OnClick();
        }

        protected void SetActive(bool isActive)
        {
            if (actionButton == null) return;

            actionButton.gameObject.SetActive(isActive);
            actionButton.graphic.enabled = isActive;
        }

        protected void CostSanity()
        {
            byte sourceId = player.playerControl.PlayerId;
            int value = -sanityCost;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CalcSanityPoint, SendOption.Reliable);
            writer.Write(sourceId);
            writer.Write(value);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.CalcSanityPoint(sourceId, value);
        }

        public void Update()
        {
            //判断是否显示按钮
            if (!IsShow() || MeetingHud.Instance || ExileController.Instance)
            {
                SetActive(false);
                return;
            }
            SetActive(true);

            //根据按钮是否可用更改按钮颜色
            if (IsAvailable())
            {
                actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.EnabledColor;
                actionButton.graphic.material.SetFloat("_Desat", 0f);
            }
            else
            {
                actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.DisabledClear;
                actionButton.graphic.material.SetFloat("_Desat", 1f);
            }

            actionButton.graphic.sprite = sprite;
            actionButton.OverrideText(text);
            if(positionOffset != Vector3.zero)
            {
                Vector3 pos = HudManager.Instance.UseButton.transform.localPosition;
                actionButton.transform.localPosition = pos + positionOffset;
            }
           

            //冷却时间相关
            if (timer >= 0)
            {
                if(player.playerControl.moveable)
                {
                    timer -= Time.deltaTime;
                }
            }
            actionButton.SetCoolDown(timer, float.MaxValue);
        }
    }
}
