using System;
using UnityEngine;

public class LandingAction : MonoBehaviour
{
    private GliderController _gliderController;

    private void Start()
    {
        _gliderController = GetComponentInParent<GliderController>();
        if (_gliderController == null)
        {
            Debug.LogError("GliderController not found");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && _gliderController != null)
        {
            _gliderController.enabled = false;
        }
    }
}
