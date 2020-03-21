using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDATest2 {

	public enum element {
		EMPTY = 0,
		FIRE, WATER, EARTH, AIR,
		HELIUM, OZONE, CARBON,
		METHANOL, PROPANE, ETHAN, ACETALDEHYDE,
		POLYVINYLCHLORIDE, POLYTETRAFLUOROETHYLENE, SLICONDIOXIDE,
		PLASTIC, GLASS,
		STEEL, ALUMINIUM, BRASS,
		PLUTONIUM, RADON, URANIUM, FERMIUM,
		SILICON, TUNGSTEN, SILVER,
		MERCURY, PLATINUM,
		GOLD,
		SIZE
	}

	public class Context {
		public element m_content;
		public int m_amount;
	}

	public class Region {
		Context m_context;

		public element m_content { get => m_context == null ? element.EMPTY : m_context.m_content; }
		public int m_amount { get => m_context == null ? 0 : m_context.m_amount; }

		public bool Connect(Region other) {
			Debug.Assert(other != null);

			if(m_context == null) {
				m_context = other.m_context;
				return true;
			}
			if(other.m_context == null) {
				other.m_context = m_context;
				return true;
			}

			if(m_content != element.EMPTY && m_content != other.m_content) {
				return false;
			}

			other.m_context.m_amount += m_context.m_amount;
			m_context = other.m_context;

			return true;
		}
	}
}
