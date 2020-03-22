using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDATest2 {
	public class Move : IInteraction {
		[SerializeField] float m_snapToPointDistance = 2;
		[SerializeField] float m_snapToLineDistance = 1;

		bool m_moving = false;
		Node m_NodeToMove = null;

		public override void DoUpdate(CurserRefHolder curser) {
			Node targetNode;
			Pipe targetPipe;
			UpdatePosition(curser, out targetNode, out targetPipe);

			if(Input.GetMouseButtonDown(0)) {
				m_moving = true;
				m_NodeToMove = targetNode;
				OnStartMove(curser, targetNode, targetPipe);
			}else if(Input.GetMouseButtonUp(0)) {
				OnStopMove(curser, targetNode, targetPipe);
				m_moving = false;
				m_NodeToMove = null;
			} else if(m_moving) {
				OnMove(curser);
			}
		}

		void OnStartMove(CurserRefHolder curser, Node targetNode, Pipe targetPipe) {
			if(targetPipe != null) {
				m_NodeToMove = new GameObject("Node_" + Node.s_references.Count).AddComponent<Node>();
				m_NodeToMove.transform.position = curser.transform.position;
				targetPipe.Split(m_NodeToMove);
			}
			if(m_NodeToMove == null) {
				return;
			}

			m_NodeToMove.EnablePipeCollision(false);
		}

		void OnStopMove(CurserRefHolder curser, Node targetNode, Pipe targetPipe) {
			if(m_NodeToMove == null) {
				return;
			}

			m_NodeToMove.EnablePipeCollision(true);

			if(targetNode != null) {
				Debug.Log("connect nodes: " + targetNode.Connect(m_NodeToMove));
			} else if(targetPipe != null) {
				Debug.Log("split pipe: " + targetPipe.Split(m_NodeToMove));
			}
		}

		void OnMove(CurserRefHolder curser) {
			if(m_NodeToMove == null) {
				return;
			}

			m_NodeToMove.transform.position = curser.transform.position;
		}

		void UpdatePosition(CurserRefHolder curser, out Node targetNode, out Pipe targetPipe) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			targetNode = null;
			targetPipe = null;

			if(Physics.Raycast(ray, out RaycastHit hit)) {
				curser.transform.position = findNearsetPoint(hit.point, out targetNode, m_snapToPointDistance, m_NodeToMove);
				if(targetNode != null) {
					return;
				}

				curser.transform.position = findNearsetPoint(hit.point, out targetPipe, m_snapToLineDistance, m_NodeToMove == null ? new Pipe[0] : m_NodeToMove.m_pipes.ToArray());
				if(targetPipe != null) {
					return;
				}

				curser.transform.position = hit.point;
				return;
			}

			Debug.LogWarning("RayCast Lost");
		}
	}
}
