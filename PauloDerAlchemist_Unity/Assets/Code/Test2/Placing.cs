using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDATest2 {
	public class Placing : IInteraction {
		[SerializeField] float m_snapToPointDistance = 2;
		[SerializeField] float m_snapToLineDistance = 1;
		[SerializeField] GameObject p_pipe;

		bool m_drawing = false;
		Node m_prevNode = null;
		Pipe m_prevPipe = null;
		Vector3 m_prevPos;
		public override void DoUpdate(CurserRefHolder curser) {
			Node targetNode;
			Pipe targetPipe;
			UpdatePosition(curser, out targetNode, out targetPipe);

			//TODO: show Preview

			if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1)) {
				m_drawing = false;
			}

			if(Input.GetMouseButtonUp(0)) {
				if(m_drawing) {
					m_prevNode = FixNode(m_prevNode, m_prevPipe, m_prevPos);
					targetNode = FixNode(targetNode, targetPipe, curser.transform.position);

					var pipe = Instantiate(p_pipe).GetComponent<Pipe>();

					pipe.Init(m_prevNode, targetNode);
				} else if(!Input.GetMouseButton(1)) {
					m_drawing = true;
				}

				m_prevNode = targetNode;
				m_prevPipe = targetPipe;
				m_prevPos = curser.transform.position;
			}
		}

		void UpdatePosition(CurserRefHolder curser, out Node targetNode, out Pipe targetPipe) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			targetNode = null;
			targetPipe = null;
			if(Physics.Raycast(ray, out RaycastHit hit)) {
				curser.transform.position = findNearsetPoint(hit.point, out targetNode, m_snapToPointDistance);
				if(targetNode != null) {
					return;
				}

				curser.transform.position = findNearsetPoint(hit.point, out targetPipe, m_snapToLineDistance);
				if(targetPipe != null) {
					return;
				}

				curser.transform.position = hit.point;
				return;
			}

			Debug.LogWarning("RayCast Lost");
		}

		Node FixNode(Node node, Pipe pipe, Vector3 pos) {
			if(node != null) {
				return node;
			}

			var newNode = new GameObject("Node_" + Node.s_references.Count).AddComponent<Node>();
			newNode.transform.position = pos;

			if(pipe != null) {
				pipe.Split(newNode);
			}

			return newNode;
		}
	}
}
