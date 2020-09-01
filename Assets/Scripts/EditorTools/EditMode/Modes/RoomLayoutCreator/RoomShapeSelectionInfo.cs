#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace EditorTools
{
    public class RoomShapeSelectionInfo
    {
        public Vector2Int VertexPositionAtStartOfDrag
            { get; set; }

        public V2IntVertex VertexFocused
        {
            get => _vertexFocused;
            set
            {
                if (UpdateValue(ref _vertexFocused, value))
                    if (IsVertexFocused && IsEdgeFocused)
                        EdgeSelected = null;
            }
        }

        public V2IntVertex VertexSelected
        {
            get => _vertexSelected;
            set
            {
                _vertexFocused = value;
                if (UpdateValue(ref _vertexSelected, value))
                    if (IsVertexSelected && IsEdgeSelected)
                        EdgeSelected = null;
            }
        }

        public Vector2Int EdgePositionAtStartOfDrag
            { get; set; }

        public V2IntEdge EdgeFocused
        {
            get => _edgeFocused;
            set
            {
                if (UpdateValue(ref _edgeFocused, value))
                    if (IsVertexFocused && IsEdgeFocused)
                        VertexSelected = null;
            }
        }

        public V2IntEdge EdgeSelected
        {
            get => _edgeSelected;
            set
            {
                _edgeFocused = value;
                if (UpdateValue(ref _edgeSelected, value))
                    if (IsVertexSelected && IsEdgeSelected)
                        VertexSelected = null;
            }
        }

        public bool IsVertexFocused
            { get => _vertexFocused != null; }

        public bool IsVertexSelected
            { get => _vertexSelected != null; }

        public bool IsEdgeFocused
            { get => _edgeFocused != null; }

        public bool IsEdgeSelected
            { get => _edgeSelected != null; }

        private V2IntEdge _edgeSelected, _edgeFocused;
        private V2IntVertex _vertexSelected, _vertexFocused;

        public event EventHandler<EventArgs> SelectionChanged;

        public void DeselectCorner()
        {
            VertexSelected = null;
            VertexPositionAtStartOfDrag = Vector2Int.zero;
        }

        public void DeselectWall()
        {
            EdgeSelected = null;
            EdgePositionAtStartOfDrag = Vector2Int.zero;
        }

        public void SelectCorner()
        {
            if (IsVertexFocused)
                VertexSelected = VertexFocused;
        }

        public void SelectWall()
        {
            if (IsEdgeFocused)
                EdgeSelected = EdgeFocused;
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        private bool UpdateValue<T>(ref T value, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(value, newValue))
            {
                value = (T)Convert.ChangeType(newValue, typeof(T));
                OnSelectionChanged(EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
#endif
