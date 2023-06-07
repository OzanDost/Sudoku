using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public abstract class AWidgetButton : MonoBehaviour
    {
        [SerializeField] protected Button button;

        public Button Button => button;
        public abstract void Animate();
        public abstract void NoFunctionAnimate();
    }
}