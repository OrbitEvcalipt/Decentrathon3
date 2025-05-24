using System.Collections.Generic;
using Sources.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private List<ActionButton> actionButtons;

        public void Prepare(int lives, List<ActionData> actions)
        {
            UpdateLivesText(lives);

            for (int i = 0; i < actionButtons.Count; i++)
            {
                actionButtons[i].SetIsOn(false);
                actionButtons[i].gameObject.SetActive(true);
                actionButtons[i].Setup(
                    actions[i].sprite,
                    actions[i].actionForce.ToString()
                );
            }
        }

        public int GetIndexInOn()
        {
            for (int i = 0; i < actionButtons.Count; i++)
            {
                if (actionButtons[i].IsOn) return i;
            }

            return -1;
        }

        public int GetRandomIndexInActive()
        {
            int index = 0;
            do
            {
                index = Random.Range(0, actionButtons.Count);
            } while (!actionButtons[index].gameObject.activeSelf);

            return index;
        }

        public void UpdateButtons(int index)
        {
            actionButtons[index].SetIsOn(false);
            actionButtons[index].gameObject.SetActive(false);
        }

        public void UpdateLivesText(int lives) => livesText.text = lives.ToString();
    }
}