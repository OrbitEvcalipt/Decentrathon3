using System.Collections;
using Sources.Scripts.Common;
using Sources.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Scripts.Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreenObject;

        private void Start()
        {
            StartCoroutine(LoadLevelScene());
        }

        private IEnumerator LoadLevelScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            CommonData.levelNumber = SaveManager.LoadInt(CommonData.PLAYERPREFS_LEVEL_NUMBER, 0);

            EventsHandler.Initialize();

            Destroy(loadingScreenObject);
        }
    }
}