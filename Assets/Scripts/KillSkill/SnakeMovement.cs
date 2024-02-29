using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;

        public static TransformData From(Transform t) => new()
        {
            position = t.position,
            rotation = t.rotation,
        };
    }
    
    public class SnakeMovement : MonoBehaviour
    {
        public List<TransformData> points = new();
        public int maxPoints = 100;
        public float minDistance = 0.5f;
        public float moveSpeed;
        public float lookSmooth = 0.3f;
        public float lookMaxSpeed = 10f;

        private Vector3 lookVector;
        private Vector3 lookVelocity;

        private void Update()
        {
            Move();
            RecordPoints();
        }

        private void RecordPoints()
        {
            if (points.Count == 0)
            {
                points.Add(TransformData.From(transform));
                return;
            }

            var latest = points[^1];
            var distance = Vector3.Distance(latest.position, transform.position);
            if (distance < minDistance) return;
            
            points.Add(TransformData.From(transform));
            if (points.Count > maxPoints)
            {
                var toDelete = points.Count - maxPoints;
                for (int i = 0; i < toDelete; i++)
                    points.RemoveAt(0);
            }
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var movement = new Vector3(horizontal, 0f, vertical);
            transform.position +=  movement * (moveSpeed * Time.deltaTime);
            if (movement.magnitude > 0.1f)
            {
                lookVector = Vector3.SmoothDamp(lookVector, movement, ref lookVelocity, lookSmooth, lookMaxSpeed);
            }
            transform.forward = lookVector;
        }

        private void OnDrawGizmos()
        {
            if (points.Count == 0) return;
            
            Gizmos.color = Color.red;

            var tempPos = points[0].position;
            
            foreach (var point in points)
            {
                Gizmos.DrawLine(tempPos, point.position);
                Gizmos.DrawSphere(tempPos, minDistance *0.1f);
                tempPos = point.position;
            }
        }
    }
}