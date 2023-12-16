using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaretTesting : MonoBehaviour
{
    [SerializeField] private RectTransform inputRect;
    [SerializeField] private RectTransform caret;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private float caretPadding = 0f;
    
    private void Update()
    {
        if (!input) return;
        if (inputText.text.Length <= 0)
        {
            SetPosition(0f, 0f);
            return;
        }

        var firstCharInfo = inputText.textInfo.characterInfo[0]; //first character because font is monospaced
        var firstLineInfo = inputText.textInfo.lineInfo[0];
        var charDimension = firstCharInfo.topRight - firstCharInfo.bottomLeft;
        charDimension.y = firstLineInfo.lineHeight;
        charDimension.x -= caretPadding;
        caret.sizeDelta = charDimension;

        var inputWidth = inputRect.rect.width;
        var supportedCharacterPerLine = Mathf.FloorToInt(inputWidth / charDimension.x);

        var moduloCount = input.caretPosition % supportedCharacterPerLine; 

        var lineIndex = moduloCount == 0
            ? inputText.textInfo.lineCount
            : inputText.textInfo.lineCount - 1;
        var y = -(firstLineInfo.lineHeight * lineIndex);
        var x = charDimension.x * moduloCount;
        SetPosition(x, y);
    }

    private void SetPosition(float x, float y)
    {
        var pos = caret.anchoredPosition;
        pos.y = y;
        pos.x = x;
        caret.anchoredPosition = pos;
    }
}
