using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhatsApp_Auto_Newslatter
{
    class Program
    {
        private const int CheckPauseSleepTimeInMilliseconds = 20000;
        private const int QuanityOfRepeats = 12;

        private static ArrayHandler _arrayHandler = new ArrayHandler();
        private static SimulationLayer _simulationLayer;
        private static InputEmulator _inputEmulator = new InputEmulator();
        private static PixelChecker _pixelChecker = new PixelChecker();

        public static void Main()
        {
            bool isProgramRunning = true;
            bool isReminding;

            Console.Write("Чтобы провести рассылку новым контактам введите '0'. \nЧтобы провести рассылку-напоминание введите '1' \nПоле для ввода: ");
            isReminding = Convert.ToBoolean(GetBooleanNumber());
            string fullFileLink = GetPathToFile();
            _simulationLayer = new SimulationLayer(_arrayHandler.HandleFileWithNumbers(fullFileLink), _inputEmulator, _pixelChecker);

            if (!isReminding)
            {
                _simulationLayer.SendFirstMessageToFirstGroup();

                //for (int i = 0; i < QuanityOfRepeats; i++)
                while (isProgramRunning)
                {
                    _simulationLayer.CheckAllFirstGroupContactsReadiness();
                    _simulationLayer.TrySendSecondMessageToSecondGroup();
                    _simulationLayer.CheckAllSecondGroupContactsReadiness();
                    List<ContactCell> finalList = _simulationLayer.GetFinalGroup();

                    if (finalList.Count > 0)
                        _arrayHandler.CreateOrUpdateFileWithContactCells(finalList);

                    _inputEmulator.PressEscape();
                    Console.WriteLine("\nStarted to sleep");
                    Thread.Sleep(CheckPauseSleepTimeInMilliseconds);
                    Console.WriteLine("\nEnded to sleep");

                    if (_simulationLayer.CheckIsTaskEnded)
                        isProgramRunning = false;
                }
            }
            else
            {
                _simulationLayer.RemindAllContacts();
            }

            //_simulationLayer.SendSecondMessageToFirstGroupLeftovers();

            Console.WriteLine("\n\nНажмите любую клавишу для завершения работы... ");
            Console.ReadKey();
        }

        private static string GetPathToFile()
        {
            Console.Write("Введите полный путь к файлу. \nПоле для ввода: ");
            string fileLink = Console.ReadLine();
            Console.Write("\nВведите имя файла. \nПоле для ввода: ");
            string fileName = Console.ReadLine();

            string fullFileLink = fileLink + "/" + fileName + ".txt";

            return fullFileLink;
        }

        private static int GetBooleanNumber()
        {
            bool isSuccess = false;
            string userInput = Console.ReadLine();
            isSuccess = userInput == "0" || userInput == "1";

            while (!isSuccess)
            {
                Console.Write("\nВведены неверные данные. Повторите попытку. \nПоле для ввода: ");
                userInput = Console.ReadLine();
                isSuccess = userInput == "0" || userInput == "1";
            }

            return Convert.ToInt32(userInput);
        }
    }
}
