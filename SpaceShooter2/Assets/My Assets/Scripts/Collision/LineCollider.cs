using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kontrol
{
    public class LineCollider : MonoBehaviour
    {
        public BoxCollider2D lineCollider;

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            UpdateColliderToLine(GetComponent<LineRenderer>(), GetComponent<LineRenderer>().GetPosition(0), GetComponent<LineRenderer>().GetPosition(1));
        }

        // Thanks to zackgrizzle! http://www.zackgrizzle.ninja/tutorials.html
        public void UpdateColliderToLine(LineRenderer line, Vector3 startPoint, Vector3 endPoint)
        {
            // get width of collider from line 
            float lineWidth = line.endWidth;
            // get the length of the line using the Distance method
            float lineLength = Vector3.Distance(startPoint, endPoint);
            // size of collider is set where X is length of line, Y is width of line
            //z will be how far the collider reaches to the sky
            lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
            // get the midPoint
            Vector3 midPoint = (startPoint + endPoint) / 2;
            // move the created collider to the midPoint
            lineCollider.transform.position = midPoint + transform.position;


            //heres the beef of the function, Mathf.Atan2 wants the slope, be careful it wants it in a weird form
            //it will divide for you so just plug in your (y2-y1),(x2,x1)
            float angle = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

            // angle now holds our answer but it's in radians, we want degrees
            // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
            angle *= Mathf.Rad2Deg;

            //Right now this angle is useful for rotating the line we just drew to line up to the x axis.
            //Since were are rotating the collider, not the line, we need to multiply by -1
            //angle *= -1;
            // now apply the rotation to the collider's transform, carful where you put the angle variable
            // in 3d space you want it to rotate on your y axis
            lineCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
