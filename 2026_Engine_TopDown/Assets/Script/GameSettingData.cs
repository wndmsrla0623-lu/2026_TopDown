using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Game Setting Data")]
public class GameSettingData : ScriptableObject
{
    public int startHp = 100;

    public int startAttack = 10;

    public float playerMoveSpeed = 5f;

    public int hpBonusPerDeath = 5;

    public int atkBonusPerDeath = 1;
}
