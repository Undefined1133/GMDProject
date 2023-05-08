using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    void LateUpdate()
    {
        if (playerManager != null)
        {
            var camera = playerManager.camera;
            if (camera != null)
            {
                transform.LookAt(camera.transform.position);
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
}
