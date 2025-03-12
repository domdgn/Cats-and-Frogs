using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "NewCat", menuName = "Game/Cat")]
public class CatSO : ScriptableObject
{
    public string catName;
    public Sprite sprite;
    public int health = 100;
    public float waitTime = 1f;
    public AnimatorController animatorController;
    public float damage = 10;
}