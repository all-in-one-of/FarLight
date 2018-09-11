using System.Collections;
using UnityEngine;
namespace fl
{
    public class CameraSwitching : MonoBehaviour
    {

        public GameObject GeneralCamera;
        public GameObject CockpitCamera;

        private bool camSwitch = false;
        private bool camSwitchSave = false;

        public GameObject[] ObjectCockpit;
        

        void Start()
        {
            // В General курсор не нужен.
            Cursor.visible = false; 
            // Ставим активность кабины пилота.
            CockpitCamera.SetActive(false);
            // Ставим активность объектов кабины пилота.
            SetActiveArrayCockpit(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                camSwitch = !camSwitch;
            }

            if (camSwitchSave != camSwitch)
            {
                if (camSwitch)
                {
                    Cursor.visible = true;
                    CockpitCamera.SetActive(true);
                    SetActiveArrayCockpit(true);
                    GeneralCamera.SetActive(false);
                }
                else
                {
                    // Возвращаем режим "изучения".
                    CockpitCamera.GetComponent<CockpitCamera>().ReleaseInput();
                    Cursor.visible = false;
                    CockpitCamera.SetActive(false);
                    SetActiveArrayCockpit(false);
                    GeneralCamera.SetActive(true);
                }
                camSwitchSave = camSwitch;
            }
        }

        private void SetActiveArrayCockpit(bool active)
        {
            foreach (GameObject _object in ObjectCockpit)
            {
                _object.SetActive(active);
            }
        }
    }
}
