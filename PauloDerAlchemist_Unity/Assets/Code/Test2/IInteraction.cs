using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDATest2 {
	public abstract class IInteraction : MonoBehaviour {

		public abstract void DoUpdate(CurserRefHolder curser);

		protected Vector3 findNearsetPoint(Vector3 pos, out Node target, float maxDist = -1, params Node[] ignore) {
			if(maxDist <= 0) {
				maxDist = float.MaxValue;
			} else {
				maxDist *= maxDist;
			}

			target = null;
			foreach(var it in Node.s_references) {
				bool found = false;
				foreach(var test in ignore) {
					if(test == it) {
						found = true;
						break;
					}
				}
				if(found) {
					continue;
				}

				float dist = (it.transform.position - pos).sqrMagnitude;
				if(dist < maxDist) {
					maxDist = dist;
					target = it;
				}
			}

			return target == null ? pos : target.transform.position;
		}

		protected Vector3 findNearsetPoint(Vector3 pos, out Pipe target, float maxDist = -1, params Pipe[] ignore) {
			if(maxDist <= 0) {
				maxDist = float.MaxValue;
			} else {
				maxDist *= maxDist;
			}

			target = null;
			Vector3 nearestPoint = pos;
			foreach(var it in Pipe.s_references) {
				bool found = false;
				foreach(var test in ignore) {
					if(test == it) {
						found = true;
						break;
					}
				}
				if(found) {
					continue;
				}

				Vector3 line = it.m_end - it.m_start;
				Vector3 toPos = pos - it.m_start;

				float distToLot = Vector3.Dot(toPos, line) / Vector3.Dot(line, line);

				if(distToLot <= 0 || distToLot >= 1) {
					continue;
				}
				Vector3 lot = distToLot * line;

				float dist = (lot - toPos).sqrMagnitude;
				if(dist < maxDist) {
					maxDist = dist;
					nearestPoint = lot + it.m_start;
					target = it;
				}
			}

			return nearestPoint;
		}
	}
}

