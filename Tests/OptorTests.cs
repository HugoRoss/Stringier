﻿using System;
using System.Text.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
	[TestClass]
	public class OptorTests {
		[TestMethod]
		public void Constructor() {
			Pattern _ = -(Pattern)"Hello";
		}

		[TestMethod]
		public void Consume() {
			Pattern Optor = -(Pattern)"Hello";
			Result Result;

			Result = Optor.Consume("Hello world!");
			Assert.That.Captures("Hello", Result);

			Result = Optor.Consume("Goodbye world!");
			Assert.That.Captures("", Result);
		}
	}
}
