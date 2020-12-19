using Microsoft.VisualStudio.TestTools.UnitTesting;
using PZ3_4.New;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        Variable x = new Variable("x");
        Variable y = new Variable("y");
        Expr expr;
        double expected;
        double actual;

        [TestMethod]
        public void ConstantSumConstant()
        {
            expr = new Constant(-1) + new Constant(11);
            expected = 10;
            Assert.IsTrue(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(null);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ConstantSubConstant()
        {
            expr = new Constant(-32.25) - new Constant(15.15);
            expected = -47.4;
            Assert.IsTrue(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(null);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ConstantMulConstant()
        {
            expr = new Constant(0.5) * new Constant(1000);
            expected = 500;
            Assert.IsTrue(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(null);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ConstantDivConstant()
        {
            expr = new Constant(32.0) / new Constant(64.0);
            expected = 0.5;
            Assert.IsTrue(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(null);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableSumVariable()
        {
            expr = x + y;
            expected = 8.15;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = -2, ["y"] = 10.15 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableSubVariable()
        {
            expr = x - y;
            expected = -18;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 14.15, ["y"] = 32.15 });

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableMulVariable()
        {
            expr = x * y;
            expected = 24;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = -3.2, ["y"] = -7.5 });

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableDivVariable()
        {
            expr = x / y;
            expected = 1.01;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 16.16, ["y"] = 16 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableDivConstant()
        {
            expr = x / new Constant(2.0);
            expected = 2;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 4 });

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ConstantDivVariable()
        {
            expr = new Constant(1) / x;
            expected = 0.008;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 125 });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvertVariable()
        {
            expr = new Minus(x);
            expected = -1.0;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 1.0 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void InvertInvertVariable()
        {
            expr = new Minus(new Minus(x));
            expected = 1.0;
            Assert.IsFalse(expr.IsConstant);
            Assert.IsTrue(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = 1.0 });

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void FunctionSin()
        {
            expr = new Sin(x);
            expected = Math.Sin(Math.PI / 4);
            Assert.IsFalse(expr.IsConstant);
            Assert.IsFalse(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = Math.PI / 4 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void FunctionCos()
        {
            expr = new Cos(x);
            expected = Math.Cos(Math.PI / 8);
            Assert.IsFalse(expr.IsConstant);
            Assert.IsFalse(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = Math.PI / 8 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void FunctionTg()
        {
            expr = new Tg(x);
            expected = Math.Tan(Math.PI / 3);
            Assert.IsFalse(expr.IsConstant);
            Assert.IsFalse(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = Math.PI / 3 });
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void FunctionCtg()
        {
            expr = new Ctg(x);
            expected = 1 / Math.Tan(Math.PI / 13);
            Assert.IsFalse(expr.IsConstant);
            Assert.IsFalse(expr.IsPolynom);
            actual = expr.Compute(new Dictionary<string, double> { ["x"] = Math.PI / 13 });

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void VariableException()
        {
            expr = x;
            actual = x.Compute(new Dictionary<string, double> { });
            expected = 0;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ExprToString()
        {
            Variable z = new Variable("z");
            Variable t = new Variable("t");
            expr = (x+z)/(t-(t+y)*y)-(new Constant(4)-new Constant(2.5));
            string expected = "(x + z) / (t - (t + y) * y) - (4 - 2,5)";
            Assert.AreEqual(expected, expr.ToString());
        }

        [TestMethod]
        public void VarVariablesProperty()
        {
            Variable z = new Variable("z");
            Variable t = new Variable("t");
            expr = (x + z) / (t - (t + y) * y) - (x - z);
            char[] expected = { 'x', 'z', 't', 'y' };
            char[] actual = expr.Variables.ToString().ToCharArray();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
        [TestMethod]
        public void ConstVariablesProperty()
        {
           Constant five = new Constant(5);
            Constant four = new Constant(4);
            expr = five + four;
            IEnumerable<string> actual = expr.Variables;
            IEnumerable<string> expected = null;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ConstVarVariablesProperty()
        {
            Constant five = new Constant(5);
            Constant four = new Constant(4);
            expr = (five + four )+ (x + y);
            char[] actual = expr.Variables.ToString().ToCharArray();
            char[] expected = { 'x', 'y' };
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
        [TestMethod]
        public void TrigToString()
        {
            Variable z = new Variable("z");
            Variable t = new Variable("t");
            expr = new Sin(x/y) * new Cos(y+x) - new Tg(z-y) / new Ctg(t*x);
            string expected = "sin(x / y) * cos(y + x) - tg(z - y) / ctg(t * x)";
            string actual = expr.ToString();
            Assert.IsFalse(expr.IsConstant);
            Assert.IsFalse(expr.IsPolynom);
            Assert.AreEqual(expected, actual);
        }
    }
}
