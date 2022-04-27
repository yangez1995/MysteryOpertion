using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities.Buttons
{
    public interface Button
    {
        void OnClick();
        bool IsShow();
        bool IsAvailable();
        void OnHudManagerUpdate();
        void OnMeetingEnd();
    }

    public abstract class ButtonBase : Button
    {
        public ActionButton actionButton;
        public HudManager hudManager;

        public float timer;
        public float cooldownTime;

        public ButtonBase(HudManager hudManager, float cooldownTime = 30f)
        {
            this.hudManager = hudManager;
            this.actionButton = UnityEngine.Object.Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);

            PassiveButton button = actionButton.GetComponent<PassiveButton>();
            button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)OnClickPatch);

            this.timer = 15f;
            this.cooldownTime = cooldownTime;

            SetActive(true);
        }

        protected void OnClickPatch()
        {
            if (timer > 0f || !IsShow() || !IsAvailable()) return;

            actionButton.graphic.color = new Color(1f, 1f, 1f, 0.3f);
            OnClick();

            timer = cooldownTime;
        }

        public abstract void OnClick();
        public abstract bool IsShow();
        public abstract bool IsAvailable();
        public abstract void OnMeetingEnd();

        protected void SetActive(bool isActive)
        {
            if (actionButton == null) return;

            actionButton.gameObject.SetActive(isActive);
            actionButton.graphic.enabled = isActive;
        }

        public void OnHudManagerUpdate()
        {
            //判断是否显示按钮
            if (!IsShow())
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

            //冷却时间相关
            if(timer >= 0)
            {
                if(PlayerControl.LocalPlayer.moveable)
                {
                    timer -= Time.deltaTime;
                }
            }
            actionButton.SetCoolDown(timer, float.MaxValue);
        }
    }
}
