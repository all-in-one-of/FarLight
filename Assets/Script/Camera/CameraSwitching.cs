using System.Collections;
using UnityEngine;
namespace fl
{
    public class CameraSwitching : MonoBehaviour
    {

        //public GameObject GeneralCamera;
        //public GameObject GeneralCameraObject;
        //public GameObject CockpitCameraObject;
        //public GameObject CockpitCamera;

        //private bool camSwitch = false;
        //private bool camSwitchSave = false;

        //private HudManager hudM;

        //public GameObject[] ObjectCockpit;
        

        void Start()
        {
            //hudM = HudManager.GetInstance();

            //hudM.activeVisualAssistant = true;

            //// В General курсор не нужен.
            //Cursor.visible = false;
            //// Ставим активность кабины пилота.
            //CockpitCameraObject.SetActive(false);
            //// Ставим активность объектов кабины пилота.
            //SetActiveArrayCockpit(false);

            //hudM.activeVisualAssistant = true;
        }

        //void Update()
        //{
        //    // TO DO. Настройка клавиши.
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        camSwitch = !camSwitch;
        //    }

        //    if (camSwitchSave != camSwitch)
        //    {
        //        if (camSwitch)
        //        {
        //            CockpitCameraObject.SetActive(true);
        //            SetActiveArrayCockpit(true);
        //            GeneralCameraObject.SetActive(false);
        //        }
        //        else
        //        {
        //            hudM.activeVisualAssistant = true;
        //            // Возвращаем режим "изучения".
        //            CockpitCamera.GetComponent<CockpitCamera>().ReleaseInput();
        //            CockpitCameraObject.SetActive(false);
        //            SetActiveArrayCockpit(false);
        //            GeneralCameraObject.SetActive(true);
        //        }
        //        camSwitchSave = camSwitch;
        //    }
        //}

        //private void SetActiveArrayCockpit(bool active)
        //{
        //    foreach (GameObject _object in ObjectCockpit)
        //    {
        //        _object.SetActive(active);
        //    }
        //}
    }
}
