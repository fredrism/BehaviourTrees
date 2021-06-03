using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
	public interface IBTVar
	{
		void Connect(Blackboard blackboard);
		string GetName();
	}

	public class BTVariable<T> : IBTVar
	{
		public string name;
		int index;
		Blackboard blackboard;
		public T defaultValue;
		bool setValue;

		public BTVariable(string name, bool setValue, T value = default(T))
		{
			this.name = name;
			this.defaultValue = value;
			this.setValue = setValue;
		}

		public void Connect(Blackboard blackboard)
		{
			this.index = blackboard.IndexOf(name);
			this.blackboard = blackboard;
			
			if(setValue)
			{
				blackboard.Set(index, defaultValue);
			}
		}

		public string GetName()
		{
			return name;
		}

		public T Get()
		{
			object o = blackboard.Get(index);

			return (T)o;
		}

		public void Set(T value)
		{
			blackboard.Set(index, value);
		}

		public static implicit operator BTVariable<T>((string, T) lhs)
		{
			return new BTVariable<T>(lhs.Item1, true, lhs.Item2);
		}

		public static implicit operator BTVariable<T>(string lhs)
		{
			return new BTVariable<T>(lhs, false);
		}

		public static implicit operator T(BTVariable<T> variable)
		{
			return variable.Get();
		}

		public override string ToString()
		{
			return blackboard.Get(index).ToString();
		}
	}
}
