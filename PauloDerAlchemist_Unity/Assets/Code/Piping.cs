using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDA {

	[System.Serializable]
	class Line {
		public Transform m_a;
		public Transform m_b;
	}

	public class Piping : MonoBehaviour {

		[Header("References")]
		[SerializeField] GameObject r_curser;
		[SerializeField] GameObject r_pipe;

		[Header("Variables")]
		[SerializeField] float m_snapToPointDistance = 3;
		[SerializeField] float m_snapToLineDistance = 1;
		[SerializeField] float m_escapeDistance = 1;

		[SerializeField] List<Line> m_pipes;

		Vector3? firstPosition = null;
		Vector3 startPos;
		void Update() {

			foreach(var it in m_pipes) {
				Debug.DrawLine(it.m_a.position, it.m_b.position, Color.gray);
			}

			PositionPreview();

			if(Input.GetMouseButtonDown(0)) {
				startPos = r_curser.transform.position;
			}

			if(Input.GetMouseButtonUp(0) &&
				Vector3.Distance(startPos, r_curser.transform.position) < m_escapeDistance) {
				if(firstPosition == null) {
					firstPosition = r_curser.transform.position;
				} else {
					var tmp = Instantiate(r_pipe);

					var a = tmp.transform.GetChild(0);
					var b = tmp.transform.GetChild(1);

					tmp.transform.position = firstPosition.Value;
					tmp.transform.rotation = Quaternion.LookRotation(r_curser.transform.position - firstPosition.Value, Vector3.up);
					tmp.transform.GetChild(2).localScale = new Vector3(1, 1, Vector3.Distance(r_curser.transform.position, firstPosition.Value));
					b.localPosition = new Vector3(0, 0, Vector3.Distance(r_curser.transform.position, firstPosition.Value));
					//b.transform.position = r_preview.transform.position;

					m_pipes.Add(new Line { m_a = a.transform, m_b = b.transform });

					firstPosition = null;
				}
			}
		}

		void PositionPreview() {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out RaycastHit hit)) {
				var snapPoint = findNearsetPoint(m_pipes, hit.point, m_snapToPointDistance);
				if(snapPoint != null) {
					r_curser.transform.position = snapPoint.Value;
					return;
				}

				snapPoint = findNearestPointOnLine(m_pipes, hit.point, m_snapToLineDistance);
				if(snapPoint != null) {
					r_curser.transform.position = snapPoint.Value;
					return;
				}

				r_curser.transform.position = hit.point;
			}

			return;
		}

		/*!
		 * @param target	the array to search in
		 * @param position	your position
		 * @param MaxValue	maximum distance (not included)
		 * @return			the nearest found entry in the in the array
		 */
		Vector3? findNearsetPoint(List<Line> targets, Vector3 position, float MaxValue = -1) {
			if(MaxValue < 0) {
				MaxValue = float.MaxValue;
			}
			if(MaxValue == 0) {
				return null;
			}

			Vector3? element = null;
			foreach(var it in targets) {
				//Optimize: squared dist
				float dist = Vector3.Distance(position, it.m_a.position);
				if(dist < MaxValue) {
					MaxValue = dist;
					element = it.m_a.position;
				}

				dist = Vector3.Distance(position, it.m_b.position);
				if(dist < MaxValue) {
					MaxValue = dist;
					element = it.m_b.position;
				}
			}


			return element;
		}

		/*!
		 * @param target	the array to search in
		 * @param position	your position
		 * @param MaxValue	maximum distance (not included)
		 * @return			the nearest found position on a line
		 */
		Vector3? findNearestPointOnLine(List<Line> targets, Vector3 position, float MaxValue = -1) {
			if(MaxValue < 0) {
				MaxValue = float.MaxValue;
			}
			if(MaxValue == 0) {
				return null;
			}

			Vector3? element = null;
			foreach(var it in targets) {
				Vector3 line = it.m_b.position - it.m_a.position;
				Vector3 target = position - it.m_a.position;

				float distToLot = Vector3.Dot(target, line) / Vector3.Dot(line, line);

				if(distToLot < 0 || distToLot > 1) {
					continue;
				}

				Vector3 lot = distToLot * line;

				if((lot - target).magnitude < MaxValue) {
					MaxValue = (lot - target).magnitude;
					element = lot + it.m_a.position;
				}
			}

			return element;
		}
	}
}
