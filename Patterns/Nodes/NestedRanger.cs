﻿using System;
using Stringier.Patterns.Errors;

namespace Stringier.Patterns.Nodes {
	/// <summary>
	/// Represents a <see cref="Ranger"/> which supports nesting of the range.
	/// </summary>
	internal sealed class NestedRanger : Ranger, IEquatable<NestedRanger> {
		/// <summary>
		/// The current nesting level.
		/// </summary>
		private Int32 Level;

		/// <summary>
		/// Initialize a new <see cref="NestedRanger"/> with the given <paramref name="from"/> and <paramref name="to"/>
		/// </summary>
		/// <param name="from">The <see cref="Pattern"/> to start from.</param>
		/// <param name="to">The <see cref="Pattern"/> to read to.</param>
		internal NestedRanger(Pattern from, Pattern to) : base(from, to) => Level = 0;

		/// <summary>
		/// Call the Consume parser of this <see cref="Node"/> on the <paramref name="source"/> with the <paramref name="result"/>.
		/// </summary>
		/// <param name="source">The <see cref="Source"/> to consume.</param>
		/// <param name="result">A <see cref="Result"/> containing whether a match occured and the captured <see cref="String"/>.</param>
		internal override void Consume(ref Source source, ref Result result) {
			From.Consume(ref source, ref result);
			if (result) {
				Level++;
			}
			while (Level > 0) {
				if (source.EOF) {
					break;
				}
				source.Position++;
				result.Length++;
				if (From.CheckHeader(ref source)) {
					From.Consume(ref source, ref result);
					if (result) {
						Level++;
					}
				}
				if (To.CheckHeader(ref source)) {
					To.Consume(ref source, ref result);
					if (result) {
						Level--;
					}
				}
			}
			if (Level != 0) {
				result.Error.Set(ErrorType.ConsumeFailed, this);
			}
		}

		/// <summary>
		/// Determines whether this instance and a specified object have the same value.
		/// </summary>
		/// <param name="other">The <see cref="Pattern"/> to compare with the current <see cref="Pattern"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Pattern"/> is equal to the current <see cref="Pattern"/>; otherwise, <c>false</c>.</returns>
		public override Boolean Equals(Pattern? other) {
			switch (other) {
			case NestedRanger ranger:
				return Equals(ranger);
			default:
				return false;
			}
		}

		/// <summary>
		/// Determines whether this instance and a specified object have the same value.
		/// </summary>
		/// <param name="other">The <see cref="NestedRanger"/> to compare with the current <see cref="Pattern"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Pattern"/> is equal to the current <see cref="Pattern"/>; otherwise, <c>false</c>.</returns>
		public Boolean Equals(NestedRanger other) => base.Equals(other);
	}
}
