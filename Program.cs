//-- Dependencies
using Newtonsoft.Json;

//-- Variables
List<Question> questions = new List<Question>();

//-- Functions
// Opens the main menu of the program and writes out all the options.
void OpenMainMenu()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("Assignment Helper v1.0");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("\n\n[1] Add new question\n[2] Start Quiz\n[3] Clear Questions\n[4] Exit");
}

// Waits for a user input with some fancy colors.
string WaitForInput()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("\n> Input: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    string inputedString = Console.ReadLine();
    Console.ForegroundColor = ConsoleColor.White;
    return inputedString;
}

// A method thats used when the user inputs something invalid.
void ShowInvalidInput()
{
    Console.Clear();
    Console.WriteLine("Invalid Input, Press any key");
    Console.ReadKey();
}

// Prompts the user to press anykey and waits using ReadKey().
void PressAnyKey()
{
    Console.Write("\n> Press any key:");
    Console.ReadKey();
}

// Checks if the user has an existing Data file and prompts them to either load the data or not.
void CheckForData()
{
    if (File.Exists("Data"))
    {
        string data = File.ReadAllText("Data");
        Question[] loadedQuestions = JsonConvert.DeserializeObject<Question[]>(data);
        string questionText = loadedQuestions.Length > 1 ? "Questions" : "Question";

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Existing question data found, do you want to load it? ({loadedQuestions.Length} {questionText}) [y/n]");
        Console.ForegroundColor = ConsoleColor.White;
        string input = WaitForInput();

        if (input == "y")
        {
            foreach (Question question in loadedQuestions)
            {
                questions.Add(question);
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{loadedQuestions.Length} {questionText.ToLower()} have been loaded!");
            Console.ForegroundColor = ConsoleColor.White;
            PressAnyKey();
        }
    }
}

// Shows the tab used for adding questions. Code allows user to add more questions.
void ShowQuestionTab()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"Current questions: {questions.Count}\n\nWrite a new question, enter 'Back' to cancel.");
        string questionInput = WaitForInput();

        if (questionInput.ToLower() != "back")
        {
            Console.Clear();
            Console.WriteLine($"Current Question: {questionInput}\n\nEnter the answer.");
            string answerInput = WaitForInput().ToLower();

            Question newQuestion = new Question();
            newQuestion.text = questionInput;
            newQuestion.answer = answerInput;
            questions.Add(newQuestion);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"New Question:\nQuestion: {questionInput}\nAnswer: {answerInput}");
            Console.ForegroundColor = ConsoleColor.White;
            PressAnyKey();

            Console.Clear();
            Console.WriteLine("Do you want to add another question? [y/n]");
            string input = WaitForInput().ToLower();

            if (input != "y")
            {
                return;
            }
        }
        else
        {
            return;
        } 
    }
}  

// Shows the tab used for exiting the program. Asks the user if they want to save their data.
void ShowExitTab()
{
    Console.Clear();
    if (questions.Count > 0)
    {
        Console.WriteLine("Do you want to save your questions? [y/n]");
        string input = WaitForInput().ToLower();

        if (input == "y")
        {
            SaveData();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data has been saved!");
            Console.ForegroundColor = ConsoleColor.White;
            PressAnyKey();
            Console.Clear();
        }
        else
        {
            Console.Clear();
        } 
    }
}

// Shows the clear question tab.
void ShowClearQuestionsTab()
{
    Console.Clear();
    Console.WriteLine("Are you sure you want to clear all questions? [y/n]");
    string input = WaitForInput().ToLower();

    if (input == "y")
    {
        questions.Clear();
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Questions cleared!");
        Console.ForegroundColor = ConsoleColor.White;
        PressAnyKey();
    }
    else
    {
        return;
    }
}

// Starts the quiz.
void ShowQuizTab()
{
    if (questions.Count > 0)
    {
        int Score = 0;
        Console.Clear();
        Console.WriteLine($"Your quiz of {questions.Count} questions will now start. Your score is displayed afterwards.");
        PressAnyKey();

        foreach (Question currentQuestion in questions)
        {
            Console.Clear();
            Console.WriteLine(currentQuestion.text);
            string input = WaitForInput().ToLower();

            Console.Clear();
            if (input == currentQuestion.answer)
            {
                Score++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You got it right! Score: {Score}/{questions.Count}");
                Console.ForegroundColor = ConsoleColor.White;
                PressAnyKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You got it wrong... The answer was '{currentQuestion.answer}'\nScore: {Score}/{questions.Count}");
                Console.ForegroundColor = ConsoleColor.White;
                PressAnyKey();
            }
        }

        Console.Clear();
        Console.WriteLine($"You finished the quiz\n\nFinal Score: {Score}/{questions.Count}");
        PressAnyKey();
    }
    else
    {
        Console.Clear();
        Console.WriteLine("You need to have atleast one question to start the quiz");
        PressAnyKey();
    }
}

// Saves the users data to a folder named "Data".
void SaveData()
{
    string ConvertedData = JsonConvert.SerializeObject(questions);
    File.WriteAllText("Data",ConvertedData);
}

//-- Runtime
CheckForData();
while (true)
{
    OpenMainMenu();
    string input = WaitForInput();

    switch (input)
    {
        case "1":
            ShowQuestionTab();
            break;
        case "2":
            ShowQuizTab();
            break;
        case "3":
            ShowClearQuestionsTab();
            break;
        case "4":
            ShowExitTab();
            return;
        default:
            ShowInvalidInput();
            break;
    }
}

//- Classes
class Question
{
    public string text;
    public string answer;
}