using System;
using System.Collections.Generic;
using System.Linq;

namespace PZ3_4.New
{
    class Program
    {
        static void Main(string[] args)
        {
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Variable z = new Variable("z");
            Variable t = new Variable("t");
            Constant five = new Constant(5);
            Constant three = new Constant(3);        
            Console.WriteLine(((x + z) / (t - (t + y) * y) - (x - z)).Compute(new Dictionary<string, double> { ["x"] = 5, ["y"] = 3, ["z"] = 4 , ["t"] = 2}));
            Console.WriteLine((x + z) / (t - (t + y) * y) - (x - z));
            foreach (var item in ((x + z) / (t - (t + y) * y) - (x - z)).Variables)
            {
                Console.WriteLine(item);
            }            
            Console.ReadLine();
        }
    }

    public interface IExpr
    {
        double Compute(IReadOnlyDictionary<string, double> variableValues);
        IEnumerable<string> Variables { get; }
        bool IsConstant { get; }
        bool IsPolynom { get; }
    }
    public abstract class Expr : IExpr
    {
        public abstract double Compute(IReadOnlyDictionary<string, double> variableValues);
        public virtual IEnumerable<string> Variables { get; }
        public abstract bool IsConstant { get; }
        public abstract bool IsPolynom { get; }

        public static Add operator +(Expr a, Expr b) => new Add(a, b);
        public static Sub operator -(Expr a, Expr b) => new Sub(a, b);
        public static Mult operator *(Expr a, Expr b) => new Mult(a, b);
        public static Divide operator /(Expr a, Expr b) => new Divide(a, b);
    }

    #region Binary Operations
    public abstract class BinaryOperation : Expr
    {
        public BinaryOperation(Expr arg1, Expr arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
        public override bool IsConstant => Arg1.IsConstant && Arg2.IsConstant;
        public override bool IsPolynom => Arg1.IsPolynom && Arg2.IsPolynom;
        public override IEnumerable<string> Variables => Arg1.Variables != null ? (Arg2.Variables != null ? Arg1.Variables.Union(Arg2.Variables) : Arg1.Variables) : Arg2.Variables;
        public Expr Arg1 { get; }
        public Expr Arg2 { get; }
    }
    public class Add : BinaryOperation
    {
        public Add(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) + Arg2.Compute(variableValues);

        public override string ToString() => $"{Arg1} + {Arg2}";
    }
    public class Sub : BinaryOperation
    {
        public Sub(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) - Arg2.Compute(variableValues);

        public override string ToString() => $"{Arg1} - " + ((Arg2 is Add || Arg2 is Sub) ? $"({Arg2})" : $"{Arg2}");
    }
    public class Mult : BinaryOperation
    {
        public Mult(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) * Arg2.Compute(variableValues);


        public override string ToString() => ((Arg1 is Add || Arg1 is Sub) ? $"({Arg1}) * " : $"{Arg1} * ")
            + ((Arg2 is Add || Arg2 is Sub) ? $"({Arg2})" : $"{Arg2}");
    }
    public class Divide : BinaryOperation
    {
        public Divide(Expr a, Expr b) : base(a, b) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) =>
            Arg1.Compute(variableValues) / Arg2.Compute(variableValues);

        public override string ToString() => ((Arg1 is Add || Arg1 is Sub) ? $"({Arg1}) / " : $"{Arg1} / ")
            + ((Arg2 is Add || Arg2 is Sub) ? $"({Arg2})" : $"{Arg2}");
    }
    #endregion

    public class Variable : Expr
    {
        public string Value
        {
            get;
        }
        public override IEnumerable<string> Variables { get; }

        public Variable(string value)
        {
            Value = value;
            Variables = new string[1] { value };
        }

        public override bool IsConstant => false;
        public override bool IsPolynom => true;

