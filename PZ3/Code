using System;
using System.Collections.Generic;

namespace PZ3
{
    class Program
    {
        static void Main(string[] args)
        {
            Variable a = new Variable("a");
            Variable b = new Variable("b");

            var exp = (new Add(a, b));
            Console.WriteLine(exp.Compute(new Dictionary<string, double> { ["a"] = 3.4, ["c"] = 7.6 }));
            Console.WriteLine(new Sub(a, b).Compute(new Dictionary<string, double> { ["a"] = 3.4, ["b"] = 7.6 }));
            Console.WriteLine(new Mult(a, b).Compute(new Dictionary<string, double> { ["a"] = 3.4, ["b"] = 7.6 }));
            Console.WriteLine(new Divide(a, b).Compute(new Dictionary<string, double> { ["a"] = 3.4, ["b"] = 7.6 }));

            Console.WriteLine("\n\n");

            Constant a1 = new Constant(3.4);
            Constant b1 = new Constant(7.6);
            var exp1 = (new Add(a1, b1));
            Console.WriteLine(exp1.Compute(new Dictionary<string, double> { ["a"] = 0, ["b"] = 0 }));
            Console.WriteLine(new Sub(a1, b1).Compute(new Dictionary<string, double> { ["a"] = 0, ["b"] = 0 }));
            Console.WriteLine(new Mult(a1, b1).Compute(new Dictionary<string, double> { ["a"] = 0, ["b"] = 0 }));
            Console.WriteLine(new Divide(a1, b1).Compute(new Dictionary<string, double> { ["a"] = 0, ["b"] = 0 }));
        }
    }

    public interface IExpr
    {
        double Compute(IReadOnlyDictionary<string, double> variableValues);

        IEnumerable<string> Variables { get; }
        bool IsConstant { get; }
        bool IsPolynom { get; }
    }
    abstract class Expr : IExpr
    {
        public abstract double Compute(IReadOnlyDictionary<string, double> variableValues);

        public IEnumerable<string> Variables { get; }
        public bool IsConstant { get; }
        public bool IsPolynom { get; }
    }
    abstract class UnaryOperation : Expr
    {
        public IEnumerable<string> Variables { get; }
        public bool IsConstant { get; }
        public bool IsPolynom { get; }

    }
    abstract class BinaryOperation : Expr
    {
        public BinaryOperation(Expr arg1, Expr arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
        public Expr Arg1 { get; }
        public Expr Arg2 { get; }

        public IEnumerable<string> Variables { get; }
        public bool IsConstant { get; }
        public bool IsPolynom { get; }

    }
    //abstract class Function : UnaryOperation { }


    class Add : BinaryOperation
    {
        public Add(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) + Arg2.Compute(variableValues);

        public IEnumerable<string> Variables { get; }
        public bool IsConstant { get; }
        public bool IsPolynom { get; }
    }
    class Sub : BinaryOperation
    {
        public Sub(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) - Arg2.Compute(variableValues);

        public IEnumerable<string> Variables { get; }
        public bool IsConstant { get; }
        public bool IsPolynom { get; }
    }

    class Mult : BinaryOperation
    {
        public Mult(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) * Arg2.Compute(variableValues);


        public IEnumerable<string> Variables { get; }
    }

    class Divide : BinaryOperation
    {
        public Divide(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) / Arg2.Compute(variableValues);

        public IEnumerable<string> Variables { get; }
    }

    class Variable : Expr
    {
        public string Value
        {
            get;
        }

        public Variable(string value)
        {
            Value = value;
        }
        public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            foreach (var obj in variableValues)
            {
                if (Value == obj.Key)
                    return obj.Value;                
            }
            throw new InvalidCastException("variableValues haven't key that equals variable");
        }
    }

    class Constant : Expr
    {
        public double Value
        {
            get;
        }
        public Constant(double value)
        {
            Value = value;
        }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => Value;
    }
}
