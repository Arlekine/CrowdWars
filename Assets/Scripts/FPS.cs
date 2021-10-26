using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;
    public float _updateTime;

    private float _nextUpdateTime;
    
    public void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int) current;

        if (Time.time > _nextUpdateTime)
        {
            _nextUpdateTime = Time.time + _updateTime;
            display_Text.text = avgFrameRate.ToString() + " FPS";
        }
    }
}
