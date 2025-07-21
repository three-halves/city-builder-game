using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private GameObject battleCharacterViewPrefab;
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject playerPartyParent;
        [SerializeField] private GameObject foePartyParent;

        private List<BattleCharacterView> _characterViews;
    
        private Battle _battle;

        public void Setup(Battle battle, Vector3 position)
        {
            _characterViews = new();
            // destroy all children from last encounter
            foreach(Transform child in playerPartyParent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(Transform child in foePartyParent.transform)
            {
                Destroy(child.gameObject);
            }
            
            parent.transform.position = position;
            parent.SetActive(true);
            _battle = battle;

            foreach(var character in battle.Foes)
            {
                var bcv = Instantiate(battleCharacterViewPrefab, foePartyParent.transform).GetComponent<BattleCharacterView>();
                bcv.Setup(character);
                _characterViews.Add(bcv);
            }

            foreach(var character in battle.Allies)
            {
                var bcv = Instantiate(battleCharacterViewPrefab, playerPartyParent.transform).GetComponent<BattleCharacterView>();
                bcv.Setup(character);
                _characterViews.Add(bcv);
            }

        }

        public void Refresh()
        {
            for (int i = 0; i < _characterViews.Count; i++)
            {
                _characterViews[i].Refresh(_battle.AllCharacters[i]);
            }
        }

        public void Hide()
        {
            parent.SetActive(false);
        }
    }
}