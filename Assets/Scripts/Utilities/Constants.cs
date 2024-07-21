using UnityEngine;

public static class Constants
{
    private static Camera mainCamera = MainCamera.Instance._camera;
    private static float cameraHeight = 2f * mainCamera.orthographicSize;
    private static float cameraWidth = cameraHeight * mainCamera.aspect;
    public static Vector3 BowPosition = new (-13f, 0f, 0f);
    public static Vector3 BossStartingPosition = new (16.35f, 0f, 0f);
    public static float OutOfBoundsYPos = 8.05f;
    public static float StartingBossXPossition = 16.35f;
    public static float MinCameraX = mainCamera.transform.position.x - cameraWidth / 2;
    public static float MaxCameraX = mainCamera.transform.position.x + cameraWidth / 2;
    public static float MinCameraY = mainCamera.transform.position.y - cameraHeight / 2;
    public static float MaxCameraY = mainCamera.transform.position.y + cameraHeight / 2;
}