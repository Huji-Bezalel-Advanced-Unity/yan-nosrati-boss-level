using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public static class Util
    {
        public static IEnumerator DoFadeLerp(Renderer renderer, float startValue, float endValue, float duration, Action onComplete)
        {
          
            Color currentColor = renderer.material.color;
            float elapsedTime = 0;
            while (elapsedTime < duration && GameManager.Instance.GetGameState() == GameState.Played)
            {
                elapsedTime += Time.deltaTime;
                float percentageCompleted = elapsedTime / duration;
                currentColor.a = Mathf.Lerp(startValue, endValue, percentageCompleted);
                renderer.material.color = currentColor;
                yield return null;
            }
            currentColor.a = endValue;
            onComplete?.Invoke();
        }


        public static IEnumerator DoFillLerp(Image imageToFill, float startValue, float endValue, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration && GameManager.Instance.GetGameState() == GameState.Played)
            {
                elapsedTime += Time.deltaTime;
                float precentageCompleted = elapsedTime / duration;
                imageToFill.fillAmount = Mathf.Lerp(startValue, endValue, precentageCompleted);
                // Wait for a small amount of time (adjust as needed)
                yield return null; // Yields control back to the calling context, letting other tasks run
            }
            imageToFill.fillAmount = endValue;
        }

        public static List<double> Solve(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // Check if the matrix is square (cols - 1 because last column is the constants)
            if (rows != cols - 1)
            {
                throw new ArgumentException("Matrix must be square to use Gaussian elimination.");
            }

            double[] solution = new double[rows];

            // Forward elimination phase
            for (int i = 0; i < rows - 1; i++)
            {
                // Pivot row selection
                int maxRow = i;
                for (int k = i + 1; k < rows; k++)
                {
                    if (Math.Abs(matrix[k, i]) > Math.Abs(matrix[maxRow, i]))
                    {
                        maxRow = k;
                    }
                }

                // Swap rows if necessary (to avoid division by zero)
                if (maxRow != i)
                {
                    for (int k = i; k < cols; k++)
                    {
                        double temp = matrix[i, k];
                        matrix[i, k] = matrix[maxRow, k];
                        matrix[maxRow, k] = temp;
                    }
                }

                // Elimination step
                for (int j = i + 1; j < rows; j++)
                {
                    double factor = matrix[j, i] / matrix[i, i];
                    for (int k = i; k < cols; k++)
                    {
                        matrix[j, k] -= factor * matrix[i, k];
                    }
                }
            }

            // Back substitution phase
            for (int i = rows - 1; i >= 0; i--)
            {
                double sum = 0.0f;
                for (int j = i + 1; j < rows; j++)
                {
                    sum += matrix[i, j] * solution[j];
                }

                solution[i] = (matrix[i, cols - 1] - sum) / matrix[i, i];
            }

            return new List<double>(solution);
        }
    }
}