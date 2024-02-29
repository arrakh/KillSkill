using System;
using UnityEngine;

namespace UI.Tooltips
{
    public class TooltipData
    {
        public readonly Sprite iconSprite;
        public readonly string title;
        public readonly string content;
        public readonly string extraContent;

        public TooltipData(Sprite iconSprite, string title, string content)
        {
            this.iconSprite = iconSprite;
            this.title = title;
            this.content = content;
            
            extraContent = String.Empty;
        }

        public TooltipData(Sprite iconSprite, string title, string content, string extraContent)
        {
            this.iconSprite = iconSprite;
            this.title = title;
            this.content = content;
            this.extraContent = extraContent;
        }
    }
}