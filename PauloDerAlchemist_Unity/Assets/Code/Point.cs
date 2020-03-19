using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDA {
	public class Point : MonoBehaviour {

		[SerializeField] bool m_canBeMoved = true;

		public Vector3 position { get => transform.position; set {
				if(m_canBeMoved) {
					transform.position = value;
				}
			}
		}
		public Quaternion rotation {
			get => transform.rotation;
			set {
				if(m_canBeMoved) {
					transform.rotation = value;
				}
			}
		}

		public bool m_isMoving = false;
	}
}
