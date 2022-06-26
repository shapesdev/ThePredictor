using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Factory", menuName = "Game/Player Factory")]
public class PlayerFactory : GameObjectFactory
{
    [SerializeField]
    Player playerPrefab = default;

    public Player Get() {
        Player instance = CreateObjectInstance(playerPrefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(Player player) {
        Debug.Assert(player.OriginFactory == this, "Wrong factory reclaimed");
        Destroy(player.gameObject);
    }
}
