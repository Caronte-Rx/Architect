using System;
using UnityEngine;

namespace Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : PropertyAttribute
    {
        public string Label 
            { get; private set; }

        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}
