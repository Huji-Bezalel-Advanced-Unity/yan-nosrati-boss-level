using UnityEngine;

public static class Constants
{
    private static Camera mainCamera = MainCamera.Instance._camera;
    private static float cameraHeight = 2f * mainCamera.orthographicSize;
    private static float cameraWidth = cameraHeight * mainCamera.aspect;


    public const float BasicArrowCooldown = 1f;
    public static UnityEngine.Vector3 BowPosition = new UnityEngine.Vector3(-13f, 0f, 0f);
    public static int OutOfBoundsXPos = 20;
    public static float OutOfBoundsYPos = 8.05f;
    public static float StartingBossXPossition = 16.35f;
    public static Vector3 SkyPosition = new Vector3(-5f, -20f, 0f);
    public static float MinCameraX = mainCamera.transform.position.x - cameraWidth / 2;
    public static float MaxCameraX = mainCamera.transform.position.x + cameraWidth / 2;
    public static float MinCameraY = mainCamera.transform.position.y - cameraHeight / 2;
    public static float MaxCameraY = mainCamera.transform.position.y + cameraHeight / 2;
}