        public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            double value;
            variableValues.TryGetValue(Value, out value);
            return value;
        }
        public override string ToString() => Value;
    }

    public class Constant : Expr
    {
        public double Value { get; }
        public Constant(double value)
        {
            Value = value;
        }

        public override bool IsConstant => true;
        public override bool IsPolynom => true;

        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => Value;
        public override string ToString() => Value.ToString();
    }

    public abstract class UnaryOperation : Expr
    {
        public UnaryOperation(Expr arg)
        {
            Arg = arg;
        }
        public override bool IsConstant => Arg.IsConstant;
        public override bool IsPolynom => Arg.IsPolynom;
        public override IEnumerable<string> Variables => Arg.Variables;
        public Expr Arg { get; }       
    }

    public class Minus : UnaryOperation
    {
        public Minus(Expr a) : base(a) { }

        public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            return -Arg.Compute(variableValues);
        }

        public override string ToString()
        {
            return Arg.GetType().IsSubclassOf(typeof(BinaryOperation)) ? $"-({Arg})" : $"-{Arg}";
        }
    }

    public class Diff : UnaryOperation
    {
        public Diff(Expr arg, string variable, double begin, double end, double step) : base(arg)
        {
            this.variable = variable;
            this.begin = begin;
            this.end = end;
            this.step = step;
        }

        double begin;
        double end;
        double step;
        string variable;
        public override double Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            double result = 0;
            int n = 0;
            for (double value = begin; value < end; value += step)
            {
                result += Arg.Compute(new Dictionary<string, double> { [variable] = value});
                n++;
            }
            result += Arg.Compute(new Dictionary<string, double> { [variable] = end });
            result *= (end - begin) / n;
            return result;
        }
    }

    public abstract class Function : UnaryOperation
    {
        public Function(Expr a) : base(a) { }
        public override bool IsPolynom => false;
    }
    public class Cos : Function
    {
        public Cos(Expr a) : base(a) { }
        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => Math.Cos(Arg.Compute(variableValues));
        public override string ToString() => $"cos({Arg})";
    }
    public class Sin : Function
    {
        public Sin(Expr a) : base(a) { }
        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => Math.Sin(Arg.Compute(variableValues));

        public override string ToString() => $"sin({Arg})";
    }
    public class Tg : Function
    {
        public Tg(Expr a) : base(a) { }
        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => Math.Tan(Arg.Compute(variableValues));

        public override string ToString() => $"tg({Arg})";
    }
    public class Ctg : Function
    {
        public Ctg(Expr a) : base(a) { }
        public override double Compute(IReadOnlyDictionary<string, double> variableValues) => (1 / Math.Tan(Arg.Compute(variableValues)));

        public override string ToString() => $"ctg({Arg})";
    }

    public abstract class VExpr //написать скалярное и векторное произведение + реверс знаков
    {
        public abstract Expr[] Values { get; }

        public abstract Vector Compute(IReadOnlyDictionary<string, double> variableValues);

        public static Vector operator +(VExpr a, VExpr b)
        {
            Expr[] result = new Expr[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                result[i] = a.Values[i] + b.Values[i];
            }
            return new Vector(result);
        }
        public static Vector operator -(VExpr a, VExpr b)
        {
            Expr[] result = new Expr[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                result[i] = a.Values[i] - b.Values[i];
            }
            return new Vector(result);
        }
        public static Vector operator *(VExpr a, VExpr b)
        {
            Expr[] result = new Expr[a.Values.Length];
            for (int i = 0; i < a.Values.Length; i++)
            {
                result[i] = a.Values[i] * b.Values[i];
            }
            return new Vector(result);
        }
    }

    public class Vector : VExpr
    {
        Expr[] values;
        public override Expr[] Values => values;
        public Vector(Expr[] values)
        {
            this.values = values;
        }

        public override Vector Compute(IReadOnlyDictionary<string, double> variableValues)
        {
            Constant[] result = new Constant[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = new Constant(values[i].Compute(variableValues));
            }
            return new Vector(result);
        }
        public Expr Norm() //нет корня
        {
            Expr result = values[0] * values[0];

            for (int i = 1; i < values.Length; i++)
            {
                result += values[i] * values[i];
            }

            return result;
        }

        public override string ToString()
        {
            string result = "(";
            for (int i = 0; i < values.Length - 1; i++)
            {
                result += $"{values[i]}, ";
            }
            result += $"{values[values.Length - 1]})";
            return result;
        }
    }
}
