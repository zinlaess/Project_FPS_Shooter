using UnityEngine;
using TMPro;

public class SC_StatisticsDate : MonoBehaviour
{
    private TMP_Text waivStat;
    private TMP_Text enemyStat;
    private void Start()
    {
        Load();
    }
    public void Load()
    {
        waivStat = GameObject.Find("WaivStat").GetComponent<TMP_Text>();
        var waivdate = SC_SaveManager.Load<SaveDate.SC_ProgressSave>("waveSave").Waiv;
        waivStat.text = $"Waiv: {waivdate}";

        // stopgap measure : unknown bug

        /*enemyStat = GameObject.Find("EnemyStat").GetComponent<TMP_Text>();
        var enemydate = SC_SaveManager.Load<SaveDate.SC_ProgressSave>("enemySave").Waiv;
        enemyStat.text = $"Enemy: {enemydate}";*/
    }
}
