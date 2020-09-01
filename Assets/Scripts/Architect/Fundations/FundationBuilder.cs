using System;
using UnityEngine;
using UnityEngine.Events;

namespace Architect
{
    public class FundationBuilder : MonoBehaviour
    {
        public UnityEvent Build = new UnityEvent();

        private bool isPending = true;

        public void Start()
        {
            OnBuild();
        }

        public void OnBuild()
        {
            if (!isPending)
                return;
            try
            {
                Build?.Invoke();
                isPending = false;
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Unable to build '{name}'. " +
                    $"Details: {ex.Message}");
                Destroy(gameObject);
            }
        }
    }
}
