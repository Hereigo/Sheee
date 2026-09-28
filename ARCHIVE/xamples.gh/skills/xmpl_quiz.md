# Quiz Skill for C#

## Purpose
This skill guides Copilot to generate structured quiz functionality in C# projects.  
It ensures all quiz questions follow a consistent format with multiple-choice options and validation logic.

---

## Rules
- Each quiz question must have **exactly 4 options**.
- One option must be marked as the **correct answer**.
- Provide a method to **check user answers**.
- Include a **display method** to show the question and options in a user-friendly format.
- Enforce **error handling** if the number of options is not 4.

---

## Example Structures

### QuizQuestion Class
```csharp
public class QuizQuestion
{
    public string QuestionText { get; set; }
    public List<string> Options { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public QuizQuestion(string questionText, List<string> options, int correctAnswerIndex)
    {
        if (options.Count != 4)
            throw new ArgumentException("Each quiz question must have exactly 4 options.");

        QuestionText = questionText;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
    }

    public bool CheckAnswer(int userChoice)
    {
        return userChoice == CorrectAnswerIndex;
    }

    public void Display()
    {
        Console.WriteLine(QuestionText);
        for (int i = 0; i < Options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Options[i]}");
        }
    }
}
