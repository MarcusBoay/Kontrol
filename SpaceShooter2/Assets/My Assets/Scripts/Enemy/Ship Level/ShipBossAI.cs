using System.Collections;
using UnityEngine;

namespace Kontrol
{
    public class ShipBossAI : MonoBehaviour
    {
        public GameObject sceneChanger;
        public GameObject flasher;

        public GameObject[] outerCores;
        public GameObject[] innerCores;
        public GameObject centerCore;

        public Animator[] outerCoresAnim;
        public Animator[] innerCoresAnim;
        public Animator centerCoreAnim;

        public GameObject laserShooter;
        public GameObject rotatingLaserShooter;
        public GameObject bigBullet;
        public GameObject homingMissile;
        public GameObject bomb;

        private GameObject player;
        private Vector2 playerPosition;

        private AudioManager audioManager;

        [Header("General variables")]
        public _bossStateA[] phaseAAlgorithm;
        public _bossStateB[] phaseBAlgorithm;
        public _bossStateC[] phaseCAlgorithm;
        public _bossStateE[] phaseEAlgorithm;

        public string laserShooterSoundName;
        public string bigBulletSoundName;
        public string homingMissileSoundName;
        public string bombSoundName;

        public float phaseChangeInvulDuration;

        private float nextPhaseTime;
        private int phaseIndex;
        private float nextShoot1;
        private float nextShoot2;
        private int currentShootQuantity;
        private int shooterIndex;

        private bool moveBoss;
        private bool hasBeenA;
        private bool hasBeenB;
        private bool hasBeenC;
        private bool hasBeenD;
        private bool hasBeenE;
        private bool hasBeenF;

        [Header("Spawning variables")]
        public float spawnTime;

        [Header("Phase A1 variables")]
        public float invulTimeA1;
        public float firstShootA1;
        public bool hasShotA1;

        [Header("Phase A2 variables")]
        public int maxShootQuantityA2;
        public int shotQuantityA2;
        public float bulletSpeedA2;
        public float deviationA2;
        public float shootIntervalA2;
        public float firstShootA2;

        [Header("Phase A3 variables")]
        public int maxShootQuantityA3;
        public float shootIntervalA3;
        public float firstShootA3;

        [Header("Phase B1 variables")]
        public int maxShootQuantityB1;
        public int shotQuantityB1;
        public float bulletSpeedB1;
        public float deviationB1;
        public float shootIntervalB1;
        public float firstShootB1;

        [Header("Phase B2 variables")]
        public int maxShootQuantityB2;
        public float shootIntervalB2;
        public float firstShootB2;

        [Header("Phase B3 variables")]
        public float invulTimeB3;
        public float firstShootB3;
        public bool hasShotB3;

        [Header("Phase C1 variables")]
        public int maxShootQuantityC1;
        public int shotQuantityC1;
        public float bulletSpeedC1;
        public float deviationC1;
        public float shootIntervalC1;
        public float firstShootC1;

        [Header("Phase C2 variables")]
        public float invulTimeC2;
        public float firstShootC2;
        public bool hasShotC2;

        [Header("Phase D variables")]
        public int shotQuantityD;
        public float bulletSpeedD;
        public float deviationD;
        public float shootIntervalD;
        public float firstShootD;

        [Header("Phase E1 variables")]
        public int maxShootQuantityE1;
        public float shootIntervalE1;
        public float firstShootE1;

        [Header("Phase E2 variables")]
        public float invulTimeE2;
        public float firstShootE2;
        public bool hasShotE2;

        [Header("Phase F variables")]
        public float shootIntervalF;
        public float firstShootF;

        [Header("Death variables")]
        public GameObject SmallExplosion;
        public GameObject MediumExplosion;
        public GameObject BigExplosion;
        public GameObject ExplosionArea;
        public string smallExplosionSoundName;
        public string mediumExplosionSoundName;
        public string bigExplosionSoundName;
        public float smallExplosionStartInterval;
        public float smallExplosionDecrement;

        public enum _bossState
        {
            SPAWNING,
            PHASE_A,
            PHASE_B,
            PHASE_C,
            PHASE_D,
            PHASE_E,
            PHASE_F,
            DEAD
        }

        public enum _bossStateA
        {
            PHASE_1,
            PHASE_2,
            PHASE_3
        }

        public enum _bossStateB
        {
            PHASE_1,
            PHASE_2,
            PHASE_3
        }

