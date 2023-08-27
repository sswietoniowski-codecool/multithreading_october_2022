using MultithreadingDemo;

var calculator = new Calculator();

var a = 32;
var b = 10;

//var result = calculator.Add(a, b); // should be 42 - meaning of life and everything ;-) 

var counter = 100;
while (counter > 0)
{
    //var result = calculator.AddMultithreadedWithRaceCondition(a, b);
    //var result = calculator.AddMultithreadedAtomic(a, b);
    //var result = calculator.AddMultithreadedLock(a, b);
    var result = calculator.AddMultithreadedMutex(a, b);
    Console.WriteLine($"{a} + {b} = {result}");
    counter--;
}
