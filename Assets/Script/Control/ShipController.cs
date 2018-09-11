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

        [SerializeField] private float m_RollEffect = 0.2f;                         // The strength of effect for roll input.
        [SerializeField] private float m_RightEffect = 0.2f;
        [SerializeField] private float m_UpEffect = 0.2f;
        [SerializeField] private float m_PitchEffect = 0.2f;                        // The strength of effect for pitch input.
        [SerializeField] private float m_YawEffect = 0.2f;                          // The strength of effect for yaw input.
        //[SerializeField] private float m_BankedTurnEffect = 0.5f;                   // Количество оборотов от поворота в сторону.
        [SerializeField] private float m_DragFactor = 0.001f;               // how much drag should increase with speed.
        [SerializeField] private float m_AngularDragFactor = 0.25f;               // how much drag should increase with speed.

        public float Throttle { get; private set; }                                 // Количество используемого дросселя.
        public float EnginePower = 0;                                               // Сколько энергии подано двигатель.
        public float MaxEnginePower { get { return m_MaxEnginePower; } }            // Максимальная мощность двигателя.
        public float MaxWarpEnginePower { get { return m_MaxWarpEnginePower; } }    // Максимальная мощность двигателя при варп-скачке.
        public float ForwardSpeed { get; private set; }

        public float RollInput { get; private set; }
        public float PitchInput { get; private set; }
        public float YawInput { get; private set; }
        public float ThrottleInput { get; private set; }
        public float RightInput { get; private set; }
        public float UpInput { get; private set; }

        [SerializeField] private float LossControlSpeed = 200f;

        //public float m_AeroFactor = 1f;

        [HideInInspector] public Vector3 InvertdirectionVelocity;
        [HideInInspector] public Vector3 directionVelocity;
        [HideInInspector] public float absoluteSpeed;
        public float speedWreckageFactor = 0.1f;
        public float speedNebulaFactor = 0.3f;

        //private float m_Throttle;
        //private float m_Yaw;
        //private float m_Roll;
        //private float m_Pitch;

        //private float m_OriginalDrag;         // The drag when the scene starts.
        //private float m_OriginalAngularDrag;  // The angular drag when the scene starts.

        //private bool m_AutoBrake = true;
        private bool m_WarpActive = false;
        //private float m_BankedTurnAmount;
        private Rigidbody m_Rigidbody;

        public Transform[] exchangeToolTransform;
        private ExchangeTool[] m_Et;
        public bool Blockage = false;


        public Text speedText;
        public Text powerText;
        public Transform flyingParticals;
        public Transform wreckageParticals;
        [HideInInspector] public ParticleSystem wreckage;
        public Transform nebulaParticals;
        [HideInInspector] public ParticleSystem nebula;
        public Transform cameraShip;
        [HideInInspector] public CameraShip cs;

        Transform CenterOfMassObject; // TO DO

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            cs = cameraShip.GetComponent<CameraShip>();

            //m_OriginalDrag = m_Rigidbody.drag;
            //m_OriginalAngularDrag = m_Rigidbody.angularDrag;

            wreckage = wreckageParticals.GetComponent<ParticleSystem>();
            nebula = nebulaParticals.GetComponent<ParticleSystem>();

            m_Et = new ExchangeTool[exchangeToolTransform.Length];

            int index = 0;
            foreach(var tool in exchangeToolTransform)
            {
                m_Et[index] = tool.GetComponent<ExchangeTool>();
                index++;
            }

            CenterOfMassObject = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform; // TO DO
            CenterOfMassObject.GetComponent<SphereCollider>().enabled = false; // TO DO
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

        public Vector3 centerOfMass;

        public void Move(float rollInput, float pitchInput, float yawInput, float throttleInput, float rightInput, float upInput)
        {
            RollInput = Mathf.Clamp(rollInput, -1, 1);
            ThrottleInput = Mathf.Clamp(throttleInput, -1, 1);
            RightInput = Mathf.Clamp(rightInput, -1, 1);
            UpInput = Mathf.Clamp(upInput, -1, 1);

            // Убрать для кручения и сделать более точно.
            PitchInput = Mathf.Clamp(pitchInput, -1, 1);
            YawInput = Mathf.Clamp(yawInput, -1, 1);

            m_Rigidbody.centerOfMass = centerOfMass; // TO DO
            CenterOfMassObject.position = centerOfMass; // TO DO
            //CenterOfMassObject.localScale = new Vector3(10f, 10f, 10f);

            CalculateForwardSpeed();

            ControlThrottle();

            CalculateDrag();

            CalculateLinearForces();

            CalculateTorque();
        }


        private void ControlThrottle()
        {
            Throttle = Mathf.Clamp01(Throttle + ThrottleInput * Time.deltaTime * m_ThrottleChangeSpeed);
            EnginePower = Throttle * m_MaxEnginePower;

            powerText.text = (EnginePower + " / " + m_MaxEnginePower);
        }


        private void CalculateDrag()
        {
            float extraDrag = m_Rigidbody.velocity.magnitude * m_DragFactor;
            // Пространственное торможение.
            m_Rigidbody.drag = 1 /*+ m_OriginalDrag*/ + extraDrag;
            // Угловое торможение.

            if (absoluteSpeed > LossControlSpeed) // TO DO
            {
                m_Rigidbody.angularDrag = 1 + /*m_OriginalAngularDrag **/ ForwardSpeed * m_AngularDragFactor; // В конце коэфф
            }
            else
            {
                m_Rigidbody.angularDrag = 1;
            }
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

            speedText.text = (absoluteSpeed).ToString();
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
            torque += PitchInput * m_PitchEffect * transform.right;
            torque += - YawInput * m_YawEffect * transform.up;
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


// Всё ещё влияет физика. Если повернуться по Z на -90 (крыло по вертикали), то скорость начнёт убывать. TO DO