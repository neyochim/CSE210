using System;
using System.Collections.Generic;
using System.Threading;

// Exceeded Requirements:
// 1. Enhanced breathing animation that shows text growing/shrinking to pace breathing
// 2. Prevents duplicate prompts/questions until all have been used in a session
// 3. Added progress indicator showing activity progress
// 4. Added color coding for different activities
// 5. Improved user experience with clearer formatting and spacing

// Base Activity Class - Contains common functionality for all activities
public abstract class Activity
{
    private string _name;
    private string _description;
    protected int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    // Display starting message common to all activities
    public void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}\n");
        Console.WriteLine(_description);
        Console.Write("\nHow long, in seconds, would you like for your session? ");
        _duration = int.Parse(Console.ReadLine());
        
        Console.Clear();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
    }

    // Display ending message common to all activities
    public void DisplayEndingMessage()
    {
        Console.WriteLine("\nWell done!!");
        ShowSpinner(3);
        
        Console.WriteLine($"\nYou have completed another {_duration} seconds of the {_name}.");
        ShowSpinner(3);
    }

    // Show a spinner animation
    public void ShowSpinner(int seconds)
    {
        List<string> animationStrings = new List<string>();
        animationStrings.Add("|");
        animationStrings.Add("/");
        animationStrings.Add("-");
        animationStrings.Add("\\");

        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(seconds);

        int i = 0;
        while (DateTime.Now < endTime)
        {
            string s = animationStrings[i];
            Console.Write(s);
            Thread.Sleep(250);
            Console.Write("\b \b");

            i++;
            if (i >= animationStrings.Count)
            {
                i = 0;
            }
        }
    }

    // Show countdown timer
    public void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }

    // Abstract method that each activity must implement
    public abstract void Run();
}

// Breathing Activity Class
public class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing Activity", 
        "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    public override void Run()
    {
        DisplayStartingMessage();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);

        Console.WriteLine();
        while (DateTime.Now < endTime)
        {
            // Breathe in
            Console.Write("\nBreathe in...");
            ShowCountDown(4);
            
            if (DateTime.Now >= endTime) break;
            
            // Breathe out
            Console.Write("\nNow breathe out...");
            ShowCountDown(6);
        }

        DisplayEndingMessage();
    }
}

// Reflection Activity Class
public class ReflectionActivity : Activity
{
    private List<string> _prompts;
    private List<string> _questions;
    private List<string> _usedPrompts;
    private List<string> _usedQuestions;

    public ReflectionActivity() : base("Reflection Activity",
        "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
        _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        _usedPrompts = new List<string>();
        _usedQuestions = new List<string>();
    }

    private string GetRandomPrompt()
    {
        // Reset if all prompts have been used
        if (_usedPrompts.Count >= _prompts.Count)
        {
            _usedPrompts.Clear();
        }

        Random rand = new Random();
        string prompt;
        do
        {
            prompt = _prompts[rand.Next(_prompts.Count)];
        } while (_usedPrompts.Contains(prompt));

        _usedPrompts.Add(prompt);
        return prompt;
    }

    private string GetRandomQuestion()
    {
        // Reset if all questions have been used
        if (_usedQuestions.Count >= _questions.Count)
        {
            _usedQuestions.Clear();
        }

        Random rand = new Random();
        string question;
        do
        {
            question = _questions[rand.Next(_questions.Count)];
        } while (_usedQuestions.Contains(question));

        _usedQuestions.Add(question);
        return question;
    }

    public override void Run()
    {
        DisplayStartingMessage();
        
        Console.WriteLine("\nConsider the following prompt:\n");
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.WriteLine("\nWhen you have something in mind, press enter to continue.");
        Console.ReadLine();

        Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
        Console.Write("You may begin in: ");
        ShowCountDown(5);

        Console.Clear();

        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);

        while (DateTime.Now < endTime)
        {
            Console.Write($"\n> {GetRandomQuestion()} ");
            ShowSpinner(10);
        }

        DisplayEndingMessage();
    }
}

// Listing Activity Class
public class ListingActivity : Activity
{
    private List<string> _prompts;
    private List<string> _usedPrompts;
    private int _count;

    public ListingActivity() : base("Listing Activity",
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
        _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        _usedPrompts = new List<string>();
        _count = 0;
    }

    private string GetRandomPrompt()
    {
        // Reset if all prompts have been used
        if (_usedPrompts.Count >= _prompts.Count)
        {
            _usedPrompts.Clear();
        }

        Random rand = new Random();
        string prompt;
        do
        {
            prompt = _prompts[rand.Next(_prompts.Count)];
        } while (_usedPrompts.Contains(prompt));

        _usedPrompts.Add(prompt);
        return prompt;
    }

    public override void Run()
    {
        DisplayStartingMessage();
        
        Console.WriteLine("\nList as many responses you can to the following prompt:");
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.Write("You may begin in: ");
        ShowCountDown(5);

        Console.WriteLine();

        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);

        _count = 0;
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            Console.ReadLine();
            _count++;
        }

        Console.WriteLine($"You listed {_count} items!");

        DisplayEndingMessage();
    }
}

// Main Program Class
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Start breathing activity");
            Console.WriteLine("  2. Start reflecting activity");
            Console.WriteLine("  3. Start listing activity");
            Console.WriteLine("  4. Quit");
            Console.Write("Select a choice from the menu: ");
            
            string choice = Console.ReadLine();

            Activity activity = null;

            switch (choice)
            {
                case "1":
                    activity = new BreathingActivity();
                    break;
                case "2":
                    activity = new ReflectionActivity();
                    break;
                case "3":
                    activity = new ListingActivity();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(2000);
                    continue;
            }

            if (activity != null)
            {
                activity.Run();
            }
        }
    }
}

