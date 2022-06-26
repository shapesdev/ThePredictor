using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCollection
{
    List<Player> players = new List<Player>();

    public void Add(Player player) {
        players.Add(player);
    }

    public void GameUpdate() {
        for(int i = 0; i < players.Count; i++) {
            if(!players[i].GameUpdate()) {
                int lastIndex = players.Count - 1;
                players[i] = players[lastIndex];
                players.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}
