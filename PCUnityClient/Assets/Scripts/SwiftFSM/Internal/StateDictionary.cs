using System;
using System.Collections;
using System.Collections.Generic;

internal class StateDictionary<TState, TEvent>
	where TState : struct, IComparable
	where TEvent : struct, IComparable
{
	private IDictionary<TState, IInnerState<TState, TEvent>> dict;
	private IFactory<TState, TEvent> factory;

	public StateDictionary(IFactory<TState, TEvent> fac)
	{
		dict = new Dictionary<TState, IInnerState<TState, TEvent>>(new FastEnumIntEqualityComparer<TState>());
		//dict = new Dictionary<TState, IInnerState<TState, TEvent>>();
		factory = fac;
	}

	public IInnerState<TState, TEvent> this[TState stateId]
	{
		get {

			if (dict.ContainsKey(stateId))
			{
				return dict[stateId];
			}

			var s = factory.Create(stateId);
			dict.Add(stateId, s);

			return s;
		}
	}
}

internal class FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum> 
	where TEnum : struct
{
	public static int ToInt(TEnum en)
	{
		return Convert.ToInt32(en);
		//return EnumInt32ToInt.Convert(en);
	}

	public static bool EnumEquals(TEnum firstEnum, TEnum secondEnum)
	{
		return ToInt(firstEnum) == ToInt(secondEnum);
	}

	public bool Equals(TEnum firstEnum, TEnum secondEnum)
	{
		return ToInt(firstEnum) == ToInt(secondEnum);
	}

	public int GetHashCode(TEnum firstEnum)
	{
		return ToInt(firstEnum);
	}
}

