using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace fl
{
    [RequireComponent(typeof (Rigidbody))]
    public class ShipController : MonoBehaviour
    {
        [SerializeField] private float m_MaxEnginePower = 10f;                      // Максимальная мощность двигателя.
        [SerializeField] private float m_MaxWarpEnginePower = 1000f;                // Максимальная мощность двигателя при варп-скачке.
        [SerializeField] private float m_ThrottleChangeSpeed = 0.3f;                // Скорость изменения дроссельной заслонки.
        [SerializeField] private float m_RollEffect = 0.2f;                         // Коэффициент Roll-движения.
        [SerializeField] private float m_DirectedRollEffect = 0.7f;                 // Коэффициент направленного Roll-движения.
        [SerializeField] private float m_RightEffect = 0.2f;                        // Коэффициент Right-движения.
        [SerializeField] private float m_UpEffect = 0.2f;                           // Коэффициент Up-движения.
        [SerializeField] private float m_PitchEffect = 0.2f;                        // Коэффициент тангажа.
        [SerializeField] private float m_YawEffect = 0.2f;                          // Коэффициент рыскания.
        [SerializeField] private float m_DragFactor = 0.001f;                       // Коэффициент position-трения.
        [SerializeField] private float m_AngularDragFactor = 0.25f;                 // Коэффициент rotation-трения.

        public float Throttle { get; private set; }                                 // Количество используемого дросселя.
        public float MaxEnginePower { get { return m_MaxEnginePower; } }            // Максимальная мощность двигателя.
        public float MaxWarpEnginePower { get { return m_MaxWarpEnginePower; } }    // Максимальная мощность двигателя при варп-скачке.
        public float ForwardSpeed { get; private set; }
        public float RollInput { get; private set; }
        public float PitchInput { get; private set; }
        public float YawInput { get; private set; }
        public float ThrottleInput { get; private set; }
        public float RightInput { get; private set; }
        public float UpInput { get; private set; }

        public float EnginePower = 0;                                               // Сколько энергии подано двигатель.
        public float speedWreckageFactor = 0.1f;                                    // Коэффициент скорости космического мусора.
        public float speedNebulaFactor = 0.3f;                                      // Коэффициент скорости туманности.
        public Transform[] exchangeToolTransform;                                   // Массив объектов курсовых орудий.
        public bool Blockage = false;                                               // Блокировка управления при смене режима камеры кабины.
        public Transform flyingParticals;                                           // Объект пролетающих частиц.
        public Transform wreckageParticals;                                         // Объект космического мусора.
        public Transform nebulaParticals;                                           // Объект туманности.
        public Transform cameraShip;                                                // Объект содержащий в себе скрипт "CameraShip".

        [HideInInspector] public Vector3 InvertdirectionVelocity;
        [HideInInspector] public Vector3 directionVelocity;
        [HideInInspector] public float absoluteSpeed;
        [HideInInspector] public ParticleSystem wreckage;
        [HideInInspector] public ParticleSystem nebula;
        [HideInInspector] public CameraShip cs;

        private bool m_WarpActive = false;                                          // Текущая активность Warp-скачка.
        private Rigidbody m_Rigidbody;                                              // Физическое жёсткое тело.
        private HudManager hudM;                                                    // HudManager, который работает с элементами hud.
        private ExchangeTool[] m_Et;                                                // Массив курсовых орудий.
        private Transform CenterOfMassObject; // TO DO                              // Центр масс космического корабля.

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            cs = cameraShip.GetComponent<CameraShip>();
            wreckage = wreckageParticals.GetComponent<ParticleSystem>();
            nebula = nebulaParticals.GetComponent<ParticleSystem>();

            m_Et = new ExchangeTool[exchangeToolTransform.Length];

            int index = 0;
            foreach (var tool in exchangeToolTransform)
            {
                m_Et[index] = tool.GetComponent<ExchangeTool>();
                index++;
            }
        }

        //ResourceManager resM; // TO DO
        public Vector3 centerOfMass = new Vector3(0f, -7.3f, 27f); // TO DO

        void Start()
        {
        //    resM = ResourceManager.GetInstance();
        //    CenterOfMassObject = Instantiate(resM.GetPrefab("CenterOfMass")).transform; // TO DO
        //    CenterOfMassObject.position = centerOfMass; // TO DO
            m_Rigidbody.centerOfMass = centerOfMass; // TO DO

            hudM = HudManager.GetInstance();
        }

        public void Attack(bool attackRight, bool attackLeft)
        {
            if (attackRight && !Blockage)
            {
                foreach (var tool in m_Et)
                {
                    tool.Attack();
                }
            }
        }

        public void Move(float rollInput, float pitchInput, float yawInput, float throttleInput, float rightInput, float upInput)
        {
            RollInput = Mathf.Clamp(rollInput, -1, 1);
            ThrottleInput = Mathf.Clamp(throttleInput, -1, 1);
            RightInput = Mathf.Clamp(rightInput, -1, 1);
            UpInput = Mathf.Clamp(upInput, -1, 1);

            PitchInput = pitchInput;
            YawInput = yawInput;

            CalculateForwardSpeed();

            ControlThrottle();

            CalculateDrag();

            CalculateLinearForces();

            CalculateTorque();

            // Установить гравитацию на все объекты системы.
            //Physics.gravity = Vector3.zero;
        }

        private void ControlThrottle()
        {
            Throttle = Mathf.Clamp01(Throttle + ThrottleInput * Time.deltaTime * m_ThrottleChangeSpeed);
            EnginePower = Throttle * m_MaxEnginePower;

            hudM.powerIndicatorText = (EnginePower.ToString("0.00") + " / " + m_MaxEnginePower);
        }

        private void CalculateDrag()
        {
            // Пространственное торможение.
            m_Rigidbody.drag = 1 + m_Rigidbody.velocity.magnitude * m_DragFactor;
            // Угловое торможение.

            float x = ForwardSpeed * m_AngularDragFactor;

            m_Rigidbody.angularDrag = 0.2f * x * x + 1; // В конце коэфф // TO DO
        }

        private void CalculateForwardSpeed()
        {
            InvertdirectionVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
            ForwardSpeed = Mathf.Max(0, InvertdirectionVelocity.z); // Доработать! От 0 до вектора скорости.

            directionVelocity = m_Rigidbody.velocity;
            if (directionVelocity != Vector3.zero)
            {
                flyingParticals.rotation = Quaternion.LookRotation(directionVelocity);
            }

            absoluteSpeed = Mathf.Abs(directionVelocity.x + directionVelocity.y + directionVelocity.z);
            var main = wreckage.main;
            main.simulationSpeed = absoluteSpeed * speedWreckageFactor;
            main = nebula.main;
            main.simulationSpeed = absoluteSpeed * speedNebulaFactor;

            hudM.exchangeSpeedIndicatorText = ForwardSpeed.ToString("0.00");
            hudM.speedIndicatorText = absoluteSpeed.ToString("0.00");
        }

        private void CalculateLinearForces()
        {
            var forces = Vector3.zero;
            forces += EnginePower * transform.forward;
            forces += RightInput * m_RightEffect * transform.right;
            forces += UpInput * m_UpEffect * transform.up;
            m_Rigidbody.AddForce(forces);
        }

        //public Vector3 torqueMy;

        private void CalculateTorque()
        {
            var torque = Vector3.zero;
            torque -= PitchInput * m_PitchEffect * transform.right;

            float Yaw = YawInput * m_YawEffect;
            float DirectedRoll = m_DirectedRollEffect * Yaw;

            if (Yaw < 0)
            {
                torque += Yaw * transform.up + (- DirectedRoll * transform.forward);
            }
            else
            {
                torque += Yaw * transform.up - DirectedRoll * transform.forward;
            }

            torque += RollInput * m_RollEffect * transform.forward;

            //torque = torqueMy;
            m_Rigidbody.AddTorque(torque/* * ForwardSpeed*/);
        }

        float SaveMaxEnginePower;

        public IEnumerator Warp()
        {
            if (!m_WarpActive)
            {
                m_WarpActive = true;
                Debug.Log("Варп скачок через 15 секунд.");
                yield return new WaitForSecondsRealtime(12f);
                yield return StartCoroutine(cs.StartShaking());
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    m_WarpActive = false;
                    Debug.Log("Отмена варп скачка.");
                    yield return null;
                }
                else
                {
                    SaveMaxEnginePower = m_MaxEnginePower;
                    m_MaxEnginePower = MaxWarpEnginePower;
                    yield return StartCoroutine(WaitForKeyUp(KeyCode.LeftShift));
                    yield return StartCoroutine(CompletionWarp());
                }
            }
        }

        public IEnumerator CompletionWarp()
        {
            m_WarpActive = false;
            m_MaxEnginePower = SaveMaxEnginePower;
            yield return StartCoroutine(cs.FinishShaking());
            yield return null;
        }

        IEnumerator WaitForKeyUp(KeyCode keyCode)
        {
            while (!Input.GetKeyUp(keyCode))
                yield return null;
        }
    }
}