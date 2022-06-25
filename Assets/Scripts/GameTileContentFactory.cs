using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Game Tile Content Factory", menuName = "Game/Tile Content Factory")]
public class GameTileContentFactory : ScriptableObject
{
    [SerializeField]
    GameTileContent destinationPrefab = default;
    [SerializeField]
    GameTileContent emptyPrefab = default;
    [SerializeField]
    GameTileContent wallPrefab = default;

    private Scene contentScene;

    public GameTileContent Get(GameTileContentType type) {
        switch(type) {
            case GameTileContentType.Destination: return Get(destinationPrefab);
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }

    public void Reclaim(GameTileContent content) {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed");
        Destroy(content.gameObject);
    }

    private GameTileContent Get(GameTileContent prefab) {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }

    private void MoveToFactoryScene(GameObject go) {
        if(!contentScene.isLoaded) {
            if(Application.isEditor) {
                contentScene = SceneManager.GetSceneByName(name);
                if(!contentScene.isLoaded) {
                    contentScene = SceneManager.CreateScene(name);
                }
            }
            else {
                contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(go, contentScene);
    }
}
