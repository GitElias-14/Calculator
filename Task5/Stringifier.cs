using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

class Stringifier: IVisitor {
   public Stringifier(IVisitable t) {
      s = new StringBuilder();
      t.Accept(this);
   }
   public void Visit(Literal e) {
      s.Append(e.Value);
   }
   public void Visit(Add e) => StringifierAgg(new[] {e.Left, e.Right}, "(",")","+");
   public void Visit(Mul e) => StringifierAgg(new[] {e.Left, e.Right}, null, null, "*");
   public void Visit(Div e) => StringifierAgg(new[] {e.Left, e.Right}, null, null, "/");
   public void Visit(Fct e) => StringifierAgg(new[] {e.Expr}, "(", ")!", null);
   public void Visit(Max e) => StringifierAgg(e, "max{", "}",",");

   private void StringifierAgg(IEnumerable<IVisitable> expr, string? prefix, string? suffix, string? separator)
   {
      if (prefix != null) s.Append(prefix);
      bool first = true;
      foreach (var arg in expr)
      {
         if (!first && separator != null) s.Append(separator);
         arg.Accept(this);
         first = false;
      }
      if (suffix != null) s.Append(suffix);
   }

   public void Clear() => s.Clear();
   public override string ToString() => s.ToString();
   private StringBuilder s;
}