﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TwitchAuthentication;

namespace WeaponSelector
{
    internal class BoxHelper : MonoBehaviour, IDragHandler
    {
        public WeaponMenu menuInstance;
        public Image img;
        public RectTransform dragRectTransform;
        public Canvas canvas;

        void Start()
        {
            dragRectTransform = GetComponent<RectTransform>();
            // maybe pass heartmenu transform here instead?
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    internal class WeaponText : MonoBehaviour
    {
        public WeaponMenu menuInstance;
        public TextMeshProUGUI tmp;
    }
    
    internal class ArrowButtons : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public WeaponMenu menuInstance;
        public Image img;
        public bool isLeft;

        public Change type;

        public Sprite Normal;

        public void OnPointerDown(PointerEventData eventData)
        {
            var data = SaveFile.SaveData;

            switch (type)
            {
                case Change.Weapon:
                    {
                        int cur = (int)WeaponPatches.Weapon;
                        int max = WeaponPatches.Weapon.EnumLength();
                        int choice = ParseChoice(cur, max);
                        data.Item1 = (WeaponChoices)choice;
                    }
                    break;
                case Change.Trait:
                    {
                        int cur = (int)WeaponPatches.Trait;
                        int max = WeaponPatches.Trait.EnumLength();
                        int choice = ParseChoice(cur, max);
                        data.Item2 = (WeaponTraits)choice;
                    }
                    break;
                case Change.Curse:
                    {
                        int cur = (int)WeaponPatches.Curse;
                        int max = WeaponPatches.Curse.EnumLength();
                        int choice = ParseChoice(cur, max);
                        data.Item3 = (CurseChoices)choice;
                    }
                    break;
            }

            SaveFile.SaveData = data;
            menuInstance.UpdateText(type);
        }

        int ParseChoice(int index, int max)
        {
            index = isLeft ? index -= 1 : index += 1;
            if (index >= max) index = 0;
            if (index < 0) index = max - 1; // Last item
            return index;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ChangeColor(isRed:true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ChangeColor(isRed:false);
        }

        void ChangeColor(bool isRed)
        {
            Color temp = isRed ? new Color(1f, 0, 0, 1f) : new Color(1f, 1f, 1f, 1f);
            img.color = temp;
        }
    }
}
