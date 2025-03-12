using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Game/Shop Card")]
public class ShopCardSO : ScriptableObject
{
    [System.Serializable]
    public class CatSpawn
    {
        public CatSO catType;
    }

    public string tileName = "Cat Tile Name";
}