using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SerializeField] private float shipHealth = 100f;

    // Только для одного игрока (у каждого своё).
    [SyncVar] private float currHealth;

    private void Awake()
    {
        currHealth = shipHealth;
    }

    public void TakeDamage(float damage, string place)
    {
        currHealth -= damage;

        Debug.Log(transform.name + " имеет уровень здоровья: " + currHealth);
    }

}
