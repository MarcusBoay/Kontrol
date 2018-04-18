using UnityEngine;

namespace Kontrol
{
    public class EnemyAI2 : MonoBehaviour
    {
        public float xSpeed;
        //makes the enemy move more in curve or less curved
        public float yCurveModifier;
        //makes the enemy move in positive surve or negative curve
        public enum _yDirection
        {
            UP,
            DOWN
        }
        public _yDirection yDirection;
        private int yDirectionModifier;
        //initial y position
        private float yPos;
        //rotation properties
        public float myRotation;
        //for initializing gameobjects
        private GameObject myCamera;
        private Rigidbody2D rb2d;

        void Start()
        {
            //translation
            yPos = transform.position.y;
            //yDirection check
            if (yDirection == _yDirection.UP)
            {
                yDirectionModifier = 1;
            }
            else if (yDirection == _yDirection.DOWN)
            {
                yDirectionModifier = -1;
            }
            //getting their respective gameobjects
            rb2d = GetComponent<Rigidbody2D>();
            myCamera = GameObject.Find("Main Camera").gameObject;
        }

        void FixedUpdate()
        {
            //rotate enemy 
            transform.Rotate(new Vector3(0, 0, myRotation) * Time.deltaTime);
            //enemy move in negative x direction, exponential in y direction
            rb2d.MovePosition(new Vector2(transform.position.x + xSpeed, yPos + yDirectionModifier * Mathf.Exp(yCurveModifier * (transform.position.x - myCamera.transform.position.x))));
        }
    }
}
