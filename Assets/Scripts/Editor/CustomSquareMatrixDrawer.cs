using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CustomSquareMatrixDrawer : OdinValueDrawer<int[,]>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            int[,] value = this.ValueEntry.SmartValue;
            int innerSize = (int)Math.Sqrt(value.GetLength(0));
            float screenWidth = EditorGUIUtility.currentViewWidth;
            int cellSize = (int)((screenWidth - 90) / value.GetLength(1));

            GUILayout.BeginVertical();
            if (label != null)
            {
                GUILayout.Label(label);
            }

            // Iterate through the rows and columns of the matrix.
            for (int i = 0; i < value.GetLength(0); i++)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < value.GetLength(1); j++)
                {
                    Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(cellSize), GUILayout.Height(cellSize));

                    // Set the background color based on the cell value.
                    Color color = GetBackgroundColor(value[i, j]);
                    EditorGUI.DrawRect(rect, color);

                    // Add padding after every 3rd cell except the last one in the row.
                    if (j % innerSize == innerSize - 1 && j != value.GetLength(1) - 1)
                    {
                        GUILayout.Space(10);
                    }

                    EditorGUI.BeginChangeCheck();

                    // Center the value inside the cell using the fontStyle.
                    string labelString = value[i, j] == 0 ? "" : value[i, j].ToString();
                    EditorGUI.LabelField(rect, labelString, new GUIStyle()
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 20,
                        fontStyle = FontStyle.Bold,
                        normal = new GUIStyleState()
                        {
                            textColor = Color.white
                        }
                    });

                    if (EditorGUI.EndChangeCheck())
                    {
                        this.ValueEntry.SmartValue = value;
                    }
                }

                GUILayout.EndHorizontal();

                // Add padding after every 3rd row except the last row.
                if (i % innerSize == innerSize - 1 && i != value.GetLength(0) - 1)
                {
                    GUILayout.Space(10);
                }
            }

            GUILayout.EndVertical();
        }

        // Method to determine the background color based on cell value
        private Color GetBackgroundColor(int value)
        {
            return value == 0 ? Color.gray : Color.black;
        }

        protected override bool CanDrawValueProperty(InspectorProperty property)
        {
            return property.ValueEntry.TypeOfValue == typeof(int[,]);
        }
    }
}