using Unity.VisualScripting;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;
    [SerializeField] float startDelay;
    
    Vector3 startPosition;
    Vector3 endPosition;
    float movementFactor;

    private void Start() 
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;    
    }

    private void Update() 
    {
        movementFactor = Mathf.PingPong(Time.time - startDelay* speed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
