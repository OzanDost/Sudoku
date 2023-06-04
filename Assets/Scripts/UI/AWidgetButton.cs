using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class AWidgetButton : MonoBehaviour
    {
        [SerializeField] protected Button button;

        public Button Button => button;
        public abstract void Animate();
    }
}