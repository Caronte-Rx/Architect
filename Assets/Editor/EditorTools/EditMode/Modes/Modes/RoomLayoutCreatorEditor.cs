using System;
using System.Collections.Generic;
using Architect;
using Core;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    [CustomEditor(typeof(RoomLayoutCreator))]
    public class RoomLayoutCreatorEditor : EditModeEditor
    {
        protected RoomLayout RoomLayout
            { get => TargetCopy as RoomLayout; }

        protected RoomShapeSelectionInfo SelectionInfo
            { get => _roomShapeHandler.SelectionInfo; }

        protected RoomShapeData ShapeData
            { get => RoomLayout.ShapeData; }

        private bool _isWallsSectionFoldedOut = true;
        private InspectorButton _resetShapeButton;

        private RoomShapeHandler _roomShapeHandler;

        public override void OnDisable()
        {
            base.OnDisable();
            _roomShapeHandler.Dispose();
            Undo.undoRedoPerformed -= OnUndoOrRedo;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (ShapeData.Shape.Count < 4)
            {
                ShapeData.Shape = GetDefaultShapeVetices();

                ShapeData.Walls.Clear();
                for (int i = 0; i < ShapeData.Shape.Count; i++)
                    ShapeData.Walls.Add(new RoomWallData());
            }

            _roomShapeHandler = new RoomShapeHandler(ShapeData);
            var shapeCenter =
                _roomShapeHandler.RoomShape.CalculateMask().center.ToXZVector3(0);
            SceneView.lastActiveSceneView.LookAtDirect(shapeCenter, Quaternion.Euler(90, 0, 0), 10f);

            _resetShapeButton = new InspectorButton("Reset Shape"); 
            _resetShapeButton.Click += OnResetShapeButtonClick;

            Undo.undoRedoPerformed += OnUndoOrRedo;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox(
                "Shift + Left Click: Create or delete a corner.\n", MessageType.Info);

            EditorGUI.BeginChangeCheck();
            {
                _isWallsSectionFoldedOut =
                    EditorGUILayout.Foldout(_isWallsSectionFoldedOut, "Walls");
                if (_isWallsSectionFoldedOut)
                {
                    int i = 0;
                    foreach (var corner in _roomShapeHandler.RoomShape)
                    {
                        var wallIterator = corner.EdgeB;
                        GUILayout.BeginHorizontal();
                        {
                            var isSelected = wallIterator == SelectionInfo.EdgeSelected;
                            GUILayout.Label($"Wall [{i:00}]");

                            var wallData = _roomShapeHandler.WallsData[i];
                            wallData.AllowConnection = GUILayout.Toggle(wallData.AllowConnection,
                                new GUIContent("Allow Connection", "Allow door creation"));

                            GUI.enabled = !isSelected;
                            if (GUILayout.Button("Select"))
                            {
                                SelectionInfo.EdgeFocused = wallIterator;
                                SelectionInfo.SelectWall();
                            }
                            GUI.enabled = true;
                        }
                        GUILayout.EndHorizontal();
                        i++;
                    }
                }
                _resetShapeButton.OnInspectorGUI();
            }
            if (EditorGUI.EndChangeCheck())
            {
                _roomShapeHandler.NeedsRepaint = true;
                SceneView.RepaintAll();
            }
        }

        public void OnSceneGUI()
        {
            var guiEvent = Event.current;
            switch (guiEvent.type)
            {
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    break;
                case EventType.Repaint:
                    HandleDrawEvent();
                    break;
                default:
                    HandleInputEvent(guiEvent);
                    break;
            }
            if (_roomShapeHandler.NeedsRepaint)
            {
                EditorUtility.SetDirty(target);
                HandleUtility.Repaint();
            }
        }

        protected override void OnSaveAndReturnClick(object sender, EventArgs e)
        {
            _roomShapeHandler.RoomShape.MoveShapeTo(Vector2Int.zero);

            ShapeData.Walls = new List<RoomWallData>();

            int i = 0;
            foreach (var corner in _roomShapeHandler.RoomShape)
            {
                var wallIterator = corner.EdgeB;
                var wallData =
                    new RoomWallData(wallIterator.VertexA.Position, wallIterator.VertexB.Position);
                wallData.SerializedSide = wallData.Direction.ToSide();
                if (_roomShapeHandler.WallsData[i] != null)
                    wallData.AllowConnection = _roomShapeHandler.WallsData[i].AllowConnection;

                switch (wallData.Direction)
                {
                    case Direction.North:
                        wallData.CornerB += Vector2Int.down;
                        break;
                    case Direction.West:
                        wallData.CornerA += Vector2Int.left;
                        break;
                    case Direction.South:
                        wallData.CornerA += Vector2Int.left + Vector2Int.down;
                        wallData.CornerB += Vector2Int.left;
                        break;
                    case Direction.East:
                        wallData.CornerA += Vector2Int.down;
                        wallData.CornerB += Vector2Int.down + Vector2Int.left;
                        break;
                }
                ShapeData.Walls.Add(wallData);
                i++;
            }
            ShapeData.RoomSize =
                _roomShapeHandler.RoomShape.CalculateMask().size;

            base.OnSaveAndReturnClick(sender, e);
        }

        private List<Vector2Int> GetDefaultShapeVetices()
        {
            const int EDGE_LENGTH = 10;
            return new List<Vector2Int> {
                Vector2Int.zero  * EDGE_LENGTH,
                Vector2Int.up    * EDGE_LENGTH,
                Vector2Int.one   * EDGE_LENGTH,
                Vector2Int.right * EDGE_LENGTH
            };
        }

        private Vector2 GetMousePositionInPlane(Vector3 mousePosition)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePosition);
            float distanceToPlane =
                (0 - mouseRay.origin.y) / mouseRay.direction.y;
            return mouseRay.GetPoint(distanceToPlane).ToXZVector2();
        }

        private void HandleDrawEvent()
        {
            _roomShapeHandler.DrawShape();
        }

        private void HandleInputEvent(Event guiEvent)
        {
            var mouseXZPosition = GetMousePositionInPlane(guiEvent.mousePosition);
            switch (guiEvent.type)
            {
                case EventType.MouseDrag:
                    if (guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
                        OnLeftMouseDrag(mouseXZPosition);
                    break;
                case EventType.MouseDown:
                    if (guiEvent.button == 0)
                    {
                        switch (guiEvent.modifiers)
                        {
                            case EventModifiers.None:
                                OnLeftMouseDown();
                                break;
                            case EventModifiers.Shift:
                                OnLeftShiftMouseDown(mouseXZPosition);
                                break;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (guiEvent.button == 0)
                        OnLeftMouseUp();
                    break;
            }
            _roomShapeHandler.UpdateShape(mouseXZPosition);
        }

        private void OnLeftMouseDown()
        {
            _roomShapeHandler.SelectUnderMouse();
        }

        private void OnLeftMouseDrag(Vector2 mousePosition)
        {
            if (SelectionInfo.IsVertexSelected)
                _roomShapeHandler.MoveCornerUnderMouse(mousePosition);

            if (SelectionInfo.IsEdgeSelected)
                _roomShapeHandler.MoveWallUnderMouse(mousePosition);
        }

        private void OnLeftMouseUp()
        {
            if (SelectionInfo.IsVertexSelected)
            {
                var position = SelectionInfo.VertexSelected.Position;
                _roomShapeHandler.MoveCornerUnderMouse(SelectionInfo.VertexPositionAtStartOfDrag);

                Undo.RecordObject(RoomLayout, "Move Corner");
                _roomShapeHandler.MoveCornerUnderMouse(position);
                _roomShapeHandler.DeselectCorner();
            }
            if (SelectionInfo.IsEdgeSelected)
            {
                var position = SelectionInfo.EdgeSelected.VertexA.Position;
                var hasIntersections = _roomShapeHandler.HasIntersections();

                _roomShapeHandler.MoveWallUnderMouse(SelectionInfo.EdgePositionAtStartOfDrag);
                _roomShapeHandler.RemoveDuplicateCorners();
                if (!hasIntersections)
                {
                    Undo.RecordObject(RoomLayout, "Move Wall");
                    _roomShapeHandler.MoveWallUnderMouse(position);
                }
                _roomShapeHandler.DeselectWall();
            }
        }

        private void OnLeftShiftMouseDown(Vector2 mousePosition)
        {
            if (SelectionInfo.IsVertexFocused)
            {
                Undo.RecordObject(RoomLayout, "Remove Corner");
                _roomShapeHandler.DeleteCornerUnderMouse();
            }
            if (SelectionInfo.IsEdgeFocused)
            {
                Undo.RecordObject(RoomLayout, "Add Corner");
                _roomShapeHandler.CreateCornerUnderMouse(mousePosition);
            }
        }

        private void OnResetShapeButtonClick(object sender, EventArgs args)
        {
            Undo.RecordObject(RoomLayout, "Reset Shape");

            ShapeData.Shape = GetDefaultShapeVetices();
            ShapeData.Walls.Clear();
            for (int i = 0; i < ShapeData.Shape.Count; i++)
                ShapeData.Walls.Add(new RoomWallData());

            _roomShapeHandler = new RoomShapeHandler(ShapeData);
        }

        private void OnUndoOrRedo()
        {
            _roomShapeHandler = new RoomShapeHandler(ShapeData);
        }
    }
}