using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Storage;
using Calendar_App.Application;
using Calendar_App.Model;
using Calendar_App.Persistence.MsSql;


internal class Program
{
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<AppDbContext>();
                    services.AddSingleton<ICoachDataProvider, CoachDataProvider>();
                    services.AddSingleton<ICoachService, CoachService>();
                    services.AddSingleton<ITrainingDataProvider, TrainingDataProvider>();
                    services.AddSingleton<ITrainingService, TrainingService>();

                })
                .Build();
        host.Start();

        using IServiceScope serviceScope = host.Services.CreateScope();

        ICoachService coachService = host.Services.GetRequiredService<ICoachService>();
        ITrainingService trainingService = host.Services.GetRequiredService<ITrainingService>();

        coachService.CoachAdded += CoachAdded;

        


        //Display
        //Menu        
        int userInput;
        bool isValidInput;
        do
        {
            Console.Clear();
            Console.WriteLine("What would you like to do? \n\n" +
                " 1.Modify/Add coach \n" +
                " 2.Add new training \n" +
                " 3.List trainings for every week (create JSON) \n" +
                " 4.List room information \n" +
                " 5.List the rate of levels \n" +
                " 6.List schedule by days \n" +
                " 0.Exit \n");
            Console.WriteLine("\nChoose a number");
            do
            {

                string? input = Console.ReadLine();

                isValidInput = int.TryParse(input, out userInput) && userInput >= 0 && userInput <= 6;

                if (!isValidInput)
                {
                    Console.WriteLine("Please choose an existing option");
                }
            } while (!isValidInput);

            if(userInput == 0)
            {
                break;
            }

            if (userInput == 1)
            {
                int userInput2;
                bool isValidInput2;
                Console.Clear();
                Console.WriteLine("1.Modify coach \n" +
                                  "2.Add coach");
                Console.WriteLine("\nChoose a number");

                do
                {
                    string? input = Console.ReadLine();

                    isValidInput2 = int.TryParse(input, out userInput2) && userInput2 >= 1 && userInput2 <= 2;

                    if (!isValidInput2)
                    {
                        Console.WriteLine("Please choose an existing option");
                    }
                }
                while (!isValidInput2);

                if (userInput2 == 1)
                {
                    Regex specificNumberPattern = new Regex("^36[1-9][0-9]{8}$");
                    int coachId;
                    bool isValidId;
                    bool coachFound;
                    Console.Clear();
                    Console.WriteLine("Which coach would you like to modify?");
                    foreach (var item in coachService.GetCoachEntities())
                    {
                        Console.WriteLine("ID:" + item.Id + "\t" + item.Name + "\t" + item.PhoneNumber);
                    }
                    Console.WriteLine("\nChoose an ID");
                    do
                    {
                        string? coachIdInput = Console.ReadLine();
                        isValidId = int.TryParse(coachIdInput, out coachId) && coachService.IsThereACoach(coachId);
                        coachFound = isValidId;
                        if (!isValidId)
                        {
                            Console.WriteLine("Please choose an existing ID");
                        }

                    } while (!coachFound);
                    Console.Clear();

                    Console.WriteLine("1.Delete coach \n" +
                                      "2.Change coach data");
                    Console.WriteLine("\nChoose a number");

                    int userInput3;
                    bool isValidInput3;
                    do
                    {
                        string? input = Console.ReadLine();

                        isValidInput3 = int.TryParse(input, out userInput3) && userInput3 >= 1 && userInput3 <= 2;

                        if (!isValidInput3)
                        {
                            Console.WriteLine("Please choose an existing option");
                        }
                    }
                    while (!isValidInput3);
                    if (userInput3 == 1)
                    {
                        coachService.RemoveCoach(coachId);
                    }
                    if (userInput3 == 2)
                    {

                        string? newName;
                        long newNumber = 0;
                        bool isValidNumber;
                        string? newNumberInput;

                        Console.Clear();
                        Console.WriteLine("Please write the new name");
                        do
                        {
                            newName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newName) || !Regex.IsMatch(newName, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"))
                            {
                                Console.WriteLine("You must type a name!");
                            }

                        } while (string.IsNullOrWhiteSpace(newName) || !Regex.IsMatch(newName, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"));

                        Console.Clear();

                        Console.WriteLine("Please write the new phone number");
                        do
                        {
                            newNumberInput = Console.ReadLine();

                            if (newNumberInput != null)
                            {
                                isValidNumber = specificNumberPattern.IsMatch(newNumberInput);


                                if (!isValidNumber)
                                {
                                    Console.WriteLine("Invalid format. Example: 36205123350");
                                }
                                else
                                {
                                    if (!long.TryParse(newNumberInput, out newNumber))
                                    {
                                        Console.WriteLine("Invalid format");
                                        isValidNumber = false;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Please write a valid phone number");
                                isValidNumber = false;
                            }

                        } while (!isValidNumber);

                        CoachEntity newCoach = new CoachEntity { Name = newName, PhoneNumber = newNumber };


                        coachService.UpdateCoach(coachId, newCoach);
                    }


                }
                if (userInput2 == 2)
                {
                    Regex specificNumberPattern = new Regex("^36[1-9][0-9]{8}$");
                    string? newName;
                    long newNumber = 0;
                    bool isValidNumber;
                    string? newNumberInput;
                    Console.Clear();
                    Console.WriteLine("Please write the new name");
                    do
                    {
                        newName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newName) || !Regex.IsMatch(newName, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"))
                        {
                            Console.WriteLine("You must type a name!");
                        }

                    } while (string.IsNullOrWhiteSpace(newName) || !Regex.IsMatch(newName, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"));

                    Console.Clear();

                    Console.WriteLine("Please write the new phone number");
                    do
                    {
                        newNumberInput = Console.ReadLine();

                        if (newNumberInput != null)
                        {
                            isValidNumber = specificNumberPattern.IsMatch(newNumberInput);


                            if (!isValidNumber)
                            {
                                Console.WriteLine("Invalid format. Example: 36205123350");
                            }
                            else
                            {
                                if (!long.TryParse(newNumberInput, out newNumber))
                                {
                                    Console.WriteLine("Invalid format");
                                    isValidNumber = false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please write a valid phone number");
                            isValidNumber = false;
                        }
                    } while (!isValidNumber);
                    CoachEntity newCoach = new CoachEntity { PhoneNumber = newNumber, Name = newName };
                    coachService.AddCoach(newCoach);


                }

            }



            if (userInput == 2)
            {

                bool trainingAddedSuccessfully = false;
                do
                {
                    int coachId;
                    bool isValidId;
                    bool coachFound;
                    Console.Clear();
                    Console.WriteLine("Which coach will get the new training?");

                    foreach (var item in coachService.GetCoachEntities())
                    {
                        Console.WriteLine("ID:" + item.Id + "\t" + item.Name + "\t" + item.PhoneNumber);
                    }
                    Console.WriteLine("\nChoose an ID");
                    do
                    {
                        string? coachIdInput = Console.ReadLine();
                        isValidId = int.TryParse(coachIdInput, out coachId) && coachService.IsThereACoach(coachId);
                        coachFound = isValidId;
                        if (!isValidId)
                        {
                            Console.WriteLine("Please choose an existing ID");
                        }

                    } while (!coachFound);
                    Console.Clear();
                    Console.WriteLine("What type of training will it be?");
                    string? newType;
                    do
                    {
                        newType = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newType) || !Regex.IsMatch(newType, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"))
                        {
                            Console.WriteLine("Invalid format");
                        }

                    } while (string.IsNullOrWhiteSpace(newType) || !Regex.IsMatch(newType, "^[a-zA-ZáéíóöőúüűÁÉÍÓÖŐÚÜŰ ]+$"));
                    Console.Clear();

                    Console.WriteLine("Please set the beginning date (YYYY.MM.DD HH:MM)");
                    string? inputBeginningDateTime = Console.ReadLine();
                    DateTime parsedBeginningDateTime;
                    string format = "yyyy.MM.dd HH:mm";

                    while (!DateTime.TryParseExact(inputBeginningDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedBeginningDateTime) || parsedBeginningDateTime.Year != 2025 || parsedBeginningDateTime.Hour < 6)
                    {
                        Console.WriteLine($"Invalid format, please use the following format and make sure the date starts with 2025: {format}");
                        inputBeginningDateTime = Console.ReadLine();
                    }
                    Console.Clear();

                    Console.WriteLine("Please set the ending date (YYYY.MM.DD HH:MM)");
                    string? inputEndingDateTime = Console.ReadLine();
                    DateTime parsedEndingDateTime;


                    while (!DateTime.TryParseExact(inputEndingDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndingDateTime) || parsedEndingDateTime.Year != 2025 || parsedEndingDateTime.Hour > 22)
                    {
                        Console.WriteLine($"Invalid format, please use the following format and make sure the date starts with 2025: {format}");
                        inputEndingDateTime = Console.ReadLine();
                    }
                    Console.Clear();

                    Console.WriteLine("Which room would you like to book?\n");
                    Console.WriteLine("1.kis terem\n" +
                                      "2.közepes terem\n" +
                                      "3.nagy terem");
                    Console.WriteLine("Choose a number");

                    string? newRoom;
                    int userInputForRoom;
                    bool isValidInputForRoom;
                    do
                    {

                        string? input = Console.ReadLine();

                        isValidInputForRoom = int.TryParse(input, out userInputForRoom) && userInputForRoom >= 1 && userInputForRoom <= 3;

                        if (!isValidInputForRoom)
                        {
                            Console.WriteLine("Choose an existing option");
                        }
                    } while (!isValidInputForRoom);
                    if (userInputForRoom == 1)
                    {
                        newRoom = "kis terem";
                    }
                    else if (userInputForRoom == 2)
                    {
                        newRoom = "közepes terem";
                    }
                    else
                    {
                        newRoom = "nagy terem";
                    }



                    Console.Clear();
                    Console.WriteLine("Set the difficulty of the training\n");
                    Console.WriteLine("1.kezdő\n" +
                                      "2.középhaladó\n" +
                                      "3.haladó");
                    Console.WriteLine("Choose a number");

                    string? newLevel;
                    int userInputForLevel;
                    bool isValidInputForLevel;
                    do
                    {

                        string? input = Console.ReadLine();

                        isValidInputForLevel = int.TryParse(input, out userInputForLevel) && userInputForLevel >= 1 && userInputForLevel <= 3;

                        if (!isValidInputForLevel)
                        {
                            Console.WriteLine("Please choose an existing option");
                        }
                    } while (!isValidInputForLevel);
                    if (userInputForLevel == 1)
                    {
                        newLevel = "kezdő";
                    }
                    else if (userInputForRoom == 2)
                    {
                        newLevel = "középhaladó";
                    }
                    else
                    {
                        newLevel = "haladó";
                    }
                    Console.Clear();

                    Console.WriteLine("Set the number of signups");

                    int userInputForSignUps;
                    bool isValidInputForSignUps;

                    do
                    {

                        string? input = Console.ReadLine();

                        isValidInputForSignUps = int.TryParse(input, out userInputForSignUps);

                        if (!isValidInputForSignUps)
                        {
                            Console.WriteLine("Please set a number");
                        }
                    } while (!isValidInputForSignUps);

                    TrainingEntity newTraining = new TrainingEntity { Type = newType, CoachEntityId = coachId, Beginning = parsedBeginningDateTime, End = parsedEndingDateTime, Location = newRoom, Level = newLevel, SignUps = userInputForSignUps };

                    try
                    {
                        trainingService.AddTraining(newTraining);
                        trainingAddedSuccessfully = true;
                    }
                    catch (RoomCapacityExceededException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();

                    }
                    catch (TrainingCollisionException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                    catch (WrongDatesGivenException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                } while (!trainingAddedSuccessfully);
                Console.WriteLine("Training added, press any key to exit");
                Console.ReadKey();

            }




            if (userInput == 3)
            {
                var coachTrainingCountsById = trainingService.GetTrainingCountsByCoachIdForWeek(new DateTime(2025, 05, 10));
                Directory.CreateDirectory("2025");

                foreach (var coachIdData in coachTrainingCountsById)
                {
                    int coachId = coachIdData.Key;
                    var trainingCounts = coachIdData.Value;

                    var coach = coachService.GetCoachById(coachId);

                    string coachDirectoryPath = Path.Combine("2025", coach.Name!);
                    Directory.CreateDirectory(coachDirectoryPath);
                    string jsonFilePath = Path.Combine(coachDirectoryPath, $"{coach.Name}.json");

                    string jsonString = JsonSerializer.Serialize(trainingCounts, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
                    File.WriteAllText(jsonFilePath, jsonString, Encoding.UTF8);
                }
                Console.WriteLine("Data saved to JSON");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }



            if (userInput == 4)
            {
                Console.WriteLine("Please enter the beginning date (YYYY.MM.DD):");
                string? beginDateInput = Console.ReadLine();
                DateTime beginningDate;

                while (!DateTime.TryParseExact(beginDateInput, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out beginningDate))
                {
                    Console.WriteLine("Invalid date format. Please use YYYY.MM.DD:");
                    beginDateInput = Console.ReadLine();
                }

                Console.WriteLine("Please enter the ending date (YYYY.MM.DD):");
                string? endDateInput = Console.ReadLine();
                DateTime endingDate;

                while (!DateTime.TryParseExact(endDateInput, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endingDate) || endingDate < beginningDate)
                {
                    Console.WriteLine("Invalid date format. Please use YYYY.MM.DD:");
                    endDateInput = Console.ReadLine();
                }

                var bookedPercentage = trainingService.GetRoomBookedPercentage(beginningDate, endingDate);

                Console.WriteLine("\nPercentage of each room:");
                for (DateTime date = beginningDate.Date; date <= endingDate.Date; date = date.AddDays(1))
                {
                    Console.WriteLine($"\n{date:yyyy.MM.dd} ({date.DayOfWeek}):");
                    if (bookedPercentage.ContainsKey(date))
                    {
                        foreach (var teremTipus in new[] { "kis terem", "közepes terem", "nagy terem" })
                        {
                            Console.WriteLine($"  {teremTipus} – {bookedPercentage[date][teremTipus]}%");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }

            if (userInput == 5)
            {
                Console.Clear();
                var trainingsByLevels = trainingService.GetTrainingsByLevels();

                foreach (var training in trainingsByLevels)
                {
                    Console.WriteLine($"{training.Key} - {training.Value}");
                    
                }
                Console.WriteLine("\nPress any key to exit");
                Console.ReadKey();
            }

            if (userInput == 6)
            {
                var allTrainings = trainingService.ReadAllTrainings();
                var trainingsByDays = allTrainings.GroupBy(t => t.Beginning.Date).OrderBy(g => g.Key).ToDictionary(g => g.Key, g => g.ToList());

                List<DateTime> days = trainingsByDays.Keys.ToList();
                int currentIndex = 0;

                if (!days.Any())
                {
                    Console.WriteLine("There are no trainings yet");
                }


                while (userInput == 6)
                {
                    Console.Clear();
                    DateTime currentDay = days[currentIndex];
                    Console.WriteLine($"Schedule - {currentDay:yyyy.MM.dd} ({currentDay.DayOfWeek})");
                    Console.WriteLine("------------------------------------");


                    if (trainingsByDays.ContainsKey(currentDay))
                    {
                        var currentDayTrainings = trainingsByDays[currentDay].OrderBy(t => t.Beginning);
                        if (currentDayTrainings.Any())
                        {
                            foreach (var training in currentDayTrainings)
                            {
                                var trainer = coachService.GetCoachById(training.CoachEntityId);
                                string? trainerName = trainer.Name;
                                Console.WriteLine($"{training.Beginning:HH:mm}-{training.End:HH:mm} | {trainerName} | {training.Type}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are no trainings for this day.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no trainings for this day");
                    }


                    Console.WriteLine("\n[J] - Next day | [B] - Previous day | [K] - Exit");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.J)
                    {
                        currentIndex++;
                        if (currentIndex >= days.Count)
                        {
                            currentIndex = 0;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.B)
                    {
                        currentIndex--;
                        if (currentIndex < 0)
                        {
                            currentIndex = days.Count - 1;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.K)
                    {
                        userInput = 999;                        
                    }
                }
            }

        } while (userInput != 0);       
    }


    static void CoachAdded()
    {
        Console.WriteLine("Coach added to database \n Press any key to continue");
        Console.ReadKey();
    }
    
}



















