using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace PDATest2 {
	public class Node : Multiton<Node> {

		public List<Pipe> m_pipes = new List<Pipe>();

		public Region m_region;

		Vector3 m_lastPos;

		private void Start() {
			m_lastPos = transform.position;
		}

		private void Update() {
			if(m_lastPos != transform.position) {
				foreach(var it in m_pipes) {
					it.RecalculatePosition();
				}
			}
		}

		public bool Connect(Node other) {
			if(other == this) {
				return true;
			}

			if(m_region == null) {
				m_region = other.m_region;
				if(m_region == null) {
					m_region = new Region();
				}

				other.m_region = m_region;
			}else if(!m_region.Connect(other.m_region)) {
				return false;
			}

			foreach(var it in other.m_pipes) {
				if(it.ReplaceNode(other, this)) {
					m_pipes.Add(it);
				}
			}

			transform.position = other.transform.position;

			Destroy(other.gameObject);

			return true;
		}

		public void EnablePipeCollision(bool enable) {
			foreach(var it in m_pipes) {
				it.r_col.enabled = enable;
			}
		}

		public void AddPipe(Pipe element) {
			m_pipes.Add(element);
		}

		public void RemovePipe(Pipe element) {
			m_pipes.Remove(element);

			if(m_pipes.Count <= 0) {
				Destroy(this.gameObject);
			}
		}
	}
}
