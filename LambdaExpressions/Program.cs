/*
C# Lambda Expression is a short block of code that accepts parameters and returns a value. 
It is defined as an anonymous function (function without a name) 

 Expression Lambda: Expression lambda contains a single expression in the lambda body.
 Statement Lambda: Statement lambda encloses one or more statements in the lambda body.

[if a delegate var returns a value it's a func
and if it's void then it's an action]

Outer variables referenced by a lambda expression are called captured variables. 
A lambda expression that captures variables is called a closure.

Expression tree is expressions arranged in a tree-like data structure.

Local functions have better performance, lambda should be used when we need to pass around a short piece of code.
*/

// Expression Lambda, type of square is Delegate of Func<int, int> (last parameter is return value)
using System.Linq.Expressions;

var square = (int x) => x * x;
Console.WriteLine($"square using Expression Lambda. for example square of 5 is {square(5)}");

// Statement Lambda, type of square is Delegate of Func<int, int, int> (last parameter is return value)
var sum = (int x, int y) =>
{
    var result = x + y;
    return result;
};
Console.WriteLine($"sum using Statement Lambda. for example sum of 6 and 9 is {sum(6,9)}");

// Closure, type of sample is action because it's void
// Captured variables, variables like n that referenced by a lambda expression
var n = 50;
var sample = () => Console.WriteLine(n);

Console.Write("Closure with parameter n: ");
sample();
n = 35;
Console.Write("Closure with changed n: ");
sample();

// Captured variables life time in a single Delegate
var seed = 0;
var sample2 = () =>
{
    return seed++;
};

Console.WriteLine("Captured variables with change local parameter: ");
Console.WriteLine(sample2());
Console.WriteLine(sample2());
Console.WriteLine(sample2());

// Expression tree
#region instance
var students = new List<Student>
{
    new Student
    {
        Name = "x1",
        Age = 13
    },
    
    new Student
    {
        Name = "x2",
        Age = 27
    },
    
    new Student
    {
        Name = "x3",
        Age = 19
    },
    
    new Student
    {
        Name = "x4",
        Age = 12
    },

};
#endregion

Expression<Func<Student, bool>> isTeenAgerExpr = s => s.Age > 12 && s.Age < 20;
var isTeenAgerFunc = isTeenAgerExpr.Compile();
/*
The compiler will translate the above expression into the following expression tree:

Expression.Lambda<Func<Student, bool>>(
                Expression.AndAlso(
                    Expression.GreaterThan(Expression.Property(pe, "Age"), Expression.Constant(12, typeof(int))),
                    Expression.LessThan(Expression.Property(pe, "Age"), Expression.Constant(20, typeof(int)))),
                        new[] { pe });
*/
Console.WriteLine("expression tree: ");
Console.WriteLine(string.Join(", ", students.Where(x => isTeenAgerFunc(x)).Select(x => x.Name)));

// Build expression tree manually ( isAdult for age >= 18 )
// [ s => s.Age >= 18 ]

// s
ParameterExpression pe = Expression.Parameter(typeof(Student), "s");

// s.Age
MemberExpression me = Expression.Property(pe, "Age");

// 18
ConstantExpression constant = Expression.Constant(18, typeof(int));

// s.Age >= 18
BinaryExpression body = Expression.GreaterThanOrEqual(me, constant);

// Create Lambda Expression
Expression<Func<Student, bool>> isAdultExprTree = Expression.Lambda<Func<Student, bool>>(body, new[] { pe });

// Compile
var isAdultExprFunc = isAdultExprTree.Compile();

// Execute
Console.WriteLine("manually expression tree: ");
Console.WriteLine(string.Join(", ", students.Where(x => isAdultExprFunc(x)).Select(x => x.Name)));


class Student
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}