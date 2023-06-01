using System;
using Data;
using DefaultNamespace;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelEditorWindow : OdinEditorWindow
    {
        private const string LevelSavePath = "Assets/Resources/LevelSaveDatas";

        public int id;

        [TableMatrix(HorizontalTitle = "Level Grid", SquareCells = true,
            IsReadOnly = true)]
        [BoxGroup("Level Grid", AnimateVisibility = true, HideWhenChildrenAreInvisible = true)]
        [HideLabel]
        [HideIf("IsLevelGridNull")]
        [SerializeField] private int[,] levelGrid;

        [PropertySpace(spaceBefore: 10)]
        [TableMatrix(HorizontalTitle = "Level Grid", SquareCells = true, DrawElementMethod = "DrawCell",
            IsReadOnly = true)]
        [BoxGroup("Solution Grid", AnimateVisibility = true, HideWhenChildrenAreInvisible = true)]
        [HideLabel]
        [HideIf("IsSolutionGridNull")]
        [SerializeField] private int[,] solutionGrid;


        [EnumToggleButtons]
        public LevelDifficulty levelDifficulty;

        [Button]
        public void GenerateLevel()
        {
            levelGrid = SudokuCreator.GenerateSudoku(levelDifficulty, out int[,] solution);
            solutionGrid = solution;
        }

        [Button]
        public void SaveLevel()
        {
            LevelSaveData saveData = CreateInstance<LevelSaveData>();
            saveData.id = id;
            saveData.difficulty = levelDifficulty;
            saveData.levelGrid = levelGrid;
            saveData.solutionGrid = solutionGrid;

            AssetDatabase.CreateAsset(saveData,
                $"{LevelSavePath}/Level{id}_{Enum.GetName(typeof(LevelDifficulty), levelDifficulty)}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(saveData);
        }

        private bool IsLevelGridNull()
        {
            return levelGrid == null;
        }

        private bool IsSolutionGridNull()
        {
            return solutionGrid == null;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            levelDifficulty = LevelDifficulty.Easy;
        }

        [MenuItem("Tools/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelEditorWindow>().Show();
        }
    }
}