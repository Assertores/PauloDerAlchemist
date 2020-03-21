using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDATest2 {
	public class Plane : MonoBehaviour {

		[SerializeField] Material r_plane;

		Quaternion[] m_rotations = new Quaternion[3];
		Color[] m_colors = new Color[3];
		Vector3[] m_axis = new Vector3[3];
		int m_currentIndex;

		private void Start() {
			m_rotations[0] = Quaternion.Euler(0, 90, 0);
			m_rotations[1] = Quaternion.Euler(90, 0, 0);
			m_rotations[2] = Quaternion.Euler(0, 0, 0);

			m_colors[0] = Color.red;
			m_colors[1] = Color.green;
			m_colors[2] = Color.blue;

			m_colors[0].a = m_colors[1].a = m_colors[2].a = r_plane.color.a;

			m_axis[0] = Vector3.right;
			m_axis[1] = Vector3.up;
			m_axis[2] = Vector3.forward;

			transform.rotation = m_rotations[m_currentIndex];
			r_plane.color = m_colors[m_currentIndex];
		}

		void Update() {
			//TODO: replace with something better
			if(Input.GetKeyUp(KeyCode.F)) {
				m_currentIndex++;
				m_currentIndex %= m_rotations.Length;

				transform.rotation = m_rotations[m_currentIndex];

				r_plane.color = m_colors[m_currentIndex];
			}

			transform.position += Input.mouseScrollDelta.y * m_axis[m_currentIndex];
		}
	}
}