        public enum _bossStateC
        {
            PHASE_1,
            PHASE_2
        }

        public enum _bossStateE
        {
            PHASE_1,
            PHASE_2
        }

        [Header("Current boss states")]
        public _bossState bossSuperstate;
        public _bossStateA bossSubstateA;
        public _bossStateB bossSubstateB;
        public _bossStateC bossSubstateC;
        public _bossStateE bossSubstateE;

        void Start()
        {
            //setting boss state when spawned
            bossSuperstate = _bossState.SPAWNING;
            nextPhaseTime = Time.time;
            currentShootQuantity = 0;
            shooterIndex = 0;
            phaseIndex = 0;
            nextShoot1 = -999;
            nextShoot2 = -999;
            moveBoss = false;

            flasher = GameObject.Find("Canvas").transform.Find("Flash").gameObject;

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }

            try
            {
                player = GameObject.Find("Player").gameObject;
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            //set core to be invulnerable
            for (int i = 0; i < 2; i++)
            {
                if (innerCores[i].gameObject != null)
                {
                    innerCores[i].transform.tag = "InvulEnemy";
                }

                if (outerCores[i].gameObject != null)
                {
                    outerCores[i].transform.tag = "InvulEnemy";
                }
            }
            if (centerCore.gameObject != null)
            {
                centerCore.transform.tag = "InvulEnemy";
            }
        }

