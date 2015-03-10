﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Chessie.ErrorHandling;
using Chessie.ErrorHandling.CSharp;

namespace Chessie.CSharp.Test
{
    public class Request
    {
        public string Name { get; set; }
        public string EMail { get; set; }
    }

    public class Validation
    {
        public static Result<Request, string> ValidateInput(Request input)
        {
            if (input.Name == "")
                return Result<Request, string>.FailWith("Name must not be blank");
            if (input.EMail == "")
                return Result<Request, string>.FailWith("Email must not be blank");
            return Result<Request, string>.Succeed(input);

        }
    }

    [TestFixture]
    public class SimpleValidation
    {
        [Test]
        public void CanCreateSuccess()
        {
            var request = new Request { Name = "Steffen", EMail = "mail@support.com" };
            var result = Validation.ValidateInput(request);
            Assert.AreEqual(Result<Request, string>.Succeed(request), result);
        }
    }

    [TestFixture]
    public class SimplePatternMatching
    {
        [Test]
        public void CanMatchSuccess()
        {
            var request = new Request { Name = "Steffen", EMail = "mail@support.com" };
            var result = Validation.ValidateInput(request);
            result.Match(
               (x, msgs) => { Assert.AreEqual(request, x); },
               msgs => { throw new Exception("wrong match case"); });
        }

        [Test]
        public void CanMatchFailure()
        {
            var request = new Request { Name = "Steffen", EMail = "" };
            var result = Validation.ValidateInput(request);
            result.Match(
               (x, msgs) => { throw new Exception("wrong match case"); },
               msgs => { Assert.AreEqual("Email must not be blank", msgs.First()); });
        }
    }
}