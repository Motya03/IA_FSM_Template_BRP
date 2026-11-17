
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class CameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]

        [SerializeField]
        private Vector2 sensitivity = new Vector2(1, 1);

        [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);

        [SerializeField]
        private bool smooth;

        [SerializeField]
        private float interpolationSpeed = 25.0f;

        [Header("Arduino Gyroscope")]

        [SerializeField]
        private ArduinoInputReader arduino;

        [SerializeField]
        private bool useGyroscope = true;

        [Tooltip("Degrees per second per analog sensor unit.")]
        private Vector2 gyroSensitivity = new Vector2(3f, 3f);

        [SerializeField]
        private float gyroDeadZone = 5f;

        [SerializeField]
        private float calibrationTime = 1f;

        #endregion

        #region FIELDS

        private CharacterBehaviour playerCharacter;
        private Rigidbody playerCharacterRigidbody;

        private Quaternion rotationCharacter;
        private Quaternion rotationCamera;

        // Gyroscope calibration
        private float gyroCenterX;
        private float gyroCenterY;

        private float calibrationTimer;
        private float calibrationSumX;
        private float calibrationSumY;
        private int calibrationSamples;

        private bool calibrated;

        #endregion

        #region UNITY

        private void Awake()
        {
            playerCharacter = ServiceLocator.Current
                .Get<IGameModeService>()
                .GetPlayerCharacter();

            playerCharacterRigidbody =
                playerCharacter.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rotationCharacter =
                playerCharacter.transform.localRotation;

            rotationCamera = transform.localRotation;
        }

        private void LateUpdate()
        {
            Vector2 frameInput = Vector2.zero;

            // Mouse input
            if (playerCharacter.IsCursorLocked())
            {
                frameInput = playerCharacter.GetInputLook();
                frameInput *= sensitivity;
            }

            // Gyroscope input
            Vector2 gyroInput = GetGyroscopeInput();

            // Combine mouse and Arduino
            frameInput += gyroInput;

            // Yaw
            Quaternion rotationYaw =
                Quaternion.Euler(0f, frameInput.x, 0f);

            // Pitch
            Quaternion rotationPitch =
                Quaternion.Euler(-frameInput.y, 0f, 0f);

            rotationCamera *= rotationPitch;
            rotationCharacter *= rotationYaw;

            Quaternion localRotation = transform.localRotation;

            if (smooth)
            {
                localRotation = Quaternion.Slerp(
                    localRotation,
                    rotationCamera,
                    Time.deltaTime * interpolationSpeed
                );

                playerCharacterRigidbody.MoveRotation(
                    Quaternion.Slerp(
                        playerCharacterRigidbody.rotation,
                        rotationCharacter,
                        Time.deltaTime * interpolationSpeed
                    )
                );
            }
            else
            {
                localRotation *= rotationPitch;
                localRotation = Clamp(localRotation);

                playerCharacterRigidbody.MoveRotation(
                    playerCharacterRigidbody.rotation * rotationYaw
                );

                rotationCamera = localRotation;
            }

            transform.localRotation = localRotation;
        }

        #endregion

        #region GYROSCOPE

        private Vector2 GetGyroscopeInput()
        {
            if (!useGyroscope || arduino == null)
                return Vector2.zero;

            // Wait for Arduino data
            if (!arduino.dataReceived)
                return Vector2.zero;

            // Calibrate while sensor is stationary
            if (!calibrated)
            {
                calibrationSumX += arduino.gyroX;
                calibrationSumY += arduino.gyroY;

                calibrationSamples++;
                calibrationTimer += Time.deltaTime;

                if (calibrationTimer >= calibrationTime)
                {
                    gyroCenterX =
                        calibrationSumX / calibrationSamples;

                    gyroCenterY =
                        calibrationSumY / calibrationSamples;

                    calibrated = true;

                    Debug.Log("Gyroscope calibrated");
                }

                return Vector2.zero;
            }

            // Calculate difference from resting position
            float x = arduino.gyroX - gyroCenterX;
            float y = arduino.gyroY - gyroCenterY;

            // Deadzone
            if (Mathf.Abs(x) < gyroDeadZone)
                x = 0f;

            if (Mathf.Abs(y) < gyroDeadZone)
                y = 0f;

            // Convert analog values into camera rotation
            float yaw = x * gyroSensitivity.x * Time.deltaTime;
            float pitch = y * gyroSensitivity.y * Time.deltaTime;

            return new Vector2(yaw, pitch);
        }

        #endregion

        #region FUNCTIONS

        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            float pitch = 2.0f * Mathf.Rad2Deg *
                          Mathf.Atan(rotation.x);

            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);

            rotation.x = Mathf.Tan(
                0.5f * Mathf.Deg2Rad * pitch
            );

            return rotation;
        }

        #endregion
    }
}
