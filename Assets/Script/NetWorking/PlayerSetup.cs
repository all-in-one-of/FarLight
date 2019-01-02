using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))] // Походу не работает.
public class PlayerSetup : NetworkBehaviour {

    private Camera sceneCamera;

    public bool SinglePlayer = true;

    [SerializeField] Behaviour[] componentsToDissable;

    private void Start()
    {
        if (!SinglePlayer)
        {
            if (!isLocalPlayer)
            {
                foreach (var components in componentsToDissable)
                {
                    components.enabled = false;
                }
            }
            else
            {
                sceneCamera = Camera.main;
                if (sceneCamera != null)
                {
                    sceneCamera.gameObject.SetActive(false);
                }
            }
        }
    }

    // Переписываем стандарт. Регистрируем клиент.
    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netID, player);
    }

    // Если игрок вышел.
    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }

}
