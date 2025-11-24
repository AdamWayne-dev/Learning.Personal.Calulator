using CalculatorLibrary;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        int calculationCount = 0;
        List<(string, double)> previousCalculationsList = new List<(string, double)>();
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");
        Calculator calculator = new Calculator();
        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            previousCalculationsList = calculator.GetCalculations();
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;
            
            Console.WriteLine("Welcome to the Calculator! To begin calculating, please type 'C'\nOr to view previous calculations, please type 'H'");
            string? userInput = Console.ReadLine();
            if (userInput != null)
            {
                if(userInput.ToUpper() == "H")
                {
                    Console.WriteLine("Previous Calculations:");
                    if(previousCalculationsList.Count == 0)
                    {
                        Console.WriteLine("No previous calculations found.");
                    }
                    else
                    {
                        bool _useFirstColour = true;
                        foreach (var calculation in previousCalculationsList)
                        {
                            ConsoleColor colourA = ConsoleColor.Cyan;
                            ConsoleColor colourB = ConsoleColor.Yellow;
                            Console.ForegroundColor = _useFirstColour ? colourA : colourB;
                            _useFirstColour = !_useFirstColour;
                            Console.WriteLine($"{calculation.Item1} {calculation.Item2}");
                        }
                        Console.ResetColor();
                    }
                    Console.WriteLine("------------------------\n");
                    Console.WriteLine("If you would like to clear this history, please type ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("'Delete'");
                    Console.ResetColor();
                    Console.WriteLine("Otherwise, please type 'C' to continue to calculations.");
                    userInput = Console.ReadLine();
                    if(userInput != null && userInput.ToUpper() == "DELETE")
                    {
                        calculator.ClearCalculations();
                        Console.WriteLine("Calculation history cleared.");
                    }
                    continue;
                }
                else if(userInput.ToUpper() != "C")
                {
                    Console.WriteLine("Error: Unrecognized input.");
                    Console.WriteLine("------------------------\n");
                    continue;
                }
            }

            // Ask the user to type the first number.
            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }

            // Ask the user to type the second number.
            Console.Write("Type another number, and then press Enter: ");
            numInput2 = Console.ReadLine();

            double cleanNum2 = 0;
            while (!double.TryParse(numInput2, out cleanNum2))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput2 = Console.ReadLine();
            }

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            // Validate input is not null, and matches the pattern
            if (op == null || !Regex.IsMatch(op, "[a|s|m|d]"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                {
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else
                    {
                        calculationCount++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" Result # {0}:", calculationCount);
                        Console.ResetColor();
                        Console.WriteLine("\t{0:0.##}\n", result);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n"); // Friendly linespacing.
        }
        calculator.Finish();
        return;
    }
}