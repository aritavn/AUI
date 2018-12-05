using UnityEngine;
using HoloToolkit.Unity.InputModule;
namespace LocalJoost.HoloToolkitExtensions
{
    public class InitialPlaceByTap : MonoBehaviour, IInputClickHandler
    {
        protected AudioSource Sound;
        protected MoveByGaze GazeMover;
        private GameObject box;

        void Start()
        {
            GazeMover = GetComponent<MoveByGaze>();
            InputManager.Instance.
            PushFallbackInputHandler(gameObject);
            box = GameObject.Find("Box8");

        }
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (!GazeMover.IsActive)
            {
                box.GetComponent<Roulette>().stopRoulette();
            }
            else
            {
                box.GetComponent<Roulette>().startRoulette();
                GazeMover.IsActive = false;
            }
        }
    }
}
