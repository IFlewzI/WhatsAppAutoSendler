using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhatsApp_Auto_Newslatter
{
    class SimulationLayer
    {
        private const string WindowName = "chrome";
        private const string RemindMessage = "";
        private const string FirstMessage = "Здравствуйте, это Виктория Олеговна";
        private const string SecondMessage = "Ваш телефон дала мама моего ученика, " +
            "который обучается в 24 ШКОЛЕ, сказала, что для вашего ребёнка, " +
            "возможно, актуален будет подарок в виде ДВУХ БЕСПЛАТНЫХ занятий " +
            "на любой из 4-х IT-курсов: " +
            "\n" +
            "\n" +
            "- Базовые навыки на компьютере (На данном курсе учим детей печатать, делать " +
            "\n" +
            "презентации, оформлять рефераты)" +
            "\n" +
            "\n" +
            "- Программирование (На данном курсе учимся самым актуальным языкам " +
            "\n" +
            "программирования - Phyton)" +
            "\n" +
            "\n" +
            "- Графический дизайн (Учимся делать дизайн сайтов, логотипов, плакатов)" +
            "\n" +
            "\n" +
            "- Создание игр (Учимся создавать и программировать игры на Unity, С#)" +
            "\n" +
            "\n" +
            "______" +
            "\n" +
            "\n" +
            "Если для вас действительно актуально, напишите ДА в ответ на данное " +
            "сообщение и я вам вышлю сертификат на 2 БЕСПЛАТНЫХ занятия👍";

        private List<ContactCell> _listForReminding = new List<ContactCell>();
        private List<ContactCell> _firstGroupContactList = new List<ContactCell>();
        private List<ContactCell> _secondGroupContactList = new List<ContactCell>();
        private List<ContactCell> _secondGroupContactsNeededForMeessage = new List<ContactCell>();
        private List<ContactCell> _finalContactList = new List<ContactCell>();
        private InputEmulator _inputEmulator;
        private PixelChecker _pixelChecker;

        private Color _newMessageIconColor = new Color(37, 211, 102);
        private Color _messageGotReadenIconColor1 = new Color(161, 215, 240);
        private Color _backgroundColor = new Color(255, 255, 255);
        private Color _messageGotReadenIconColor2 = new Color(83, 189, 235);

        private ValuePair<int> _searchButtonPosition = new ValuePair<int>
        {
            X = 350,
            Y = 170,
        };

        private ValuePair<int> _firstInSearchContactButtonPosition = new ValuePair<int>
        {
            X = 350,
            Y = 300,
        };

        private ValuePair<int> _writeButtonPosition = new ValuePair<int>
        {
            X = 1000,
            Y = 770,
        };

        private ValuePair<int> _newMessageIconPosition = new ValuePair<int>
        {
            X = 545,
            Y = 400,
        };

        private ValuePair<int> _newMessageIconPosition2 = new ValuePair<int>
        {
            X = 530,
            Y = 400,
        };

        private ValuePair<int> _accAvatarPosition = new ValuePair<int>
        {
            X = 50,
            Y = 380,
        };

        private ValuePair<int> _messageGotReadenIconPosition = new ValuePair<int>
        {
            X = 125,
            Y = 402,
        };

        public SimulationLayer(List<ContactCell> contactCells, InputEmulator inputEmulator, PixelChecker pixelChecker)
        {
            _listForReminding = contactCells;
            _firstGroupContactList = contactCells;
            _inputEmulator = inputEmulator;
            _pixelChecker = pixelChecker;
        }

        public bool CheckIsTaskEnded => _firstGroupContactList.Count == 0 && _secondGroupContactList.Count == 0;

        public List<ContactCell> GetFinalGroup()
        {
            List<ContactCell> listForReturning = new List<ContactCell>();

            Console.WriteLine("\nСписок контактов финальной группы: ");

            foreach (var contactCell in _finalContactList)
            {
                Console.WriteLine(contactCell.FullName);
                listForReturning.Add(contactCell);
            }

            return listForReturning;
        }

        public void RemindAllContacts()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(200);

            foreach (var contactCell in _listForReminding)
            {
                Console.WriteLine("Отправляю напоминание контакту " + contactCell.FullName);

                _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);
                _inputEmulator.Click();

                HighlightAndDeleteCurrentText();

                Thread.Sleep(100);

                _inputEmulator.Click();
                _inputEmulator.WriteText(contactCell.PhoneNumber);

                Thread.Sleep(1800);

                if (CheckIsAccountExisting())
                {
                    _inputEmulator.SetCursorPosition(_firstInSearchContactButtonPosition.X, _firstInSearchContactButtonPosition.Y);
                    _inputEmulator.Click();

                    Thread.Sleep(100);

                    _inputEmulator.SetCursorPosition(_writeButtonPosition.X, _writeButtonPosition.Y);
                    HighlightAndDeleteCurrentText();

                    Thread.Sleep(500);

                    _inputEmulator.Click();
                    _inputEmulator.WriteText(RemindMessage);
                    _inputEmulator.PressEnter();

                    Thread.Sleep(1800);
                }
            }
        }

        public void SendFirstMessageToFirstGroup()
        {
            List<ContactCell> listForRemoving = new List<ContactCell>();

            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(100);

            foreach (var contactCell in _firstGroupContactList)
            {
                Console.WriteLine("Отправляю первое сообщение контакту " + contactCell.FullName);

                _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);

                HighlightAndDeleteCurrentText();

                Thread.Sleep(100);

                _inputEmulator.WriteText(contactCell.PhoneNumber);

                Thread.Sleep(2000);

                if (CheckIsAccountExisting())
                {
                    _inputEmulator.SetCursorPosition(_firstInSearchContactButtonPosition.X, _firstInSearchContactButtonPosition.Y);
                    _inputEmulator.Click();

                    Thread.Sleep(500);

                    _inputEmulator.SetCursorPosition(_writeButtonPosition.X, _writeButtonPosition.Y);
                    //HighlightAndDeleteCurrentText();

                    _inputEmulator.Click();

                    Thread.Sleep(100);

                    _inputEmulator.WriteText(FirstMessage);
                    _inputEmulator.PressEnter();

                    Thread.Sleep(500);
                }
                else
                {
                    listForRemoving.Add(contactCell);
                }
            }

            if (listForRemoving.Count > 0)
            {
                foreach (var element in listForRemoving)
                    _firstGroupContactList.Remove(element);
            }
        }

        public void TrySendSecondMessageToSecondGroup()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(200);

            if (_secondGroupContactsNeededForMeessage.Count > 0)
            {
                foreach (var contactCell in _secondGroupContactsNeededForMeessage.ToArray())
                {
                    Console.WriteLine("Отправляю второе сообщение контакту " + contactCell.FullName);

                    _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);
                    _inputEmulator.Click();

                    HighlightAndDeleteCurrentText();

                    Thread.Sleep(400);

                    _inputEmulator.Click();

                    Thread.Sleep(100);

                    _inputEmulator.WriteText(contactCell.PhoneNumber);

                    Thread.Sleep(2000);

                    if (CheckIsAccountExisting())
                    {
                        _inputEmulator.SetCursorPosition(_firstInSearchContactButtonPosition.X, _firstInSearchContactButtonPosition.Y);
                        _inputEmulator.Click();

                        Thread.Sleep(500);

                        _inputEmulator.SetCursorPosition(_writeButtonPosition.X, _writeButtonPosition.Y);
                        //HighlightAndDeleteCurrentText();

                        Thread.Sleep(100);

                        _inputEmulator.Click();
                        _inputEmulator.WriteText(SecondMessage);
                        _inputEmulator.PressEnter();
                        _secondGroupContactsNeededForMeessage.Remove(contactCell);

                        Thread.Sleep(500);
                    }
                }
            }
        }

        public void SendSecondMessageToFirstGroupLeftovers()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(200);

            if (_firstGroupContactList.Count > 0)
            {
                foreach (var contactCell in _firstGroupContactList.ToArray())
                {
                    Console.WriteLine("Отправляю второе сообщение контакту " + contactCell.FullName);

                    _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);
                    _inputEmulator.Click();

                    HighlightAndDeleteCurrentText();

                    Thread.Sleep(100);

                    _inputEmulator.Click();
                    _inputEmulator.WriteText(contactCell.PhoneNumber);

                    Thread.Sleep(1800);

                    if (CheckIsAccountExisting())
                    {
                        _inputEmulator.SetCursorPosition(_firstInSearchContactButtonPosition.X, _firstInSearchContactButtonPosition.Y);
                        _inputEmulator.Click();

                        Thread.Sleep(400);

                        _inputEmulator.SetCursorPosition(_writeButtonPosition.X, _writeButtonPosition.Y);

                        _inputEmulator.Click();
                        _inputEmulator.WriteText(SecondMessage);
                        _inputEmulator.PressEnter();
                        _secondGroupContactsNeededForMeessage.Remove(contactCell);

                        Thread.Sleep(800);
                    }
                }
            }
        }

        public void CheckAllFirstGroupContactsReadiness()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(200);

            foreach (var contactCell in _firstGroupContactList.ToArray())
            {
                Console.WriteLine("Проверяем контакт " + contactCell.FullName + " из первой группы");

                _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);
                HighlightAndDeleteCurrentText();

                _inputEmulator.Click();
                _inputEmulator.WriteText(contactCell.PhoneNumber);

                Thread.Sleep(2800);

                bool isReady = CheckIsNewMessageThere() || CheckIsMessageGotReaden();

                if (isReady)
                {
                    Console.WriteLine(contactCell.FullName + " переходит в группу 2");

                    _firstGroupContactList.Remove(contactCell);
                    _secondGroupContactList.Add(contactCell);
                    _secondGroupContactsNeededForMeessage.Add(contactCell);
                }
            }

            Console.WriteLine("\nСписок контактов первой группы: ");

            foreach (var contactCell in _firstGroupContactList)
                Console.WriteLine(contactCell.FullName);
        }

        public void CheckAllSecondGroupContactsReadiness()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);

            Thread.Sleep(200);

            foreach (var contactCell in _secondGroupContactList.ToArray())
            {
                Console.WriteLine("Проверяем контакт " + contactCell.FullName + " из второй группы");

                _inputEmulator.SetCursorPosition(_searchButtonPosition.X, _searchButtonPosition.Y);
                HighlightAndDeleteCurrentText();

                _inputEmulator.Click();
                _inputEmulator.WriteText(contactCell.PhoneNumber);

                Thread.Sleep(2800);

                bool isReady = CheckIsNewMessageThere() || CheckIsMessageGotReaden();

                if (isReady)
                {
                    Console.WriteLine(contactCell.FullName + " переходит в финальную группу");

                    _secondGroupContactList.Remove(contactCell);
                    _finalContactList.Add(contactCell);
                }
            }

            Console.WriteLine("\nСписок контактов второй группы: ");

            foreach (var contactCell in _secondGroupContactList)
                Console.WriteLine(contactCell.FullName);
        }

        public void HighlightAndDeleteCurrentText()
        {
            _inputEmulator.Click();
            Thread.Sleep(5);
            _inputEmulator.Click();
            Thread.Sleep(5);
            _inputEmulator.Click();
            Thread.Sleep(5);

            _inputEmulator.PressDelete();
        }

        private bool CheckIsAccountExisting()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);
            Color currentColor = _pixelChecker.GetColorOfPyxel(_accAvatarPosition.X, _accAvatarPosition.Y);

            if (currentColor.R == _backgroundColor.R && currentColor.G == _backgroundColor.G && currentColor.B == _backgroundColor.B)
                Console.WriteLine("Аккаунт не существует или не найден в списке контактов");

            return !(currentColor.R == _backgroundColor.R && currentColor.G == _backgroundColor.G && currentColor.B == _backgroundColor.B);
        }

        private bool CheckIsNewMessageThere()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);
            Color currentColor = _pixelChecker.GetColorOfPyxel(_newMessageIconPosition.X, _newMessageIconPosition.Y);
            Color currentColor2 = _pixelChecker.GetColorOfPyxel(_newMessageIconPosition2.X, _newMessageIconPosition2.Y);

            if ((currentColor.R == _newMessageIconColor.R && currentColor.G == _newMessageIconColor.G && currentColor.B == _newMessageIconColor.B) || (currentColor2.R == _newMessageIconColor.R && currentColor2.G == _newMessageIconColor.G && currentColor2.B == _newMessageIconColor.B))
                Console.WriteLine("New Message There");

            return (currentColor.R == _newMessageIconColor.R && currentColor.G == _newMessageIconColor.G && currentColor.B == _newMessageIconColor.B);
        }

        private bool CheckIsMessageGotReaden()
        {
            _inputEmulator.SetActiveSpecificProcess(WindowName);
            Color currentColor = _pixelChecker.GetColorOfPyxel(_messageGotReadenIconPosition.X, _messageGotReadenIconPosition.Y);

            bool isColor1 = (currentColor.R == _messageGotReadenIconColor1.R && currentColor.G == _messageGotReadenIconColor1.G && currentColor.B == _messageGotReadenIconColor1.B);
            bool isColor2 = (currentColor.R == _messageGotReadenIconColor2.R && currentColor.G == _messageGotReadenIconColor2.G && currentColor.B == _messageGotReadenIconColor2.B);

            if (isColor1 || isColor2)
                Console.WriteLine("Message Got Readen");

            return (isColor1 || isColor2);
        }
    }

    struct ValuePair<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}
