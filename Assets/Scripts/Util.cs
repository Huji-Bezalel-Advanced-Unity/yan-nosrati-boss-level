using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public static class Util
    {
        public static async Task DoFadeLerp(Renderer renderer, float startValue, float endValue, float duration)
        {
            Color currentColor = renderer.material.color;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                if (!renderer) return;

                elapsedTime += Time.deltaTime;
                float percentageCompleted = elapsedTime / duration;
                currentColor.a = Mathf.Lerp(startValue, endValue, percentageCompleted);
                renderer.material.color = currentColor;
                await Task.Yield(); // Yields control back to the calling context, letting other tasks run
            }
        }

        // public static async Task DoFillLerp(Image imageToFill, float startValue, float endValue, float duration)
        // {
        //     float elapsedTime = 0;
        //     while (elapsedTime < duration)
        //     {
        //         elapsedTime += Time.deltaTime;
        //         float precentageCompleted = elapsedTime / duration;
        //         imageToFill.fillAmount = Mathf.Lerp(startValue, endValue, precentageCompleted);
        //         await Task.Yield();
        //     }
        // }\
        public static async Task DoFillLerp(Image imageToFill, float startValue, float endValue, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float precentageCompleted = elapsedTime / duration;
                imageToFill.fillAmount = Mathf.Lerp(startValue, endValue, precentageCompleted);

                // Wait for a small amount of time (adjust as needed)
                await Task.Delay(TimeSpan.FromMilliseconds(10));
            }
        }
    }
}