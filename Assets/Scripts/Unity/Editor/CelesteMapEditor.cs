using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Unity.Celeste.Editor
{
    /// <summary>
    /// Unity-based map editor with Loenn compatibility
    /// Allows editing Celeste levels within Unity while maintaining compatibility with Loenn format
    /// </summary>
    public class CelesteMapEditor : EditorWindow
    {
        [Header("Loenn Compatibility")]
        public bool maintainLoennCompatibility = true;
        public string loennMapsDirectory = "Maps";
        
        [Header("Editor Settings")]
        public int gridSize = 8;
        public bool showGrid = true;
        public bool snapToGrid = true;
        
        private Vector2 scrollPosition;
        private string currentMapPath;
        private CelesteMap currentMap;
        private Tool selectedTool = Tool.Select;
        
        private enum Tool
        {
            Select,
            Brush,
            Rectangle,
            Entity,
            Decal
        }
        
        [MenuItem("Celeste/Map Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<CelesteMapEditor>("Celeste Map Editor");
            window.minSize = new Vector2(800, 600);
            window.Show();
        }
        
        void OnGUI()
        {
            DrawToolbar();
            
            EditorGUILayout.BeginHorizontal();
            DrawSidebar();
            DrawMapView();
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            if (GUILayout.Button("New", EditorStyles.toolbarButton))
            {
                CreateNewMap();
            }
            
            if (GUILayout.Button("Open", EditorStyles.toolbarButton))
            {
                OpenMap();
            }
            
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                SaveMap();
            }
            
            GUILayout.Space(20);
            
            selectedTool = (Tool)GUILayout.Toolbar((int)selectedTool, 
                new string[] { "Select", "Brush", "Rectangle", "Entity", "Decal" }, 
                EditorStyles.toolbarButton);
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Loenn Export", EditorStyles.toolbarButton))
            {
                ExportToLoennFormat();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSidebar()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            
            EditorGUILayout.LabelField("Map Properties", EditorStyles.boldLabel);
            
            if (currentMap != null)
            {
                currentMap.name = EditorGUILayout.TextField("Name", currentMap.name);
                currentMap.width = EditorGUILayout.IntField("Width", currentMap.width);
                currentMap.height = EditorGUILayout.IntField("Height", currentMap.height);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tool Settings", EditorStyles.boldLabel);
            
            gridSize = EditorGUILayout.IntSlider("Grid Size", gridSize, 1, 32);
            showGrid = EditorGUILayout.Toggle("Show Grid", showGrid);
            snapToGrid = EditorGUILayout.Toggle("Snap to Grid", snapToGrid);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Loenn Compatibility", EditorStyles.boldLabel);
            
            maintainLoennCompatibility = EditorGUILayout.Toggle("Loenn Compatible", maintainLoennCompatibility);
            loennMapsDirectory = EditorGUILayout.TextField("Maps Directory", loennMapsDirectory);
            
            if (GUILayout.Button("Import from Loenn"))
            {
                ImportFromLoenn();
            }
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawMapView()
        {
            EditorGUILayout.BeginVertical();
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            Rect mapRect = GUILayoutUtility.GetRect(800, 600);
            
            if (showGrid)
            {
                DrawGrid(mapRect);
            }
            
            if (currentMap != null)
            {
                DrawMap(mapRect);
            }
            
            HandleMapInput(mapRect);
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawGrid(Rect rect)
        {
            Handles.BeginGUI();
            Handles.color = Color.gray * 0.5f;
            
            for (float x = rect.x; x < rect.xMax; x += gridSize)
            {
                Handles.DrawLine(new Vector3(x, rect.y), new Vector3(x, rect.yMax));
            }
            
            for (float y = rect.y; y < rect.yMax; y += gridSize)
            {
                Handles.DrawLine(new Vector3(rect.x, y), new Vector3(rect.xMax, y));
            }
            
            Handles.EndGUI();
        }
        
        private void DrawMap(Rect rect)
        {
            // Draw map tiles, entities, and decorations
            // TODO: Implement visual representation of the map
        }
        
        private void HandleMapInput(Rect rect)
        {
            Event e = Event.current;
            
            if (rect.Contains(e.mousePosition))
            {
                switch (selectedTool)
                {
                    case Tool.Brush:
                        HandleBrushTool(e);
                        break;
                    case Tool.Rectangle:
                        HandleRectangleTool(e);
                        break;
                    case Tool.Entity:
                        HandleEntityTool(e);
                        break;
                }
            }
        }
        
        private void HandleBrushTool(Event e) { }
        private void HandleRectangleTool(Event e) { }
        private void HandleEntityTool(Event e) { }
        
        private void CreateNewMap()
        {
            currentMap = new CelesteMap
            {
                name = "New Map",
                width = 320,
                height = 180
            };
            currentMapPath = "";
        }
        
        private void OpenMap()
        {
            string path = EditorUtility.OpenFilePanel("Open Celeste Map", "", "bin");
            if (!string.IsNullOrEmpty(path))
            {
                LoadMapFromFile(path);
            }
        }
        
        private void SaveMap()
        {
            if (currentMap == null) return;
            
            if (string.IsNullOrEmpty(currentMapPath))
            {
                currentMapPath = EditorUtility.SaveFilePanel("Save Celeste Map", "", currentMap.name, "bin");
            }
            
            if (!string.IsNullOrEmpty(currentMapPath))
            {
                SaveMapToFile(currentMapPath);
            }
        }
        
        private void LoadMapFromFile(string path)
        {
            // TODO: Implement map loading from Celeste format
            Debug.Log($"Loading map from: {path}");
        }
        
        private void SaveMapToFile(string path)
        {
            // TODO: Implement map saving to Celeste format
            Debug.Log($"Saving map to: {path}");
        }
        
        private void ExportToLoennFormat()
        {
            if (currentMap == null) return;
            
            // TODO: Export current map to Loenn-compatible format
            Debug.Log("Exporting to Loenn format");
        }
        
        private void ImportFromLoenn()
        {
            string path = EditorUtility.OpenFilePanel("Import from Loenn", "", "bin");
            if (!string.IsNullOrEmpty(path))
            {
                // TODO: Import map from Loenn format
                Debug.Log($"Importing from Loenn: {path}");
            }
        }
    }
    
    // Celeste map data structure
    [System.Serializable]
    public class CelesteMap
    {
        public string name;
        public int width;
        public int height;
        public List<CelesteRoom> rooms = new List<CelesteRoom>();
    }
    
    [System.Serializable]
    public class CelesteRoom
    {
        public string name;
        public int x, y, width, height;
        public List<CelesteEntity> entities = new List<CelesteEntity>();
    }
    
    [System.Serializable]
    public class CelesteEntity
    {
        public string type;
        public float x, y;
        public Dictionary<string, object> attributes = new Dictionary<string, object>();
    }
}
#endif