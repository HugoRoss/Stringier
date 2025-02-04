﻿using System;
using System.Runtime.Serialization;

namespace Stringier {
	/// <summary>
	/// Represents any possible Stringier specific exception
	/// </summary>
	[Serializable]
	public abstract class StringierException : Exception {
		protected StringierException() { }
		protected StringierException(String message) : base(message) { }
		protected StringierException(String message, Exception inner) : base(message, inner) { }
		protected StringierException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
