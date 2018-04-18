using UnityEngine;

namespace Kontrol
{
    public class SetSimSpace : MonoBehaviour
    {
        private ParticleSystem ps;

        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            var main = ps.main;
            main.customSimulationSpace = Camera.main.transform;
        }
    }
}
