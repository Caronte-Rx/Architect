using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class BaseGrid<T>
    {
        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }

        public T this[Vector2Int coordinate]
        {
            get => GetValue(coordinate.x, coordinate.y);
            set => SetValue(coordinate.x, coordinate.y, value);
        }

        public int Height
            { get => Size.y; }

        public int Width
            { get => Size.x; }

        public Vector2Int Size
            { get; private set; }

        private T[,] _grid;

        public BaseGrid(int width, int height, T defaultValue = default)
            : this(new Vector2Int(width, height), defaultValue) { }

        public BaseGrid(Vector2Int size, T defaultValue = default)
        {
            Size = size;
            _grid = new T[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _grid[x, y] = defaultValue;
        }

        public void Fill(Vector2Int position, T newValue, T toFill)
        {
            if (IsIndexOutOfBonds(position))
                return;
            if (EqualityComparer<T>.Default.Equals(this[position], toFill))
            {
                this[position] = newValue;
                Fill(position + Vector2Int.up       , newValue, toFill);
                Fill(position + Vector2Int.right    , newValue, toFill);
                Fill(position + Vector2Int.down     , newValue, toFill);
                Fill(position + Vector2Int.left     , newValue, toFill);
            }
        }

        public bool IsIndexOutOfBonds(Vector2Int index)
        {
            return
                index.x < 0 || index.x >= Width ||
                index.y < 0 || index.y >= Height;
        }

        private T GetValue(int x, int y)
        {
            try
            {
                return _grid[x, y];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception($"{ex.Message} " +
                    $"Index: {new Vector2Int(x, y)} Bounds: {Size - Vector2Int.one}.");
            }
        }

        private void SetValue(int x, int y, T value)
        {
            try
            {
                _grid[x, y] = value;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception($"{ex.Message} " +
                    $"Index: {new Vector2Int(x, y)} Bounds: {Size - Vector2Int.one}.");
            }
        }
    }
}