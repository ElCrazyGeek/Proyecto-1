using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum ItemType { Medkit, MeleeWeapon, Pistol }
    public ItemType itemType;
    public GameObject weaponPrefab; // Para armas
}