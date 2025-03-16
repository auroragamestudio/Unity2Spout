using UnityEngine;

public class StereoCameraRigOffAxis : MonoBehaviour
{
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;
    public float eyeSeparation = 0.065f; // Distance interoculaire
    public float convergenceDistance = 10.0f; // Distance de convergence

    void Start()
    {
        // Positionner les caméras
        leftEyeCamera.transform.localPosition = new Vector3(-eyeSeparation / 2, 0, 0);
        rightEyeCamera.transform.localPosition = new Vector3(eyeSeparation / 2, 0, 0);

        // Ajuster les matrices de projection
        UpdateCameraProjections();
    }

    void UpdateCameraProjections()
    {
        // Calculer les matrices de projection off-axis
        Matrix4x4 leftProjectionMatrix = CreateOffAxisProjectionMatrix(leftEyeCamera, eyeSeparation, convergenceDistance);
        Matrix4x4 rightProjectionMatrix = CreateOffAxisProjectionMatrix(rightEyeCamera, -eyeSeparation, convergenceDistance);

        // Appliquer les matrices de projection aux caméras
        leftEyeCamera.projectionMatrix = leftProjectionMatrix;
        rightEyeCamera.projectionMatrix = rightProjectionMatrix;
    }

    Matrix4x4 CreateOffAxisProjectionMatrix(Camera camera, float offset, float convergenceDistance)
    {
        float near = camera.nearClipPlane;
        float far = camera.farClipPlane;
        float top = near * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2.0f);
        float bottom = -top;
        float aspect = camera.aspect;
        float left = -aspect * top + offset * near / convergenceDistance;
        float right = aspect * top + offset * near / convergenceDistance;

        Matrix4x4 projectionMatrix = Matrix4x4.zero;
        projectionMatrix[0, 0] = 2.0f * near / (right - left);
        projectionMatrix[0, 2] = (right + left) / (right - left);
        projectionMatrix[1, 1] = 2.0f * near / (top - bottom);
        projectionMatrix[1, 2] = (top + bottom) / (top - bottom);
        projectionMatrix[2, 2] = -(far + near) / (far - near);
        projectionMatrix[2, 3] = -(2.0f * far * near) / (far - near);
        projectionMatrix[3, 2] = -1.0f;

        return projectionMatrix;
    }
}
