
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundSettings", menuName = "Haywire/Round Settings")]
public class RoundSettings : ScriptableObject
{
    [Serializable]
    public class EnemyWeight
    {
        public EnemyData enemyData;

        [Min(0)]
        public int weight = 1;
    }

    [Serializable]
    public class RoundConfig
    {
        [Min(1)]
        public int roundNumber = 1;

        [Min(0)]
        public int enemyCount = 5;

        public List<EnemyWeight> enemyWeights = new();
    }

    [Header("Rounds 1-30")]
    public List<RoundConfig> rounds = new();

    [Header("Round 31 onwards")]
    [Min(0)]
    public int enemiesPerRoundAfter30 = 35;

    public RoundConfig GetRoundConfig(int roundNumber)
    {
        // Rounds 31+ use the weight profile from round 30
        int configuredRound = Mathf.Min(roundNumber, 30);

        return rounds.Find(r => r.roundNumber == configuredRound);
    }

    public int GetEnemyCount(int roundNumber)
    {
        if (roundNumber > 30)
            return enemiesPerRoundAfter30;

        RoundConfig config = GetRoundConfig(roundNumber);
        return config != null ? config.enemyCount : 0;
    }
}
