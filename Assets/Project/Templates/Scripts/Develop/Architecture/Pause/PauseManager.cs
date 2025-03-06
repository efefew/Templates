using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.Templates.Scripts.Develop.Architecture.Pause
{
    public class PauseManager : MonoBehaviour
    {
        [Interface(typeof(IPause))]
        [SerializeField]
        private MonoBehaviour[] _monoBehaviors;

        private readonly List<IPause> _pauses = new();
        [SerializeField]
        private bool _isPause;

        private static PauseManager Instance { get; set; }

        #region Unity Methods

        private void Start()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this);
            foreach (var mono in _monoBehaviors)
            {
                _pauses.Add(mono as IPause);
            }
#if !UNITY_EDITOR
        _monoBehaviors = null;
#endif
        }

        private void Update()
        {
            if (_isPause)
            {
                return;
            }

            for (int id = 0; id < _pauses.Count; id++)
            {
                _pauses[id].UpdatePause();
            }
        }

        private void FixedUpdate()
        {
            if (_isPause)
            {
                return;
            }

            for (int id = 0; id < _pauses.Count; id++)
            {
                _pauses[id].FixedUpdatePause();
            }
        }

        private void LateUpdate()
        {
            if (_isPause)
            {
                return;
            }

            for (int id = 0; id < _pauses.Count; id++)
            {
                _pauses[id].LateUpdatePause();
            }
        }

        #endregion Unity Methods

        public void Add(IPause p)
        {
            _pauses.Add(p);
        }

        public void Remove(IPause p)
        {
            _pauses.Remove(p);
        }

        [MenuItem("Tools/Pause")]
        public static void Pause()
        {
            Instance._isPause = true;
        }

        [MenuItem("Tools/Resume")]
        public static void Resume()
        {
            Instance._isPause = false;
        }
    }
}