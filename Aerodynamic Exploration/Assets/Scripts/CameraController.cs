using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("An array of transforms representing camera positions")]
    [SerializeField] private Transform[] povs;
    [Tooltip("The speed at which the camera follows the plane")]
    [SerializeField] private float speed;

    private int _index;
    private Vector3 _target;

    private void Start()
    {
        transform.position = povs[_index].position;
        transform.forward = povs[_index].forward;
        _target = povs[_index].position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) _index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) _index = 3;
        _target = povs[_index].position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
        transform.forward = povs[_index].forward;
    }
}
