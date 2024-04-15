using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamEnum InverseTeam(TeamEnum team)
    {
        if (team == TeamEnum.None)
        {
            return team;
        }

        return team == TeamEnum.Player ? TeamEnum.Enemy : TeamEnum.Player;
    }
}
