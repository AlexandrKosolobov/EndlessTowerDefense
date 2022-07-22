using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Damagable _damagableEnemy;
    private WaveList _waveList;

    private void Awake()
    {
        if (TryLoadWavesFromFile(JsonParser.DEFAULT_WAVES_FILENAME, out _waveList))
        {
            Debug.Log("Loading waves completed");
        }
        else
        {
            Debug.LogWarning("Error loading waves");
        }
        StartCoroutine(StartSpawnWaves());
    }

    private bool TryLoadWavesFromFile(string fileName, out WaveList waveList)
    {
        waveList = JsonParser.ReadWavesFile(fileName);
        return waveList.Waves != null && waveList.Waves.Length != 0;
    }

    private IEnumerator StartSpawnWaves()
    {

        foreach (Wave wave in _waveList.Waves)
        {
            List<Damagable> curWave = new List<Damagable>();
            foreach (WavePart part in wave.WaveParts)
            {
                for (int i = part.EnemyAmount; i > 0; i--)
                {
                    Damagable enemy = Instantiate<Damagable>(_damagableEnemy, transform);
                    curWave.Add(enemy);
                    enemy.GetComponent<WaypointMovement>().StartMoving();
                    yield return new WaitForSeconds(1 / wave.Density);
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitUntil(() => 
            {
                foreach (Damagable c in curWave)
                {
                    if (c != null)
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        Debug.Log("Waves ended. VICTORY!");
    }

    /// <summary>
    ///! For debugging only
    /// </summary>
    /// <param name="waveList"></param>
    private void PrintWaveListManually(WaveList waveList)
    {
        if (waveList.Waves == null || waveList.Waves.Length == 0)
        {
            Debug.LogError("WaveList.Waves must not be null or contain 0 items");
            return;
        }

        string data = "";
        foreach (Wave wave in waveList.Waves)
        {
            data = data + "\n\nID: " + wave.Id + "\nDensity: " + wave.Density;
            foreach (WavePart part in wave.WaveParts)
            {
                data = string.Format("{0}\n   [ EnemyName: {1}, EnemyAmount: {2} ]", data, part.EnemyName, part.EnemyAmount);
            }
        }

        Debug.Log(data);
    }
}
