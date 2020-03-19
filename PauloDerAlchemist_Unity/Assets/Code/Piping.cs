using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDA {

	enum inputState {
		NON,
		MOVE,
		PLACE,
	}
	public class Piping : MonoBehaviour {

		[Header("References")]
		[SerializeField] GameObject r_curser;
		[SerializeField] GameObject p_pipe;

		[Header("Variables")]
		[SerializeField] float m_snapToPointDistance = 3;
		[SerializeField] float m_snapToLineDistance = 1;
		[SerializeField] float m_escapeDistance = 1;

		[SerializeField] inputState m_currentState = inputState.NON;

		Transform pointHolder;

		Vector3 m_startPos;
		Vector3? m_firstPos = null;
		Point m_firstPoint;
		Pipe m_firstLineToSplit;
		List<Point> m_points = new List<Point>();
		Point m_currentPoint;
		Pipe m_currentLineToSplit;

		private void Start() {
			pointHolder = new GameObject("PointHolder").transform;
		}

		void Update() {

			foreach(var it in Pipe.s_references) {
				Debug.DrawLine(it.m_start.position, it.m_stop.position, Color.gray);
			}

			switch(m_currentState) {
			case inputState.NON:
				break;
			case inputState.MOVE:
				UpdateMove();
				break;
			case inputState.PLACE:
				UpdatePlace();
				break;
			default:
				break;
			}
		}

		Point m_target;

		void UpdateMove() {
			PositionPreview(m_target == null);

			if(Input.GetMouseButtonDown(0)) {
				m_target = m_currentPoint;
				if(m_target == null) {
					return;
				}
				m_target.m_isMoving = true;
			}
			if(m_target == null) {
				return;
			}

			if(Input.GetMouseButtonUp(0)) {
				m_target.m_isMoving = false;
				m_target = null;
				return;
			}

			m_target.position = r_curser.transform.position;
		}

		void UpdatePlace() {
			PositionPreview();

			if(Input.GetMouseButtonDown(0)) {
				m_startPos = r_curser.transform.position;
			} else if(Input.GetMouseButtonUp(0) &&
				 Vector3.Distance(m_startPos, r_curser.transform.position) < m_escapeDistance) {
				if(m_firstPos == null) {
					m_firstPos = r_curser.transform.position;
					m_firstPoint = m_currentPoint;
					m_firstLineToSplit = m_currentLineToSplit;
				} else {
					if(m_firstPoint == null) {
						m_firstPoint = new GameObject().AddComponent<Point>();
						m_firstPoint.position = m_firstPos.Value;
						m_firstPoint.transform.parent = pointHolder;
						m_points.Add(m_firstPoint);

						if(m_firstLineToSplit != null) {
							m_firstLineToSplit.Split(m_firstPoint);
						}
					}
					if(m_currentPoint == null) {
						m_currentPoint = new GameObject().AddComponent<Point>();
						m_currentPoint.position = r_curser.transform.position;
						m_currentPoint.transform.parent = pointHolder;
						m_points.Add(m_currentPoint);

						if(m_currentLineToSplit != null) {
							m_currentLineToSplit.Split(m_currentPoint);
						}
					}

					var element = Instantiate(p_pipe).GetComponent<Pipe>();
					element.Init(m_firstPoint, m_currentPoint);

					m_firstPos = null;
				}
			}
		}

		void PositionPreview(bool snap = true) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out RaycastHit hit)) {
				var snapPoint = findNearsetPoint(m_points, hit.point, m_snapToPointDistance);
				if(snap) {
					if(snapPoint != null) {
						r_curser.transform.position = snapPoint.position;
						m_currentPoint = snapPoint; //TODO: bad Pracktice
						return;
					}
				}

				m_currentPoint = null;

				if(snap) {
					var snapPos = findNearestPointOnLine(Pipe.s_references, hit.point, m_snapToLineDistance);
					if(snapPos != null) {
						r_curser.transform.position = snapPos.Value;
						return;
					}
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
		Point findNearsetPoint(List<Point> targets, Vector3 position, float MaxValue = -1) {
			if(MaxValue < 0) {
				MaxValue = float.MaxValue;
			}
			if(MaxValue == 0) {
				return null;
			}

			Point element = null;
			foreach(var it in targets) {
				//Optimize: squared dist
				float dist = Vector3.Distance(position, it.position);
				if(dist < MaxValue) {
					MaxValue = dist;
					element = it;
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
		Vector3? findNearestPointOnLine(List<Pipe> targets, Vector3 position, float MaxValue = -1) {
			if(MaxValue < 0) {
				MaxValue = float.MaxValue;
			}
			if(MaxValue == 0) {
				return null;
			}

			Vector3? element = null;
			m_currentLineToSplit = null;
			foreach(var it in targets) {
				Vector3 line = it.m_stop.position - it.m_start.position;
				Vector3 target = position - it.m_start.position;

				float distToLot = Vector3.Dot(target, line) / Vector3.Dot(line, line);

				if(distToLot < 0 || distToLot > 1) {
					continue;
				}

				Vector3 lot = distToLot * line;

				if((lot - target).magnitude < MaxValue) {
					MaxValue = (lot - target).magnitude;
					element = lot + it.m_start.position;
					m_currentLineToSplit = it;//TODO: bad Pracktice
				}
			}

			return element;
		}
	}
}
