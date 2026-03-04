using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
class Program
{
    static void Test(Action block)
    {
        try { block();}
        // 3 Catchblocks added (Message set in exceptions)
        catch (DivideByZeroException e) {Console.WriteLine(e.Message);}
        catch (FactorialOfNegativeException e) {Console.WriteLine(e.Message);}
        catch (ArgumentException e) { Console.WriteLine(e.Message);}
        catch (Exception) { Console.WriteLine("generic exception");}
    }

    // From Assignment
    static void DivCase()
    {
        IVisitable exp = new Div(new Literal(120), new Add(new Literal(8), new
        Literal(4)));
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
        exp = new Div(new Mul(new Literal(5), new Literal(4)), new Literal(0));
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
    }
    static void FctCase()
    {
        IVisitable exp = new Fct(new Mul(new Literal(2), new Literal(3)));
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
        exp = new Fct(new Mul(new Literal(-2), new Literal(7)));
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
    }
    static void MaxCase()
    {
        IVisitable[] args = new IVisitable[] { new Add(new Literal(-2), new
        Literal(9)), new Literal(6), new Literal(0) };
        IVisitable exp = new Max(args);
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
        args = new IVisitable[0];
        exp = new Max(args);
        Console.Write(new Stringifier(exp) + " = ");
        Console.WriteLine(new Evaluator(exp));
    }
    static void Main()
    {
        Test(DivCase);
        Test(FctCase);
        Test(MaxCase);
    }
}
