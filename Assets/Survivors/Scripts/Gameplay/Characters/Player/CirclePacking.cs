using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Survivors {

    public class CirclePacking : MonoBehaviour {

        public Transform largerCircleCenter;
        public float largerCircleRadius;
        public int numberOfSmallerCircles;
        public GameObject smallerCirclePrefab;

        private void Start() {
            ArrangeCircles();
        }

        private void ArrangeCircles() {
            List<Vector2> circleCenters = ApollonianGasket(largerCircleCenter.position, largerCircleRadius, numberOfSmallerCircles);

            foreach (Vector2 center in circleCenters) {
                float smallerCircleRadius = CalculateSmallerCircleRadius();

                GameObject smallerCircle = Instantiate(smallerCirclePrefab, center, Quaternion.identity);
                smallerCircle.transform.localScale = Vector3.one * smallerCircleRadius * 2;
            }
        }

        private List<Vector2> ApollonianGasket(Vector2 center, float radius, int numCircles) {
            List<Vector2> circleCenters = new List<Vector2>();
            PriorityQueue<Circle> queue = new PriorityQueue<Circle>();

            // Start with three mutually tangent circles
            Circle c1 = new Circle(center, radius);
            Circle c2 = new Circle(center + new Vector2(radius * 3, 0), radius);
            Circle c3 = new Circle(center + new Vector2(radius * 1.5f, radius * Mathf.Sqrt(3)), radius);

            queue.Enqueue(c1);
            queue.Enqueue(c2);
            queue.Enqueue(c3);

            while (circleCenters.Count < numCircles && queue.Count > 0) {
                Circle current = queue.Dequeue();
                circleCenters.Add(current.center);

                Circle next1 = current.PackWith(c1);
                Circle next2 = current.PackWith(c2);
                Circle next3 = current.PackWith(c3);

                queue.Enqueue(next1);
                queue.Enqueue(next2);
                queue.Enqueue(next3);
            }

            return circleCenters;
        }

        private float CalculateSmallerCircleRadius() {
            float maxAllowedRadius = (largerCircleRadius * 2) / Mathf.Sqrt(numberOfSmallerCircles);
            float padding = 0.1f;
            float smallerCircleRadius = maxAllowedRadius - padding;

            return smallerCircleRadius;
        }

    }

    class Circle : IComparable<Circle> {
        public Vector2 center;
        public float radius;

        public Circle(Vector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }

        public float Curvature() {
            return 1 / radius;
        }

        public Circle PackWith(Circle c) {
            float r = Mathf.Abs(radius - c.radius);
            float d = (center - c.center).magnitude;
            float newRadius = 1 / (1 / radius + 1 / c.radius + 2 * Mathf.Sqrt(r * r - d * d));
            Vector2 newCenter = center + (c.center - center).normalized * (newRadius - radius);
            return new Circle(newCenter, newRadius);
        }

        public int CompareTo(Circle other) {
            return radius.CompareTo(other.radius);
        }
    }

    class PriorityQueue<T> {
        private List<T> elements = new List<T>();

        public int Count { get { return elements.Count; } }

        public void Enqueue(T item) {
            elements.Add(item);
            int ci = elements.Count - 1;
            while (ci > 0) {
                int pi = (ci - 1) / 2;
                if (Comparer<T>.Default.Compare(elements[ci], elements[pi]) >= 0)
                    break;
                T tmp = elements[ci]; elements[ci] = elements[pi]; elements[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue() {
            int li = elements.Count - 1;
            T frontItem = elements[0];
            elements[0] = elements[li];
            elements.RemoveAt(li);

            --li;
            int pi = 0;
            while (true) {
                int ci = pi * 2 + 1;
                if (ci > li)
                    break;
                int rc = ci + 1;
                if (rc <= li && Comparer<T>.Default.Compare(elements[rc], elements[ci]) < 0)
                    ci = rc;
                if (Comparer<T>.Default.Compare(elements[pi], elements[ci]) <= 0)
                    break;
                T tmp = elements[pi]; elements[pi] = elements[ci]; elements[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }
    }
}