        void FixedUpdate()
        {
            //find player in case player is dead
            try
            {
                if (player == null)
                {
                    player = GameObject.Find("Player").gameObject;
                }
                //test
                player = GameObject.Find("Player").gameObject;

                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            //add camera movement to boss
            //transform.position += new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0);

            //check cores for phase change
            if (bossSuperstate == _bossState.SPAWNING || bossSuperstate == _bossState.DEAD)
            {
                //do nothing
            }
            else if (bossSuperstate != _bossState.DEAD && outerCores[0].gameObject == null && outerCores[1].gameObject == null && innerCores[0].gameObject == null && innerCores[1].gameObject == null && centerCore == null)
            {
                bossSuperstate = _bossState.DEAD;
                StartCoroutine(BossDie());
                StartCoroutine(RandomExplosions());
            }
            else if (!hasBeenA && bossSuperstate != _bossState.PHASE_A && outerCores[0].gameObject != null && outerCores[1].gameObject != null && innerCores[0].gameObject != null && innerCores[1].gameObject != null && centerCore != null)
            {
                hasBeenA = true;
                bossSuperstate = _bossState.PHASE_A;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 1;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }
            else if (!hasBeenF && bossSuperstate != _bossState.PHASE_F && outerCores[0].gameObject == null && outerCores[1].gameObject == null && innerCores[0].gameObject == null && innerCores[1].gameObject == null && centerCore != null)
            {
                hasBeenF = true;
                hasBeenC = true;
                bossSuperstate = _bossState.PHASE_F;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 0;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }
            else if (!hasBeenD && bossSuperstate != _bossState.PHASE_D && outerCores[0].gameObject == null && outerCores[1].gameObject == null && centerCore == null)
            {
                hasBeenD = true;
                hasBeenB = true;
                hasBeenC = true;
                bossSuperstate = _bossState.PHASE_D;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 1;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }
            else if (!hasBeenE && bossSuperstate != _bossState.PHASE_E && innerCores[0].gameObject == null && innerCores[1].gameObject == null && centerCore == null)
            {
                hasBeenE = true;
                hasBeenB = true;
                bossSuperstate = _bossState.PHASE_E;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 1;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }
            else if (!hasBeenB && bossSuperstate != _bossState.PHASE_B && centerCore == null)
            {
                hasBeenB = true;
                bossSuperstate = _bossState.PHASE_B;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 1;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }
            else if (!hasBeenC && bossSuperstate != _bossState.PHASE_C && outerCores[0].gameObject == null && outerCores[1].gameObject == null)
            {
                hasBeenC = true;
                bossSuperstate = _bossState.PHASE_C;
                StartCoroutine(InvulBoss());
                //reset all variables
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 1;
                nextShoot1 = Time.time + phaseChangeInvulDuration;
                nextShoot2 = Time.time + phaseChangeInvulDuration;
                nextPhaseTime = Time.time + phaseChangeInvulDuration;
            }

            switch (bossSuperstate)
            {
                case (_bossState.SPAWNING):
                    //change phase
                    if (Time.time > nextPhaseTime + spawnTime)
                    {
                        nextPhaseTime = Time.time;

                        bossSuperstate = _bossState.PHASE_A;

                        bossSubstateA = phaseAAlgorithm[phaseIndex];
                        ++phaseIndex;
                    }
                    break;

                case (_bossState.PHASE_A):
                    switch (bossSubstateA)
                    {
                        case (_bossStateA.PHASE_1):
                            //laser shooter
                            if (!hasShotA1 && Time.time >= nextPhaseTime + firstShootA1)
                            {
                                hasShotA1 = true;
                                GameObject _shooter = Instantiate(laserShooter, centerCore.transform.position, transform.rotation);
                                _shooter.transform.parent = gameObject.transform;

                                audioManager.PlaySound(laserShooterSoundName);

                                ShieldBoss();
                            }

                            //change phase
                            if (Time.time >= nextPhaseTime + invulTimeA1 + firstShootA1)
                            {
                                UnshieldBoss();

                                hasShotA1 = false;

                                currentShootQuantity = 0;
                                shooterIndex = 0;
                                if (phaseIndex >= phaseAAlgorithm.Length)
                                {
                                    phaseIndex = 0;
                                }
                                bossSubstateA = phaseAAlgorithm[phaseIndex];
                                phaseIndex++;
                                nextShoot1 = Time.time;
                                nextShoot2 = Time.time;
                                nextPhaseTime = Time.time;
                            }
                            break;

                        case (_bossStateA.PHASE_2):
                            //spread shoot
                            if (Time.time >= shootIntervalA2 + nextShoot1 + firstShootA2)
                            {
                                nextShoot1 = Time.time - firstShootA2;

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }

                                if (innerCores[shooterIndex].gameObject != null)
                                {
                                    Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - innerCores[shooterIndex].transform.position;
                                    ShootInDirection.ShootEqualSpread(bigBullet, shotQuantityA2, deviationA2, innerCores[shooterIndex].transform.position, distToPlayer, bulletSpeedA2);
                                }

                                audioManager.PlaySound(bigBulletSoundName);

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }
                                else if (shooterIndex == 0)
                                {
                                    ++shooterIndex;
                                }
                                else
                                {
                                    shooterIndex = 0;
                                }

                                ++currentShootQuantity;
                            }

                            ChangePhaseAOnBullet(maxShootQuantityA2, phaseAAlgorithm);
                            break;

                        case (_bossStateA.PHASE_3):
                            //missile
                            if (Time.time >= shootIntervalA3 + nextShoot1 + firstShootA3)
                            {
                                nextShoot1 = Time.time - firstShootA3;

                                for (int i = 0; i < 2; i++)
                                {
                                    if (outerCores[i].gameObject != null)
                                    {
                                        Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - outerCores[i].transform.position;
                                        Instantiate(homingMissile, outerCores[i].transform.position, Quaternion.Euler(0, 0, 180f));
                                    }
                                }

                                audioManager.PlaySound(homingMissileSoundName);

                                ++currentShootQuantity;
                            }

                            ChangePhaseAOnBullet(maxShootQuantityA3, phaseAAlgorithm);
                            break;

                    }
                    break;

                case (_bossState.PHASE_B):
                    switch (bossSubstateB)
                    {
                        case (_bossStateB.PHASE_1):
                            //spread shoot
                            if (Time.time >= shootIntervalB1 + nextShoot1 + firstShootB1)
                            {
                                nextShoot1 = Time.time - firstShootB1;

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }

                                if (innerCores[shooterIndex].gameObject != null)
                                {
                                    Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - innerCores[shooterIndex].transform.position;
                                    ShootInDirection.ShootEqualSpread(bigBullet, shotQuantityA2, deviationA2, innerCores[shooterIndex].transform.position, distToPlayer, bulletSpeedA2);
                                }

                                audioManager.PlaySound(bigBulletSoundName);

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }
                                else if (shooterIndex == 0)
                                {
                                    ++shooterIndex;
                                }
                                else
                                {
                                    shooterIndex = 0;
                                }

                                ++currentShootQuantity;
                            }

                            ChangePhaseBOnBullet(maxShootQuantityB1, phaseBAlgorithm);
                            break;

