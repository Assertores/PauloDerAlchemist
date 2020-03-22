using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace PDATest2 {
	public class Pipe : Multiton<Pipe> {

		[SerializeField] Transform r_end;
		[SerializeField] Transform r_pipe;
		[SerializeField] public Collider r_col;

		public Vector3 m_start { get => m_targetStart.transform.position; }
		public Vector3 m_end { get => m_targetEnd.transform.position; }
		public Region m_region { get => m_targetStart.m_region; }//TODO: could be null

		Node m_targetStart;
		Node m_targetEnd;

		protected override void OnMyDestroy() {
			m_targetStart?.RemovePipe(this);
			m_targetEnd?.RemovePipe(this);
		}

		public bool Init(Node start, Node end) {
			Debug.Assert(start != null);
			Debug.Assert(end != null);

			if(start == end) {
				return false;
			}
			if(end.m_region == null) {
				end.m_region = start.m_region;
				if(end.m_region == null) {
					end.m_region = start.m_region = new Region();
				}
			}
			if(!start.m_region.Connect(end.m_region)) {
				return false;
			}

			m_targetStart = start;
			m_targetEnd = end;
			start.AddPipe(this);
			end.AddPipe(this);

			RecalculatePosition();

			return true;
		}

		public bool Split(Node pos) {
			if(pos.m_region == null) {
				pos.m_region = m_targetStart.m_region;
			}else if(!m_targetStart.m_region.Connect(pos.m_region)) {
				return false;
			}

			var element = Instantiate(this.gameObject).GetComponent<Pipe>();

			if(!element.Init(pos, m_targetEnd)) {
				Destroy(element.gameObject);
				return false;
			}
			Node oldEnd = m_targetEnd;
			if(!Init(m_targetStart, pos)) {
				Destroy(element.gameObject);
				return false;
			} else {
				oldEnd.RemovePipe(this);
			}

			return true;
		}

		public bool ReplaceNode(Node oldNode, Node newNode) {
			if(m_targetStart == oldNode) {
				m_targetStart = newNode;
			} else if(m_targetEnd = oldNode) {
				m_targetEnd = newNode;
			}

			if(m_targetStart == m_targetEnd) {
				Destroy(this.gameObject);
				return false;
			}

			return true;
		}

		public void RecalculatePosition() {
			Vector3 relVec = m_targetEnd.transform.position - m_targetStart.transform.position;

			transform.position = m_targetStart.transform.position;
			transform.rotation = Quaternion.LookRotation(relVec, Vector3.up);
			r_pipe.localScale = new Vector3(1, 1, relVec.magnitude);
			r_end.localPosition = new Vector3(0, 0, relVec.magnitude);
		}
	}
}
