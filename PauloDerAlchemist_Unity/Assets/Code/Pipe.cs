using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDA {
	public class Pipe : MonoBehaviour {

		public static List<Pipe> s_references = new List<Pipe>();

		[SerializeField] Transform r_start;
		[SerializeField] Transform r_stop;
		[SerializeField] Transform r_pipe;
		public Transform m_start { get => r_start; }
		public Transform m_stop { get => r_stop; }

		Point m_targetStart;
		Point m_targetStop;

		private void Start() {
			s_references.Add(this);
		}

		private void OnDestroy() {
			s_references.Remove(this);
		}

		private void Update() {
			if(m_targetStart.IsMoving || m_targetStop.IsMoving) {
				Recalculate(m_targetStart.position, m_targetStop.position);
			}
		}

		public void Init(Point start, Point stop) {
			m_targetStart = start;
			m_targetStop = stop;

			Recalculate(start.position, stop.position);
		}

		public void Split(Point pos) {
			var element = Instantiate(this.gameObject).GetComponent<Pipe>();

			element.Init(pos, m_targetStop);

			Init(m_targetStart, pos);
		}

		void Recalculate(Vector3 start, Vector3 stop) {
			Vector3 relVec = stop - start;

			transform.position = start;
			transform.rotation = Quaternion.LookRotation(relVec, Vector3.up);
			r_pipe.localScale = new Vector3(1, 1, relVec.magnitude);
			r_stop.localPosition = new Vector3(0, 0, relVec.magnitude);
		}
	}
}
