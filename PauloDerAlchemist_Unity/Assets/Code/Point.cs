using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDA {
	public class Point : MonoBehaviour {

		public Vector3 position { get => transform.position; set => transform.position = value; }
		public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }

		public bool IsMoving = false;
	}
}