                        case (_bossStateB.PHASE_2):
                            //missile
                            if (Time.time >= shootIntervalB2 + nextShoot1 + firstShootB2)
                            {
                                nextShoot1 = Time.time - firstShootB2;

                                for (int i = 0; i < 2; i++)
                                {
                                    if (outerCores[i].gameObject != null)
                                    {
                                        Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - outerCores[i].transform.position;
                                        Instantiate(homingMissile, outerCores[i].transform.position, Quaternion.Euler(0, 0, 180f));
                                    }
                                }

                                audioManager.PlaySound(homingMissileSoundName);

                                ++currentShootQuantity;
                            }

                            ChangePhaseBOnBullet(maxShootQuantityB2, phaseBAlgorithm);
                            break;

                        case (_bossStateB.PHASE_3):
                            //bomb
                            if (!hasShotB3 && Time.time >= nextPhaseTime + firstShootB3)
                            {
                                hasShotB3 = true;
                                for (int i = 0; i < 2; i++)
                                {
                                    if (outerCores[i].gameObject != null)
                                    {
                                        GameObject _bomb = Instantiate(bomb, outerCores[i].transform.position, transform.rotation);
                                        //_bomb.transform.parent = gameObject.transform;
                                    }
                                }

                                audioManager.PlaySound(bombSoundName);

                                ShieldBoss();
                            }

                            //change phase
                            if (Time.time >= nextPhaseTime + invulTimeB3 + firstShootB3)
                            {
                                UnshieldBoss();

                                hasShotB3 = false;

                                currentShootQuantity = 0;
                                shooterIndex = 0;
                                if (phaseIndex >= phaseBAlgorithm.Length)
                                {
                                    phaseIndex = 0;
                                }
                                bossSubstateB = phaseBAlgorithm[phaseIndex];
                                phaseIndex++;
                                nextShoot1 = Time.time;
                                nextShoot2 = Time.time;
                                nextPhaseTime = Time.time;
                            }

                            break;
                    }
                    break;

                case (_bossState.PHASE_C):
                    switch (bossSubstateC)
                    {
                        case (_bossStateC.PHASE_1):
                            //spread shoot
                            if (Time.time >= shootIntervalC1 + nextShoot1 + firstShootC1)
                            {
                                nextShoot1 = Time.time - firstShootC1;

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }

                                if (innerCores[shooterIndex].gameObject != null)
                                {
                                    Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - innerCores[shooterIndex].transform.position;
                                    ShootInDirection.ShootEqualSpread(bigBullet, shotQuantityC1, deviationC1, innerCores[shooterIndex].transform.position, distToPlayer, bulletSpeedC1);
                                }

                                audioManager.PlaySound(bigBulletSoundName);

                                //change bullet spawn position
                                if (innerCores[0].gameObject == null)
                                {
                                    shooterIndex = 1;
                                }
                                else if (innerCores[1].gameObject == null)
                                {
                                    shooterIndex = 0;
                                }
                                else if (shooterIndex == 0)
                                {
                                    ++shooterIndex;
                                }
                                else
                                {
                                    shooterIndex = 0;
                                }

                                ++currentShootQuantity;
                            }

                            ChangePhaseCOnBullet(maxShootQuantityC1, phaseCAlgorithm);
                            break;

                        case (_bossStateC.PHASE_2):

                            //laser shooter
                            if (!hasShotC2 && Time.time >= nextPhaseTime + firstShootC2)
                            {
                                hasShotC2 = true;
                                GameObject _shooter = Instantiate(laserShooter, centerCore.transform.position, transform.rotation);
                                _shooter.transform.parent = gameObject.transform;

                                audioManager.PlaySound(laserShooterSoundName);

                                ShieldBoss();
                            }

