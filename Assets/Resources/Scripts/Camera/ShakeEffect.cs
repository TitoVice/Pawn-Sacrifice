using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    public Transform cameraTransform;
    
    // Desired duration of the shake effect
    private float shakeDuration = 0f;
    
    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.2f;
    
    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;
    
    // The initial position of the GameObject
    Vector3 initialPosition;

    bool shaking = false;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            initialPosition = cameraTransform.localPosition;
            shakeDuration = 0.1f;
            shaking = true;
        }

        if (shaking)
        {
            if (shakeDuration > 0)
            {
                cameraTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                shakeDuration = 0f;
                cameraTransform.localPosition = initialPosition;
                shaking = false;
            }
        }
        
    }
}
