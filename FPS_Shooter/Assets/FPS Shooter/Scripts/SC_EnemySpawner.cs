using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public SC_DamageReceiver player;
    public Texture crosshairTexture;
    public float spawnInterval = 2;
    public int enemiesPerWave = 5; 
    public Transform[] spawnPoints;
    public SC_CharacterController CharacterController;
    public SC_WeaponManager weaponManager;
    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    private bool Dying = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;
    
    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;
    public int enemydeathcounter;
    private const string saveKeyWave = "waveSave";
    private const string saveKeyEnemy = "enemySave";

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newWaveTimer = 5;
        waitingForWave = true;
    }


    void Update()
    {
        if (waitingForWave)
        {
            if(newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                //Initialize new wave
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
        }
        else
        {
            if(Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;

                //Spawn enemy 
                if(totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    SC_NPCEnemy npc = enemy.GetComponent<SC_NPCEnemy>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }

        if (player.playerHP <= 0)
        {
            if (Dying)
            {
                weaponManager.primaryWeapon.ActivateWeapon(false);
                weaponManager.secondaryWeapon.ActivateWeapon(false);
                CharacterController.PlayerDyingAnim();
                Dying = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Save();
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, Screen.height - 35, 100, 25), ((int)player.playerHP).ToString() + " HP");
        GUI.Box(new Rect(Screen.width / 2 - 35, Screen.height - 35, 70, 25), player.weaponManager.selectedWeapon.bulletsPerMagazine.ToString());

        if(player.playerHP <= 0)
        {
            GUI.Box(new Rect(Screen.width / 2 - 85, Screen.height / 2 - 20, 170, 40), "Game Over\n(Press 'Space' to Restart)");
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }

        GUI.Box(new Rect(Screen.width / 2 - 50, 10, 100, 25), (enemiesToEliminate - enemiesEliminated).ToString());

        if (waitingForWave)
        {
            GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 4 - 12, 250, 25), "Waiting for Wave " + waveNumber.ToString() + " (" + ((int)newWaveTimer).ToString() + " seconds left...)");
        }
    }

    public void EnemyEliminated(SC_NPCEnemy enemy)
    {
        enemiesEliminated++;

        if(enemiesToEliminate - enemiesEliminated <= 0)
        {
            //Start next wave
            newWaveTimer = 5;
            waitingForWave = true;
            waveNumber++;
        }
    }
    private void Save()
    {
        /*SC_SaveManager.Save(saveKeyEnemy, GetSaveEnemy());*/
        if (SC_SaveManager.Load<SaveDate.SC_ProgressSave>(saveKeyWave).Waiv >= waveNumber)
            return;
        else
            SC_SaveManager.Save(saveKeyWave, GetSaveWave());
        
    }
    
    private SaveDate.SC_ProgressSave GetSaveWave()
    {
        var wave = new SaveDate.SC_ProgressSave()
        {
            Waiv = waveNumber
        };
        return wave;
    }

    // stopgap measure : unknown bug

    /*    private SaveDate.SC_ProgressSave GetSaveEnemy()
        {
            var save = new SaveDate.SC_ProgressSave()
            {
                Enemy = enemydeathcounter
            };
            return save;
        }*/
}