                            //change phase
                            if (Time.time >= nextPhaseTime + invulTimeC2 + firstShootC2)
                            {
                                UnshieldBoss();

                                hasShotC2 = false;

                                currentShootQuantity = 0;
                                shooterIndex = 0;
                                if (phaseIndex >= phaseCAlgorithm.Length)
                                {
                                    phaseIndex = 0;
                                }
                                bossSubstateC = phaseCAlgorithm[phaseIndex];
                                phaseIndex++;
                                nextShoot1 = Time.time;
                                nextShoot2 = Time.time;
                                nextPhaseTime = Time.time;
                            }
                            break;
                    }
                    break;

                case (_bossState.PHASE_D):
                    //spread shoot
                    if (Time.time >= shootIntervalD + nextShoot1 + firstShootD)
                    {
                        nextShoot1 = Time.time - firstShootD;

                        //change bullet spawn position
                        if (innerCores[0].gameObject == null)
                        {
                            shooterIndex = 1;
                        }
                        else if (innerCores[1].gameObject == null)
                        {
                            shooterIndex = 0;
                        }

                        if (innerCores[shooterIndex].gameObject != null)
                        {
                            Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - innerCores[shooterIndex].transform.position;
                            ShootInDirection.Shoot(bigBullet, shotQuantityD, deviationD, innerCores[shooterIndex].transform.position, distToPlayer, bulletSpeedD, true);
                        }

                        audioManager.PlaySound(bigBulletSoundName);

                        //change bullet spawn position
                        if (innerCores[0].gameObject == null)
                        {
                            shooterIndex = 1;
                        }
                        else if (innerCores[1].gameObject == null)
                        {
                            shooterIndex = 0;
                        }
                        else if (shooterIndex == 0)
                        {
                            ++shooterIndex;
                        }
                        else
                        {
                            shooterIndex = 0;
                        }

                        ++currentShootQuantity;
                    }
                    break;

                case (_bossState.PHASE_E):
                    switch (bossSubstateE)
                    {
                        case (_bossStateE.PHASE_1):
                            //missile
                            if (Time.time >= shootIntervalE1 + nextShoot1 + firstShootE1)
                            {
                                nextShoot1 = Time.time - firstShootE1;

                                for (int i = 0; i < 2; i++)
                                {
                                    if (outerCores[i].gameObject != null)
                                    {
                                        Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - outerCores[i].transform.position;
                                        Instantiate(homingMissile, outerCores[i].transform.position, Quaternion.Euler(0, 0, 180f));
                                    }
                                }

                                audioManager.PlaySound(homingMissileSoundName);

                                ++currentShootQuantity;
                            }

                            ChangePhaseEOnBullet(maxShootQuantityE1, phaseEAlgorithm);
                            break;

                        case (_bossStateE.PHASE_2):
                            //bomb
                            if (!hasShotE2 && Time.time >= nextPhaseTime + firstShootE2)
                            {
                                hasShotE2 = true;
                                for (int i = 0; i < 2; i++)
                                {
                                    if (outerCores[i].gameObject != null)
                                    {
                                        GameObject _bomb = Instantiate(bomb, outerCores[i].transform.position, transform.rotation);
                                        //_bomb.transform.parent = gameObject.transform;
                                    }
                                }

                                audioManager.PlaySound(bombSoundName);

                                ShieldBoss();
                            }

                            //change phase
                            if (Time.time >= nextPhaseTime + invulTimeE2 + firstShootE2)
                            {
                                UnshieldBoss();

                                hasShotE2 = false;

                                currentShootQuantity = 0;
                                shooterIndex = 0;
                                if (phaseIndex >= phaseEAlgorithm.Length)
                                {
                                    phaseIndex = 0;
                                }
                                bossSubstateE = phaseEAlgorithm[phaseIndex];
                                phaseIndex++;
                                nextShoot1 = Time.time;
                                nextShoot2 = Time.time;
                                nextPhaseTime = Time.time;
                            }
                            break;
                    }
                    break;

                case (_bossState.PHASE_F):
                    //rotating laser shooter
                    if (Time.time >= shootIntervalF + nextShoot1 + firstShootF)
                    {
                        nextShoot1 = Time.time - firstShootF;
                        GameObject _shooter = Instantiate(rotatingLaserShooter, centerCore.transform.position, transform.rotation);
                        _shooter.transform.parent = gameObject.transform;

                        audioManager.PlaySound(laserShooterSoundName);
                    }
                    break;

                case (_bossState.DEAD):
                    //do nothing for now
                    break;
            }
        }

        private void ChangePhaseAOnBullet(int _maxShootQuantity, _bossStateA[] phaseAAlgorithm)
        {
            if (currentShootQuantity >= _maxShootQuantity)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                if (phaseIndex >= phaseAAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                bossSubstateA = phaseAAlgorithm[phaseIndex];
                phaseIndex++;
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                nextPhaseTime = Time.time;
            }
        }

        private void ChangePhaseBOnBullet(int _maxShootQuantity, _bossStateB[] phaseBAlgorithm)
        {
            if (currentShootQuantity >= _maxShootQuantity)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                if (phaseIndex >= phaseBAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                bossSubstateB = phaseBAlgorithm[phaseIndex];
                phaseIndex++;
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                nextPhaseTime = Time.time;
            }
        }

        private void ChangePhaseCOnBullet(int _maxShootQuantity, _bossStateC[] phaseCAlgorithm)
        {
            if (currentShootQuantity >= _maxShootQuantity)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                if (phaseIndex >= phaseCAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                bossSubstateC = phaseCAlgorithm[phaseIndex];
                phaseIndex++;
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                nextPhaseTime = Time.time;
            }
        }

        private void ChangePhaseEOnBullet(int _maxShootQuantity, _bossStateE[] phaseEAlgorithm)
        {
            if (currentShootQuantity >= _maxShootQuantity)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                if (phaseIndex >= phaseEAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                bossSubstateE = phaseEAlgorithm[phaseIndex];
                phaseIndex++;
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                nextPhaseTime = Time.time;
            }
        }

        private void ShieldBoss()
        {
            //set core to be invulnerable
            for (int i = 0; i < 2; i++)
            {
                if (innerCores[i].gameObject != null)
                {
                    innerCores[i].transform.tag = "InvulEnemy";
                    innerCoresAnim[i].Play("BossCoreHide");
                }

                if (outerCores[i].gameObject != null)
                {
                    outerCores[i].transform.tag = "InvulEnemy";
                    outerCoresAnim[i].Play("BossCoreHide");
                }
            }
            if (centerCore.gameObject != null)
            {
                centerCore.transform.tag = "InvulEnemy";
                centerCoreAnim.Play("BossCoreHide");
            }
        }

        private void UnshieldBoss()
        {
            //set core to be vulnerable
            for (int i = 0; i < 2; i++)
            {
                if (innerCores[i].gameObject != null)
                {
                    innerCores[i].transform.tag = "Enemy";
                    innerCoresAnim[i].Play("BossCoreReveal2");
                }

                if (outerCores[i].gameObject != null)
                {
                    outerCores[i].transform.tag = "Enemy";
                    outerCoresAnim[i].Play("BossCoreReveal2");
                }
            }
            if (centerCore.gameObject != null)
            {
                centerCore.transform.tag = "Enemy";
                centerCoreAnim.Play("BossCoreReveal2");
            }
        }

        IEnumerator InvulBoss()
        {
            ShieldBoss();

            yield return new WaitForSeconds(phaseChangeInvulDuration);

            UnshieldBoss();
        }

        IEnumerator BossDie()
        {
            yield return new WaitForSeconds(6);

            //explosions
            Transform EAT = ExplosionArea.transform;
            BoxCollider2D EABC2D = ExplosionArea.GetComponent<BoxCollider2D>();

            for (int i = 0; i < 12; i++)
            {
                Instantiate(BigExplosion, transform.position + new Vector3(Random.Range(-EABC2D.size.x / 2, EABC2D.size.x / 2), Random.Range(-EABC2D.size.y / 2, EABC2D.size.y / 2), 0), Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }

            audioManager.PlaySound(bigExplosionSoundName);
            flasher.GetComponent<ScreenFadeWhite>().FadeOutWhite();
            Camera.main.GetComponent<ScreenShake>().Shake();

            Destroy(gameObject);
            //change to end
            Instantiate(sceneChanger, transform.position, transform.rotation);
            //stop boss music
            audioManager.StopMusic();
        }

        IEnumerator RandomExplosions()
        {
            Transform EAT = ExplosionArea.transform;
            BoxCollider2D EABC2D = ExplosionArea.GetComponent<BoxCollider2D>();

            while (true)
            {
                yield return new WaitForSeconds(smallExplosionStartInterval);

                Instantiate(SmallExplosion, transform.position + new Vector3(Random.Range(-EABC2D.size.x / 2, EABC2D.size.x / 2), Random.Range(-EABC2D.size.y / 2, EABC2D.size.y / 2), 0), Quaternion.Euler(0, 0, Random.Range(0, 359)));
                audioManager.PlaySound(smallExplosionSoundName);

                smallExplosionStartInterval -= smallExplosionDecrement;
                if (smallExplosionStartInterval <= 0.05)
                {
                    smallExplosionStartInterval = 0.05f;
                }
            }
        }
    }
}