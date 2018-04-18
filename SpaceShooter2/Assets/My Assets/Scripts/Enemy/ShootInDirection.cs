using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class ShootInDirection : MonoBehaviour
    {
        public static void Shoot(GameObject bullet, int bulletQuantity, float deviation, Vector3 spawnPosition, Vector2 shootDirection, float travelSpeed, bool shootAtDirection)
        {
            Assert.IsTrue(bulletQuantity > 0);

            int currentBulletQuantity = 0;
            if (shootAtDirection)
            {
                GameObject bullet1 = Instantiate(bullet, spawnPosition,
                    Quaternion.identity);
                bullet1.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * travelSpeed;
                bullet1.transform.eulerAngles += new Vector3(0, 0, Mathf.Atan(shootDirection.y / shootDirection.x) * Mathf.Rad2Deg);
                currentBulletQuantity++;
            }
            while (currentBulletQuantity < bulletQuantity)
            {
                Vector3 rotatedUnitVector = Quaternion.AngleAxis(Random.Range(-deviation, deviation), Vector3.forward) * shootDirection.normalized;
                GameObject bullet2 = Instantiate(bullet, spawnPosition, Quaternion.identity);
                bullet2.GetComponent<Rigidbody2D>().velocity = rotatedUnitVector.normalized * travelSpeed;
                bullet2.transform.eulerAngles += new Vector3(0, 0, Mathf.Atan(shootDirection.y / shootDirection.x) * Mathf.Rad2Deg);
                currentBulletQuantity++;
            }
        }

        public static void ShootEqualSpread(GameObject bullet, int bulletQuantity, float deviation, Vector3 spawnPosition, Vector2 shootDirection, float travelSpeed)
        {
            //if bullet quantity is equal to 1, use shoot and shoot at direction passed in
            if (bulletQuantity == 1)
            {
                Shoot(bullet, bulletQuantity, deviation, spawnPosition, shootDirection, travelSpeed, true);
                return;
            }
            Assert.IsTrue(bulletQuantity > 0);

            int currentBulletQuantity = 0;
            float curDeviation = deviation;
            float smallDeviation = deviation / (bulletQuantity - 1);

            while (currentBulletQuantity < bulletQuantity)
            {
                Vector3 rotatedUnitVector = Quaternion.AngleAxis(curDeviation, Vector3.forward) * shootDirection.normalized;
                GameObject bullet2 = Instantiate(bullet, spawnPosition, Quaternion.identity);
                bullet2.GetComponent<Rigidbody2D>().velocity = rotatedUnitVector.normalized * travelSpeed;
                bullet2.transform.eulerAngles += new Vector3(0, 0, Mathf.Atan(rotatedUnitVector.y / rotatedUnitVector.x) * Mathf.Rad2Deg);
                currentBulletQuantity++;
                curDeviation -= smallDeviation * 2;
            }
        }
    }
}
