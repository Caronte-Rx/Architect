using System;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public class EditModeEditor : Editor
    {
        protected ScriptableObject Target
            { get; set; }
        protected ScriptableObject TargetCopy
            { get; set; }

        private CameraData _cameraData;
        private SaveAndReturnGUI _saveAndReturnButtons;

        public virtual void OnDisable()
        {
            Tools.hidden = false;
            SceneView.lastActiveSceneView.isRotationLocked = false;
            SceneView.lastActiveSceneView.LookAtDirect(_cameraData.Postion, _cameraData.Rotation, _cameraData.Scale);

            _saveAndReturnButtons.SaveAndReturnButtonClick  -= OnSaveAndReturnClick;
            _saveAndReturnButtons.DiscardChangesButtonClick -= OnDiscardChangesClick;
        }

        public virtual void OnEnable()
        {
            SaveCameraData();

            Target = (target as IBaseEditMode).EditModeTarget;
            TargetCopy = Instantiate(Target);

            Tools.hidden = true;
            SceneView.lastActiveSceneView.isRotationLocked = true;

            _saveAndReturnButtons = new SaveAndReturnGUI((target as IBaseEditMode).PreviousScenePath);
            _saveAndReturnButtons.SaveAndReturnButtonClick  += OnSaveAndReturnClick;
            _saveAndReturnButtons.DiscardChangesButtonClick += OnDiscardChangesClick;
        }

        protected virtual void OnDiscardChangesClick(object target, EventArgs e)
        {

        }

        protected virtual void OnSaveAndReturnClick(object target, EventArgs e)
        {
            var tempName = Target.name;
            EditorUtility.CopySerialized(TargetCopy, Target);
            Target.name = tempName;
            EditorUtility.SetDirty(Target);
        }

        private void SaveCameraData()
        {
            _cameraData =
                new CameraData(SceneView.lastActiveSceneView.camera.transform);
        }

        protected struct CameraData
        {
            public Vector3 Postion; public Quaternion Rotation; public float Scale;

            public CameraData(Transform camera)
            {
                Postion     = camera.position;
                Rotation    = camera.rotation;
                Scale       = camera.localScale.magnitude;
            }
        }
    }
}
