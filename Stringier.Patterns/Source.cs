﻿using System.IO;

namespace System.Text.Patterns {
	/// <summary>
	/// Represents a parsing source
	/// </summary>
	/// <remarks>
	/// This is a text buffer with an included implicit cursor. This cursor (<see cref="Position"/>) is not exposed to the downstream programmer, but is available for direct manipulation by patterns and parsers within this assembly.
	/// </remarks>
	public ref struct Source {
		/// <summary>
		/// Buffer the source represents
		/// </summary>
		private readonly ReadOnlySpan<Char> Buffer;

		/// <summary>
		/// Construct a new <see cref="Source"/> from the specified <paramref name="String"/>
		/// </summary>
		/// <param name="String">A <see cref="String"/> to use as a source</param>
		public Source(String String) {
			Buffer = String.AsSpan();
			Position = 0;
		}

		/// <summary>
		/// Construct a new <see cref="Source"/> from the specified <paramref name="Stream"/>
		/// </summary>
		/// <param name="Stream">A <see cref="Stream"/> to use as a source</param>
		public Source(Stream Stream) {
			using (StreamReader Reader = new StreamReader(Stream)) {
				Buffer = Reader.ReadToEnd().AsSpan();
			}
			Position = 0;
		}

		/// <summary>
		/// Construct a new <see cref="Source"/> from the specified <paramref name="Span"/>
		/// </summary>
		/// <param name="Span">A <see cref="Span{T}"/> to use as a source</param>
		public Source(Span<Char> Span) {
			Buffer = Span;
			Position = 0;
		}

		/// <summary>
		/// Construct a new <see cref="Source"/> from the specified <paramref name="Span"/>
		/// </summary>
		/// <param name="Span">A <see cref="ReadOnlySpan{T}"/> to use as a source</param>
		public Source(ReadOnlySpan<Char> Span) {
			Buffer = Span;
			Position = 0;
		}

		/// <summary>
		/// Whether currently at the end of the source
		/// </summary>
		public Boolean EOF => Length == 0;

		/// <summary>
		/// The remaining length of the source
		/// </summary>
		public Int32 Length => Buffer.Length - Position;

		/// <summary>
		/// The position within the source buffer
		/// </summary>
		/// <remarks>
		/// This is for internal manipulation, such as resetting the index after a failed consume
		/// </remarks>
		internal Int32 Position { get; set; }

		/// <summary>
		/// Peek at the next <see cref="Char"/> without advancing the position
		/// </summary>
		/// <returns>The next <see cref="Char"/> in the <see cref="Source"/></returns>
		internal ref readonly Char Peek() => ref Buffer[Position];

		/// <summary>
		/// Peek at the next <paramref name="Count"/> <see cref="Char"/> without advancing the position
		/// </summary>
		/// <param name="Count">A 32-bit integer specifying how many <see cref="Char"/> to return</param>
		/// <returns>The next <paramref name="Count"/> <see cref="Char"/> in the <see cref="Source"/></returns>
		internal ReadOnlySpan<Char> Peek(Int32 Count) => Buffer.Slice(Position, Count);

		/// <summary>
		/// Read the next <see cref="Char"/> and advance the position
		/// </summary>
		/// <returns>The next <see cref="Char"/> in the <see cref="Source"/></returns>
		internal ref readonly Char Read() => ref Buffer[Position++];

		/// <summary>
		/// Read the next <paramref name="Count"/> <see cref="Char"/> and advance the position
		/// </summary>
		/// <param name="Count">A 32-bit integer specifying how many <see cref="Char"/> to return</param>
		/// <returns>The next <paramref name="Count"/> <see cref="Char"/> in the <see cref="Source"/></returns>
		internal ReadOnlySpan<Char> Read(Int32 Count) {
			ReadOnlySpan<Char> Result = Peek(Count);
			Position += Count;
			return Result;
		}

		/// <summary>
		/// Retrieves a substring from this instance. The substring starts at a specified character position and continues to the end of the string.
		/// </summary>
		/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
		/// <returns>A <see cref="String"/> that is equivalent to the substring that begins at <paramref name="startIndex"/> in this instance, or Empty if <paramref name="startIndex"/> is equal to the length of this instance.</returns>
		internal ReadOnlySpan<Char> Substring(Int32 startIndex) => Buffer.Slice(startIndex);

		/// <summary>
		/// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
		/// </summary>
		/// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
		/// <param name="length">The number of characters in the substring.</param>
		/// <returns>A <see cref="String"/> that is equivalent to the substring of length <paramref name="length"/> that begins at <paramref name="startIndex"/> in this instance, or Empty if <paramref name="startIndex"/> is equal to the length of this instance and <paramref name="length"/> is zero.</returns>
		internal ReadOnlySpan<Char> Substring(Int32 startIndex, Int32 length) => Buffer.Slice(startIndex, length);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override String ToString() => Buffer.Slice(Position).ToString();

		/// <summary>
		/// Check for the end of the <see cref="Source"/>
		/// </summary>
		public static Pattern End => new EndChecker();
	}
}
