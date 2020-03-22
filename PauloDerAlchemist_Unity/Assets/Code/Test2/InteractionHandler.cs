using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace PDATest2 {

	enum interactionState {
		NON,
		MOVING,
		PLACING,
		SIZE
	}
	public class InteractionHandler : Singleton<InteractionHandler> {
		[System.Serializable]
		struct element {
			public interactionState key;
			public IInteraction value;
		}

		[SerializeField] element[] r_interactions;
		IInteraction[] m_interactions = new IInteraction[(int)interactionState.SIZE];

		[SerializeField] interactionState m_currentState;
		[SerializeField] CurserRefHolder r_curser;

		protected override void OnMyAwake() {
			foreach(var it in r_interactions) {
				m_interactions[(int)it.key] = it.value;
			}
			r_interactions = null;
		}

		private void Update() {
			m_interactions[(int)m_currentState]?.DoUpdate(r_curser);
		}
	}
}
