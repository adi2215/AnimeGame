using UnityEngine;
using Cinemachine;
using StarterAssets;

public class CinemachinePovExten: CinemachineExtension
{
    private Vector3 startingRotation;

    [SerializeField] private float horizontalSpeed = 20f;
    [SerializeField] private float verticalSpeed = 20f;
    [SerializeField] private float clampAngle = 10f;

    public StarterAssetsInputs _input;

    private void Awake()
    {
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                {
                    startingRotation = transform.localRotation.eulerAngles;
                }

                Vector2 deltaInput = _input.look;

                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0.0f);
            }
        }
}