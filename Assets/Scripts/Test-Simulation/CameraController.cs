using System;
using UnityEngine;

namespace Test_Simulation
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector2 limitX = new Vector2(-10, 10);
        [SerializeField] private Vector2 limitZ = new Vector2(-10, 10);
        
        Vector3 targetPosition;
        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            targetPosition = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Clamp(transform.position.x -horizontal, limitX.x, limitX.y), 
                    transform.position.y, Mathf.Clamp(transform.position.z-vertical, limitZ.x, limitZ.y)),
                speed * Time.deltaTime);
            
        }

        private void LateUpdate()
        {
            transform.position = targetPosition;
        }
    }
}