using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private const string PLAYER_ID = "Player ";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    // Регистрация игрока.
    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    // Удаление игрока.
    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPLayer(string playerID)
    {
        return players[playerID];
    }
}
