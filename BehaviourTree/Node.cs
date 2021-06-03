using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public enum StatusFlag
	{
		Success, Failure, Running, None
	}

	

	public abstract class Node
	{
		private StatusFlag m_status;
		protected StatusFlag status
		{
			get
			{
				return m_status;
			}
			set
			{
				m_status = value;
			}
		}

		public virtual IEnumerable<Node> Children()
		{
			return new Node[0];
		}

		public virtual StatusFlag Evaluate()
		{
			m_status = StatusFlag.Failure;

			return m_status;
		}

		public virtual void Reset()
		{
			m_status = StatusFlag.None;
		}
	}
}
